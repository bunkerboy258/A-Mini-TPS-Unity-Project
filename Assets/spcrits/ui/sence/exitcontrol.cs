using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class exitcontrol : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public GameObject target;
    private float dis;
    private bool isfound=false;
    private Coroutine showCoroutine; // ´æ´¢Ð­³ÌÊµÀý

    void Start()
    {
        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        showCoroutine = StartCoroutine(show());
    }

    private void Update()
    {
        if (isfound)
        {
            image.transform.LookAt(target.transform.position);
            text.transform.LookAt(target.transform.position);
            text.transform.Rotate(0, 180, 0);
        }
    }
    private IEnumerator show()
    {
        while (!isfound)
        {
                
            dis = Vector3.Distance(this.transform.position, target.transform.position);         
            if (dis < 200f)
            {
                image.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
                isfound = true;
            }
            yield return new WaitForSeconds(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(showCoroutine);
            DisPlayControl.Instance.overgame();
            gamemanager.Instance.restatrt();
            Destroy(gameObject);
        }
    }
}