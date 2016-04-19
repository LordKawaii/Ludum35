using UnityEngine;
using System.Collections;

public class CamraController : MonoBehaviour {
    public Transform playerTranform;
    public float floatX = 1;
    public float floatY = 1;
    public float speed = 10;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (playerTranform != null)
        {
            /*
            float positionX = transform.position.x;
            float positionY = transform.position.y;
            if (( playerTranform.position.x - transform.position.x) > floatX)
            {
                positionX = playerTranform.position.x + floatX;
            }
            if (( playerTranform.position.x - transform.position.x) < -floatX)
            {
                positionX = playerTranform.position.x - floatX;
            }
            if (transform.position.y + floatY > playerTranform.position.y)
            {
                positionY = playerTranform.position.y + floatY;
            }
            if (transform.position.y + floatY < playerTranform.position.y)
            {
                positionY = playerTranform.position.y - floatY;
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(positionX, positionY, transform.position.z), Time.deltaTime);
            */
            transform.position = new Vector3(playerTranform.position.x, playerTranform.position.y, transform.position.z);
        }
    }
}
