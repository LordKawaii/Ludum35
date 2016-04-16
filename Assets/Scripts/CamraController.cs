using UnityEngine;
using System.Collections;

public class CamraController : MonoBehaviour {
    public Transform playerTranform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (playerTranform != null)
        {
            transform.position = new Vector3(playerTranform.position.x, playerTranform.position.y, transform.position.z);
        }
	}
}
