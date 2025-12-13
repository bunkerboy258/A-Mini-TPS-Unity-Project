using UnityEngine;

public class TankStabilizer : MonoBehaviour
{
    [Header("台阶跨越设置")]
    public float maxStepHeight = 0.15f; // 最大可跨越台阶高度
    public float stepCheckDistance = 1.5f; // 台阶检测距离
    public float stepClimbForce = 50000f; // 爬台阶的力

    [Header("滞空修正")]
    public float airStabilizeForce = 100000f; // 滞空稳定力
    public float maxAirTime = 2.0f; // 最大滞空时间
    public LayerMask groundLayer = 1; // 地面层级

    [Header("翻转恢复")]
    public float tiltThreshold = 45.0f; // 倾斜阈值(度)
    public float flipThreshold = 80.0f; // 翻转阈值(度)
    public float rightingTorque = 50000f; // 恢复扭矩
    public float flipRecoveryTime = 3.0f; // 强制恢复时间

    private Rigidbody rb;
    private bool isGrounded;
    private float airTime;
    private float flipTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("TankStabilizer 需要Rigidbody组件!");
        }
    }

    void FixedUpdate()
    {
        CheckGroundStatus();
        HandleStepClimbing();
        HandleAirStabilization();
        HandleFlipRecovery();
    }

    void CheckGroundStatus()
    {
        // 使用多个射线检测确保坦克是否着地
        RaycastHit hit;
        Vector3[] rayOrigins = {
            transform.position,
            transform.position + transform.forward * 0.5f,
            transform.position - transform.forward * 0.5f,
            transform.position + transform.right * 0.5f,
            transform.position - transform.right * 0.5f
        };

        bool wasGrounded = isGrounded;
        isGrounded = false;

        foreach (Vector3 origin in rayOrigins)
        {
            if (Physics.Raycast(origin, -transform.up, out hit, 1.2f, groundLayer))
            {
                isGrounded = true;
                break;
            }
        }

        // 更新滞空时间
        if (isGrounded)
        {
            airTime = 0f;
        }
        else
        {
            airTime += Time.fixedDeltaTime;
        }
    }

    void HandleStepClimbing()
    {
        if (!isGrounded) return;

        // 在坦克前方检测台阶
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + transform.up * 0.1f;

        // 向前方发射射线检测台阶
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, stepCheckDistance, groundLayer))
        {
            // 检查是否是台阶
            float stepHeight = hit.point.y - transform.position.y;

            if (stepHeight > 0 && stepHeight <= maxStepHeight)
            {
                // 计算爬台阶需要的力
                Vector3 climbForce = transform.up * stepClimbForce;
                rb.AddForce(climbForce, ForceMode.VelocityChange);

                // 可选：添加一点向前的力帮助跨越
                rb.AddForce(transform.forward * stepClimbForce * 0.5f, ForceMode.VelocityChange);
            }
        }
    }

    void HandleAirStabilization()
    {
        if (isGrounded || airTime < 0.1f) return;

        // 滞空时施加稳定力
        // 1. 减少旋转
        if (rb.angularVelocity.magnitude > 1.0f)
        {
            rb.angularVelocity *= 0.95f;
        }

        // 2. 保持坦克直立
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        if (tiltAngle > 5.0f)
        {
            // 计算恢复扭矩
            Vector3 torque = CalculateRightingTorque();
            rb.AddTorque(torque * airStabilizeForce);
        }

        // 3. 如果滞空时间过长，施加向下的力帮助落地
        if (airTime > maxAirTime * 0.5f)
        {
            rb.AddForce(Vector3.down * airStabilizeForce * 0.5f);
        }
    }

    void HandleFlipRecovery()
    {
        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);

        // 轻微倾斜时施加恢复扭矩
        if (tiltAngle > tiltThreshold && tiltAngle < flipThreshold)
        {
            Vector3 torque = CalculateRightingTorque();
            rb.AddTorque(torque * rightingTorque);
        }

        // 完全翻转时处理
        if (tiltAngle > flipThreshold)
        {
            flipTimer += Time.fixedDeltaTime;

            // 施加更强的恢复力
            Vector3 torque = CalculateRightingTorque();
            rb.AddTorque(torque * rightingTorque * 2.0f);

            // 如果翻转时间过长，强制恢复
            if (flipTimer > flipRecoveryTime)
            {
                ForceUpright();
            }
        }
        else
        {
            flipTimer = 0f;
        }
    }

    Vector3 CalculateRightingTorque()
    {
        // 计算使坦克恢复直立的扭矩
        // 使用叉积找出旋转轴
        Vector3 currentUp = transform.up;
        Vector3 targetUp = Vector3.up;

        // 如果坦克完全倒置，需要特殊处理
        if (Vector3.Dot(currentUp, targetUp) < -0.5f)
        {
            targetUp = -targetUp;
        }

        Vector3 rotationAxis = Vector3.Cross(currentUp, targetUp);
        float rotationAngle = Vector3.Angle(currentUp, targetUp) * Mathf.Deg2Rad;

        return rotationAxis * rotationAngle;
    }

    void ForceUpright()
    {
        // 强制将坦克恢复直立
        // 1. 停止所有旋转
        rb.angularVelocity = Vector3.zero;

        // 2. 设置正确朝向
        // 保持当前前进方向，只修正上下方向
        Vector3 currentForward = transform.forward;
        Vector3 newUp = Vector3.up;

        // 确保前进方向与新的上方垂直
        Vector3 newForward = Vector3.ProjectOnPlane(currentForward, newUp).normalized;
        if (newForward.magnitude < 0.1f)
        {
            // 如果当前前进方向几乎垂直，使用默认前方
            newForward = Vector3.forward;
        }

        transform.rotation = Quaternion.LookRotation(newForward, newUp);

        // 3. 重置计时器
        flipTimer = 0f;

        Debug.Log("坦克已强制恢复直立!");
    }

    // 可视化调试
    void OnDrawGizmosSelected()
    {
        // 绘制地面检测射线
        if (Application.isPlaying)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Vector3[] rayOrigins = {
                transform.position,
                transform.position + transform.forward * 0.5f,
                transform.position - transform.forward * 0.5f,
                transform.position + transform.right * 0.5f,
                transform.position - transform.right * 0.5f
            };

            foreach (Vector3 origin in rayOrigins)
            {
                Gizmos.DrawRay(origin, -transform.up * 1.2f);
            }

            // 绘制台阶检测射线
            Gizmos.color = Color.blue;
            Vector3 stepRayOrigin = transform.position + transform.up * 0.1f;
            Gizmos.DrawRay(stepRayOrigin, transform.forward * stepCheckDistance);
        }
    }

    // 公共方法，可从其他脚本调用
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetAirTime()
    {
        return airTime;
    }

    public float GetTiltAngle()
    {
        return Vector3.Angle(transform.up, Vector3.up);
    }
}