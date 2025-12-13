using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settinguicontrol : MonoBehaviour
{
    public static settinguicontrol instance;
    public Button exitbutton;
    public AudioClip clicksound;
    private AudioSource audioSource;

    void Start()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        audioSource = this.GetComponent<AudioSource>();
        gamemanager.Instance.ResumeGame();
        exitbutton.onClick.AddListener(exitsetting);
    }

    public void UpdateUIPositions()
    {
        float fixedX = Screen.width * 0.5f;
        float fixedY = Screen.height * 0.5f;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(fixedX, fixedY);
        StartCoroutine(lateupdateposition());
    }

    private void exitsetting()
    {
        audioSource.PlayOneShot(clicksound);
        DisPlayControl.Instance.hidesettingui();
    }

    private IEnumerator lateupdateposition()
    {
        yield return new WaitForSeconds(0.3f);
        float fixedX = Screen.width * 0.5f;
        float fixedY = Screen.height * 0.5f;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(fixedX, fixedY);
    }
}
