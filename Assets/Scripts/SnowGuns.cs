using UnityEngine;

public class SnowGuns : MonoBehaviour
{
    public GameObject snowballPrefab;
    public Transform firePoint;

    public float fireDelay = 4f;
    private float timer = 0f;
    private bool isFiring = false;
    


    void Update()
    {
        if(isFiring)
        {
            timer += Time.deltaTime;
            if (timer >= fireDelay)
            {
                ShootSnowball();
                timer = 0f;
            }
        }
    }
  

    public void FireSingleShot()
    {
        ShootSnowball();
    }

    public void StartFiring()
    {
        isFiring = true;
        timer = fireDelay; 
        Debug.Log(gameObject.name + " STARTED firing.");
    }

    public void StopFiring()
    {
        isFiring = false;
        timer = 0f;
        Debug.Log(gameObject.name + " STOPPED firing.");
    }

    void ShootSnowball()
    {
        Debug.Log(gameObject.name + " fired a snowball!");
        Instantiate(snowballPrefab, firePoint.position, Quaternion.identity);
    }
}
