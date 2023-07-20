using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public Text killCountText;
    public Text gameTimeText;

    public int killCount { get; set; } = 0;
    private float gameTime = 0f;

    private void Update()
    {
        UpdateKillCountText();
        UpdateGameTimeText();
    }

    public void UpdateKillCount()
    {
        killCount++;
    }

    private void UpdateKillCountText()
    {
        killCountText.text = "Kills: " + killCount.ToString();
    }

    private void UpdateGameTimeText()
    {
        gameTime += Time.deltaTime;
        gameTimeText.text = "Time: " + FormatTime(gameTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
