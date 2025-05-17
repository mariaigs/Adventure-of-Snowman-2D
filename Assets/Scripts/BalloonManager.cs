using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
public class BalloonManager : MonoBehaviour
{
    public static BalloonManager instance;
    public GameObject yellowBalloonPrefab; 
    public GameObject blackBalloonPrefab; 
    
    private List<Vector2> spawnPositions = new List<Vector2>();
    private int balloonCounter = 0;

    public float minBalloonDistance = 3f;
    public int maxBalloons = 5;

    private List<GameObject> activeBalloons = new List<GameObject>();

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        GenerateSpawnPositions();
        SpawnInitialBalloons();
        StartCoroutine(SpawnBalloonsRoutine());
    }

    public void ResumeGame()
    {
        Debug.Log("BalloonManager: Resuming game");

        
        for (int i = activeBalloons.Count - 1; i >= 0; i--)
        {
            if (activeBalloons[i] == null)
            {
                activeBalloons.RemoveAt(i);
            }
        }

        
        if (activeBalloons.Count < 3)
        {
            
            SpawnInitialBalloons();
        }
    }

    public void SpawnInitialBalloons()
    {
       
        int balloonsToSpawn = maxBalloons - activeBalloons.Count;

        Debug.Log($"BalloonManager: Spawning {balloonsToSpawn} initial balloons");

        for (int i = 0; i < balloonsToSpawn; i++)
        {
            SpawnBalloon();
        }
    }

    void GenerateSpawnPositions()
    {
        spawnPositions.Clear();
        Tilemap groundTilemap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();

        if (groundTilemap != null)
        {

            BoundsInt bounds = groundTilemap.cellBounds;


            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);


                    if (groundTilemap.HasTile(tilePos))
                    {

                        Vector3Int aboveTilePos = new Vector3Int(x, y + 1, 0);
                        if (!groundTilemap.HasTile(aboveTilePos))
                        {

                            Vector2 worldPos = groundTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 1.5f, 0);
                            spawnPositions.Add(worldPos);
                        }
                    }
                }
            }

            Debug.Log("Generated " + spawnPositions.Count + " possible balloon spawn positions");
        }
    }

    IEnumerator SpawnBalloonsRoutine()
    {
        while (true)
        {

            yield return new WaitForSeconds(2f);

            for (int i = activeBalloons.Count - 1; i >= 0; i--)
            {
                if (activeBalloons[i] == null)
                {
                    activeBalloons.RemoveAt(i);
                }
            }


            if (activeBalloons.Count < maxBalloons && spawnPositions.Count > 0)
            {
                SpawnBalloon();
            }
        }
    }

    void SpawnBalloon()
    {

        if (spawnPositions.Count == 0)
        {
            Debug.LogWarning("BalloonManager: No spawn positions available!");
            return;
        }
        GameObject balloonToSpawn;
        if (balloonCounter % 6 == 5) // Every 6th balloon (0-based index)
        {
            balloonToSpawn = blackBalloonPrefab;
            Debug.Log("Spawning a BLACK balloon");
        }
        else
        {
            balloonToSpawn = yellowBalloonPrefab;
            Debug.Log("Spawning a YELLOW balloon");
        }

        for (int attempts = 0; attempts < 10; attempts++)
        {
            int randomIndex = Random.Range(0, spawnPositions.Count);
            Vector2 spawnPos = spawnPositions[randomIndex];

            bool tooClose = false;
            foreach (GameObject balloon in activeBalloons)
            {
                if (balloon != null && Vector2.Distance(spawnPos, balloon.transform.position) < minBalloonDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                GameObject newBalloon = Instantiate(balloonToSpawn, spawnPos, Quaternion.identity);
                Vector3 position = newBalloon.transform.position;
                position.z = -11.72f;
                newBalloon.transform.position = position;
                activeBalloons.Add(newBalloon);
                balloonCounter++; // Increment counter
                Debug.Log("BalloonManager: Spawned balloon at " + spawnPos);
                return;
            }
        }

        int fallbackIndex = Random.Range(0, spawnPositions.Count);
        Vector2 fallbackPos = spawnPositions[fallbackIndex];
        GameObject fallbackBalloon = Instantiate(balloonToSpawn, fallbackPos, Quaternion.identity);
        Vector3 fallbackPosition = fallbackBalloon.transform.position;
        fallbackPosition.z = -11.72f;
        fallbackBalloon.transform.position = fallbackPosition;
        activeBalloons.Add(fallbackBalloon);
        balloonCounter++; 
        Debug.Log("BalloonManager: Forced balloon spawn at " + fallbackPos);
    }


    public void BalloonPopped(GameObject balloon, bool isBlackBalloon)
    {
        activeBalloons.Remove(balloon);

        if (ScoreManager.instance != null)
        {
            if (isBlackBalloon)
            {
                Debug.Log("Black balloon popped! -3 points");
                ScoreManager.instance.AddPoints(-3); 
                
            }
            else
            {
                Debug.Log("Yellow balloon popped! +1 point");
                ScoreManager.instance.AddPoints(1); 
                
            }
        }
    }

}