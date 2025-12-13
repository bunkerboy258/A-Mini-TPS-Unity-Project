using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misslelaunercontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isactive = false;
    public float lastactivetime = 0f;
    public float risetime = 0.5f;
    //
    public GameObject missle;
    public float spwandis = 0.1f;
    public float riseheight = 0.3f;
    public float risespeed = 3f;
    public float maxmisslev = 200f;
    public float misslev = 200f;
    public float misslefillspeed = 10f;
    public float shootspeed = 100f;
    public AudioClip shootsound;
    public float shootvolume = 1.0f;
    public Light shootlight;
    public float lighttime = 0.2f;
    public ParticleSystem particlesystem;
    //
    private AudioSource audiosource;
    private Vector3 startpos;
    private Vector3 despos;


    void Start()
    {
        startpos = this.transform.localPosition;
        despos = startpos + new Vector3(0, riseheight, 0);
        audiosource = this.GetComponent<AudioSource>();
    }
     
    // Update is called once per frame
    void FixedUpdate()
    {
        if(misslev < maxmisslev)
        {
            misslev += misslefillspeed * Time.fixedDeltaTime;
            if (misslev > maxmisslev ) misslev = maxmisslev;
        }
        if(isactive)
        {
                transform.localPosition = Vector3.Lerp(transform.localPosition, despos, Time.fixedDeltaTime  * risespeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startpos, Time.fixedDeltaTime * risespeed);
        }

    }

   public void shoot(Vector3 missledes)
    {
        if (Time .time-lastactivetime >0.5f&&misslev>=100f)
        {
            Vector3 spwanpos = this.transform.position + this.transform.forward  * spwandis;
            GameObject misslex= Instantiate(missle, spwanpos, this.transform.rotation);
            Rigidbody rbm = misslex.GetComponent<Rigidbody>();
            if (rbm != null)
            {
                Vector3 shootdir = missledes - spwanpos;
                rbm.velocity = shootdir.normalized * shootspeed;
            }
            Instantiate(particlesystem, spwanpos, Quaternion.identity);
            audiosource.PlayOneShot(shootsound, shootvolume);
            StartCoroutine(activelight(lighttime));
            misslev -= 100f;
        }
    }

    IEnumerator activelight(float t)
    {
        shootlight.enabled = true;
        yield return new WaitForSeconds(t);
        shootlight.enabled = false;
    }
}
