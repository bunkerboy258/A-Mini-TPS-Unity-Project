using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainmenucontrol : MonoBehaviour
{
    public static mainmenucontrol instance;
    public AudioClip clicksound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    public Button startbutton;
    public Button settingbuttion;
    public Button exitbutton;
    [System.Serializable]
    public class UIElementPosition
    {
        public RectTransform element;
        public float xOffset;
        public float yOffset;
        public bool updateInRealTime = true;
    }

    public UIElementPosition[] uiElements;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        if (instance != null) Destroy(gameObject);
        else instance = this;
        UpdateUIPositions();
        startbutton.onClick.AddListener(OnStartBtnClick1);
        settingbuttion.onClick.AddListener(OnStartBtnClick2);
        exitbutton.onClick.AddListener(OnStartBtnClick3);
    }

    private void OnEnable()
    {
        UpdateUIPositions();
    }
    public void UpdateUIPositions()
    {
        foreach (var uiElement in uiElements)
        {
            if (uiElement.element != null && uiElement.updateInRealTime)
            {
                float fixedX = Screen.width * uiElement.xOffset;
                float fixedY = Screen.height * uiElement.yOffset;
                uiElement.element.anchoredPosition = new Vector2(fixedX, fixedY);
            }
        }
    }

    private void OnStartBtnClick1()
    {
        audioSource.PlayOneShot(clicksound);
        gamemanager.Instance.StartGame();
    }

    private void OnStartBtnClick2()
    {
        audioSource.PlayOneShot(clicksound);
        DisPlayControl.Instance.showsettingsui();
    }

    private void OnStartBtnClick3()
    {
        Application.Quit();
    }
}
