using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class resbutton : MonoBehaviour
{
    public GameObject resDropdownPanel;
    public TextMeshProUGUI text;
    public resuicontrol resuicontrol;
    public AudioClip clicksound;
    private AudioSource audioSource;
    private Button thisbutton;


    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        thisbutton = GetComponent<Button>();
        thisbutton.onClick.AddListener(OnStartBtnClick);
    }
    private void OnStartBtnClick()
    {
        audioSource.PlayOneShot(clicksound);
        resDropdownPanel.SetActive(false);
        resuicontrol.updateres(text.text);
    }
}
