using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitch : MonoBehaviour
{
    public Image img1; // 第一张图
    public TextMeshProUGUI TextMeshPro1;
    public Image img2; // 第二张图
    public TextMeshProUGUI TextMeshPro2;
    public int flag = 1; // 切换开关

    public hatchcontrol hatchcontrol;

    void Start()
    {
        UpdateDisplay();
    }

    void Update()
    {
        if (hatchcontrol != null)
        {
            flag = hatchcontrol.weaponstyle;
            UpdateDisplay();
        }
        else
        {
            Debug.LogError("hatchcontrol is not assigned!");
        }
    }

    void UpdateDisplay()
    {
        if (flag == 0)
        {
            SetActiveState(img1, TextMeshPro1, true);
            SetActiveState(img2, TextMeshPro2, false);
        }
        else
        {
            SetActiveState(img1, TextMeshPro1, false);
            SetActiveState(img2, TextMeshPro2, true);
        }
    }

    void SetActiveState(Image image, TextMeshProUGUI text, bool state)
    {
        if (image != null) image.gameObject.SetActive(state);
        if (text != null) text.gameObject.SetActive(state);
    }
}