using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None=0,
    Tool=1,
    Seed=2,
    Crop=3,   // 作物（已成熟）
    Animal=4,
}

public enum SubType
{
    None=0,
    Carrot=10,
    Tomato=20,
    Hoe=30,
    Chicken=40,
    Cow=50,
}

[CreateAssetMenu()]
public class ItemData :ScriptableObject
{
    public ItemType type=ItemType.None; // 大类（种子、工具、作物等）
    public SubType subType = SubType.None; // 具体种类（胡萝卜种子、锄头等）
    public Sprite sprite;
    public GameObject prefab;
    public int maxCount=1;

    // 作物生长的相关字段
    public Sprite[] growthSprites; // 作物生长阶段的图片（数组，每阶段一张图）
    public float totalGrowthTime; // 总生长时间（秒）
}
