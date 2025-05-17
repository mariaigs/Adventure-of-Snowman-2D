using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlGame : MonoBehaviour
{
    public GameObject PauseBtn;
    public GameObject RestartBtn;
    public GameObject ResumeBtn;

   

    void Start()
    {

        bool returningFromSave = PlayerPrefs.GetInt("ReturnedFromSave", 0) == 1;

        if (returningFromSave)
        {
            
            PlayerPrefs.SetInt("ReturnedFromSave", 0);
            PlayerPrefs.Save();

           
            Time.timeScale = 0f;
            PauseBtn.SetActive(false);
            ResumeBtn.SetActive(true);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && PlayerPrefs.HasKey("PlayerPosX"))
            {
                float x = PlayerPrefs.GetFloat("PlayerPosX");
                float y = PlayerPrefs.GetFloat("PlayerPosY");
                float z = PlayerPrefs.GetFloat("PlayerPosZ");
                player.transform.position = new Vector3(x, y, z);

               
                if (PlayerPrefs.HasKey("PlayerScaleX"))
                {
                    Vector3 scale = player.transform.localScale;
                    scale.x = PlayerPrefs.GetFloat("PlayerScaleX");
                    player.transform.localScale = scale;
                }
            }

            if (ScoreManager.instance != null && PlayerPrefs.HasKey("CurrentGameScore"))
            {
                int savedScore = PlayerPrefs.GetInt("CurrentGameScore", 0);
                ScoreManager.instance.currentScore = savedScore;
                ScoreManager.instance.UpdateScoreDisplay();
                Debug.Log("Restored score: " + savedScore);
            }
        }
        else
        {
            
            Time.timeScale = 1f;
            PauseBtn.SetActive(true);
            ResumeBtn.SetActive(false);
        }

        RestartBtn.SetActive(true);
    }

    
    public void Pause()
    {
        Debug.Log("Pause clicked, loading menu scene");

        if (ScoreManager.instance != null)
        {
            int currentScore = ScoreManager.instance.GetScore();
            string playerName = ScoreManager.instance.playerName;

            PlayerPrefs.SetInt("CurrentGameScore", currentScore);
            PlayerPrefs.SetString("CurrentPlayerName", playerName);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
                PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
                PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);
            
                PlayerPrefs.SetFloat("PlayerScaleX", player.transform.localScale.x);
                PlayerPrefs.Save();
            }

            PlayerPrefs.Save();

            if (DataManager.instance != null && currentScore > 0)
            {
                DataManager.instance.SavePlayerScore(playerName, currentScore);
                Debug.Log($"Saved current score to high scores: {playerName} - {currentScore}");
            }
        }
       

        Time.timeScale = 0f;
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }
    public void Resume()
    {
        Debug.Log("Resume button clicked");
       


        ResumeBtn.SetActive(false);
        PauseBtn.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && PlayerPrefs.HasKey("PlayerPosX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            player.transform.position = new Vector3(x, y, z);

           
            if (PlayerPrefs.HasKey("PlayerScaleX"))
            {
                Vector3 scale = player.transform.localScale;
                scale.x = PlayerPrefs.GetFloat("PlayerScaleX");
                player.transform.localScale = scale;
            }
        }

        if (BalloonManager.instance != null)
        {
            BalloonManager.instance.ResumeGame();
        }
        else
        {
            Debug.LogError("No BalloonManager instance found!");
        }

        Time.timeScale = 1f;
            
       
    }
    public void SaveGame()
    {
        if (ScoreManager.instance != null)
        {

            string playerName = ScoreManager.instance.playerName;
            int currentScore = ScoreManager.instance.GetScore();

           
            PlayerPrefs.SetInt("CurrentGameScore", currentScore);
            PlayerPrefs.SetString("CurrentPlayerName", playerName);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
                PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
                PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);
                
                PlayerPrefs.SetFloat("PlayerScaleX", player.transform.localScale.x);
            }

            PlayerPrefs.Save();

            if (DataManager.instance != null && currentScore > 0)
            {
                DataManager.instance.SavePlayerScore(playerName, currentScore);
                Debug.Log($"Saved current score to high scores: {playerName} - {currentScore}");
            }

            Debug.Log("Game saved successfully!");
        }
    }
    public void Restart()
    {
        SaveGame();

       
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
