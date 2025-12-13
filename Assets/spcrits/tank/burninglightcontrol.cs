using UnityEngine;

public class LightFadeControl : MonoBehaviour
{
    [Header("光效参数")]
    public float maxIntensity = 1.5f;
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 1.5f;
    public float holdDuration = 0.8f;

    private Light _light;
    private float _currentInt;
    private int _state; // 0=淡入 1=保持 2=淡出 3=结束
    private float _timer;

    void Start()
    {
        _light = GetComponent<Light>();
        if (!_light) { Debug.LogError("无Light组件！"); enabled = false; return; }
        _light.intensity = 0;
    }

    void Update()
    {
        if (_state == 0) { _currentInt += Time.deltaTime * fadeInSpeed; if (_currentInt >= maxIntensity) { _currentInt = maxIntensity; _state = 1; } }
        else if (_state == 1) { _timer += Time.deltaTime; if (_timer >= holdDuration) { _timer = 0; _state = 2; } }
        else if (_state == 2) { _currentInt -= Time.deltaTime * fadeOutSpeed; if (_currentInt <= 0) { _currentInt = 0; _state = 3; } }
        else if (_state == 3) _light.enabled = false;
        _light.intensity = _currentInt;
    }
}