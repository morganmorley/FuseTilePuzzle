using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FuseEntranceBehavior : MonoBehaviour {

	private GameObject[] rotatingTiles = new GameObject[42];
	public GameObject door;
	public GameObject entranceTile; //this does not rotate
	public GameObject exitTile; //this does not rotate
	private Collider entranceCollider; 
	public bool unlocked; //true if entrance is unlocked

	// Use this for initialization
	void Start () {
		unlocked = false;
		string t = "tile";
		rotatingTiles = GameObject.FindGameObjectsWithTag (t);
		entranceCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckifCorrect ();
		//if click
		if (Input.GetMouseButtonDown (0)) {
			if (unlocked == true) {
				WaitForClick ();
			}
		}
	}

	//if unlocked, clicking the wall will unlock the entrance
	void WaitForClick() {
		//on click
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
	
		//check if raycast hits entrance
		if (Physics.Raycast (ray, out hit)) {
			//what is clicked
			if (hit.collider == entranceCollider) {
				//how you enter the fuse puzzle
				UnlockEntrance();
			}
		}
	}

	//How the wall disappears
	public void UnlockEntrance () {
		door.SetActive (false);
		entranceTile.SetActive(false);
		exitTile.SetActive(false);
		for (int i = 0; i<42; i++) {
			rotatingTiles[i].SetActive (false);
		}
	}

	//Checks to see if all of the tile orientations are correct
	void CheckifCorrect() {
		int numCorrect = 0;
		for (int i=0; i<42; i++) {
			if (rotatingTiles[i].GetComponent<FuseTileBehavior>().isCorrect == true) {
				numCorrect = numCorrect + 1;
				//Debug:
				//tiles[i].SetActive(false);
			}
		}
		if (numCorrect >=42) {
			unlocked = true;
		}	
	}
}
