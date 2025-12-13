using System.Net;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-100)]
public class DisPlayControl : MonoBehaviour
{
    public static DisPlayControl Instance { get; private set; }
    [Header("核心引用")]
    public Camera mainCamera;
    public Transform player;
    public hatchcontrol hatchcontrol;
    public GameObject mainMenuUI;
    public GameObject settingsui;
    public GameObject playerUI;
    public GameObject settlementui;
    public maincameracontrol maincameracontrol; // 第三人称控制脚本

    [Header("部署 相机移动配置")]
    public float offsetX = 0f;
    public float offsetY = 10f;
    public float offsetZ = 0f;
    public float moveSpeed = 2f;
    public float rotmoveSpeed = 8f;// 相机移动速度
    private Vector3 menuCamPos;        // 主菜单相机位置
    private bool isdeploy = false;

    [Header("结算 相机移动配置")]
    public float soffsetX = 0f;
    public float soffsetY = 10f;
    public float soffsetZ = 0f;
    public float smoveSpeed = 2f;
    public float srotmoveSpeed = 8f;
    private Vector3 settleCamPos;
    private bool issettle = false;

    void Start()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        //
        menuCamPos = mainCamera.transform.position;
        maincameracontrol = mainCamera.GetComponent<maincameracontrol>();
        //
        mainMenuUI.SetActive(true);
        playerUI.SetActive(false);
        settlementui.SetActive(false);
        settingsui.SetActive(false);
        //
        maincameracontrol.enabled = false;
        hatchcontrol.enabled = false;
    }

    void Update()
    {
        if (isdeploy) deploy();
        if (issettle) settle();
        else settleCamPos = mainCamera.transform.position;
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        playerUI.SetActive(true);
        TankUIManager.instance.ShowMissionDemonstration(0, 5);
        isdeploy = true;
        issettle = false;
    }

    public void overgame()
    {
        issettle = true;
    }
   
    
    private void deploy()
    {
        if (isdeploy)
        {
            // 用三个float拼接偏移量，计算目标位置
            Vector3 targetPos = player.position + new Vector3(offsetX, offsetY, offsetZ);
            // 平滑移动位置
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, moveSpeed * Time.deltaTime);

            // 视角渐变看向玩家
            Quaternion targetRot = Quaternion.LookRotation(player.position - mainCamera.transform.position);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRot, rotmoveSpeed * Time.deltaTime);

            // 位置+旋转到位后启用控制
            float posDis = Vector3.Distance(mainCamera.transform.position, targetPos);
            float rotDis = Quaternion.Angle(mainCamera.transform.rotation, targetRot);
            if (posDis < 5f && rotDis < 10f)
            {
                isdeploy = false;
                maincameracontrol.enabled = true;
                hatchcontrol.enabled = true;
            }
        }
    }

    private void settle()
    {
        maincameracontrol.enabled=false;
        hatchcontrol.enabled = false;
        playerUI.gameObject.SetActive(false);
        settlementui.gameObject.SetActive(true);

        Vector3 stargetPos = settleCamPos + new Vector3(soffsetX, soffsetY, soffsetZ);
        Vector3 currentForward = mainCamera.transform.forward;
        Vector3 horizontalForward = new Vector3(currentForward.x, 0, currentForward.z).normalized;
        Quaternion targetRot = Quaternion.LookRotation(horizontalForward);

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position, 
            stargetPos, 
            smoveSpeed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Lerp(
            mainCamera.transform.rotation,
            targetRot,
            srotmoveSpeed * Time.deltaTime
        );
    }

    public void showsettingsui()
    {
        settingsui.gameObject.SetActive(true);
        playerUI.gameObject.SetActive(false);
        mainMenuUI.gameObject.SetActive(false);
    }

    public void hidesettingui()
    {
        settingsui.gameObject.SetActive(false);
        //
        gamemanager.GameState state = gamemanager.Instance.currentState;
        switch (state) {
            case gamemanager.GameState.Idle:
                mainMenuUI.gameObject.SetActive(true);break;
            case gamemanager.GameState.Paused:
                playerUI.gameObject.SetActive(true); gamemanager.Instance.ResumeGame(); break;
        }
    }

}
