using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance { get; private set; }

    private Dictionary<GameObject, AnimalState> animals = new Dictionary<GameObject, AnimalState>();

    private class AnimalState
    {
        public ItemData animalData;
        public float spawnTime; // 动物被生成的时间
        public int currentStage; // 当前成长阶段

        public AnimalState(ItemData data, float time)
        {
            this.animalData = data;
            this.spawnTime = time;
            this.currentStage = 0;
        }
    }

    private void Awake()
    {
        Instance = this;
        Debug.Log("Animal Awake.");
    }

    private void Start()
    {
        // 示例：初始化两只动物
        AddAnimal(new Vector3(-2, -10, 0), Resources.Load<ItemData>($"Data/Animal_Chicken"));
        Debug.Log("Initiate a cream chicken in your farm");
        AddAnimal(new Vector3(2, -5, 0), Resources.Load<ItemData>("Data/Animal_Cow"));
        Debug.Log("Initiate a silver cow in your farm");
    }

    public void AddAnimal(Vector3 position, ItemData animalData)
    {
        GameObject animal = new GameObject($"Animal_{animalData.subType}");
        
        SpriteRenderer renderer = animal.AddComponent<SpriteRenderer>();
        renderer.sprite = animalData.growthSprites[0]; // 设置初始阶段图标
        renderer.sortingLayerName = "PlantInfo";
        renderer.sortingOrder = 6;

        animal.transform.position = position;
        //animal.transform.localScale = new Vector3(2, 2, 1);
        animal.transform.localScale = new Vector3(4f, 4f, 1f);

        animals[animal] = new AnimalState(animalData, Time.time);
    }

    private void Update()
    {
        foreach (var kvp in animals)
        {
            GameObject animal = kvp.Key;
            AnimalState state = kvp.Value;

            float elapsedTime = Time.time - state.spawnTime;
            int totalStages = state.animalData.growthSprites.Length;
            float timePerStage = state.animalData.totalGrowthTime / totalStages;

            int newStage = Mathf.Min((int)(elapsedTime / timePerStage), totalStages - 1);
            if (newStage != state.currentStage)
            {
                state.currentStage = newStage;
                animal.GetComponent<SpriteRenderer>().sprite = state.animalData.growthSprites[newStage];
                Debug.Log($"{state.animalData.subType} 长到了阶段 {newStage}");
            }
        }
    }
}
