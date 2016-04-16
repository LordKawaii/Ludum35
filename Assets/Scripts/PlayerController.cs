using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    const float waterJumpDevider = 2.3f;

    public float speed = 1;
    public float startingJumpSpeed = 1;
    public float buoyancy = 100f;
    public float waterGrav = .03f;

    [HideInInspector]
    public PlayerStates playerState;

    float jumpSpeed;

    Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        playerState = new PlayerStates();
        jumpSpeed = startingJumpSpeed;

    }
	
	// Update is called once per frame
	void Update () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate ()
    {
        MovePlayer();
        CheckForGround();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
            playerState.insidePlatform = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
            playerState.insidePlatform = false;
    }

    void OnTriggerStay2D (Collider2D col)
    {
        if (col.tag == "Water")
        {
            rb2d.gravityScale = waterGrav;
            if (!playerState.isInWater)
                jumpSpeed /= waterJumpDevider;
            playerState.isInWater = true;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Water")
        {
            rb2d.gravityScale = 1;
            if (playerState.isInWater)
                jumpSpeed *= waterJumpDevider;
            playerState.isInWater = false;
        }

    }


    void MovePlayer()
    {
        if (Input.GetButton("Horizontal"))
        {
            float horSpeed = speed;
            if (Input.GetAxis("Horizontal") < 0)
                horSpeed *= -1;

            gameObject.transform.position = new Vector3(transform.position.x + horSpeed, transform.position.y);
        }

        if (Input.GetAxis("Vertical") > 0 && !playerState.isjumping)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.up * jumpSpeed);
            playerState.isjumping = true;
        }

        if (playerState.isInWater && !playerState.canEnterWater)
        {
            rb2d.AddForce(Vector2.up * buoyancy);
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + buoyancy);
        }
    }

    void CheckForGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, .6f); //transform.lossyScale.y/2 +
        if (hit.collider != null && !playerState.insidePlatform && (hit.collider.tag == "Ground" || hit.collider.tag == "Platform"))
        {
            playerState.isjumping = false;
            jumpSpeed = startingJumpSpeed;

        }
        else if (hit.collider != null && hit.collider.tag == "Water")
        {
            playerState.isjumping = false;
            jumpSpeed /= waterJumpDevider;
        }
    }

}