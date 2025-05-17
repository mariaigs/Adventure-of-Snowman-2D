using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI MyscoreText;
    private int ScoreNum;
   
    void Start()
    {
     
        ScoreNum = 0;
        MyscoreText.text = "Score: "+ ScoreNum;

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "balloon")
        {
            ScoreNum += 1;
            Destroy(collision.gameObject);
            MyscoreText.text = "Score: " + ScoreNum;
        }
        
    }



}
