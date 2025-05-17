using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void Awake()
    {
        
        Debug.Log("DeathZone Awake called");
    }

    private void Start()
    {
        Debug.Log("DeathZone Start called - Object is active");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the DeathZone: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Purly fell and died!");

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX("GameOver");
            }


            if (ScoreManager.instance != null)
            {
                string playerName = ScoreManager.instance.playerName;
                int finalScore = ScoreManager.instance.GetScore();

                if (finalScore > 0)
                {
                    PlayerPrefs.SetInt("LastScore", finalScore);
                    PlayerPrefs.Save();

                    if (DataManager.instance != null)
                    {
                        Debug.Log($"Saving final score for player: {playerName} - Score: {finalScore}");
                        DataManager.instance.SavePlayerScore(playerName, finalScore);
                    }
                }
            }

            SceneManager.LoadScene("Menu");
            Destroy(other.gameObject);
        }
    }
}