using Suriyun.MCS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NetcodePlus.Demo
{
    public class SuperGunPlayer : SNetworkPlayer
    {
        [Header("PlayerState")]
        public float move_speed = 2f;
        public float jump_force = 10f;
        public int maxJumpCount = 2;
        public float hp_max = 100;
        public float hp;
        public float life;
        public float life_max;
        private float forceRecession = 1.5f;
        private float rushDistance = 5f;
        private float rushTime = 0.2f;
        private float rushTimeCoolDown = 1f;
        public Vector3 rushDir;
        public bool Ondie = false;

        //[Header("Synchonization Parameters")]
        public float sync_refresh_rate = 0.02f;
        public float sync_threshold = 0.1f;
        public float sync_interpolate = 5f;
        public float sync_teleportTime = 0.2f;

        [Header("Cooling Time")]
        public float firstFunc_timer = 0;
        public float secondFunc_timer = 0;
        public float rush_timer = 0;

        [Header("Other Setting")]
        public LayerMask groundLayer;
        public Weapons weapon;
        public PlatForm2D touchedGround;

        public Camera playerCamera;

        [Header("Player Force")]
        public Vector3 playerForce;

        [Header("RushButton")]
        public UniversalButton rushButton;
        public RectTransform poinerTrans;
        public RectTransform amierTrans;

        [Header("Animator")]
        public Animator animator;

        private int currentJumpCount;
        private int currenFacing = 0;

        private PlayerControl2D controls;
        private Transform weaponHandler;
        private Rigidbody2D rigid;
        private Collider2D col;
        private SNetworkActions actions;

        private SuperGunPlayerState sync_state = new SuperGunPlayerState();
        private float refresh_timer = 0f;

        private static List<SuperGunPlayer> superGunPlayer_list = new List<SuperGunPlayer>();

        protected override void Awake()
        {
            base.Awake();
            superGunPlayer_list.Add(this);
            rigid = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            sync_state.position = transform.position;
            hp = hp_max;
            life = life_max; 
            weaponHandler = transform.Find("WeaponHandler");
            playerCamera = GetComponentInChildren<Camera>();
            animator = transform.Find("LiveBody").GetChild(0).GetComponent<Animator>();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            superGunPlayer_list.Remove(this);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            actions = new SNetworkActions(this);
            actions.RegisterString("equip", DoEquipWeapon);
            actions.Register("jump", DoJump);
            actions.Register("fall", DoFall);
            actions.Register("func1", DoUseWeapon1stFunc);
            actions.Register("func2", DoUseWeapon2ndFunc);
            actions.RegisterFloat("damage", DoDamage);
            actions.RegisterVector("addForce", DoAddForce);
            actions.RegisterVector("rush", DoRush);
            actions.Register("kill", DoKill);
            actions.Register("die", DoDie);
            actions.RegisterSerializable("sync", ReceiveSync, NetworkDelivery.Unreliable);
        }

        protected override void OnDespawn()
        {
            base.OnDespawn();
            actions.Clear();
        }
        private void Start()
        {
            if (!IsOwner)
            {
                Destroy(playerCamera.gameObject);
            }
            else if(Application.platform == RuntimePlatform.Android)
            {
                rushButton = GameObject.Find("Skill0").GetComponent<UniversalButton>();
                amierTrans = rushButton.aimer.GetComponent<RectTransform>();
                poinerTrans = rushButton.pointer.GetComponent<RectTransform>();
            }

            if(IsOwner)
            {
                EquipWeapon("pistol");
            }
        }

        protected virtual void Update()
        {
            UpdateOwner();
            UpdateNotOwner();
            firstFunc_timer += Time.deltaTime;
            secondFunc_timer += Time.deltaTime;
            rush_timer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log(weaponHandler.transform.position);
            }
        }

        private void UpdateOwner()
        {
            if (!IsOwner)
                return;

            if (weapon == null)
            {
                EquipWeapon("pistol");
            }

            //Get Key Controls
            controls = PlayerControl2D.Get();
            Vector3 cmove = controls.GetMove();
            bool isJumpPressed = controls.IsPressJump();
            bool isFunc1stPressed = controls.IsPress1stFunc();
            bool isFunc2ndPressed = controls.IsPress2ndFunc();
            bool isFallPressed = controls.IsPressFall();
            bool isRushPressed = controls.IsPressRush();

            if (Application.platform == RuntimePlatform.Android)
            {
                rushDir = (Vector2)TouchControlManager.instance.rushButton.direction;
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                rushDir = controls.rushDir;
            }
            else if(Application.platform == RuntimePlatform.WindowsPlayer)
            {
                rushDir = controls.rushDir;
            }

            Vector3 move = new Vector3(cmove.x * move_speed, rigid.velocity.y, 0);
            animator.SetFloat("velocityY",rigid.velocity.y);

            //Rotate controls to camera angle
            CameraPlayer cam = CameraPlayer.Get();
            if (cam != null)
                move = cam.GetFacingRotation() * move;

            //Move
            Turn(cmove.x);
            Move(move);
            Jump(isJumpPressed);
            Fall(isFallPressed);
            Rush(isRushPressed);

            if (playerForce != Vector3.zero)
            {
                AddForce(playerForce);
            }

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
            sync_state.hp = hp;
            sync_state.life = life;
            sync_state.move = move;
            sync_state.jump = isJumpPressed;
            sync_state.fall = isFallPressed;
            sync_state.rushDir = rushDir;
            sync_state.rush = isRushPressed;
            sync_state.useWeapon1st = isFunc1stPressed;
            sync_state.useWeapon2nd = isFunc2ndPressed;
            sync_state.playerForce = playerForce;
            //sync_jumpCount = currentJumpCount;

            actions?.Trigger("sync", sync_state); // ReceiveSync(sync_state)
        }

        private void UpdateNotOwner()
        {
            if (IsOwner)
                return;

            if (weapon == null)
            {
                EquipWeapon("pistol");
            }

            Vector3 offset = sync_state.position - transform.position; //Is the character position out of sync?

            if (offset.magnitude > sync_threshold && offset.magnitude < sync_threshold * 10f)
            {
               transform.position = Vector3.MoveTowards(transform.position, sync_state.position, sync_interpolate * Time.deltaTime);
            }

            if (!sync_state.onRush)//stop syncropos in rush
            {
                if (offset.magnitude >= sync_threshold * 10f)
                    StartCoroutine(OffsetMove(sync_state.position, rushTime));
            }


            //currentJumpCount = sync_jumpCount;
            AddForce(sync_state.playerForce);
            Turn(sync_state.move.x);
            Move(sync_state.move);
            Jump(sync_state.jump);
            Fall(sync_state.fall);
            playerForce = sync_state.playerForce;
            rushDir = sync_state.rushDir;
            life = sync_state.life;
            Rush(sync_state.rush);
            UseWeapon1stFunc(sync_state.useWeapon1st);
            UseWeapon2ndFunc(sync_state.useWeapon2nd);
        }
        IEnumerator OffsetMove(Vector3 targetPos, float offsetTime)
        {
            float elapsedTime = 0; 
            Vector2 startPosition = transform.position; 

            while (elapsedTime < offsetTime)
            {
                float ratio = elapsedTime / offsetTime;
                transform.position = Vector2.Lerp(startPosition, targetPos, ratio);
                elapsedTime += Time.deltaTime; 
                yield return null; 
            }

            transform.position = targetPos;
        }
        private void Move(Vector3 dir)
        {
            rigid.velocity = dir;
            bool playerHasXAxisSpeed = Mathf.Abs(rigid.velocity.x) > Mathf.Epsilon;
            animator.SetBool("move",playerHasXAxisSpeed);
        }
        private void Turn(float xVelocity)
        {
            if (xVelocity != 0)
            {
                if (xVelocity > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    currenFacing = 1;
                    weapon.SetGunFacing(currenFacing);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    currenFacing = -1; 
                    weapon.SetGunFacing(currenFacing);
                }

            }
        }
        private void Jump(bool isJumpPressed)
        {

            bool isOnGround = IsOnGroundCheck();

            if (isOnGround)
                //Debug.Log("onground");
                currentJumpCount = maxJumpCount - 1;

            if (isJumpPressed && isOnGround)
            {
                //Debug.Log("try to jump");
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
            animator.SetBool("jump", true);
            rigid.velocity = new Vector3(rigid.velocity.x, jump_force);
        }

        private void Fall(bool isFallPressed)
        {
            if (isFallPressed && IsOnGroundCheck())
            {
                //Debug.Log("Pressed FallKey");
                actions?.Trigger("fall");
            }
        }
        private void DoFall()
        {
            if (touchedGround != null)
            {
                touchedGround.PassThroughPlatForm(col);
            }
        }

        private bool IsOnGroundCheck()
        {
            //Debug.Log(col.IsTouchingLayers());
            if (col.IsTouchingLayers(groundLayer))
            {
                animator.SetBool("jump", false);
                return true;
            }
            else
            {
                animator.SetBool("jump", true);
                return false;
            }
        }
        public void EquipWeapon(string weaponName)
        {
            if (!Ondie)
            {
                actions?.Trigger("equip", weaponName);
            }
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
                weapon = WeaponsManager.Get().InstantiateWeapon(weapon, weaponHandler).GetComponent<Weapons>();
            }
        }
        public void Damage(float damage)
        {
            if (IsServer)
                actions?.Trigger("damage", damage);
        }
        private void DoDamage(float damage)
        {
            hp -= damage;
            Debug.Log(PlayerID + "hp:" + hp);
            if (hp < 0)
                Kill();
        }
        public void Kill()
        {
            Debug.Log(player_id + "die");
            if(IsOwner)
            {
                actions?.Trigger("kill");
            }
        }
        private void DoKill()
        {
            PlayerSpawn spawn =  PlayerSpawn.Get(player_id);

            hp = hp_max;
            this.transform.position = spawn.transform.position;

            if (IsOwner)
            {
                EquipWeapon("pistol");
            }
            life--;
            if(life <= 0 && !Ondie)
            {
                Die();
            }
        }
        public void Die()
        {
            actions?.Trigger("die");
        }
        private void DoDie()
        {
            Ondie = true;
            transform.Find("LiveBody").gameObject.SetActive(false);
            transform.Find("DeathBody").gameObject.SetActive(true);
            this.gameObject.layer = 9;
            jump_force *= 1.5f;
            weapon.Discard();
        }
        private void Rush(bool isRushPressed)
        {
            if (isRushPressed && rush_timer > rushTimeCoolDown)
            {

                if (rushDir == Vector3.zero  && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
                {
                    rushDir = new Vector2(GetUserFacing(), 0);
                    actions?.Trigger("rush", rushDir);
                    return;
                }
                else if( Application.platform == RuntimePlatform.Android)
                {
                    rushDir = (poinerTrans.position-amierTrans.position);
                    Debug.Log("get button: " + rushDir);
                    if(rushDir.magnitude > 0.8f)
                    {
                        actions?.Trigger("rush", rushDir);
                    }
                    return;
                }
                actions?.Trigger("rush", rushDir);
            }
        }
        private void DoRush(Vector3 _rushDir)
        {
           // Debug.Log("rushDir:"+ rushDir);
            StartCoroutine(IEnumRush(_rushDir));
            rush_timer = 0;
            if (!IsOwner)
            {
                Debug.Log("other player DO rush");
                Debug.Log("rush Dir = " + rushDir);
            }
        }
        IEnumerator IEnumRush(Vector3 _rushDir)
        {
            float elapsedTime = 0; // 已过时间
            Vector2 startPosition = transform.position; // 开始位置
            Vector2 targetPosition = startPosition + (Vector2)_rushDir.normalized * rushDistance; // 目标位置
            Debug.Log("rushDir = :"+_rushDir);
            Debug.Log("rushDir normal:" + rushDir.normalized);
            if(IsOwner)
            {
                sync_state.onRush = true;
            }

            while (elapsedTime < rushTime)
            {

                // 计算当前时间在总时间的比例
                float ratio = elapsedTime / rushTime;
                // 使用插值函数更新位置
                transform.position = Vector2.Lerp(startPosition, targetPosition, ratio);
                elapsedTime += Time.deltaTime; // 更新已过时间
                yield return null; // 等待下一帧
            }

            // 确保物体完全移动到目标位置
            transform.position = targetPosition;
            
            if (IsOwner)
            {
                sync_state.onRush = false;
            }
        }
        public void SetForce(Vector3 force)
        {
            playerForce += force;
        }
        private void AddForce(Vector3 force)
        {
            actions?.Trigger("addForce", force);
        }
        private void DoAddForce(Vector3 force)
        {
            rigid.AddForce(force, ForceMode2D.Impulse);
            playerForce.x = playerForce.x / forceRecession;
            playerForce.y = playerForce.y / forceRecession;
            if (playerForce.magnitude < 1)
            {
                playerForce = Vector3.zero;
            }
        }
        public void UseWeapon1stFunc(bool isPressed)
        {
            if (Ondie)
                return;

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
            if (Ondie)
                return;

            if (isPressed && IsOwner && secondFunc_timer > weapon.secondFuncCoolTime)
            {
                actions?.Trigger("func2");
                secondFunc_timer = 0;
            }
        }

        public int GetUserFacing()
        {
            if (this.transform.localScale.x > 0)
            {
                return 1;
            }
            else
            {
                return -1;
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
            hp = state.hp;
        }
        public static new SuperGunPlayer Get(int player_id)
        {
            foreach (SuperGunPlayer superGunPlayer in superGunPlayer_list)
            {
                if (superGunPlayer.player_id == player_id)
                    return superGunPlayer;
            }
            return null;
        }
        public static new List<SuperGunPlayer> GetAll()
        {
            return superGunPlayer_list;
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
        public bool fall;
        public bool rush;
        public int jumpCount;
        public bool useWeapon1st;
        public bool useWeapon2nd;
        public float hp;
        public float life;
        public Vector3 playerForce;
        public Vector2 rushDir;
        public bool onRush;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerForce);
            serializer.SerializeValue(ref hp);
            serializer.SerializeValue(ref life);
            serializer.SerializeValue(ref timing);
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref move);
            serializer.SerializeValue(ref jump);
            serializer.SerializeValue(ref scale);
            serializer.SerializeValue(ref jumpCount);
            serializer.SerializeValue(ref fall);
            serializer.SerializeValue(ref rush);
            serializer.SerializeValue(ref useWeapon1st);
            serializer.SerializeValue(ref useWeapon2nd);
            serializer.SerializeValue(ref hp);
            serializer.SerializeValue(ref rushDir);
            serializer.SerializeValue(ref onRush);
        }
    }
}
