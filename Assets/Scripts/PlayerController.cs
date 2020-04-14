using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private string horizontalAxe = "";
    [SerializeField]
    private string verticalAxe = "";
    [SerializeField]
    private string jumpAxe = "";

    public float speed = 5;
    public float jumpForce = 100;

    private Rigidbody2D rb;

    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Movement Controls\\

        float moveHorizontal = Input.GetAxis(horizontalAxe);
        float moveVertical = Input.GetAxis(verticalAxe);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.velocity = movement * speed;

        if (Input.GetButtonDown(jumpAxe) && isGrounded)
        {
            Debug.Log("JUMP !");
            rb.velocity += Vector2.up * jumpForce;
            Debug.Log("velocity : " + rb.velocity);
            isGrounded = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("GROUNDED !");
            isGrounded = true;
        }
    }
}
