using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class resuicontrol : MonoBehaviour
{
    private Button thisbutton;

    public GameObject resDropdownPanel; 
    public TextMeshProUGUI resShowText;
    public string currentresstr="1920*1080";
    public AudioClip clicksound;
    public AudioSource audiosource;

    void Start()
    {
        //
        audiosource = this.GetComponent<AudioSource>();
        thisbutton = GetComponent<Button>();
        thisbutton.onClick.AddListener(OnStartBtnClick);
        //
        resDropdownPanel.SetActive(false);
        updateres(currentresstr);
    }

   public void updateres(string resstr)
    {
        string[] res = resstr.Split('*');
        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(width, height,false);
        resShowText.text = resstr;
        currentresstr = resstr;
        //
        settinguicontrol.instance.UpdateUIPositions();
    }

    private void OnStartBtnClick()
    {
        audiosource.PlayOneShot(clicksound);
        resDropdownPanel.gameObject.SetActive(true);
    }

}
