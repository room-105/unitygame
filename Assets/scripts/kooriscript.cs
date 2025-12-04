using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class kooriscript : MonoBehaviour
{
    public float enemyspeed = 2;
    public GameObject player;
    private float koorihp = 30;
    public float attackdamage = 10f;
    public float bulletdamage = 5f;
    public float dropprobility = 0.2f;
    public GameObject[] dropitem;//プレハブ化したアイテム
    void Start()
    {
        player = GameObject.Find("player");
        koorihp = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyspeed * Time.deltaTime);//追いかける
        
        if (playerscript.timermanager == false)
        {
            Destroy(this.gameObject, 0.1f);
        }
        if (koorihp <= 0)
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
            koorihp -= attackdamage;
            Debug.Log("ダメージ　近接");
        }
        if(col.gameObject.tag == "bullet")
        {
            koorihp -= bulletdamage;
            playerscript.attackcurrentcount++;//必殺ゲージ溜まる
            Debug.Log("ダメージ遠距離氷");
        }
    }
    void Drop()
    {
        if (Random.value < dropprobility)
        {
            int l = Random.Range(0, dropitem.Length);
            Instantiate(dropitem[l], this.transform.position, Quaternion.identity);
        }
    }

}
