using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
 
    public static ScoreManager instance;

    public int currentScore = 0;
    public TextMeshProUGUI scoreText;

    public string playerName = "Player";

    private bool isNewGame = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            bool returningFromSave = PlayerPrefs.GetInt("ReturnedFromSave", 0) == 1;
            if (returningFromSave)
            {
                if (PlayerPrefs.HasKey("CurrentGameScore"))
                {
                currentScore = PlayerPrefs.GetInt("CurrentGameScore");
                Debug.Log("ScoreManager: Loaded score from PlayerPrefs: " + currentScore);
            }
            string savedName = PlayerPrefs.GetString("CurrentPlayerName", "");
            if (!string.IsNullOrEmpty(savedName))
            {
                playerName = savedName;
                Debug.Log("ScoreManager: Loaded player name: " + playerName);
            }
        }
            else
            {
                
                currentScore = 0;
                Debug.Log("ScoreManager: This is a new game, starting score at 0");

               
                string savedName = PlayerPrefs.GetString("CurrentPlayerName", "");
                if (!string.IsNullOrEmpty(savedName))
                {
                    playerName = savedName;
                    Debug.Log("ScoreManager: Loaded player name: " + playerName);
                }
            }
        }
        else
        {
            
            Debug.Log("ScoreManager: Duplicate instance found, destroying");
            if (scoreText != null)
            {
                instance.scoreText = scoreText;
                instance.UpdateScoreDisplay();
            }
            Destroy(gameObject);
            return;
        }
       
    }

    void Start()
    {

        UpdateScoreDisplay();
    }
   
    public void FindScoreText()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText")?.GetComponent<TextMeshProUGUI>();
        //UpdateScoreDisplay();
    }

    public void AddPoints(int points)
    {
        int oldScore = currentScore;
        currentScore += points;
        Debug.Log($"Score changed from {oldScore} to {currentScore} by adding {points} points");
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay()
    {
        if (scoreText == null)
        {
            FindScoreText();
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }
    public int GetScore()
    {
        return currentScore;
    }
    public void ResetScore()
    {
        currentScore = 0;
        isNewGame = true;
        PlayerPrefs.SetInt("CurrentGameScore", 0);
        PlayerPrefs.Save();
        UpdateScoreDisplay();
        Debug.Log("Score has been reset to 0 for new game");
    }
    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            playerName = name;

            PlayerPrefs.SetString("CurrentPlayerName", name);
            PlayerPrefs.Save();
            Debug.Log("Player name set to: " + playerName);
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt("CurrentGameScore", currentScore);
            PlayerPrefs.Save();
            Debug.Log("ScoreManager: Saved score on pause: " + currentScore);
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CurrentGameScore", currentScore);
        PlayerPrefs.Save();
        Debug.Log("ScoreManager: Saved score on quit: " + currentScore);
    }

    // Ensure score is saved when scenes change
    void OnDisable()
    {
        if (instance == this)
        {
            PlayerPrefs.SetInt("CurrentGameScore", currentScore);
            PlayerPrefs.Save();
            Debug.Log("ScoreManager: Saved score on disable: " + currentScore);
        }
    }


}


