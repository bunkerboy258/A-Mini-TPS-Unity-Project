using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class survival : MonoBehaviour
{
    public Image singal;
    public Image enablesingal;
    public Image talkingimage;
    public TextMeshProUGUI timetext;
    public TextMeshProUGUI talkingtext;
    public GameObject target;
    public float timelimits=300f;
    public float talkingdis=40f;
    public float changetexttime=2f;
    public string[] strings;
    public float rescuedis = 6f;
    public bool istiming = false;
    public AudioClip ressound;
    private AudioSource AudioSource;
    private float dis;
    private float lasttime;
    private float lastchangetexttime=0f;
    private bool isdead = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = this.GetComponent<AudioSource>();
        singal.gameObject.SetActive(true);
        timetext.gameObject.SetActive(true);
        enablesingal.gameObject.SetActive(false);
        talkingimage.gameObject.SetActive(false);
        lasttime = timelimits;
    }

    // Update is called once per frame
    void Update()
    {
        if (isdead) return;
        timetext.text = lasttime.ToString();
        if (lasttime<=0) handledead();
        else
        {
            singal.transform.LookAt(target.transform.position);
            enablesingal.transform.LookAt(target.transform.position);
            singal.transform.Rotate(0, 180, 0);
            enablesingal.transform.Rotate(0, 180, 0);
            dis = Vector3.Distance(this.transform.position, target.transform.position);
            //
            if (dis <= rescuedis) handlerescue();
            if (dis <= talkingdis) handletalking();
            else handlesleep();
        }
    }

    private void FixedUpdate()
    {
        if (isdead) return;
        if(istiming) lasttime -= Time.deltaTime;
        if (lasttime < 0) lasttime = 0;
    }

    private void handlerescue()
    {
        if (isdead) return;
        TankUIManager.instance.ShowMissionDemonstration(1, 5);
        bool ein = InputRecorder.Instance.E;
        if (ein)
        {
            AudioSource.PlayOneShot(ressound,1f);
            gamemanager.Instance.addgrade(10000);
            counter.instance.addrescue();
            handledead();
        }
    }

    private void handledead()
    {
        isdead = true;
        singal.gameObject.SetActive(false);
        enablesingal.gameObject.SetActive(true);
        talkingimage.gameObject.SetActive(false);
    }

    private void handletalking()
    {
        if (isdead) return;
        talkingimage.gameObject.SetActive(true);
        talkingimage.transform.LookAt(target.transform.position);
        talkingimage.transform.Rotate(0, 180, 0);
        if (Time.time - lastchangetexttime >= changetexttime)
        {
            int n = strings.Length;
            int dex = Random.Range(0, n);
            talkingtext.text = strings[dex];
            lastchangetexttime= Time.time;
        }
    }

    private void handlesleep()
    {
        singal.gameObject.SetActive(true);
        timetext.gameObject.SetActive(true);
        enablesingal.gameObject.SetActive(false);
        talkingimage.gameObject.SetActive(false);
    }
}
