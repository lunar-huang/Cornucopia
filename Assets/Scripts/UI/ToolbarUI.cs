using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarUI : MonoBehaviour
{
    public List<ToolbarSlotUI> slotuiList;

    private ToolbarSlotUI selectedSlotUI;//data type
    private SlotData slotData;

    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }

    private void Update()
    {
        ToolbarSelectControl();

    }
    public ToolbarSlotUI GetSelectedSlotUI()
    {
        return selectedSlotUI;
    }

    void InitUI()
    {
        slotuiList = new List<ToolbarSlotUI>(new ToolbarSlotUI[9]);
        ToolbarSlotUI[] slotuiArray = transform.GetComponentsInChildren<ToolbarSlotUI>();

        foreach (ToolbarSlotUI slotUI in slotuiArray)
        {
            slotuiList[slotUI.index] = slotUI;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        List<SlotData> slotdataList = InventoryManager.Instance.toolbarData.slotList;

        for (int i = 0; i < slotdataList.Count; i++)
        {
            Debug.Log($"slotNumber {i} Data inside: ItemType={slotdataList[i]?.item?.type ?? ItemType.None}, SubType={slotdataList[i]?.item?.subType ?? SubType.None}");
            slotuiList[i].SetData(slotdataList[i]);
        }
    }


    void ToolbarSelectControl()
    {
        for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                if (selectedSlotUI != null)
                {
                    selectedSlotUI.UnHighlight();
                }
                int index = i - (int)KeyCode.Alpha1;
                selectedSlotUI = slotuiList[index];
                selectedSlotUI.Highlight();
            }
        }
    }

    public void RemoveOneFromSelectedSlot()
    {
        if (selectedSlotUI != null && selectedSlotUI.GetData().count > 0)
        {
            selectedSlotUI.GetData().count -= 1; // 减少数量
            if (selectedSlotUI.GetData().count == 0)
            {
                slotData=selectedSlotUI.GetData();
                slotData.Clear(); // 如果数量为 0，清空槽位数据
            }
            UpdateUI(); // 更新 UI 显示
        }

    }

}
