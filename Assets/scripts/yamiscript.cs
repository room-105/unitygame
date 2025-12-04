using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yamiscript: MonoBehaviour
{
    public float enemyspeed = 2;
    private float yamihp = 30;
    public GameObject player;
    public float attackdamage　= 10f;
    public float bulletdamage = 5f;
    public GameObject[] dropitem;
    int drlentgh;

    // Start is called before the first frame update
    void Start()
    {
       player = GameObject.Find("player");
       yamihp = 30;
       drlentgh = dropitem.Length;//配列の長さ
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyspeed * Time.deltaTime);
        if (playerscript.timermanager == false)//ゲーム終了と同時に敵消す
        {
            Destroy(this.gameObject,0.1f);
        }

        if(yamihp <= 0) 
        {
            Die();
        }
    }

    void Die() 
    {
        Destroy(this.gameObject);
        Debug.Log("敵倒したよ");
        Drop();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "attack") 
        {
            yamihp -= attackdamage;
            Debug.Log("ダメージ　近接");
        }

        if (col.gameObject.tag == "bullet")
        {
            yamihp -= bulletdamage;
            playerscript.attackcurrentcount++;//必殺ゲージ溜める
            Debug.Log("ダメージ　遠距離闇");
        }
    }

    void Drop()
    {
        if (drlentgh == 0) return;
        int l = Random.Range(0, drlentgh);
        GameObject selectitem = dropitem[l];
        dropscript dr = selectitem.GetComponent<dropscript>();

        if (dr != null)
        {
            float prob = dr.dropRate;

            if (Random.value < prob)
            {
                Instantiate(selectitem, this.transform.position, Quaternion.identity);
                Debug.Log("落とした");
            }
            else
            {
                Debug.Log("落ちませんでした");
            }

        }
        else
        {
            Debug.Log("アタッチされてません");
            Debug.Log(selectitem);
        }
    }


}
