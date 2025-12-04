using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sponescript2 : MonoBehaviour
{
    public GameObject yami;
    public GameObject koori;
    public float timer2 = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spone2());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spone2()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            if (playerscript.timermanager)
            {
                GameObject newkoori = Instantiate(koori, this.transform.position + new Vector3(Random.Range(-50, 50), 0, 0), this.transform.rotation) as GameObject;
                GameObject newyami = Instantiate(yami, this.transform.position + new Vector3(Random.Range(-50, 50), 0, 0), this.transform.rotation) as GameObject;
            }

            yield return new WaitForSeconds(timer2);
        }
    }
}
