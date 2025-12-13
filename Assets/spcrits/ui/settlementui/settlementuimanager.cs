using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
//using static UnityEditor.Rendering.FilterWindow;

public class settlementuimanager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    [System.Serializable]
    public class UIElementPosition
    {
        public RectTransform element;
        public float xOffset;
        public float yOffset;
        public bool updateInRealTime = true;
    }

    public UIElementPosition[] uiElements;
    public void OnEnable()
    {
        Updatedatas();
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

    private void Updatedatas()
    {
        text2.text = "Point: "+gamemanager.Instance.grade.ToString();
        text3.text = "Killed: "+counter.instance.gzkillcount.ToString();
        text4.text="Rescue: "+counter.instance.rescuenum.ToString();
    }
}
