using UnityEngine;
using System.Collections;

public enum PickupType
{
    Water,
    Flight
}

public class PickupController : MonoBehaviour {
    public float rotationSpeed = 1f;
    public PickupType pickupType;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(1,0,-1.5f), rotationSpeed);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            PlayerController player = GameObject.FindGameObjectWithTag(col.tag).GetComponent<PlayerController>();
            if (pickupType == PickupType.Water)
                player.playerState.canEnterWater = true;
            if (pickupType == PickupType.Flight)
                player.playerState.canGlide = true;
            Destroy(gameObject);
        }
    }
}
