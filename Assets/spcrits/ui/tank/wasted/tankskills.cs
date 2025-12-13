using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tankskills : MonoBehaviour
{
    public Image img1; // 第一张图
    public Image img2; // 第二张图
    public int flag = 0; // 切换开关 1:active
    //
    public TankControl tank;

    void Start()
    {
    }

    void Update()
    {
        if (Time.time - tank.lastfixtime >= tank.fixwaiting) flag = 1;
        else flag = 0;
        //
        if (flag == 1)
        {
            img1.gameObject.SetActive(true); 
            img2.gameObject.SetActive(false); 
        }
        if (flag == 0)
        {
            img2.gameObject.SetActive(true); 
            img1.gameObject.SetActive(false); 
        }
    }
}
