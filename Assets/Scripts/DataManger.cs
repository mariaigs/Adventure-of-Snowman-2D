using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private string saveFilePath;
    public PlayerData playerData = new PlayerData();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Application.persistentDataPath + "/scores.json";
            LoadHighScores();

            Debug.Log("DataManager initialized at " + saveFilePath);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void SavePlayerScore(string playerName, int score)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = PlayerPrefs.GetString("CurrentPlayerName", "Player");
            Debug.Log($"Using player name from PlayerPrefs: {playerName}");
        }

        Debug.Log($"Saving score for player: {playerName} = {score}");
        PlayerScore newScore = new PlayerScore(playerName, score);
        playerData.highScores.Add(newScore);
        Debug.Log($"Added new score for {playerName}: {score}");

        SaveHighScores();
    }


    
    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("High scores saved to: " + saveFilePath);
        Debug.Log("Total scores saved: " + playerData.highScores.Count);
    }

    // Load all high scores from the file
    private void LoadHighScores()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("High scores loaded from: " + saveFilePath);
            Debug.Log("Total scores loaded: " + playerData.highScores.Count);
        }
        else
        {
            Debug.Log("No high score file found. Creating a new one.");
            playerData = new PlayerData();
        }
    }

    // Get the list of high scores
    public List<PlayerScore> GetHighScores()
    {
        playerData.highScores.Sort((a, b) => b.score.CompareTo(a.score));

        List<PlayerScore> topScores = new List<PlayerScore>();
        int count = Mathf.Min(5, playerData.highScores.Count);

        for (int i = 0; i < count; i++)
        {
            topScores.Add(playerData.highScores[i]);
        }

        Debug.Log($"Returning top {count} high scores, sorted by score descending");
        foreach (var score in topScores)
        {
            Debug.Log($"Top Score: {score.playerName} - {score.score}");
        }

        return topScores;
    }
}