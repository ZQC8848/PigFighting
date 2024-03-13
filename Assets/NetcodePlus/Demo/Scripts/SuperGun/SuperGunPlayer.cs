using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NetcodePlus.Demo
{
    public class SuperGunPlayer : SNetworkPlayer
    {
        public float move_speed = 2f;
        public float jump_force = 10f;
        public float sync_refresh_rate = 0.05f;
        public float sync_threshold = 0.1f;
        public float sync_interpolate = 5f;
        private int sync_jumpCount = 2;
        public float firstFunc_timer = 0;
        public float secondFunc_timer = 0;

        private int currentJumpCount;
        public int maxJumpCount = 2;
        public LayerMask groundLayer;
        public Weapons weapon;

        private Transform weaponHandler;
        private Rigidbody2D rigid;
        private Collider2D col;
        private SNetworkActions actions;
        private SuperGunPlayerState sync_state = new SuperGunPlayerState();
        private float refresh_timer = 0f;

        protected override void Awake()
        {
            base.Awake();
            rigid = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            sync_state.position = transform.position;
            weaponHandler = transform.Find("WeaponHandler");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            actions = new SNetworkActions(this);
            actions.RegisterString("equip", DoEquipWeapon);
            actions.Register("jump", DoJump);
            actions.Register("func1", DoUseWeapon1stFunc);
            actions.Register("func2", DoUseWeapon2ndFunc);
            actions.RegisterSerializable("sync", ReceiveSync, NetworkDelivery.Unreliable);

            EquipWeapon("pistol");
        }

        protected override void OnDespawn()
        {
            base.OnDespawn();
            actions.Clear();
        }

        protected virtual void Update()
        {
            UpdateOwner();
            UpdateNotOwner();
            firstFunc_timer += Time.deltaTime;
            secondFunc_timer += Time.deltaTime;
        }

        private void UpdateOwner()
        {
            if (!IsOwner)
                return;

            //Get Key Controls
            PlayerControl2D controls = PlayerControl2D.Get();
            Vector3 cmove = controls.GetMove();
            bool isJumpPressed = controls.IsPressJump();
            bool isFunc1stPressed = controls.IsPress1stFunc();
            bool isFunc2ndPressed = controls.IsPress2ndFunc();
            Vector3 move = new Vector3(cmove.x * move_speed, rigid.velocity.y, 0);

            //Rotate controls to camera angle
            CameraPlayer cam = CameraPlayer.Get();
            if (cam != null)
                move = cam.GetFacingRotation() * move;

            //Move
            Turn(cmove.x);
            Move(move);
            Jump(isJumpPressed);
            UseWeapon1stFunc(isFunc1stPressed);
            UseWeapon2ndFunc(isFunc2ndPressed);

            //Refresh Timer
            refresh_timer += Time.deltaTime;
            if (refresh_timer < sync_refresh_rate)
                return;

            //Refresh to other clients
            refresh_timer = 0f;
            sync_state.timing = sync_state.timing + 1;
            sync_state.position = transform.position;
            sync_state.move = move;
            sync_state.jump = isJumpPressed;
            sync_state.useWeapon1st = isFunc1stPressed;
            sync_state.useWeapon2nd = isFunc2ndPressed;

            sync_jumpCount = currentJumpCount;

            actions?.Trigger("sync", sync_state); // ReceiveSync(sync_state)
        }

        private void UpdateNotOwner()
        {
            if (IsOwner)
                return;

            Vector3 offset = sync_state.position - transform.position; //Is the character position out of sync?

            if (offset.magnitude > sync_threshold)
                transform.position = Vector3.MoveTowards(transform.position, sync_state.position, sync_interpolate * Time.deltaTime);

            if (offset.magnitude > sync_threshold * 10f)
                transform.position = sync_state.position; //Teleport if too far

            currentJumpCount = sync_jumpCount;

            Turn(sync_state.move.x);
            Move(sync_state.move);
            Jump(sync_state.jump);
            UseWeapon1stFunc(sync_state.useWeapon1st);
            UseWeapon2ndFunc(sync_state.useWeapon2nd);
        }

        private void Move(Vector3 dir)
        {
            rigid.velocity = dir;
        }
        private void Turn(float xVelocity)
        {
            if (xVelocity != 0)
            {
                if (xVelocity > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

            }
        }
        private void Jump(bool isJumpPressed)
        {
            bool isOnGround = IsOnGroundCheck();

            if (isOnGround)
            {
                currentJumpCount = maxJumpCount - 1;
            }
            if (isJumpPressed && isOnGround)
            {
                actions?.Trigger("jump");
                currentJumpCount--;
            }
            else if (isJumpPressed && currentJumpCount > 0 && !isOnGround)
            {
                actions?.Trigger("jump");
                currentJumpCount--;
            }
        }
        private void DoJump()
        {
            rigid.velocity = new Vector3(rigid.velocity.x, jump_force);
        }
        private bool IsOnGroundCheck()
        {
            if (col.IsTouchingLayers(groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void EquipWeapon(string weaponName)
        {
            actions?.Trigger("equip", weaponName);
        }
        private void DoEquipWeapon(string weaponName)
        {
            Weapons _weapon = WeaponsManager.Get().FindWeapon(weaponName);
            if (_weapon != null)
            {
                if (weapon != null)
                {
                    weapon.Discard();
                }
                weapon = _weapon;
                WeaponsManager.Get().InstantiateWeapon(weapon, weaponHandler);
            }
        }
        public void UseWeapon1stFunc(bool isPressed)
        {
            if (isPressed && IsOwner && firstFunc_timer > weapon.firstFuncCoolTime)
            {
                actions?.Trigger("func1");
            }
        }
        private void DoUseWeapon1stFunc()
        {
            weapon.FisrtFunc();
            firstFunc_timer = 0;
        }
        public void UseWeapon2ndFunc(bool isPressed)
        {
            if (isPressed && IsOwner && secondFunc_timer > weapon.secondFuncCoolTime)
            {
                actions?.Trigger("func2");
                secondFunc_timer = 0;
            }
        }

        private void DoUseWeapon2ndFunc()
        {
            weapon.SecondFunc();
        }
        private void ReceiveSync(SerializedData sdata)
        {
            if (IsOwner)
                return;

            SuperGunPlayerState state = sdata.Get<SuperGunPlayerState>();
            if (state.timing < sync_state.timing)
                return; //Old timing, ignore package, this means packages arrived in wrong order, prevent glitch

            sync_state = state;
        }
    }

    [System.Serializable]
    public struct SuperGunPlayerState : INetworkSerializable
    {
        public ulong timing; //Increased by 1 each frame
        public Vector3 position;
        public Vector3 move;
        public Vector3 scale;
        public bool jump;
        public int jumpCount;
        public bool useWeapon1st;
        public bool useWeapon2nd;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref timing);
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref move);
            serializer.SerializeValue(ref jump);
            serializer.SerializeValue(ref scale);
            serializer.SerializeValue(ref jumpCount);
        }
    }
}
