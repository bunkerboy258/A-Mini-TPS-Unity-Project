using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound : MonoBehaviour
{
    //public AudioClip shootsound;
    public AudioClip activesound;
    public AudioClip movingsound;
    public AudioClip warningsound;
    public AudioClip broke;
    public AudioClip fixsound;

    //各音效音量控制（0~1范围，可在Inspector调整）
    [Header("音量设置（0~1）")]
    [Range(0f, 1f)] public float moveVolume = 0.7f;     // 移动音量
    [Range(0f, 1f)] public float activeVolume = 0.7f;   // 激活音量（预留）
    [Range(0f, 1f)] public float warningVolume = 0.7f;
    [Range(0f, 1f)] public float brokeVolume = 0.7f;
    [Range(0f, 1f)] public float fixVolume = 0.7f;// 警告音量（预留）

    //private float lastactivetime = 0f;
    private AudioSource audioSource;
    private Rigidbody tankRigidbody; // 用于获取坦克速度
    public  float maxSpeed = 15f; // 速度最大值（对应音量最大值）
    public  float minVolume = 0.1f; // 初始最小音量（速度为0时）

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        // 获取坦克刚体组件（用于获取移动速度）
        tankRigidbody = GetComponent<Rigidbody>();

        // 初始就播放移动音效（以0.1音量开始）
        if (movingsound != null)
        {
            audioSource.clip = movingsound;
            audioSource.volume = minVolume;
            audioSource.Play();
        }
    }

    void Update()
    {
        bool act = InputRecorder.Instance.A || InputRecorder.Instance.S || InputRecorder.Instance.W || InputRecorder.Instance.D;
        bool fire = InputRecorder.Instance.MouseLeftDown;

        // 计算当前速度（取绝对值，限制在0-maxSpeed范围）
        Vector3  tankv = tankRigidbody.velocity;
        float v = new Vector3(tankv.x, 0, tankv.z).magnitude;
        //Debug.Log(v);

        // 速度0对应0.1音量，速度7对应预设moveVolume，线性渐变
        float speedRatio = v / maxSpeed; // 0-1范围
        audioSource.volume = Mathf.Lerp(minVolume, moveVolume, speedRatio);

    }

    public void warning()
    {
        audioSource.clip = warningsound;
        audioSource.volume = warningVolume;
    }

    public void fix()
    {
        audioSource.PlayOneShot(fixsound, fixVolume);
    }
    public void stopwarning()
    {
        audioSource.clip = movingsound;
        audioSource.volume = moveVolume;
    }
    public void Diee()
    {
        audioSource.PlayOneShot(broke, brokeVolume);
    }
    // 新增：激活音效播放（带自定义音量，原代码未实现，预留扩展）
    public void PlayActiveSound()
    {
        if (activesound != null)
        {
            audioSource.PlayOneShot(activesound, activeVolume);
        }
    }

    // 新增：警告音效播放（带自定义音量，原代码未实现，预留扩展）
    public void PlayWarningSound()
    {
        if (warningsound != null)
        {
            audioSource.PlayOneShot(warningsound, warningVolume);
        }
    }
}