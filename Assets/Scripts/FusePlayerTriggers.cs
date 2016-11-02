using UnityEngine;
using System.Collections;

public class FusePlayerTriggers : MonoBehaviour {

	int numTriggers; //total number of correct path colliders
	int score; //number of correct path colliders you have disabled
	public GameObject exit; //goal of the puzzle is to unlock this door to the orb
	
	private GameObject[] fallingTiles = new GameObject[4]; //the maze ground cubes that disappear when you fall
	private GameObject fallenTile; 
	private GameObject[] fallingCeiling = new GameObject[4]; //the ceiling cubes under the maze ground cubes that also dissapear when you fall
	public GameObject backWall; //the wall that opens up the ramp and lower levels. Disappears when you fall.

	public int fallenText; //number of times you've fallen, for CheckForText()
	public bool win; //true if you have unlocked the exit door

	
	// Use this for initialization
	void Start () {
		score = 0;
		numTriggers = 14;
		fallingTiles = GameObject.FindGameObjectsWithTag ("FallingTile");
		fallingCeiling = GameObject.FindGameObjectsWithTag ("FallingCeiling");
		fallenText = 0;
		win = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void UnlockExit() {
		//how you win
		exit.gameObject.SetActive (false);
	}

	IEnumerator Wait(float duration){
		//This is a coroutine that waits to make the tile fall for a a duration of seconds.
		backWall.SetActive (false);
		yield return new WaitForSeconds(duration);   //Wait
		fallenTile.SetActive (false);	
		fallenText = fallenText+1;
		for (int i = 0; i < 4; i++) {
			GameObject fallenCeiling = fallingCeiling[i];
			float xDifferenceCeiling = transform.position.x - fallenCeiling.transform.position.x;
			float zDifferenceCeiling = transform.position.z - fallenCeiling.transform.position.z;
			if (xDifferenceCeiling <= 1 && xDifferenceCeiling >= -1) {
				if (zDifferenceCeiling <= 1 && zDifferenceCeiling >= -1) {
					fallenCeiling.SetActive(false);
				}
			}
		}
	}

	void Fall() {
		//Removes the tile beneath you and the trigger cube you entered
		for (int i = 0; i < 4; i++) {
			float xDifferenceFloor = transform.position.x - fallingTiles[i].transform.position.x;
			float zDifferenceFloor = transform.position.z - fallingTiles[i].transform.position.z;
			if (xDifferenceFloor <= 1 && xDifferenceFloor >= -1 ) {
				if (zDifferenceFloor <= 1 && zDifferenceFloor >= -1) {
					fallingTiles[i].GetComponent<FuseFallingBehavior>().hasFallen = true;
					fallenTile = fallingTiles[i];
					StartCoroutine(Wait(1f));
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider cube){
		//when roll through a box, it checks the trigger for its type (falling or correct path) and it checks your score to see if you have won.
		CheckTrigger (cube);
		if (score == numTriggers) {
			UnlockExit();
			win = true;
		}
	}
	
	void CheckTrigger (Collider cube) {
		//Checks if the trigger is a falling cube or if you're on the correct path
		if (cube.tag == "FallingCube") { //this causes you to fall
			cube.gameObject.SetActive (false);
			Fall ();
		} else if (cube.tag == "CorrectPath") {//this brings you closer to unlocking the final door
			cube.gameObject.SetActive (false);
			score = score + 1;
		}
	}
	

}
