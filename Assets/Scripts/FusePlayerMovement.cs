using UnityEngine;
using System.Collections;


public class FusePlayerMovement : MonoBehaviour {


	Rigidbody playerRigidbody;
	float movementSpeed; 
	bool freeze; //freezes the mouse rotation to make it easier to click things
	public float mouseSensitivity = 4f; 

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody>();
		movementSpeed = 1f; //because my movement speed changes
		freeze = true; // begins the game with the freeze on 
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if frozen
		KeyStroke ();
		//Changes Movement Speed Based on Y-axis position
		if (transform.position.y <= -2) {
			movementSpeed = 3f;
		} else {
			movementSpeed = 1f;
		}
		//Checks to see if frozen before rotating the player by mouse
		if (freeze == false) {
			RotatePlayer ();
		}
		//Moves the player by arrow key
		MovePlayer();

	}
	
	void RotatePlayer () {
		// looking around the the mouse
		float mouseY = -Input.GetAxis ("Mouse Y");
		float mouseX = Input.GetAxis ("Mouse X");
		Vector3 playerRotation = transform.localEulerAngles;
		playerRotation.x += mouseY * mouseSensitivity;
		playerRotation.y += mouseX * mouseSensitivity;
		transform.localEulerAngles = playerRotation;
	}
	
	void MovePlayer () {
		// movement - foward, back, side to side
		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		direction = direction.normalized * Time.deltaTime * movementSpeed;
		// move in the direction that the player's looking
		direction = transform.TransformDirection(direction);
		playerRigidbody.MovePosition(transform.position + direction);	
	}

	
	void KeyStroke () {
		//Changes freeze variable based on the "f" key
		if (freeze == true) {
			if (Input.GetKeyDown ("f")) {
				freeze = false;
			}
		} else {
			if (Input.GetKeyDown("f")) {
				freeze = true;
			}
		}
	}

}
