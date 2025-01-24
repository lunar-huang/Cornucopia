using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

//    private Dictionary<ItemType, ItemData> itemDataDict = new Dictionary<ItemType, ItemData>();
    private Dictionary<(ItemType, SubType), ItemData> itemDataDict = new Dictionary<(ItemType, SubType), ItemData>();

    [HideInInspector]
    public InventoryData backpack;
    [HideInInspector]
    public InventoryData toolbarData;

    private void Awake()
    {
        Instance = this;
        Init();
    }
    
    // private void Init()
    // {
    //     ItemData[] itemDataArray = Resources.LoadAll<ItemData>("Data");
    //     foreach(ItemData data in itemDataArray)
    //     {
    //         itemDataDict.Add(data.type, data);
    //     }
        
    //     backpack = Resources.Load<InventoryData>("Data/Backpack");
    //     toolbarData = Resources.Load<InventoryData>("Data/Toolbar");
    //     foreach (SlotData slotData in backpack.slotList)
    //     {
    //         Debug.Log($"背包槽位：ItemType={slotData?.item?.type ?? ItemType.None}, SubType={slotData?.item?.subType ?? SubType.None}");
    //     }
    // }

    private void Init()
    {
        ItemData[] itemDataArray = Resources.LoadAll<ItemData>("Data");
        foreach (ItemData data in itemDataArray)
        {
            var key = (data.type, data.subType); // 使用 ItemType 和 SubType 组合作为键
            if (!itemDataDict.ContainsKey(key))
            {
                itemDataDict.Add(key, data);
                Debug.Log($"Added item: Type={data.type}, SubType={data.subType}");
            }
            else
            {
                Debug.LogWarning($"Duplicate item detected: Type={data.type}, SubType={data.subType}");
            }
        }

        backpack = Resources.Load<InventoryData>("Data/Backpack");
        toolbarData = Resources.Load<InventoryData>("Data/Toolbar");
        foreach (SlotData slotData in backpack.slotList)
        {
            Debug.Log($"背包槽位：ItemType={slotData?.item?.type ?? ItemType.None}, SubType={slotData?.item?.subType ?? SubType.None}");
        }
    }

    // private ItemData GetItemData(ItemType type)
    // {
    //     ItemData data;
    //     bool isSuccess = itemDataDict.TryGetValue(type, out data);
    //     if (isSuccess)
    //     {
    //         return data;
    //     }
    //     else
    //     {
    //         Debug.LogWarning("你传递的type：" + type + "不存在，无法得到物品信息。");
    //         return null;
    //     }
    // }

    public ItemData GetItemData(ItemType type, SubType subType)
    {
        var key = (type, subType);
        if (itemDataDict.TryGetValue(key, out ItemData data))
        {
            return data;
        }
        Debug.LogWarning($"Item not found: Type={type}, SubType={subType}");
        return null;
    }


    // public void AddToBackpack(ItemType type)
    // {
    //     ItemData item = GetItemData(type);
    //     if (item == null) return;

    //     foreach(SlotData slotData in backpack.slotList)
    //     {
    //         if (slotData.item == item && slotData.CanAddItem())
    //         {
    //             slotData.Add();return;
    //         }
    //     }

    //     foreach (SlotData slotData in backpack.slotList)
    //     {
    //         if (slotData.count == 0)
    //         {
    //             slotData.AddItem(item);return;
    //         }
    //     }

    //     Debug.LogWarning("无法放入仓库，你的背包" + backpack + "已满。");
    // }


    public void AddToBackpack(ItemData itemData)
    {
        if (itemData == null) return;

        Debug.Log($"尝试存入背包的物品：Type={itemData.type}, SubType={itemData.subType}");

        // 遍历背包槽位，尝试堆叠物品
        foreach (SlotData slotData in backpack.slotList)
        {
            if (slotData.item == itemData && slotData.CanAddItem())
            {
                slotData.Add(); // 增加数量
                return;
            }
        }

        // 如果无法堆叠，寻找空槽位存放
        foreach (SlotData slotData in backpack.slotList)
        {
            if (slotData.count == 0)
            {
                slotData.AddItem(itemData); // 存入新物品
                return;
            }
        }

        Debug.LogWarning("背包已满，无法存放物品！");
    }


}
