using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> ownedItemIds = new List<string>();

    // 所持済みかどうかを確認する関数
    public bool HasItem(string itemId)
    {
        return ownedItemIds.Contains(itemId);
    }

    // 新しいアイテムを追加（重複してなければ）
    public void AddItem(string itemId)
    {
        if (!HasItem(itemId))
        {
            ownedItemIds.Add(itemId);
            Debug.Log($"アイテム {itemId} を獲得！");
        }
        else
        {
            Debug.Log($"アイテム {itemId} はすでに所持済み！");
        }
    }
}



