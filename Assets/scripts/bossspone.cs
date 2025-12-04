using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossspone : MonoBehaviour
{
    public GameObject boss;
    public static bool bossfrag = false;//フラグでボスの出る数を1体にする
    AudioSource a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerscript.bosstimermanager && !bossfrag)
        {
            bossmethod();
        }
        if (!playerscript.bosstimermanager)//ステージ出たらフラグ復活
        {
            bossfrag = false;
        }
        if (playerscript.bosstimermanager)
        {
            a.volume = 0.3f;
        }
        else
        {
            a.volume = 0f;
        }
    }

    void bossmethod()
    {
        Instantiate(boss,this.transform.position,this.transform.rotation);
        bossfrag = true;
    }
}
