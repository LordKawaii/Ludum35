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
    public ParticleSystem morphParticals;

    public List<Sprite> landSprites;
    public List<Sprite> seaSprites;
    public List<Sprite> airSprites;

    public List<AudioClip> hurtSounds;
    public List<AudioClip> jumpSounds;
    public List<AudioClip> flySounds;
    public AudioClip enterWater;
    public AudioClip exitWater;

    [HideInInspector]
    public PlayerStates playerState;

    float jumpSpeed;
    SpriteRenderer spriteRend;
    AudioSource auSource;
    Rigidbody2D rb2d;
    bool hasLeftGround = false;
	// Use this for initialization
	void Start () {
        playerState = new PlayerStates();
        jumpSpeed = startingJumpSpeed;
        spriteRend = GetComponent<SpriteRenderer>();
        auSource = GetComponent<AudioSource>();

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
            auSource.clip = exitWater;
            if (!auSource.isPlaying)
                auSource.Play();
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.tag == "Water") 
		{
			rb2d.velocity = rb2d.velocity.normalized * waterSlowdown;
            auSource.clip = enterWater;
            if (!auSource.isPlaying)
                auSource.Play();
        }
		
        if (col.tag == "Enemy" || col.tag == "JumpingEnemy")
        {
            if (!playerState.isInvuln)
            {
                {
                    playerState.isInvuln = true;
                    playerState.health -= col.gameObject.GetComponent<EnemyStates>().damage;
                    StartCoroutine(Invuln());
                    if (hurtParticals != null)
                        hurtParticals.Play();
                    auSource.clip = hurtSounds[Random.Range(0, jumpSounds.Count)];
                    if (!auSource.isPlaying)
                        auSource.Play();
                }
            }

            if (playerState.health <= 0)
            { 
                Destroy(gameObject);
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

            gameObject.transform.position = new Vector3(transform.position.x + horSpeed * Time.deltaTime, transform.position.y);
        }
        else 
            ChangeSprite(SpriteActions.Stand);

        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Fire1")) && !playerState.isjumping)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.up * jumpSpeed);
            playerState.isjumping = true;
            ChangeSprite(SpriteActions.Stand);
            auSource.clip = jumpSounds[Random.Range(0, jumpSounds.Count)];
            if (!auSource.isPlaying)
                auSource.Play();
        }

        if ((Input.GetAxis("Vertical") > 0 || Input.GetButton("Fire1")) && playerState.isInWater && rb2d.velocity.y < 3)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.up * 80);
            ChangeSprite(SpriteActions.Stand);
        }

        if ((Input.GetAxis("Vertical") < 0) && playerState.isInWater && playerState.canEnterWater && rb2d.velocity.y > -3)
        {
            //gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + jumpSpeed);
            rb2d.AddForce(Vector2.down * 80);
            ChangeSprite(SpriteActions.Fall);
        }

        if ((Input.GetButtonDown("Vertical") || Input.GetButtonDown("Fire1")) && !(Input.GetAxis("Vertical") < 0) && playerState.canGlide && playerState.isjumping && !playerState.hasFlapped)
        {
            if (rb2d.velocity.y < -1)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -1);
            }
            rb2d.AddForce(Vector2.up * flapForce);
            playerState.hasFlapped = true;
            if (jumpParticals != null)
                jumpParticals.Play();
            ChangeSprite(SpriteActions.Stand);

            auSource.clip = flySounds[Random.Range(0, jumpSounds.Count)];
            if (!auSource.isPlaying)
                auSource.Play();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Renderer>().bounds.size.y/2 +.1f); //transform.lossyScale.y/2 +
        if (hit.collider == null)
        {
            hasLeftGround = true;
        }
        if (hit.collider != null && !playerState.insidePlatform && hasLeftGround && (hit.collider.tag == "Ground" || hit.collider.tag == "Platform"))
        {
            hasLeftGround = false;
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
