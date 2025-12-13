using UnityEngine;

public class BurnZombieInRange : MonoBehaviour
{
    [Header("燃烧设置")]
    public float detectRadius = 5f; // 检测范围半径
    public float triggerInterval = 0.5f; // 调用间隔（避免频繁触发）
    private ParticleSystem _particle;
    private float _timer;

    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        if (!_particle) { Debug.LogError("无ParticleSystem组件！"); enabled = false; }
    }

    void Update()
    {
        if (!_particle.isPlaying) return; 
        _timer += Time.deltaTime;
        if (_timer >= triggerInterval)
        {
            _timer = 0;
            DetectAndBurnZombies();
        }
    }

    void DetectAndBurnZombies()
    {
        // 检测范围内所有碰撞体
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
        foreach (var col in colliders)
        {
            if (col.CompareTag("zombie"))
                col.GetComponent<zombiecontrol>().Burn();
        }
    }

}