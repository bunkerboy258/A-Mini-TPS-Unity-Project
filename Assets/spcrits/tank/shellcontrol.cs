using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shellcontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip audioclip;
    [Range(0f, 1f)] public float explosionVolume = 0.7f;
    public ParticleSystem particlesystem;
    public float timelimit = 5f;
    public float explosionpower = 50f;
    public float explosionRadius = 5f;
    public float explosionpush = 1000f;
    private AudioSource audiosource;
    private float activetime;private float timedis;
    private bool isexploded = false;
    private Rigidbody rb;
    void Start()
    {
        activetime = Time.time;
        rb = this.GetComponent <Rigidbody>();
        audiosource =this.GetComponent<AudioSource>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isexploded) return;
        timedis = Time.time - activetime;
        if (timedis >= timelimit) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) return;
        else explode();
    }
    private void explode()
    {
        if (isexploded) return;
        isexploded = true;
        if(rb!=null ) rb.velocity = rb.velocity * 0.1f;
        if (audioclip !=null) audiosource.PlayOneShot(audioclip, explosionVolume);
        Instantiate(particlesystem, transform.position, Quaternion.identity);
        Collider[] hitobjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider item in hitobjects)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionpushdir = item.transform.position - this.transform.position;
                explosionpushdir = explosionpushdir.normalized;
                item.GetComponent<Rigidbody>().AddForce(explosionpushdir * explosionpush, ForceMode.Impulse );
            }
            if (item.tag == "zombie")
            {
                zombiecontrol zc = item.GetComponent<zombiecontrol>();
                if (zc!=null&&zc.isDead != true)
                {
                    zc.BeHit(explosionpower);
                }
            }
        }
    }
}
