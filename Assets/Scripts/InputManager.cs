using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    PlayerCharacter character;

    [Header("控制是否使用自定义按键")]
    public bool keyIsSet;
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
        get
        {
            if(Input.GetKeyDown(Jump))
                return true;
            else if(JumpFrame > 0)
                return true;
            
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


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        character = GetComponent<PlayerCharacter>();
        KeyInit();
    }

    private void KeyInit()
    {
        if (!keyIsSet)
        {
            Jump = KeyCode.K;
            Dash = KeyCode.J;
            Climb = KeyCode.L;
            LeftMoveKey = KeyCode.A;
            RightMoveKey = KeyCode.D;
        }
    }

    private void Update()
    {
        CheckHorizontalMove();
    }

    void CheckHorizontalMove()
    {
        //按下右键时判断左键是否正按着，是的话返回右方向
        if (Input.GetKeyDown(RightMoveKey) && Input.GetAxisRaw("Horizontal") <= 0)
            MoveDir = 1;
        //同上
        else if (Input.GetKeyDown(LeftMoveKey) && Input.GetAxisRaw("Horizontal") >= 0)
            MoveDir = -1;
        else if (Input.GetKeyUp(RightMoveKey))
        {
            //松开右键时左键是否正按着
            if (Input.GetKey(LeftMoveKey))
            {
                MoveDir = -1;
                MoveStartTime = Time.time;
            }
            else
                MoveDir = 0;
        }
        else if (Input.GetKeyUp(LeftMoveKey))
        {
            //同上
            if (Input.GetKey(RightMoveKey))
                MoveDir = 1;
            else
                MoveDir = 0;
        }
    }
}
