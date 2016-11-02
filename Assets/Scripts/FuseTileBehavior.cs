using UnityEngine;
using System.Collections;

public class FuseTileBehavior : MonoBehaviour {

	//Direction at start that should be pointing down if in correct rotation:
	public GameObject entrance; //This disappears when unlocked
	public bool rotationN; //Directions at the start of the game since they always replace
	public bool rotationE; //exactly on replay
	public bool rotationS;
	public bool rotationW;

	private Rigidbody tileBody;
	private Collider tileCollider;

	public bool isCorrect; //True if the tile is in the correct rotation
	private char pointingDown; //The current direction that is pointing down

	float currentRotation = 0; //keeps track of the current rotation of the tile

	//Initialization
	void Start () {
		tileBody = GetComponent<Rigidbody> ();
		tileCollider = GetComponent<Collider>();
		pointingDown = 'S'; //the direction pointing down at the start is always south, each tile has unique directions that are correct for its orientation and type
		if (rotationS == true) {
			isCorrect = true;
		} else {
			isCorrect = false;
		}
		currentRotation = transform.rotation.eulerAngles.x;
		//This is handling the weird case where the quaternions turn everything upside down
		if (transform.rotation.eulerAngles.z == 180) {
			currentRotation = 180;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if click, rotates tile
		if (Input.GetMouseButtonDown (0)) {
			RotateTile ();
		}
	}

	void RotateTile () {
		//on click
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		//check if raycast hits tile
		if (Physics.Raycast (ray, out hit)) {
			//if Entrance Locked
			if (entrance.GetComponent<FuseEntranceBehavior>().unlocked == false ) {
				//which tile
				if (hit.collider == tileCollider) {							
					//Rotates Tile
					float targetAngle = 0;

					if (currentRotation == 90 ){
						targetAngle = 180;
					}else if (currentRotation == 180){
						targetAngle = 270;
					}else if (currentRotation == 270) {
						targetAngle = 0;
					}else if (currentRotation == 0) { 
						targetAngle = 90;
					}


					Quaternion newRotation = Quaternion.Euler (new Vector3 (targetAngle,0,0));
					tileBody.MoveRotation (newRotation); 
					currentRotation = targetAngle;

					UpdateDirections ();
				}
			}
			else { //if Entrance is Unlocked
				if (hit.collider == tileCollider) {						
					//makes entrance disappear
					entrance.GetComponent<FuseEntranceBehavior>().UnlockEntrance();
				}
			}
		}
	}

	void UpdateDirections () {
		//Updates the pointingDown and isCorrect variables, which depend on variables set in the scene and are unique to each tile.
		isCorrect = false;
		switch (pointingDown) {
		case 'S':
			pointingDown = 'W';
			if (rotationW == true){
				isCorrect = true;
			}
			break;
		case 'E':
			pointingDown = 'S';
			if (rotationS == true){
				isCorrect = true;
			}
			break;
		case 'N':
			pointingDown = 'E';
			if (rotationE == true){
				isCorrect = true;
			}
			break;
		case 'W':
			pointingDown = 'N';
			if (rotationN == true){
				isCorrect = true;
			}
			break;
		}
	}

}
