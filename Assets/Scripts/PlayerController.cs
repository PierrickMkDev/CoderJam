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
    public float wallJumpLerp = 10;

    private Rigidbody2D rb;

    private Collision coll;

    public bool canMove = true;
    public bool wallJumped;
    public bool wallSlide;

    public float wallJumpAngle = 45;

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

        //rb.velocity = movement;

        if (canMove)
        {
            if (!wallJumped)
            {
                rb.velocity = new Vector2(movement.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(movement.x, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
            }
        }

        //contre un mur sans toucher le sol
        if (coll.onWall && !coll.onGround)
        {
            //on glisse
            wallSlide = true;
            WallSlide();
        }

        if (coll.onGround)
        {
            wallJumped = false;
            GetComponent<BetterJump>().enabled = true;
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
        Debug.Log("Dir : " + dir);

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        //Debug.Log("velocity : " + rb.velocity);

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

        int wallOrientation = coll.onRightWall ? -1 : 1;

        float radAngle = Mathf.Deg2Rad * wallJumpAngle;
        Vector2 angleDir = new Vector2(Mathf.Cos(radAngle) * wallOrientation, Mathf.Sin(radAngle));

        Jump(angleDir);

        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
