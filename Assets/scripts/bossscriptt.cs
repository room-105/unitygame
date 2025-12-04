using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossscriptt : MonoBehaviour
{
    public float bosshp = 500f;
    public float bossspeed = 6f;
    public float attackdamage = 10f;
    public float bulletdamage = 5f;
    public static float bossattackdamage = 8f;//
    GameObject player;
    public GameObject bossattack;
    public GameObject bossenemy;
    public int bossattackcount = 6;
    public float bossattackfirstrag = 2f;
    public float bossattackrag = 2f;
    public float minX = -34f, maxX = 34f;
    public float minY = 67f, maxY = 93f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        StartCoroutine(GodJudgment());
        StartCoroutine(spone2());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, bossspeed * Time.deltaTime);

        if (!playerscript.bosstimermanager) 
        {
            Destroy(this.gameObject);
        }

        if (bosshp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);

        playerscript ps = player.GetComponent<playerscript>();

        // 関数を呼び出す
        if (ps != null)
        {
            ps.bosswin();
        }
        else
        {
            Debug.Log("プレイヤースクリプトが見つかりませんでした");
        }
        Debug.Log("敵倒したよ");
    }

    IEnumerator GodJudgment()
    {
        yield return new WaitForSeconds(bossattackfirstrag);
        while (true)
        {
            for (int i = 0; i < bossattackcount; i++)
            {
                Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                GameObject zone = Instantiate(bossattack, randomPos, Quaternion.identity);
                Destroy(zone, 3f); // 3秒後に消える（演出時間）
            }
            yield return new WaitForSeconds(bossattackrag);
        }
    }
    IEnumerator spone2()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (playerscript.bosstimermanager)
            {
               Instantiate(bossenemy, this.transform.position , this.transform.rotation) ;
            }
            yield return new WaitForSeconds(7f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "attack")
        {
            bosshp -= attackdamage;
            Debug.Log("ダメージ　近距離ボス");
        }
        if (col.gameObject.tag == "bullet")
        {
            bosshp -= bulletdamage;
            playerscript.attackcurrentcount++;//必殺ゲージ溜める
            Debug.Log("ダメージ　遠距離ボス");
        }
    }
}
