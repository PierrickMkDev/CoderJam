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
    public string jumpAxe = "";

    public float speed = 5;
    [Range(1, 20)]
    public float jumpForce = 5;
    public float slideSpeed = 5;
    //public float wallJumpLerp = 10;

    private Rigidbody2D rb;

    private Collision coll;

    public bool canMove;
    public bool wallJumped;
    public bool wallSlide;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis(horizontalAxe);
        float moveVertical = Input.GetAxis(verticalAxe);

        Vector2 movement = new Vector3(moveHorizontal * speed, rb.velocity.y);

        rb.velocity = movement;

        //contre un mur sans toucher le sol
        if (coll.onWall && !coll.onGround)
        {
            //on glisse
            wallSlide = true;
            WallSlide();
        }

        if (Input.GetButtonDown(jumpAxe))
        {
            //sur le sol
            if (coll.onGround)
                Jump(Vector2.up);

            //contre un mur sans toucher le sol
            if (coll.onWall && !coll.onGround)
                WallJump();
        }
    }

    public void Jump(Vector2 dir)
    {
        Debug.Log("JUMP !");

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        Debug.Log("velocity : " + rb.velocity);

    }

    private void WallSlide()
    {
        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f));

        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
