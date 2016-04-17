using UnityEngine;
using System.Collections;

public class JumpingEnemyController : MonoBehaviour {
    public float timeTillJump = .5f;
    public float jumpForceX = 50;
    public float jumpForceY = 50;
    public float spinForce = 10;
    public int damageAmount = 10;

    float jumpTimer;
    bool jumpedLeft = false;
    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        jumpTimer = Time.time + timeTillJump;
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, gameObject.GetComponent<Renderer>().bounds.size.y / 2 + .01f); //transform.lossyScale.y/2 +
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform")
                rb2d.isKinematic = true;
        }

        if (jumpTimer <= Time.time)
        {
            if (jumpedLeft == false)
            {
                rb2d.isKinematic = false;
                rb2d.AddForce(new Vector2(-jumpForceX, jumpForceY));
                rb2d.AddTorque(spinForce);
                jumpedLeft = true;
            }
            else
            {
                rb2d.isKinematic = false;
                rb2d.AddForce(new Vector2(jumpForceX, jumpForceY));
                rb2d.AddTorque(-spinForce);
                jumpedLeft = false;
            }

            jumpTimer = Time.time + timeTillJump;
        }

    }


}
