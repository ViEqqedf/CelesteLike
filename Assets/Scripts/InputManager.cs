using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;
    // PlayerCharacter character;

    #region Key
    [Header("控制是否使用自定义按键")]
    public bool IsKeySet;
    [Header("左移动")]
    public KeyCode LeftMoveKey;
    [Header("右移动")]
    public KeyCode RightMoveKey;
    [Header("跳跃按键")]
    public KeyCode Jump;
    [Header("冲刺按键")]
    public KeyCode Dash;
    [Header("爬墙按键")]
    public KeyCode Climb;
    [HideInInspector]
    public bool ClimbKey => Input.GetKey(Climb);
    [HideInInspector]
    public bool ClimbKeyDown => Input.GetKeyDown(Climb);
    [HideInInspector]
    public bool ClimbKeyUp => Input.GetKeyUp(Climb);
    [HideInInspector]
    public bool JumpKey => Input.GetKey(Jump);
    [HideInInspector]
    public bool JumpKeyDown {
        get {
            if(Input.GetKeyDown(Jump)) {
                return true;
            } else if(JumpFrame > 0) {
                return true;
            }
            return false;
        }
    }
    [HideInInspector]
    public bool JumpKeyUp => Input.GetKeyUp(Jump);
    [HideInInspector]
    public bool DashKey => Input.GetKey(Dash);
    [HideInInspector]
    public bool DashKeyDown => Input.GetKeyDown(Dash);
    [HideInInspector]
    public bool DashKeyUp => Input.GetKeyUp(Dash);
    #endregion

    [HideInInspector]
    public float v = 0;
    public float h = 0;
    public AnimationCurve MoveStartCurve;
    public AnimationCurve MoveEndCurve;
    [SerializeField]
    float MoveStartTime;
    [SerializeField]
    float MoveEndTime;
    [SerializeField]
    public int MoveDir;

    private int JumpFrame;
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        // character = GetComponent<PlayerCharacter>();
        KeyInit();
    }
    public void KeyInit() {
        if (!IsKeySet) {
            Jump = KeyCode.K;
            Dash = KeyCode.J;
            Climb = KeyCode.L;
            LeftMoveKey = KeyCode.A;
            RightMoveKey = KeyCode.D;
        }
    }

    private void FixedUpdate() {
        if(JumpFrame >= 0) {
            JumpFrame--;
        }
    }

    private void Update() {
        CheckHorizontalMove();

        bool isRightMoveKeyDown = Input.GetKey(RightMoveKey);
        bool isLeftMoveKeyDown = Input.GetKey(LeftMoveKey);
        if (isRightMoveKeyDown && isLeftMoveKeyDown || !isRightMoveKeyDown && !isLeftMoveKeyDown) {
            h = 0;
        } else if (isRightMoveKeyDown) {
            h = 1;
        } else if (isLeftMoveKeyDown) {
            h = -1;
        }

        // v = Input.GetAxisRaw("Vertical");
        // h = Input.GetAxisRaw("Horizontal");

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