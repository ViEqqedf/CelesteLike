using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ViE.CelesteLike {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance;

    #region Key
        public bool UseCustomKey;
        public KeyCode UpMoveKey;
        public KeyCode DownMoveKey;
        public KeyCode LeftMoveKey;
        public KeyCode RightMoveKey;
        public KeyCode Jump;
        public KeyCode Dash;
        public KeyCode Climb;
        public bool ClimbKey => Input.GetKey(Climb);
        public bool ClimbKeyDown => Input.GetKeyDown(Climb);
        public bool ClimbKeyUp => Input.GetKeyUp(Climb);
        public bool JumpKey => Input.GetKey(Jump);
        public bool JumpKeyDown {
            get {
                if (Input.GetKeyDown(Jump)) {
                    return true;
                } else if (JumpFrame > 0) {
                    return true;
                }
                return false;
            }
        }
        public bool JumpKeyUp => Input.GetKeyUp(Jump);
        public bool DashKey => Input.GetKey(Dash);
        public bool DashKeyDown => Input.GetKeyDown(Dash);
        public bool DashKeyUp => Input.GetKeyUp(Dash);
    #endregion

        public float v = 0;
        public float h = 0;
        public AnimationCurve MoveStartCurve;
        public AnimationCurve MoveEndCurve;
        [SerializeField]
        public int MoveDir;
        [SerializeField]
        private float MoveStartTime;
        [SerializeField]
        private float MoveEndTime;

        private int JumpFrame;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            KeyInit();
        }
        public void KeyInit() {
            if (!UseCustomKey) {
                Jump = KeyCode.K;
                Dash = KeyCode.J;
                Climb = KeyCode.L;
                UpMoveKey = KeyCode.W;
                DownMoveKey = KeyCode.S;
                LeftMoveKey = KeyCode.A;
                RightMoveKey = KeyCode.D;
            }
        }

        private void FixedUpdate() {
            if (JumpFrame >= 0) {
                JumpFrame--;
            }
        }

        private void Update() {
            CheckHorizontalMove();

            bool isRightMoveKeyDown = Input.GetKey(RightMoveKey);
            bool isLeftMoveKeyDown = Input.GetKey(LeftMoveKey);
            if (isRightMoveKeyDown && isLeftMoveKeyDown ||
                !isRightMoveKeyDown && !isLeftMoveKeyDown) {
                h = 0;
            } else if (isRightMoveKeyDown) {
                h = 1;
            } else if (isLeftMoveKeyDown) {
                h = -1;
            }

            // v = Input.GetAxisRaw("Vertical");
            bool isUpMoveKeyDown = Input.GetKey(UpMoveKey);
            bool isDownMoveKeyDown = Input.GetKey(DownMoveKey);
            if (isUpMoveKeyDown && isDownMoveKeyDown ||
                !isUpMoveKeyDown && !isDownMoveKeyDown) {
                v = 0;
            } else if (isUpMoveKeyDown) {
                v = 1;
            } else if (isDownMoveKeyDown) {
                v = -1;
            }

            if (Input.GetKeyDown(Jump)) {
                JumpFrame = 3;
            }
        }

        void CheckHorizontalMove() {
            if (Input.GetKeyDown(RightMoveKey) && h <= 0) {
                MoveDir = 1;
            } else if (Input.GetKeyDown(LeftMoveKey) && h >= 0) {
                MoveDir = -1;
            } else if (Input.GetKeyUp(RightMoveKey)) {
                if (Input.GetKey(LeftMoveKey)) {
                    MoveDir = -1;
                    MoveStartTime = Time.time;
                } else {
                    MoveDir = 0;
                }
            } else if (Input.GetKeyUp(LeftMoveKey)) {
                if (Input.GetKey(RightMoveKey)) {
                    MoveDir = 1;
                } else {
                    MoveDir = 0;
                }
            }
        }
    }
}