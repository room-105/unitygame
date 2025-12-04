using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sponescript3 : MonoBehaviour
{
    public GameObject yami3;
    public GameObject koori3;
    public float timer3 = 5;
    AudioSource a;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spone3());
        a = GetComponent<AudioSource>();
        a.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerscript.timermanager)
        {
            a.volume = 0.3f;
        }
        else
        {
            a.volume = 0f;
        }
    }

    IEnumerator spone3()
    {
        yield return new WaitForSeconds(4f);
        while (true)
        {
            if (playerscript.timermanager)
            {
                GameObject newkoori = Instantiate(koori3, this.transform.position + new Vector3(0,Random.Range(-20, 20), 0), this.transform.rotation) as GameObject;
                GameObject newyami = Instantiate(yami3, this.transform.position + new Vector3(0,Random.Range(-20, 20),0), this.transform.rotation) as GameObject;
            }

            yield return new WaitForSeconds(timer3);
        }
    }
}
