using UnityEngine;


public class Balloon : MonoBehaviour
{
    public bool isBlackBalloon = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.instance != null)
            {
                if (isBlackBalloon)
                {
                    AudioManager.instance.PlaySFX("BlackBalloon");
                }
                else
                {
                    AudioManager.instance.PlaySFX("YellowBalloon");
                }
            }


            if (BalloonManager.instance != null)
            {
                BalloonManager.instance.BalloonPopped(gameObject, isBlackBalloon);
            }

            Destroy(gameObject);
        }
    }

}
