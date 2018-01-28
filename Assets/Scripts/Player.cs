using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int[] currentPosition = new int[2];
	public Color darkColor;
	public GameController gameController;
	public int id;
	public float inputTimeout = 0.1f;
	public float inputTimer = 0.1f;
	public Color lightColor;
	public int selectionsMade = 0;
	public Node[] Tokens;

	void Update () {
		if (Input.GetAxis ("Joy" + id + "Select") > 0.8f) {
			gameController.SelectNode (this);
		}
		
		if (inputTimer < inputTimeout) {
			inputTimer += Time.deltaTime;
			return;
		}
		
		if (Input.GetAxis ("Joy" + id + "Y") > 0.8f) {
			Debug.Log (Input.GetAxis ("Joy" + id + "Y"));
			gameController.MovePlayer (this, 2);
			inputTimer = 0.0f;
		}

		if (Input.GetAxis ("Joy" + id + "X") > 0.8f) {
			gameController.MovePlayer (this, 1);
			inputTimer = 0.0f;
		}

		if (Input.GetAxis ("Joy" + id + "Y") < -0.8f) {
			gameController.MovePlayer (this, 0);
			inputTimer = 0.0f;
		}

		if (Input.GetAxis ("Joy" + id + "X") < -0.8f) {
			gameController.MovePlayer (this, 3);
			inputTimer = 0.0f;
		}
	}

	public void MakeSelection () {
		Tokens [selectionsMade].gameObject.SetActive (false);
		selectionsMade++;
	}

	public void EnableAllTokens () {
		Tokens [0].gameObject.SetActive (true);
		Tokens [1].gameObject.SetActive (true);
		selectionsMade = 0;
	}

	public void DestroyTokens () {
		foreach (Node token in Tokens) {
			Destroy (token.gameObject);
		}
	}
}
