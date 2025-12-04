using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossenemyscript : MonoBehaviour
{
    public float enemyspeed = 3;
    private float bossenemyhp = 30;
    public GameObject player;
    public float attackdamage = 10f;
    public float bulletdamage = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        bossenemyhp = 30;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyspeed * Time.deltaTime);
        if (playerscript.bosstimermanager == false)//ÉQÅ[ÉÄèIóπÇ∆ìØéûÇ…ìGè¡Ç∑
        {
            Destroy(this.gameObject, 0.1f);
        }

        if (bossenemyhp <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(this.gameObject);
        Debug.Log("ìGì|ÇµÇΩÇÊ");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "attack")
        {
            bossenemyhp -= attackdamage;
        }

        if (col.gameObject.tag == "bullet")
        {
            bossenemyhp -= bulletdamage;
            playerscript.attackcurrentcount++;//ïKéEÉQÅ[ÉWó≠ÇﬂÇÈ
        }
    }
}
