using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumeslidercontrol : MonoBehaviour
{
    [Header("音频混合器")]
    public AudioMixer mainMixer; // 拖入创建的MainMixer

    [Header("音乐滑条")]
    public Slider musicSlider; // 拖入MusicSlider
    [Header("音效滑条")]
    public Slider sfxSlider;   // 拖入SFXSlider

    void Start()
    {
        // 初始化滑条值（默认最大音量）
        musicSlider.value = 1;
        sfxSlider.value = 1;

        // 绑定滑条值变化事件
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // 初始设置一次音量（确保启动时生效）
        SetMusicVolume(1);
        SetSFXVolume(1);
    }

    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Lerp(-80, 0, value);
        mainMixer.SetFloat("BGMVOL", volume);
    }

    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Lerp(-80, 0, value);
        mainMixer.SetFloat("SFXVOL", volume);
    }
}
