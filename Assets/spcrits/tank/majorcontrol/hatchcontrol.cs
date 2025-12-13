using System.Collections;
using System.Data;
using UnityEngine;
[DefaultExecutionOrder(-50)]
public class hatchcontrol : MonoBehaviour
{
    public Camera cam;
    public GameObject shell;
    public GameObject gunbarrel;
    private misslelaunercontrol misslecontrol;
    public float speed = 2f;
    public float shelldis = 1f;
    public float shootspeed = 0.2f;
    public float shellspeed = 70f;
    private float lastshoott = 0;
    public AudioClip shootsound;
    [Range(0f, 1f)] public float shootVolume = 0.7f;
    public Light firelight;
    public float lighttime = 0.1f;
    //
    public GameObject misslelauncher;
    public float missleshootspeed = 0.3f;
    private float lastmissletime = 0f;
    //
    private  Vector3 hatchdes;
    private  Vector3 shelldes;
    //
    private  Vector3 _smoothedLookDir;
    private AudioSource audioSource;
    //
    public AudioClip changesound;
    public float changevolume = 0.5f;
    public int weaponstyle = 0;
    private int lastweaponstyle = 0;//0:shell 1:missle
    public Vector3 rayEndPoint;
    //
    public bool isdead = false;


    private void Awake()
    {
        audioSource=this.GetComponent<AudioSource>();
        misslecontrol = misslelauncher.GetComponent<misslelaunercontrol >();
    }

    void FixedUpdate()
    {
        if (isdead) return;
        bool isShoot = InputRecorder.Instance.MouseLeft;
        bool onein = InputRecorder.Instance.one;
        bool twoin = InputRecorder.Instance.two;


        Ray centerray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, (Screen.height/4)*3));
        
        rayEndPoint = centerray.origin + centerray.direction * 50f;

        hatchdes = new Vector3(rayEndPoint.x, transform.position.y, rayEndPoint.z);
        Vector3 lookDir = hatchdes - transform.position;
        lookDir.y = 0; // 限制在XZ平面
        lookDir = lookDir.normalized; // 归一化方向


        // 平滑方向
        if (lookDir.sqrMagnitude > 0.001f)
        {
            _smoothedLookDir = Vector3.Lerp(_smoothedLookDir, lookDir, 0.15f);
        }

        // 执行旋转
        if (_smoothedLookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(_smoothedLookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                                                 speed * Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        //
        //
        //
        if (onein) weaponstyle = 0; 
        if (twoin) weaponstyle = 1; 
        if (lastweaponstyle == 0 && weaponstyle == 1) { misslecontrol.lastactivetime = Time.time; }
        if (weaponstyle == 1) misslecontrol.isactive = true;
        else misslecontrol.isactive = false;
        if (lastweaponstyle != weaponstyle) audioSource.PlayOneShot(changesound, changevolume);
        lastweaponstyle = weaponstyle;

        if (isShoot && weaponstyle == 0 && _smoothedLookDir.sqrMagnitude > 0.001f && Time.time - lastshoott >= shootspeed)
        {
            shoot(); lastshoott = Time.time;
        }
        if(isShoot && weaponstyle==1 && _smoothedLookDir.sqrMagnitude > 0.001f && Time.time - lastmissletime  >= missleshootspeed )
        {
            misslecontrol.shoot(rayEndPoint);lastmissletime = Time.time;
        }
    }
    IEnumerator ActivateLight(float duration)
    {
        firelight.enabled = true;
        yield return new WaitForSeconds(duration);
        firelight.enabled = false;
    }

    void shoot()
    {
        Vector3 spawnPos = gunbarrel.transform.position + gunbarrel.transform.up  * shelldis;
        GameObject shells = Instantiate(shell, spawnPos, gunbarrel.transform.rotation);
        shelldes = rayEndPoint - spawnPos;
        Rigidbody rbs = shells.GetComponent<Rigidbody>();
        if (rbs != null)
        {
            shelldes = shelldes.normalized;
            rbs.velocity = shelldes * shellspeed;
        }
        StartCoroutine(ActivateLight(lighttime));
        audioSource.PlayOneShot(shootsound, shootVolume);
    }
}