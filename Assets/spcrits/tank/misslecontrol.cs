using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misslecontrol1 : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip audioclip;
    [Range(0f, 1f)] public float explosionVolume = 1f;
    public ParticleSystem particlesystem;
    public ParticleSystem burnparticlesystem;
    public Light burnlight;
    public float timelimit = 5f;
    public float explosionpower = 360f;
    public float explosionRadius = 8f;
    public float explosionpush = 10000f;
    private AudioSource audiosource;
    private float activetime; private float timedis;
    private bool isexploded = false;
    private Rigidbody rbm;
    void Start()
    { 
        activetime = Time.time;
        rbm = this.GetComponent<Rigidbody>();
        audiosource = this.GetComponent<AudioSource>();
        rbm.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

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

    // Update is called once per frame
    private void explode()
    {
        if (isexploded) return;
        isexploded = true;
        rbm.velocity = rbm.velocity * 0.1f;
        if (audioclip != null) audiosource.PlayOneShot(audioclip, explosionVolume);
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
                if (zc != null && zc.isDead != true)
                {
                    zc.BeHit(explosionpower);
                }
            }
        }
        Instantiate(burnparticlesystem, this.transform.position, this.transform.rotation);
        Instantiate(burnlight, this.transform.position, this.transform.rotation);
    }
}
