using UnityEngine;

[DefaultExecutionOrder(-99)] // 确保优先执行
public class InputRecorder : MonoBehaviour
{
    public static InputRecorder Instance;

    // WASD按键状态
    public bool one;
    public bool two;
    public bool three;
    public bool W;    
    public bool S;   
    public bool A;   
    public bool D;
    public bool E;
    //
    public bool ESCDOWN;

    // 虚拟轴（WASD方向）
    public float HorizontalAxis; // 水平轴：A(左)=-1，D(右)=1
    public float VerticalAxis;   // 垂直轴：S(下)=-1，W(上)=1

    // 鼠标按键状态
    public bool MouseLeft;       // 左键按住
    public bool MouseRight;      // 右键按住
    public bool MouseLeftDown;   // 左键按下瞬间
    public bool MouseRightDown;  // 右键按下瞬间
 
    // 鼠标移动轴（增量值，每帧鼠标移动的距离）
    public float MouseX; // 水平移动：鼠标向右移为正，向左为负
    public float MouseY; // 垂直移动：鼠标向上移为正，向下为负

    // 新增：鼠标滚轮滚动增量（向上滚为正，向下滚为负）
    public float MouseScrollWheel; // 每帧滚轮滚动的增量值


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        RecordKeyStates();   
        RecordVirtualAxes(); // 更新虚拟轴
        RecordMouseInput();  // 更新鼠标（含滚轮）
    }

    private void RecordKeyStates()
    {
        one = Input.GetKey(KeyCode.Alpha1);
        two = Input.GetKey(KeyCode.Alpha2);
        three = Input.GetKey(KeyCode.Alpha3);
        W = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        S = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        A = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        D = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        E = Input.GetKey(KeyCode.E);
        ESCDOWN = Input.GetKeyDown(KeyCode.Escape);
    }

    private void RecordVirtualAxes()
    {
        HorizontalAxis = Input.GetAxis("Horizontal");
        VerticalAxis = Input.GetAxis("Vertical");
    }

    private void RecordMouseInput()
    {
        // 原有鼠标按键检测
        MouseLeft = Input.GetKey(KeyCode.Mouse0);
        MouseRight = Input.GetKey(KeyCode.Mouse1);
        MouseLeftDown = Input.GetKeyDown(KeyCode.Mouse0);
        MouseRightDown = Input.GetKeyDown(KeyCode.Mouse1);

        // 鼠标X/Y轴移动增量检测
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        // 新增：鼠标滚轮滚动增量检测
        // Input.GetAxis("Mouse ScrollWheel")：向上滚返回正值，向下滚返回负值，静止时为0
        MouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
    }
}