using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int[] currentPosition = new int[2];
	public Color darkColor;
	public GameController gameController;
	public int id;
	public Color lightColor;
	public List<KeyCode> playerKeyCodes = new List<KeyCode> (); // indx 0 == up, indx 1 == right, indx 2 == down, indx 3 == left, 4 == select
	public int selectionsMade = 0;
	public Node[] Tokens;

	void Update () {
		for (int i = 0; i < 4; i++) {
			if (Input.GetKeyDown (playerKeyCodes [i])) {
				gameController.MovePlayer (this, i);
			}
		}

		if (Input.GetKeyDown (playerKeyCodes[4]))
			gameController.SelectNode (this);
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
