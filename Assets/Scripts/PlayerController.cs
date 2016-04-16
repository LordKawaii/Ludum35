using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float speed = 1;
    public float jumpSpeed = 1;

    [HideInInspector]
    public PlayerStates playerState;

    Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        playerState = new PlayerStates();
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
    }

    void CheckForGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, .6f); //transform.lossyScale.y/2 +
        if (hit.collider != null && !playerState.insidePlatform && (hit.collider.tag == "Ground" || hit.collider.tag == "Platform"))
            playerState.isjumping = false;
    }
}