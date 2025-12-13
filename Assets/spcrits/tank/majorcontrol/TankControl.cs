using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TankControl : MonoBehaviour // Renamed class to follow PascalCase naming convention
{
    [Header("移动参数")]
    [Tooltip("最大前进速度（米/秒）")]
    public float maxForwardSpeed = 14f;       // 前进速度上限
    [Tooltip("最大后退速度（米/秒）")]
    public float maxBackwardSpeed = 8f;       // 后退
    [Tooltip("加速度（米/秒²）")]
    public float acceleration = 5f;          // 加速度
    [Tooltip("减速度（米/秒²）")]
    public float deceleration = 15f;          // 减速/停止时的减速度

    [Header("旋转参数")]
    [Tooltip("最大旋转角速度（度/秒）")]
    public float maxAngularSpeed = 120f;      // 转向角速度上限
    [Tooltip("旋转角加速度（度/秒²）")]
    public float angularAcceleration = 200f;  // 转向加速时的角加速度
    [Tooltip("旋转角减速度（度/秒²）")]
    public float angularDeceleration = 300f;  // 减速时

    public float explosionRadius = 10f;   // 爆炸范围
    public float explosionSpeed = 20f; // 爆炸推送速度

    private float _currentSpeed;       // 当前移动速度（米/秒）
    private float _currentAngularSpeed;// 当前旋转角速度（度/秒）

    public hatchcontrol hatch;
    public Light leftlight;
    public Light rightlight;
    public float maxhealth = 1000f;
    public  float health = 10000f;
    public float fixhelth = 4000f;
    public float fixwaiting = 10f;
    public float lastfixtime = 0f;
    public ParticleSystem brokeeffect1;
    public ParticleSystem brokeeffect2;
    public ParticleSystem brokeeffect3;
    private bool iswarning = false;
    private bool isdead = false;




    private void Update()
    {
        if (isdead) return;
        if (health <= 0f)
        {
            Broken();
            return;
        }
        warning();
        lightcontrol();
        repair();
        HandleMovement();
        HandleRotation();
        ApplyMovement();
        ApplyRotation();
        //Debug.Log("当前重力: " + Physics.gravity);

    }

    private void warning()
    {
        if (health <=maxhealth *0.3)
        {
            GetComponent<sound>().warning();
        }
        else if(iswarning)
        {
            GetComponent<sound>().stopwarning();
        }
    }
    private void lightcontrol()
    {
        float horizontalInput = InputRecorder.Instance.HorizontalAxis;
        if (horizontalInput > 0f) { leftlight.enabled = true; rightlight.enabled = false; }
        if (horizontalInput < 0f) { leftlight.enabled = false; rightlight.enabled = true; }
        if(horizontalInput == 0f) { leftlight.enabled = false;rightlight.enabled = false; }
    }

    private void repair()
    {
        bool threein = InputRecorder.Instance.three;
        if (isdead) return;
        else if(Time .time-lastfixtime >=fixwaiting &&threein ==true)
        {
            health += fixhelth;
            if (health > maxhealth) health = maxhealth;
            this.GetComponent<sound>().fix();
            lastfixtime = Time.time;
        }
    }
    private void Broken() // Renamed method to follow PascalCase naming convention
    {
        isdead = true;
        hatch.isdead = true;
        brokeeffect1.gameObject.SetActive(true);
        brokeeffect2.gameObject.SetActive(true);
        brokeeffect3.gameObject.SetActive(true);
        GetComponent<sound>().Diee();
    }

    public void BeHit(float damage) // Renamed method to follow PascalCase naming convention
    {
        health -= damage;
    }


    private void HandleMovement()
    {
        float verticalInput = InputRecorder.Instance.VerticalAxis;
        float targetSpeed = 0f;
        if (verticalInput > 0.1f)
        {
            targetSpeed = verticalInput * maxForwardSpeed;  
        }
        else if (verticalInput < -0.1f)
        {
            targetSpeed = verticalInput * maxBackwardSpeed; 
        }

        if (Mathf.Abs(targetSpeed) > Mathf.Abs(_currentSpeed))
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        float horizontalInput = InputRecorder.Instance.HorizontalAxis;
        float targetAngularSpeed = horizontalInput * maxAngularSpeed;

        if (Mathf.Abs(targetAngularSpeed) > Mathf.Abs(_currentAngularSpeed))
        {
            _currentAngularSpeed = Mathf.MoveTowards(_currentAngularSpeed, targetAngularSpeed,
                                                   angularAcceleration * Time.deltaTime);
        }
        else
        {
            _currentAngularSpeed = Mathf.MoveTowards(_currentAngularSpeed, targetAngularSpeed,
                                                   angularDeceleration * Time.deltaTime);
        }
    }

    private void ApplyMovement()
    {
        transform.Translate(Vector3.forward * (_currentSpeed * Time.deltaTime));
    }

    private void ApplyRotation()
    {
        transform.Rotate(Vector3.up * (_currentAngularSpeed * Time.deltaTime));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}