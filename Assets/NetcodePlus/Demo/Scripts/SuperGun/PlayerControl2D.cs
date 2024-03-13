using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetcodePlus.Demo
{
    /// <summary>
    /// Keyboard controls manager
    /// </summary>

    public class PlayerControl2D : MonoBehaviour
    {
        [Header("First Weapon Function")]
        public KeyCode func1st_key = KeyCode.J;
        [Header("Second Weapon Function")]
        public KeyCode func2nd_key = KeyCode.K;
        [Header("Jump")]
        public KeyCode jump_key = KeyCode.W;

        [Header("Camera")]
        public KeyCode cam_rotate_left = KeyCode.Q;
        public KeyCode cam_rotate_right = KeyCode.E;

        [Header("Menu")]
        public KeyCode menu_accept = KeyCode.Return;
        public KeyCode menu_cancel = KeyCode.Backspace;
        public KeyCode menu_pause = KeyCode.Escape;

        private Vector3 move;
        private float rotate_cam;
        private float mouse_scroll = 0f;

        private bool press_2ndfunc;
        private bool press_1stfunc;
        private bool press_jump;

        private bool press_accept;
        private bool press_cancel;
        private bool press_pause;

        private static PlayerControl2D instance = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        void Update()
        {
            move = Vector3.zero;
            rotate_cam = 0f;
            press_2ndfunc = false;
            press_1stfunc = false;
            press_jump = false;

            press_accept = false;
            press_cancel = false;

            Vector3 wasd = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
                wasd -= Vector3.right;
            if (Input.GetKey(KeyCode.D))
                wasd += Vector3.right;

            if (Input.GetKey(cam_rotate_left))
                rotate_cam += -1f;
            if (Input.GetKey(cam_rotate_right))
                rotate_cam += 1f;

            if (Input.GetKeyDown(func2nd_key))
                press_2ndfunc = true;
            if (Input.GetKeyDown(func1st_key))
                press_1stfunc = true;
            if (Input.GetKeyDown(jump_key))
                press_jump = true;

            if (Input.GetKeyDown(menu_accept))
                press_accept = true;
            if (Input.GetKeyDown(menu_cancel))
                press_cancel = true;
            if (Input.GetKeyDown(menu_pause))
                press_pause = true;

            move = wasd;
            move = move.normalized * Mathf.Min(move.magnitude, 1f);
            mouse_scroll = Input.mouseScrollDelta.y;
        }

        public Vector3 GetMove() { return move; }
        public bool IsMoving() { return move.magnitude > 0.1f; }
        public float GetRotateCam() { return rotate_cam; }

        public bool IsPress1stFunc() { return press_1stfunc; }
        public bool IsPress2ndFunc() { return press_2ndfunc; }
        public bool IsPressJump() { return press_jump; }

        public bool IsPressMenuAccept() { return press_accept; }
        public bool IsPressMenuCancel() { return press_cancel; }
        public bool IsPressPause() { return press_pause; }

        public float GetMouseScroll()
        {
            return mouse_scroll;
        }

        public bool IsPressedByName(string name)
        {
            return Input.GetKeyDown(name);
        }

        public static PlayerControl2D Get()
        {
            return instance;
        }
    }

}