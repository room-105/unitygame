using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sponescript : MonoBehaviour
{
    public GameObject yami3;
    public GameObject koori3;
    public float timer1 = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spone());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spone()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (playerscript.timermanager)
            {
                GameObject newkoori = Instantiate(koori3, this.transform.position + new Vector3(0, Random.Range(-20, 20), 0), this.transform.rotation) as GameObject;
                GameObject newyami = Instantiate(yami3, this.transform.position + new Vector3(0, Random.Range(-20, 20), 0), this.transform.rotation) as GameObject;
            }

            yield return new WaitForSeconds(timer1);
        }
    }
}
