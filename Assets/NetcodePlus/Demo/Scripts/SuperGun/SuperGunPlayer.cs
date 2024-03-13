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

        private Rigidbody2D rigid;
        private SNetworkActions actions;
        private SuperGunPlayerState sync_state = new SuperGunPlayerState();
        private float refresh_timer = 0f;

        protected override void Awake()
        {
            base.Awake();
            rigid = GetComponent<Rigidbody2D>();
            sync_state.position = transform.position;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            actions = new SNetworkActions(this);
            actions.RegisterSerializable("sync", ReceiveSync, NetworkDelivery.Unreliable);
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
        }

        private void UpdateOwner()
        {
            if (!IsOwner)
                return;

            //Get Key Controls
            PlayerControl2D controls = PlayerControl2D.Get();
            Vector3 cmove = controls.GetMove();
            bool isJumpPressed = controls.IsPressJump();
            Vector3 move = new Vector3(cmove.x*move_speed, rigid.velocity.y, 0);

            //Rotate controls to camera angle
            CameraPlayer cam = CameraPlayer.Get();
            if (cam != null)
                move = cam.GetFacingRotation() * move;

            //Move
            Turn(cmove.x);
            Move(move);
            Jump(isJumpPressed);

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

            Turn(sync_state.move.normalized.x);
            Move(sync_state.move);
            Jump(sync_state.jump);
        }

        private void Move(Vector3 dir)
        {
            rigid.velocity = dir;
        }
        private void Turn(float xVelocity)
        {
            if (xVelocity != 0)
            {
                transform.localScale=new Vector3(xVelocity,1,1);
            }
        }
        private void Jump(bool isJumpPressed)
        {
            if (isJumpPressed)
                rigid.velocity = new Vector3(rigid.velocity.x, jump_force);
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

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref timing);
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref move);
            serializer.SerializeValue(ref jump);
            serializer.SerializeValue(ref scale);
        }
    }
}
