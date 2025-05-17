using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreUI : MonoBehaviour
{
    public Transform scoresPanel; 
    public GameObject scoreEntryPrefab;
    public Button backButton;

    

    void Start()
    {
        Debug.Log("HighScoreUI Start called");
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(() => {
                SceneManager.LoadScene("Menu");
            });
        }

        
        if (DataManager.instance != null)
        {
            DisplayHighScores();
        }
        else
        {
            Debug.LogError("DataManager is null, creating dummy scores");
            CreateDummyScores();
        }
    }

    void DisplayHighScores()
    {
        // Get scores from DataManager
        List<PlayerScore> scores = DataManager.instance.GetHighScores();
        Debug.Log($"Found {scores.Count} scores to display");

        
        foreach (Transform child in scoresPanel)
        {
            if (child.name.Contains("Clone"))
            {
                Destroy(child.gameObject);
            }
        }
       
      
        int displayCount = Mathf.Min(scores.Count, 5);
        for (int i = 0; i < displayCount; i++)
        {
            GameObject entry = Instantiate(scoreEntryPrefab);
            entry.transform.SetParent(scoresPanel, false);
            entry.SetActive(true);

            TextMeshProUGUI playerNameText = entry.transform.Find("PlayerTxt").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreText = entry.transform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();

           

            if (playerNameText != null && scoreText != null)
            {
               
                string name = string.IsNullOrEmpty(scores[i].playerName) ? "Player" : scores[i].playerName;
                playerNameText.text = $"{i + 1}. {name}";
                scoreText.text = scores[i].score.ToString();

                Debug.Log($"Set score entry {i + 1}: {playerNameText.text} - {scoreText.text}");
            }
            else
            {
                Debug.LogError("Could not find PlayerTxt or ScoreTxt components in the prefab");
            }
        }
    }

    void CreateDummyScores()
    {
        Debug.Log("Creating dummy scores");

      
        string[] names = { "Player1", "Player2", "Player3", "Player4", "Player5" };
        int[] scores = { 500, 400, 300, 200, 100 };

       
        foreach (Transform child in scoresPanel)
        {
            Destroy(child.gameObject);
        }

       
        for (int i = 0; i < 5; i++)
        {
            GameObject entry = Instantiate(scoreEntryPrefab, scoresPanel);

            
            Text[] texts = entry.GetComponentsInChildren<Text>();

            if (texts.Length >= 2)
            {
                texts[0].text = names[i];
                texts[1].text = scores[i].ToString();
            }
        }
    }
}