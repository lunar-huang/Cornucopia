using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance { get; private set; }

    public Tilemap interactableMap;

    public Tile interactableTile;
    public Tile groundHoedTile;
    //private ItemData CropData;

    // 存储种植状态
    private Dictionary<Vector3Int, PlantedSeedData> plantedSeeds = new Dictionary<Vector3Int, PlantedSeedData>();
    private Dictionary<Vector3Int, GameObject> seedObjects = new Dictionary<Vector3Int, GameObject>(); // 存储种子图标的对象

    // 内部类：仅供 PlantManager 使用
    private class PlantedSeedData
    {
        public ItemData seedData; // 种子数据
        public float plantedTime; // 种植时间
        public int currentStage; // 当前的生长阶段

        public PlantedSeedData(ItemData seedData, float plantedTime)
        {
            this.seedData = seedData;
            this.plantedTime = plantedTime;
            this.currentStage = 0; // 初始阶段
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitInteractableMap();
    }

    void InitInteractableMap()
    {
        
        foreach(Vector3Int position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null)
            {
                interactableMap.SetTile(position, interactableTile);
            }
        }
    }

    public void HoeGround(Vector3 position)
    {
        Vector3Int tilePosition = interactableMap.WorldToCell(position);
        TileBase tile = interactableMap.GetTile(tilePosition);

        if(tile!=null && tile.name== interactableTile.name)
        {
            interactableMap.SetTile(tilePosition, groundHoedTile);
        }
    }

    // 示例方法：种植种子
    public void PlantSeed(Vector3 position, ItemData seedData)
    {
        Vector3Int tilePosition = interactableMap.WorldToCell(position);
        TileBase tile = interactableMap.GetTile(tilePosition);

        // 检查地块是否符合种植条件
        if (tile == null || tile != groundHoedTile) // 地块未锄过
        {
            Debug.LogWarning("无法种植！目标地块未锄过。");
            return;
        }

        if (plantedSeeds.ContainsKey(tilePosition)) // 地块已种植
        {
            Debug.LogWarning("无法种植！目标地块已种植其他种子。");
            return;
        }

        // 动态生成种子图标
        GameObject seedObject = new GameObject($"Seed_{tilePosition}");
        SpriteRenderer renderer = seedObject.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "PlantInfo";
        renderer.sprite = seedData.growthSprites[0]; // 初始阶段的图标
        renderer.sortingOrder = 5; // 显示在地块之上

        seedObject.transform.position = interactableMap.GetCellCenterWorld(tilePosition);
        seedObject.transform.localScale = new Vector3(2, 2, 1); // 调整大小

        // 记录种植状态
        plantedSeeds[tilePosition] = new PlantedSeedData(seedData, Time.time);
        seedObjects[tilePosition] = seedObject;

        Debug.Log($"种植了种子：{seedData.subType} 在位置：{tilePosition},使用图标：{renderer.sprite.name}");

        // **减少种子数量**
        ToolbarUI toolbarUI = Player.Instance.toolbarUI; // 获取玩家的工具栏
        toolbarUI.RemoveOneFromSelectedSlot();

    }

    private void Update()
    {
        foreach (var kvp in plantedSeeds)
        {
            Vector3Int tilePosition = kvp.Key;
            PlantedSeedData seedData = kvp.Value;

            // 计算当前阶段
            float elapsedTime = Time.time - seedData.plantedTime;
            int totalStages = seedData.seedData.growthSprites.Length;
            float timePerStage = seedData.seedData.totalGrowthTime / totalStages;

            int newStage = Mathf.Min((int)(elapsedTime / timePerStage), totalStages - 1);
            if (newStage != seedData.currentStage)
            {
                // 更新当前阶段
                seedData.currentStage = newStage;
                seedObjects[tilePosition].GetComponent<SpriteRenderer>().sprite = seedData.seedData.growthSprites[newStage];

                // 在 y 轴上向上偏移 0.3
                if (seedData.currentStage == 1){
                    Vector3 currentPosition = seedObjects[tilePosition].transform.position;
                    seedObjects[tilePosition].transform.position = new Vector3(currentPosition.x, currentPosition.y + 0.3f, currentPosition.z);
                }
                
                Debug.Log($"地块 {tilePosition} 更新到生长阶段：{newStage}");
            }
        }
    }

    // public void Harvest(Vector3 position)
    // {
    //     Vector3Int tilePosition = interactableMap.WorldToCell(position);

    //     // 检查地块是否有种植的植物
    //     if (!plantedSeeds.ContainsKey(tilePosition))
    //     {
    //         Debug.LogWarning("无法收获！该地块没有种植任何植物。");
    //         return;
    //     }

    //     // 获取种植数据
    //     PlantedSeedData seedData = plantedSeeds[tilePosition];

    //     // 检查植物是否成熟
    //     int totalStages = seedData.seedData.growthSprites.Length;
    //     if (seedData.currentStage != totalStages - 1) // 检查是否达到最后阶段
    //     {
    //         Debug.LogWarning("植物未成熟，无法收获！");
    //         return;
    //     }

    //     // 收获植物
    //     Debug.Log($"收获植物：{seedData.seedData.subType} 在位置：{tilePosition}");
        
    //     CropData.subType = seedData.seedData.subType;
    //     CropData.type = ItemType.Crop;

    //     // 将成熟作物添加到玩家背包
    //     Debug.Log($"trying to harvest type:{CropData.type}, subtype:{CropData.subType}");
    //     InventoryManager.Instance.AddToBackpack(CropData);

    //     if (!seedObjects.ContainsKey(tilePosition))
    //     {
    //         Debug.LogWarning("无法收获！该地块的种子对象不存在。");
    //         return;
    //     }
    //     // 删除种植的植物对象
    //     Destroy(seedObjects[tilePosition]);

    //     // 恢复地块为锄地状态
    //     interactableMap.SetTile(tilePosition, groundHoedTile);

    //     // 移除种植数据
    //     plantedSeeds.Remove(tilePosition);
    //     seedObjects.Remove(tilePosition);
    // }

    public void Harvest(Vector3 position)
    {
        Vector3Int tilePosition = interactableMap.WorldToCell(position);

        if (!plantedSeeds.ContainsKey(tilePosition))
        {
            Debug.LogWarning("无法收获！该地块没有种植任何植物。");
            return;
        }

        PlantedSeedData seedData = plantedSeeds[tilePosition];

        int totalStages = seedData.seedData.growthSprites.Length;
        if (seedData.currentStage != totalStages - 1)
        {
            Debug.LogWarning("植物未成熟，无法收获！");
            return;
        }

        Debug.Log($"收获植物：{seedData.seedData.subType} 在位置：{tilePosition}");

        // 根据种子类型找到对应的作物数据
        ItemData cropData = Resources.Load<ItemData>($"Data/Crop_{seedData.seedData.subType}");
        if (cropData == null)
        {
            Debug.LogError($"未找到对应的作物数据：Crop_{seedData.seedData.subType}");
            return;
        }

        // 添加到玩家背包
        Debug.Log($"trying to harvest type:{cropData.type}, subtype:{cropData.subType}");
        InventoryManager.Instance.AddToBackpack(cropData);

        if (!seedObjects.ContainsKey(tilePosition))
        {
            Debug.LogWarning("无法收获！该地块的种子对象不存在。");
            return;
        }

        Destroy(seedObjects[tilePosition]);
        interactableMap.SetTile(tilePosition, groundHoedTile);

        plantedSeeds.Remove(tilePosition);
        seedObjects.Remove(tilePosition);
    }


}