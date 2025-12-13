using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum ZombieState
{
    Idle,
    Chase,
    Attack,
    Hurt,
    Burn,
    Dead,
    walk
}

public class zombiecontrol : MonoBehaviour
{
    [Header("核心配置")]
    public GameObject target;
    public float runSpeed = 6f;
    public float attackSpeed = 2f;
    public float attackDis = 6f;
    public float attackPower = 30f;
    public float findDis = 70f;
    public float health = 300f;
    public float clearTime = 10f;
    public float hitTime = 0.8f;

    public bool initiativechase = false;

    [Header("特效")]
    public ParticleSystem burningParti;
    public Light burningLight;

    [Header("音效配置")]
    public AudioClip gSound; [Range(0f, 1f)] public float gVolume = 0.4f;
    public AudioClip attackSound; [Range(0f, 1f)] public float attackVolume = 0.5f;
    public AudioClip beHitSound; [Range(0f, 1f)] public float beHitVolume = 0.8f;
    public AudioClip deadSound; [Range(0f, 1f)] public float deadSoundVolume = 1f;

    // 原有变量保留
    private NavMeshAgent _navAgent;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _targetDis;
    private float _lastAttackTime;
    private float _lastBurningTime;
    private float _lastHitTime;
    private bool _isBurning;
    public bool isDead = false;
    private float _wanderTimer;
    private const float _wanderRadius = 5f;
    private const float _minWanderInterval = 3f;
    private const float _maxWanderInterval = 8f;

    // 状态队列/优先级变量
    private Dictionary<ZombieState, int> _statePriority;
    private List<ZombieState> _stateQueue;
    private int _currentMaxPriority;

    //外部的传入临时参数
    private float _currentHurtPower; // 存储BeHit传入的伤害值
    private bool _isBurnEffectInit;
    private bool ishurt;// 标记燃烧特效是否已初始化
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.loop = true;
        _audioSource.volume = gVolume;
        _audioSource.clip = gSound;
        _audioSource.Play();

        if (_navAgent != null) _navAgent.speed = runSpeed;

        if (target == null)
        {
            target = GameObject.FindWithTag("Palyer");
        }

        // 初始化状态优先级和队列
        InitStatePriority();
        _stateQueue = new List<ZombieState>();
        AddStateToQueue(ZombieState.Idle);

        _currentHurtPower = 0f;
        _isBurnEffectInit = false;
    }//初始化

    void Update()
    {
        if (isDead || _stateQueue.Contains(ZombieState.Dead)) return;

        if (health <= 0f && !isDead)
        {
            StartCoroutine(Break(clearTime));
            AddStateToQueue(ZombieState.Dead);
            return;
        }

        UpdateStateQueueByCondition();
        ExecuteHighPriorityStates();
    }//帧更新逻辑

    #region 状态队列管理
    private void InitStatePriority()//各状态优先级
    {
        _statePriority = new Dictionary<ZombieState, int>()
        {
            { ZombieState.Dead, 10 },
            { ZombieState.Hurt, 9 },
            { ZombieState.Burn, 9 },
            { ZombieState.Attack, 9 },
            { ZombieState.Chase, 9 },
            { ZombieState.walk, 8 },
            { ZombieState.Idle, 7 }
        };
    }

    private void AddStateToQueue(ZombieState state)
    {
        if (isDead) return;

        if (state == ZombieState.Dead)
        {
            _stateQueue.Clear();
            _stateQueue.Add(ZombieState.Dead);
            return;
        }

        if (!_stateQueue.Contains(state))
        {
            _stateQueue.Add(state);
        }
    }

    private void UpdateStateQueueByCondition()
    {
        _targetDis = Vector3.Distance(transform.position, target.transform.position);
        _stateQueue.Clear();

        if (ishurt)
        {
            AddStateToQueue(ZombieState.Hurt);
        }
        else if (_isBurning)
        {
            AddStateToQueue(ZombieState.Burn);
        }
        else if (_targetDis <= attackDis)
        {
            AddStateToQueue(ZombieState.Attack);
        }
        else if ((_targetDis < findDis && _targetDis > attackDis) || initiativechase)//索敌范围判断
        {
            AddStateToQueue(ZombieState.Chase);
        }
        else
        {
            AddStateToQueue(ZombieState.Idle);
        }

        _stateQueue.Sort((a, b) => _statePriority[b].CompareTo(_statePriority[a]));
        //这行代码的作用是对 _stateQueue（僵尸状态队列）进行排序，
        //排序的依据是 _statePriority（状态优先级字典）中每个状态的优先级值。
        //排序结果是优先级高的状态排在队列的前面。
        //sort 貌似是list里的方法 默认降序
    }

    private void ExecuteHighPriorityStates()
    {
        if (_stateQueue.Count == 0) return;

        _currentMaxPriority = _statePriority[_stateQueue[0]];
        List<ZombieState> highPriorityStates = _stateQueue
            .Where(s => _statePriority[s] == _currentMaxPriority)
            .ToList();

        foreach (var state in highPriorityStates)
        {
            switch (state)
            {
                case ZombieState.Idle: HandleIdle(); break;
                case ZombieState.Chase: HandleChase(); break;
                case ZombieState.Attack: HandleAttack(); break;
                case ZombieState.Hurt: HandleHurt(); break;
                case ZombieState.Burn: HandleBurn(); break;
                case ZombieState.Dead: HandleDead(); return;
            }
        }
    }
    #endregion

    #region 状态核心
    private void HandleIdle()
    {
        _navAgent.isStopped = true;
        _animator.SetBool("find target", false);
    }

    private void HandleChase()
    {
        _navAgent.isStopped = false;
        _navAgent.SetDestination(target.transform.position);
        _animator.SetBool("find target", true);
        _animator.SetBool("desenough", false);
    }

    private void HandleAttack()
    {
        _navAgent.isStopped = true;
        if (Time.time - _lastAttackTime > attackSpeed)
        {
            _audioSource.PlayOneShot(attackSound, attackVolume);
            _animator.SetTrigger("desenough");
            try
            {
                target.GetComponent<TankControl>().BeHit(attackPower);
            }
            catch { }
            _lastAttackTime = Time.time;
        }
    }


    private void HandleHurt()
    {
        initiativechase = true;
        _navAgent.isStopped = true;

        if (_currentHurtPower > 0f && health > 0f)
        {
            health -= _currentHurtPower;
            _lastHitTime = Time.time; 
            initiativechase = true;   

            _animator?.SetTrigger("behit");
            _audioSource?.PlayOneShot(beHitSound, beHitVolume);

            _currentHurtPower = 0f;
        }

        ishurt = false;
    }

    private void HandleBurn()
    {
        // 1. 初始化燃烧特效（仅第一次进入状态时执行）
        if (!_isBurnEffectInit)
        {
            Instantiate(burningParti, transform.position, transform.rotation, transform);
            Instantiate(burningLight, transform.position, transform.rotation, transform);
            _isBurnEffectInit = true;
            _lastBurningTime = Time.time; 
        }

        if (Time.time - _lastBurningTime > 0.5f && !isDead)
        {
            _audioSource.PlayOneShot(beHitSound, beHitVolume);
            health -= 70f;
            _lastBurningTime = Time.time;
        }
    }

    private void HandleDead()
    {
        _navAgent.isStopped = true;
        _animator.SetBool($"dead{Random.Range(1, 6)}", true);
    }
    #endregion

    #region 外部调用
    public void Burn()
    {
        if (isDead) return;
        _isBurning = true; 
        AddStateToQueue(ZombieState.Burn);
    }

    public void BeHit(float power)
    {
        if (isDead) return;
        _currentHurtPower = power;
        ishurt = true;// 存储伤害值（交给HandleHurt处理）
        AddStateToQueue(ZombieState.Hurt); 
    }

    private IEnumerator Break(float t)
    {
        gamemanager.Instance.addgrade(Random.Range(5, 30));

        string deadTag = "dead" + Random.Range(1, 6);
        if (deadTag.Length != 5) deadTag = "dead1";
        _animator.SetBool(deadTag, true);

        _audioSource.clip = deadSound;
        _audioSource.loop = false;
        _audioSource.volume = deadSoundVolume;
        _audioSource.Play();

        isDead = true;
        _navAgent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        counter.instance.addgzkill();

        yield return new WaitForSeconds(t);
        gamemanager.Instance.zombienum-=1;
        LightObjectPool.ReturnObject(this.gameObject);
    }

    #endregion
}