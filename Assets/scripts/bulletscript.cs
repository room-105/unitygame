using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletscript : MonoBehaviour
{
    public float bullettime =  3f;
    List<string> enemyname = new List<string> { "enemy" ,"boss","bossenemy"};
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject,bullettime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enemyname.Contains(col.gameObject.tag))//col.gameObject.tag == "enemy"||col.gameObject.tag=="boss"
        {
            Destroy(this.gameObject);
        }
    }


}
