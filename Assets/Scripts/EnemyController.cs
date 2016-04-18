using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    public float moveSpeed = 1;
    public int damageAmount = 10;

    bool isMovingRight = true;
    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
        if (Time.timeScale != 0)
            MoveEnemy();

        CheckForEdge();
	}

    void MoveEnemy ()
    {
        float movement = moveSpeed;
        if (!isMovingRight)
            movement *= -1;
        transform.position = new Vector3(transform.position.x + movement, transform.position.y, transform.position.z);
    }

    void CheckForEdge()
    {
        RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, gameObject.GetComponent<Renderer>().bounds.size.x/2 + .01f);
        if (hitright.collider != null)
        {
          CheckCollider(hitright);
        }

        RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, gameObject.GetComponent<Renderer>().bounds.size.x/2 + .01f);
        if (hitleft.collider != null)
        {
          CheckCollider(hitleft);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, gameObject.GetComponent<Renderer>().bounds.size.y/2 + .04f); //transform.lossyScale.y/2 +
        if (hit.collider == null)
        {
            ChangeDirection();
            rb2d.isKinematic = false;
        }
		else if (hit.collider.tag != "Ground" && hit.collider.tag != "Platform" && hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
			ChangeDirection();
        else if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform")
        {
            rb2d.isKinematic = true;
        }
    }

    void CheckCollider(RaycastHit2D hitarea)
    {
      if (hitarea.collider.tag == "Ground")
      {
          ChangeDirection();
      }
    }

    void ChangeDirection()
    {
      if (isMovingRight == false)
          isMovingRight = true;
      else
          isMovingRight = false;
    }
}
