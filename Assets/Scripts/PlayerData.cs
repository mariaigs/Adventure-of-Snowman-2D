using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;


    public PlayerScore(string name, int score)
    {
        this.playerName = string.IsNullOrEmpty(name) ? "Player" : name;
        this.score = score;

        Debug.Log($"Created new PlayerScore: {this.playerName} - {this.score}");
    }
}

[Serializable]
public class PlayerData
{
    public List<PlayerScore> highScores = new List<PlayerScore>();
}

