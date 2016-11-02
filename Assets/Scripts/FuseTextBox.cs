using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuseTextBox : MonoBehaviour {
	public GameObject player;
	public GameObject entrance;
	private string text; //displayed in GUI text box

	private bool entranceUnlocked;
	private bool exitUnlocked;
	private int numFallen;
	public bool isWithProfessor;

	private string currentText; //current textfile being read.
	
	private int numE;//list of time e is pressed, advancing the text

	TextAsset startTextFile; //beginning instructions
	string[] startTextArray;

	TextAsset entranceTextFile; //upon entrance unlock, more instructions
	string[] entranceTextArray; 
	
	TextAsset fallingTextFile; //upon falling
	string[] fallingTextArray;

	TextAsset winTextFile;
	string[] winTextArray; //upon winning


	void Start(){
		int numE = 0; 
		int numFallen = 0;

		//isWithProfessor = true;
		//Determine if isWithProfessor is true here.

		//These set up by part of the plot - change these by text file
		startTextFile = (TextAsset)Resources.Load ("FuseStart"); //name of your text file
		startTextArray = startTextFile.text.Split('\n'); //read line by line

		entranceTextFile = (TextAsset)Resources.Load ("FuseEntranceUnlocked");
		entranceTextArray = entranceTextFile.text.Split ('\n');

		fallingTextFile = (TextAsset)Resources.Load ("FuseUponFalling");
		fallingTextArray = fallingTextFile.text.Split ('\n');

		winTextFile = (TextAsset)Resources.Load ("FuseUponWinning");
		winTextArray = winTextFile.text.Split ('\n');

		entranceUnlocked = false;
		exitUnlocked = false;

		if (isWithProfessor == true) {
			//first dialog
			SetText ("start");
			currentText = "start";
		}
	}

	bool KeyStroke () {
		//When e is pressed, updates the count.
		if (Input.GetKeyDown ("e")) {
			numE += 1;
			return true;
		} else {
			return false;
		}
	}

	void Update(){
		if (isWithProfessor == true) {
			CheckForText ();
			bool waitForE = KeyStroke ();
			if (waitForE == true) {
				if (currentText == "start") {
					SetText ("start");
				} else if (currentText == "entrance") {
					SetText ("entrance");
				} else if (currentText == "exit") {
					SetText ("exit");
				} else if (currentText == "fall") {
					SetText ("fall");
				}
			}
		}
	}

	void CheckForText() {
		//Checks what text needs to be updated by comparing previous and updated variables
		bool previousEntrance = entranceUnlocked;
		bool updateEntrance = entrance.GetComponent<FuseEntranceBehavior>().unlocked;
		bool previousExit = exitUnlocked;
		bool updateExit = player.GetComponent<FusePlayerTriggers>().win;
		int previousFallen = numFallen;
		int updateFallen = player.GetComponent<FusePlayerTriggers>().fallenText;

		if (previousEntrance != updateEntrance) {
			if (updateEntrance == true) {
				numE = 0;
				entranceUnlocked = true;
				currentText = "entrance";
				SetText ("entrance");
			}
		}
		if (previousExit != updateExit) {
			if (updateExit == true) {
				numE = 0;
				exitUnlocked = true;
				currentText = "exit";
				SetText ("exit");
			}
		}
		if (previousFallen != updateFallen) {
			if (updateFallen+1 == previousFallen) {
				numE = 0;
				numFallen = numFallen+1;
				currentText = "fall";
				SetText ("fall");
			}
		}
	}
	
	
	string SetText (string type) {
		//Sets the text to be loaded onscreen by type.
		if (type == "start") {
			if (numE < startTextArray.Length) {
				return text = startTextArray [numE];
			}
		} else if (type == "entrance") {
			if (numE < entranceTextArray.Length) {
				return text = entranceTextArray [numE];
			}
		} else if (type == "fall") {
			if (numE < fallingTextArray.Length) {
				return text = fallingTextArray [numE];
			}
		} else if (type == "exit") {
			if (numE < winTextArray.Length) {
				return text = winTextArray [numE];
			}
		}
		return "";
	}

	private GUIStyle guiStyle = new GUIStyle(); //GUI style is how it looks - don't change
	void OnGUI(){
		guiStyle.fontSize = 20;
		guiStyle.wordWrap = true;
		guiStyle.padding.right = 10;
		guiStyle.padding.left = 10;
		guiStyle.normal.textColor = Color.white;
		float screenWidth = Screen.width;
		float screenHeight = Screen.height - 150f;
		GUI.Label (new Rect (5f,screenHeight, screenWidth, 50f), text, guiStyle);
	}
}
