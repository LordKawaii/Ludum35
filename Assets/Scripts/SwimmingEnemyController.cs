using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwimmingEnemyController : MonoBehaviour {

	public float minTimeToMove = 1f;
	public float maxTimeToMove = 2f;
	public float minMovement = -1f;
	public float maxMovement = 1f;
	public float moveSpeed = 1f;

	float timeTillMove;
	Vector2 destination;
	SpriteRenderer spriteRend;

	// Use this for initialization
	void Start () {
		timeTillMove = Time.time + Random.Range(minTimeToMove, maxTimeToMove);
		spriteRend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}

	void Move ()
	{
		if (Time.time > timeTillMove)
		{
			float randX = Random.Range(minMovement, maxMovement);
            destination = new Vector2(transform.position.x + randX, transform.position.y + Random.Range(minMovement, maxMovement));
			if (randX < 0)
				spriteRend.flipX = true;
			else
				spriteRend.flipX = false;

			timeTillMove = Time.time + Random.Range(minTimeToMove, maxTimeToMove);
		}

		transform.position = Vector2.Lerp(transform.position, destination, moveSpeed * Time.deltaTime);
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.tag == "Water")
		{
			destination = new Vector2(col.transform.position.x, col.transform.position.y);

			if ((col.transform.position.x - transform.position.x) < 0)
				spriteRend.flipX = true;
			else
				spriteRend.flipX = false;
		}
    }
}
