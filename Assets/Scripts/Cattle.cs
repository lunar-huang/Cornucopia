using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cattle : MonoBehaviour
{
    public ItemData itemData; // 完整的物品数据

    public ItemType GetItemType()
    {
        if (itemData != null)
        {
            return itemData.type; // 返回物品类型
        }
        Debug.LogWarning($"{gameObject.name} 未绑定 ItemData！");
        return ItemType.None;
    }

    public SubType GetSubType()
    {
        if (itemData != null)
        {
            return itemData.subType; // 返回物品子类型
        }
        Debug.LogWarning($"{gameObject.name} 未绑定 ItemData！");
        return SubType.None;
    }
}
