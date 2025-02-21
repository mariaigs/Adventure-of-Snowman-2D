using UnityEngine;

public class PurlyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private float moveSpeed = 5.0f; //speed for movement left right up and down
    [SerializeField]
    private float rotationSpeed = 100.0f; //speed for rotation

    //variables for movement and rotation 
    private float translation = 0, rotation = 0;

    void Start()
    {
        Debug.Log("PurlyScript initialized.");
        
    }

    // Update is called once per frame
    void Update()
    {
        translation = moveSpeed * Time.deltaTime;
        rotation = rotationSpeed * Time.deltaTime;

        //handle movement
        // Move left (A key or Left Arrow)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * translation, Space.World);
        }

        // Move right (D key or Right Arrow)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * translation, Space.World);
        }

        // Move up (W key or Up Arrow)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * translation, Space.World);
        }

        // Move down (S key or Down Arrow)
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * translation, Space.World);
        }
        // Rotation around Y-axis (Q and E keys)
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationInput = 1f;
        }
        transform.Rotate(0f, rotationInput * rotationSpeed * Time.deltaTime, 0f, Space.World);
    }


}

