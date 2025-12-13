using UnityEngine;
using UnityEngine.UI;

public class FixedPositionCrosshair : MonoBehaviour
{
    private RectTransform crosshairRect;
    //private float crosshairHalfWidth; // 准星宽度的一半（避免超出屏幕）

    void Start()
    {
        crosshairRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 计算固定位置的屏幕坐标：x终点（右边缘） + y上四分点
        float fixedX = Screen.width *0.5f; 
        float fixedY = Screen.height * 0.75f; // y轴上四分点

        // 赋值给准星位置（锚点是左下角，所以直接用屏幕坐标）
        crosshairRect.anchoredPosition = new Vector2(fixedX, fixedY);
    }
}