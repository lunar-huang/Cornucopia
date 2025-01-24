using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None=0,
    Tool=1,
    Seed=2,
    Crop=3   // 作物（已成熟）
}

public enum SubType
{
    None=0,
    Seed_Carrot=10,
    Seed_Tomato=20,
    Hoe=30
}

[CreateAssetMenu()]
public class ItemData :ScriptableObject
{
    public ItemType type=ItemType.None; // 大类（种子、工具、作物等）
    public SubType subType = SubType.None; // 具体种类（胡萝卜种子、锄头等）
    public Sprite sprite;
    public GameObject prefab;
    public int maxCount=1;
}
