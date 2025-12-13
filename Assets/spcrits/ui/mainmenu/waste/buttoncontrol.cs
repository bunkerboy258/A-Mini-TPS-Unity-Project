using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttoncontrol: MonoBehaviour
{
    public float xscale;
    public float yscale;
    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 计算固定位置的屏幕坐标：x终点（右边缘） + y上四分点
        float fixedX = Screen.width *xscale; // 准星右边缘贴屏幕右边缘
        float fixedY = Screen.height * yscale ; // y轴上四分点

        // 赋值给准星位置（锚点是左下角，所以直接用屏幕坐标）
        rt.anchoredPosition = new Vector2(fixedX, fixedY);
    }
}