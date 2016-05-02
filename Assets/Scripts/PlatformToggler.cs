using UnityEngine;
using System.Collections;

public class PlatformToggler : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
		CheckToMakeSolid();
		CheckToMakeTrigger();

	}

	void CheckToMakeSolid()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, GetComponent<Renderer>().bounds.size.y/2 +.1f); //transform.lossyScale.y/2 +
		if (hit.collider != null)
		{ 
			if (hit.collider.tag == "Platform")
			{
				BoxCollider2D platformCollider = hit.collider.gameObject.GetComponent<BoxCollider2D>();
				if (platformCollider != null)
				{
					if (platformCollider.isTrigger)
						platformCollider.isTrigger = false;
				}
			}
		}
	}

	void CheckToMakeTrigger()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, GetComponent<Renderer>().bounds.size.y / 2 + .1f); //transform.lossyScale.y/2 +
		if (hit.collider != null)
		{
			if (hit.collider.tag == "Platform")
			{
				BoxCollider2D platformCollider = hit.collider.gameObject.GetComponent<BoxCollider2D>();
				if (platformCollider != null)
				{
					if (!platformCollider.isTrigger)
						platformCollider.isTrigger = true;
				}
			}
		}
	}
}
