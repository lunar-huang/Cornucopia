using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI TimeDisplay; // 绑定 TextMeshPro 对象
    public float timeMultiplier = 1f; // 时间流逝倍率
    private float currentTime = 360; // 初始时间（早上 6:00）

    void Update()
    {
        currentTime += Time.deltaTime * timeMultiplier; // 时间推进
        UpdateTimeDisplay();
    }

    void UpdateTimeDisplay()
    {
        int hours = Mathf.FloorToInt(currentTime / 60) % 24; // 计算小时
        int minutes = Mathf.FloorToInt(currentTime % 60); // 计算分钟

        // 格式化时间为 `<sprite>` 格式
        string timeString = $"<sprite={hours / 10}><sprite={hours % 10}> <sprite={minutes / 10}><sprite={minutes % 10}>";
        TimeDisplay.text = timeString; // 更新到 TextMeshPro 文本
    }
}