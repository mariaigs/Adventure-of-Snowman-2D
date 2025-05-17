using UnityEngine;

public class WaterSplashController : MonoBehaviour
{
    public GameObject splashEffectPrefab; 
    private bool wasInAir = false;

    void Update()
    {
        
        if (!GetComponent<PurlyScript>().isGrounded)
        {
            wasInAir = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && wasInAir)
        {
           
            CreateSplash();
            wasInAir = false;
        }
    }

    void CreateSplash()
    {
      
        Vector3 splashPosition = transform.position;
        splashPosition.y -= 0.5f; 

        
        GameObject splash = Instantiate(splashEffectPrefab, splashPosition, Quaternion.identity);

        
        Destroy(splash, 2f);
    }
}