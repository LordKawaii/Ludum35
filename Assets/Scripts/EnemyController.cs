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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, gameObject.GetComponent<Renderer>().bounds.size.y/2 + .01f); //transform.lossyScale.y/2 +
        if (hit.collider == null)
        {
            if (isMovingRight == false)
                isMovingRight = true;
            else
                isMovingRight = false;
            rb2d.isKinematic = false;
        }
        else if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform")
        {
            rb2d.isKinematic = true;
        }
            

    }
}
