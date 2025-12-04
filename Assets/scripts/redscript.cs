using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redscript : MonoBehaviour
{
    public GameObject player;
    public GameObject yellow;
    public GameObject red;
    float distance;//プレイヤーとの距離を取得
    public float enemyattackbefore = 0.7f;
    public float enemyattacking = 0.1f;
    public float enemyattackafter = 0.4f;
    public static float enemyattack = 7f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        red = this.transform.Find("red_ougi").gameObject;//セットアクティブごとOFFにする
        yellow = this.transform.Find("kiiro_ougi").gameObject;
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator Attack()
    {
        while (true)
        {
            distance = (player.transform.position - this.gameObject.transform.position).magnitude;

            if (distance < 5)
            {
                yellow.SetActive(true);
                yield return new WaitForSeconds(enemyattackbefore); // 攻撃一瞬だけ有効
                Debug.Log("攻撃開始");
                yellow.SetActive(false);
                red.SetActive(true);
                yield return new WaitForSeconds(enemyattacking); // 攻撃一瞬だけ有効
                red.SetActive(false);
                yield return new WaitForSeconds(enemyattackafter);//攻撃のログ
            }
            else
            {
                red.SetActive(false);//衝突判定をOFF
                yellow .SetActive(false);
                yield return null;//わざわざクールタイムの時間をずらしてる理由は距離が4.99の時も0.4秒後にチェックするため最大0.4秒のラグが発生してしまう
            }           
        }


       
    }
}
