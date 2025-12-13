using UnityEngine;

[DefaultExecutionOrder(50)]
public class maincameracontrol : MonoBehaviour
{
    public Transform target;
    public float height = 2f, distance = 5f, speedsmooth = 10f, skewing = 0f;
    private float totalRotationX = 0f; private float totalRotationY = 0f;
    public float sensitivity = 2f;

    // 新增：滚轮缩放参数
    [Header("滚轮缩放设置")]
    [Tooltip("滚轮灵敏度（值越大缩放越快）")]
    public float scrollSensitivity = 1f;
    [Tooltip("最小距离（拉近极限）")]
    public float minDistance = 2f;
    [Tooltip("最大距离（拉远极限）")]
    public float maxDistance = 10f;

    // 新增：视角上下旋转限制
    [Header("视角上下旋转限制")]
    [Tooltip("向上旋转最大角度（正值，单位：度）")]
    public float maxUpRotation = 30f;   // 例如抬头最多30度
    [Tooltip("向下旋转最大角度（负值，单位：度）")]
    public float maxDownRotation = -60f; // 例如低头最多60度

    // 新增：防穿透参数
    [Header("防穿透设置")]
    [Tooltip("射线检测忽略的层（如目标自身、相机层）")]
    public LayerMask ignoreLayers;
    [Tooltip("相机与障碍物的最小距离（避免贴紧模型）")]
    public float obstacleOffset = 0.5f;


    void LateUpdate()
    {
        // 第一阶段：输入检测（仅获取并处理原始输入）
        float mouseX = InputRecorder.Instance.MouseX;
        float mouseY = InputRecorder.Instance.MouseY;
        // 新增：获取鼠标滚轮输入
        float mouseScroll = InputRecorder.Instance.MouseScrollWheel;

        if (Mathf.Abs(mouseX) < 0.01f) mouseX = 0;
        if (Mathf.Abs(mouseY) < 0.01f) mouseY = 0;// 过滤微小输入

        // 新增：处理滚轮拉近拉远
        if (Mathf.Abs(mouseScroll) > 0.01f)
        {
            // 滚轮向上（正值）→ 距离减小（拉近）；滚轮向下（负值）→ 距离增大（拉远）
            distance -= mouseScroll * scrollSensitivity;
            // 限制距离在最小和最大之间
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }


        // 第二阶段：逻辑计算（基于输入计算目标旋转和位置，不直接操作相机）
        // 计算总旋转角度
        totalRotationX += mouseX * sensitivity;
        totalRotationY += mouseY * sensitivity;

        // 新增：限制视角上下旋转角度（totalRotationY控制上下）
        totalRotationY = Mathf.Clamp(totalRotationY, maxDownRotation, maxUpRotation);

        // 创建旋转四元数（绕Y,X轴旋转）
        Quaternion rotation = Quaternion.Euler(0, totalRotationX, totalRotationY);

        // 计算偏移量和目标位置
        Vector3 offset = rotation * new Vector3(skewing, height, -distance);
        Vector3 des = target.position + offset;


        // 新增：防穿透射线检测（在原有逻辑后插入，不修改原有代码）
        // 射线起点：目标位置；射线方向：从目标指向相机目标位置（des - target.position）
        Vector3 rayDir = des - target.position;
        // 射线检测（忽略指定层，检测所有碰撞体）
        if (Physics.Raycast(target.position, rayDir, out RaycastHit hit, rayDir.magnitude, ~ignoreLayers))
        {
            // 若检测到障碍物，将相机位置调整到障碍物前（预留offset距离）
            des = hit.point - rayDir.normalized * obstacleOffset;
        }


        // 第三阶段：执行跟随（应用计算结果到相机，完成移动和朝向）
        // 平滑移动到目标位置
        transform.position = Vector3.Lerp(transform.position, des, speedsmooth * Time.deltaTime);
        // 看向目标
        transform.LookAt(target);
    }
}