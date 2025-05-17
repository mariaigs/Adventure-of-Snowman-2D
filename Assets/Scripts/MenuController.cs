
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MenuController : MonoBehaviour
{
    public Button newGameButton;
    public Button saveGameButton;
    public Button highScoresButton;
    public Button endGameButton;

    public TMP_InputField playerNameInput;

    private bool isPauseMenu = false;



    private void Start()
    {
        if (SceneManager.sceneCount > 1)
        {
            isPauseMenu = true;

            if (playerNameInput != null && ScoreManager.instance != null)
            {
                playerNameInput.text = ScoreManager.instance.playerName;
                Debug.Log("Pause menu: Using current player name: " + ScoreManager.instance.playerName);
            }
        }
        else
        {
            
            if (playerNameInput != null)
            {
                playerNameInput.text = "";
                Debug.Log("Initial menu: Clearing player name input field");
            }
        }

        SetupButtons();
    }

private void SetupButtons()
    {

        if (playerNameInput != null)
        {
            playerNameInput.onEndEdit.AddListener((value) =>
            {
                if (!string.IsNullOrEmpty(value))
                {
                    // Save player name to PlayerPrefs
                    PlayerPrefs.SetString("CurrentPlayerName", value);
                    PlayerPrefs.Save();

                    // Also update in ScoreManager if it exists
                    if (ScoreManager.instance != null)
                    {
                        ScoreManager.instance.SetPlayerName(value);
                        Debug.Log("Player name changed to: " + value);
                    }
                }
            });
        }

        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(() =>
            {

                if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
                {

                    if (ScoreManager.instance != null)
                    {
                        PlayerPrefs.SetInt("ReturnedFromSave", 0);
                        ScoreManager.instance.SetPlayerName(playerNameInput.text);
                        ScoreManager.instance.ResetScore();
                        Debug.Log("Starting new game with player: " + playerNameInput.text);
                    }
                    else
                    {
                       
                        PlayerPrefs.SetString("CurrentPlayerName", playerNameInput.text);
                        PlayerPrefs.SetInt("CurrentGameScore", 0);
                        PlayerPrefs.Save();
                    }
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("FirstScene");
                }
                else
                {
                    Debug.Log("Please enter a player name");

                }
            });
        }

        if (highScoresButton != null)
        {
            highScoresButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("HighScores");
            });
        }

        if (saveGameButton != null)
        {
            saveGameButton.onClick.AddListener(() =>
            {
                string playerName = "Player";
                if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
                {
                    playerName = playerNameInput.text;

                    
                    if (ScoreManager.instance != null)
                    {
                        ScoreManager.instance.SetPlayerName(playerName);
                    }
                }

                if (ScoreManager.instance != null )
                {
                    int currentScore = ScoreManager.instance.GetScore();

                    PlayerPrefs.SetInt("CurrentGameScore", currentScore);
                    PlayerPrefs.SetString("CurrentPlayerName", playerName);
                   

                    PlayerPrefs.SetInt("ReturnedFromSave", 1);
                    PlayerPrefs.Save();


                    Debug.Log("Game saved successfully! Player: " + playerName + ", Score: " + currentScore);
                }
                else
                {
                  
                    PlayerPrefs.SetInt("ReturnedFromSave", 1);
                    PlayerPrefs.SetString("CurrentPlayerName", playerName);
                    PlayerPrefs.Save();
                }



                SceneManager.LoadScene("FirstScene");
            });

        }


            if (endGameButton != null)
            {
                endGameButton.onClick.AddListener(() =>
                {
                    if (ScoreManager.instance != null && DataManager.instance != null)
                    {
                        DataManager.instance.SavePlayerScore(
                            ScoreManager.instance.playerName,
                            ScoreManager.instance.GetScore()
                        );
                    }

                    Debug.Log("Quitting game");
                    Application.Quit();
                });
            }
        }
    }


