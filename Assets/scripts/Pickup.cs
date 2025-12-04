using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("limit", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerscript.timermanager)
        {
            limit();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag =="Player")
        {
            Debug.Log("アイテムがプレイヤーに触れた");
            Inventory inventory = col.gameObject.GetComponent<Inventory>();//プレイヤーのインベントリ情報をコンポーネントして取得

            if (inventory != null)
            {
                dropscript data = GetComponent<dropscript>();

                if (data != null && !inventory.HasItem(data.itemId))//アイテム側からインベントリ内に名前があるか参照
                {
                    Debug.Log("アイテム獲得");
                    inventory.AddItem(data.itemId);
                    Destroy(gameObject); // 拾ったら消す
                }
                else
                {
                    Debug.Log("すでに所持しているアイテムです。拾えません。");
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("アタッチされてません");
            }
        }

    }


void limit()
    {
        Destroy(gameObject);
    }

}
