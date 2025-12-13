using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.TerrainTools;

public class TankUIManager : MonoBehaviour
{
    // 单例实例
    public static TankUIManager instance;


    public Image tankcrosshair;
    public TextMeshProUGUI gradetext;
    // UI元素引用
    [Header("血量相关")]
    public Slider healthSlider;
    public TankControl tank;

    [Header("武器切换")]
    public Image weaponImg1;
    public TextMeshProUGUI weaponText1;
    public Image weaponImg2;
    public TextMeshProUGUI weaponText2;
    public hatchcontrol hatchControl;

    [Header("技能冷却")]
    public Image skillImg1;
    public Image skillImg2;

    [Header("任务提示")]
    public RectTransform missionPanel;
    public TextMeshProUGUI missiontext;
    public float missionDuration = 2f;

    private bool isshow=false;
    // 位置偏移参数
    [System.Serializable]
    public class UIElementPosition
    {
        public RectTransform element;
        public float xOffset;
        public float yOffset;
        public bool updateInRealTime = true;
    }

    public UIElementPosition[] uiElements;
    public string[] hints;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        // 初始化UI元素
        if (healthSlider != null && tank != null)
        {
            healthSlider.maxValue = tank.maxhealth;
            healthSlider.value = tank.maxhealth;
        }

        UpdateWeaponDisplay();
        UpdateSkillDisplay();
    }

    private void OnEnable()
    {
        UpdateUIPositions();
    }
    private void Update()
    { 
        // 更新血量
        if (healthSlider != null && tank != null)
        {
            healthSlider.value = tank.health;
        }

        // 更新武器显示
        if (hatchControl != null)
        {
            UpdateWeaponDisplay();
        }

        // 更新技能显示
        if (tank != null)
        {
            UpdateSkillDisplay();
        }

        Updategradedisplay();
    }

    public void UpdateUIPositions()
    {
        if (uiElements == null) return;

        foreach (var uiElement in uiElements)
        {
            if (uiElement.element != null && uiElement.updateInRealTime)
            {
                UpdateElementPosition(uiElement.element, uiElement.xOffset, uiElement.yOffset);
            }
        }
    }

    // 内部方法：设置单个元素位置
    private void UpdateElementPosition(RectTransform element, float xOffset, float yOffset)
    {
        float fixedX = Screen.width * xOffset;
        float fixedY = Screen.height * yOffset;
        element.anchoredPosition = new Vector2(fixedX, fixedY);
    }

    // 武器显示更新
    private void UpdateWeaponDisplay()
    {
        if (hatchControl == null) return;

        int flag = hatchControl.weaponstyle;

        if (flag == 0)
        {
            SetActiveState(weaponImg1, weaponText1, true);
            SetActiveState(weaponImg2, weaponText2, false);
        }
        else
        {
            SetActiveState(weaponImg1, weaponText1, false);
            SetActiveState(weaponImg2, weaponText2, true);
        }
    }

    // 技能显示更新
    private void UpdateSkillDisplay()
    {
        if (skillImg1 == null || skillImg2 == null || tank == null) return;

        int flag = (Time.time - tank.lastfixtime >= tank.fixwaiting) ? 1 : 0;

        if (flag == 1)
        {
            skillImg1.gameObject.SetActive(true);
            skillImg2.gameObject.SetActive(false);
        }
        else
        {
            skillImg1.gameObject.SetActive(false);
            skillImg2.gameObject.SetActive(true);
        }
    }

    private void Updategradedisplay()
    {
        gradetext.text = "point: "+gamemanager.Instance.grade.ToString();
    }

    // 设置元素激活状态
    private void SetActiveState(Image image, TextMeshProUGUI text, bool state)
    {
        if (image != null) image.gameObject.SetActive(state);
        if (text != null) text.gameObject.SetActive(state);
    }

    // 显示任务提示
    public void ShowMissionDemonstration(int dex,float duration = 2f)
    {
        if(!isshow) StartCoroutine(MissionDemonstrationCoroutine(dex, duration));
    }

    // 任务提示协程
    private IEnumerator MissionDemonstrationCoroutine(int dex, float duration)
    {
        if (missionPanel == null) yield break;
        missiontext.text = hints[dex].ToString();

        missionPanel.gameObject.SetActive(true);
        missiontext.gameObject.SetActive(true);

        isshow = true;

        yield return new WaitForSeconds(duration);

        missionPanel.gameObject.SetActive(false);
        missiontext.gameObject.SetActive(false);

        isshow = false;
    }

    // 公共方法：手动更新UI元素位置
    public void UpdateElementPositionImmediate(RectTransform element, float xOffset, float yOffset)
    {
        UpdateElementPosition(element, xOffset, yOffset);
    }
}