using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class WaterfallHazard : MonoBehaviour
{
   

    private void Awake()
    {
        Debug.Log("WaterfallHazard Awake called on: " + gameObject.name);

       
    }

    private void Start()
    {
        Debug.Log("WaterfallHazard Start called - Object is active");

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something touched the waterfall: " + other.gameObject.name + " with tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Purly touched waterfall and melted!");

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX("GameOver");
            }

            KillPlayer(other.gameObject);
        }
    }

   
    private void KillPlayer(GameObject player)
    {
        // Save score
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

        // Load menu scene
        SceneManager.LoadScene("Menu");

        // Destroy player
        Destroy(player);
    }
}