using UnityEngine;

public class SnowGunManager : MonoBehaviour
{
    public SnowGuns gun1;
    public SnowGuns gun2;

    private bool gun1FiredLast = false;

    void Start()
    {
        Debug.Log("SnowGunManager STARTED.");
        gun1.StopFiring();
        gun2.StopFiring();

        gun1FiredLast = Random.value > 0.5f;

        Invoke("FireNextGun", Random.Range(1f, 3f));
    }

    void FireNextGun()
    {
        
        if (gun1FiredLast)
        {
            Debug.Log("Gun 2 firing");
            gun2.FireSingleShot();
            gun1FiredLast = false;
        }
        else
        {
            Debug.Log("Gun 1 firing");
            gun1.FireSingleShot();
            gun1FiredLast = true;
        }

       
        float nextShotDelay = Random.Range(2f, 5f);
        Invoke("FireNextGun", nextShotDelay);
    }
}
