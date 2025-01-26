using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3;

    private Animator anim;

    private Vector2 direction = Vector2.zero;

    public ToolbarUI toolbarUI;
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this; // 创建全局单例
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAction();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        direction = new Vector2(x, y);

        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void HandleMovement()
    {
        if (direction.magnitude > 0)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("horizontal", direction.x);
            anim.SetFloat("vertical", direction.y);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void HandleAction()
    {
        // 检测鼠标左键点击，用于种植种子
        if (Input.GetMouseButtonDown(0))
        {
            ToolbarSlotUI selectedSlot = toolbarUI.GetSelectedSlotUI();
            if (selectedSlot != null)
            {
                ItemData selectedItem = selectedSlot.GetData().item;
                if (selectedItem.type == ItemType.Seed) // 检查是否选中了种子
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 playerPosition = transform.position;
                    mouseWorldPosition.z = 0; // 确保 2D 游戏 Z 坐标为 0
                    Debug.Log($"尝试种植：{selectedItem.subType}，位置：{playerPosition}");

                    PlantManager.Instance.PlantSeed(playerPosition, selectedItem); // 调用 PlantManager 种植方法
                }
                else
                {
                    Debug.Log("当前选中的物品不是种子，无法种植！");
                }
            }
            else
            {
                Debug.LogWarning("工具栏未选中任何物品，无法执行种植操作！");
            }
        }

        // 检测空格键锄地
        if (toolbarUI.GetSelectedSlotUI() != null)
        {
            ItemData selectedItem = toolbarUI.GetSelectedSlotUI().GetData().item;
            if (selectedItem.subType == SubType.Hoe && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Hoe Anim is triggered!");
                PlantManager.Instance.HoeGround(transform.position);
                anim.SetTrigger("hoe");
            }
        }

        if (Input.GetMouseButtonDown(1)) // 按right mouse尝试收获
        {
            Vector3 playerPosition = transform.position; // 玩家当前位置
            Debug.Log($"尝试Harvest位置：{playerPosition}");
            PlantManager.Instance.Harvest(playerPosition);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickable")) // 确保物品是可拾取的
        {
            Pickable pickable = collision.GetComponent<Pickable>();
            if (pickable != null)
            {
                ItemData itemData = pickable.itemData; // 获取完整的物品数据
                Debug.Log($"拾取物品：Type={itemData.type}, SubType={itemData.subType}");

                InventoryManager.Instance.AddToBackpack(itemData); // 将完整物品数据存入背包
                Destroy(collision.gameObject); // 删除地上的物品
            }
        }
    }
}
