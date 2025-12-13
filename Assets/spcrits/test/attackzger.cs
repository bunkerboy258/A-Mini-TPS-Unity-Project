using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackzger : MonoBehaviour
{
    private float generateintervaltime = 0.2f;
    public int generatenummax = 30;
    public float generatedis = 100f;
    private int generatenum = 0;
    private float lastgeneratetime = 0f;
    private float currentdis = 0f;
    public GameObject[] zombiePrefabs; // 拖入7种僵尸预制体
    public int numzombiestyle = 7;
    public GameObject target;
    public AudioClip[] roarClips, attackClips, behitClips, deadClips;
    private int currentzombienum;

    void Start()
    {
        numzombiestyle = zombiePrefabs.Length;
        StartCoroutine(disdetection());
    }

    private void FixedUpdate()
    {
        currentzombienum = gamemanager.Instance.zombienum;
        if (currentdis <= generatedis&&currentzombienum<100)
        {
            if (generatenum >= generatenummax)
            {
                Destroy(this.gameObject);
                return;
            }
            if (Time.time - lastgeneratetime >= generateintervaltime && zombiePrefabs.Length > 0)
            {
                int randZombie = Random.Range(0, zombiePrefabs.Length);
                string zombieName = "gz" + Time.time; // 修复变量命名冲突
                //GameObject zombieInstance = Instantiate(zombiePrefabs[randZombie], transform.position, Quaternion.identity);
                GameObject zombieInstance = LightObjectPool.GetObject(zombiePrefabs[randZombie], transform.position, Quaternion.identity);
                generatenum += 1;
                gamemanager.Instance.zombienum += 1;// 修复变量命名冲突
                zombieInstance.name = zombieName; // 设置实例的名称
                zombiecontrol zc = zombieInstance.GetComponent<zombiecontrol>();
                if (zc != null)
                {
                    zc.gSound = roarClips[Random.Range(1, roarClips.Length)];
                    zc.attackSound = attackClips[Random.Range(1, attackClips.Length)];
                    zc.beHitSound = behitClips[Random.Range(1, behitClips.Length)];
                    zc.deadSound = deadClips[Random.Range(1, deadClips.Length)];
                    zc.target = target;
                    zc.initiativechase = true;
                }
                lastgeneratetime = Time.time;
            }
        }
    }

    private IEnumerator disdetection()
    {
        while (true)
        {
            currentdis = Vector3.Distance(this.transform.position, target.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
}
