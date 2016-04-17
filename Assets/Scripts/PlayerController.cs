using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpriteActions
{
    Stand,
    WalkRight,
    WalkLeft,
    Fall
}

public class PlayerController : MonoBehaviour {
    const float waterJumpDevider = 1.5f;

    public float flapForce = 2.3f;
    public float speed = 1;
    public float startingJumpSpeed = 1;
    public float buoyancy = 100f;
    public float waterGrav = .03f;
    public float InvulnTime = 1f;
    public float InvulnFlashTime = .2f;
	public float waterSlowdown = .8f;
    public ParticleSystem jumpParticals;
    public ParticleSystem hurtParticals;

    public List<Sprite> landSprites;
    public List<Sprite> seaSprites;
    public List<Sprite> airSprites;

    [HideInInspector]
    public PlayerStates playerState;

    float jumpSpeed;
    SpriteRenderer spriteRend;

    Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        playerState = new PlayerStates();
        jumpSpeed = startingJumpSpeed;
        spriteRend = GetComponent<SpriteRenderer>();

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

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.tag == "Water") 
		{
			rb2d.velocity = rb2d.velocity.normalized * waterSlowdown;
		}
		
        if (col.tag == "Enemy")
        {
            if (!playerState.isInvuln)
            {
                if (playerState.health <= 0)
                    Destroy(gameObject);
                else
                {
                    playerState.isInvuln = true;
                    playerState.health -= col.gameObject.GetComponent<EnemyController>().damageAmount;
                    StartCoroutine(Invuln());
                    if (hurtParticals != null)
                        hurtParticals.Play();
                }
            }
        }

        if (col.tag == "JumpingEnemy")
        {
            if (!playerState.isInvuln)
            {
                if (playerState.health <= 0)
                    Destroy(gameObject);
                else
                {
                    playerState.isInvuln = true;
                    playerState.health -= col.gameObject.GetComponent<JumpingEnemyController>().damageAmount;
                    StartCoroutine(Invuln());
                    if (hurtParticals != null)
                        hurtParticals.Play();
                }
            }
        }

    }

    void MovePlayer()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            float horSpeed = speed;
            if (Input.GetAxis("Horizontal") < 0)
            {
                ChangeSprite(SpriteActions.WalkLeft);
                horSpeed *= -1;
            }
            else
                ChangeSprite(SpriteActions.WalkRight);

            gameObject.transform.position = new Vector3(transform.position.x + horSpeed, transform.position.y);
        }
        else 
            ChangeSprite(SpriteActions.Stand);

        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Fire1")) && !playerState.isjumping)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.up * jumpSpeed);
            playerState.isjumping = true;
            ChangeSprite(SpriteActions.Stand);
        }

        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Fire1")) && playerState.isInWater && rb2d.velocity.y < 5)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.up * 80);
            ChangeSprite(SpriteActions.Stand);
        }

        if ((Input.GetButtonDown("Vertical") || Input.GetButtonDown("Fire1")) && playerState.canGlide && playerState.isjumping && !playerState.hasFlapped)
        {
            rb2d.AddForce(Vector2.up * flapForce);
            playerState.hasFlapped = true;
            if (jumpParticals != null)
                jumpParticals.Play();
            ChangeSprite(SpriteActions.Stand);
        }
        if (!Input.GetButton("Vertical") && !Input.GetButton("Fire1"))
            playerState.hasFlapped = false;

        if (playerState.isInWater && !playerState.canEnterWater)
        {
            rb2d.AddForce(Vector2.up * buoyancy);
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + buoyancy);
        }

        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Fire1")) && playerState.isInWater && rb2d.velocity.y < 5)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.up * 80);
            ChangeSprite(SpriteActions.Stand);
        }

        if (rb2d != null)
        {
            if (rb2d.velocity.y < -1f)
                ChangeSprite(SpriteActions.Fall);
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

    void ChangeSprite(SpriteActions action)
    {
        switch (action)
        {
            case SpriteActions.Stand:
                spriteRend.sprite = GetCurrentSpriteList()[0];
                if (spriteRend.flipX == true)
                    spriteRend.flipX = false;
                break;

            case SpriteActions.WalkRight:
                spriteRend.sprite = GetCurrentSpriteList()[1];
                if (spriteRend.flipX == true)
                    spriteRend.flipX = false;
                break;

            case SpriteActions.WalkLeft:
                spriteRend.sprite = GetCurrentSpriteList()[1];
                if (spriteRend.flipX == false)
                    spriteRend.flipX = true;
                break;

            case SpriteActions.Fall:
                spriteRend.sprite = GetCurrentSpriteList()[2];
                if (spriteRend.flipX == true)
                    spriteRend.flipX = false;
                break;
        }
    }

    List<Sprite> GetCurrentSpriteList()
    {
        if (playerState.canEnterWater)
            return seaSprites;
        else if (playerState.canGlide)
            return airSprites;
        else
            return landSprites;
    }

    public IEnumerator Invuln()
    {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float timeTillComplete = Time.time + InvulnTime;

        while (Time.time < timeTillComplete)
        {
            if (renderer.enabled == true)
                renderer.enabled = false;
            else renderer.enabled = true;
            yield return new WaitForSeconds(InvulnFlashTime);
        }

        if (renderer.enabled == false)
            renderer.enabled = true;
        playerState.isInvuln = false;

        yield return null;
    }

}
