using UnityEngine;
using System.Collections;

public class PlatformSolidifer : MonoBehaviour {

    BoxCollider2D platformCollider;

    void Start()
    {
        platformCollider = GetComponentInParent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            platformCollider.isTrigger = false;
    }
}
