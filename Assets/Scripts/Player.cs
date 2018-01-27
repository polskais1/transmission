using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Color color;
	public int[] currentPosition = new int[2];
	public GameController gameController;
	public List<KeyCode> playerKeyCodes = new List<KeyCode> (); // indx 0 == up, indx 1 == right, indx 2 == down, indx 3 == left, 4 == select
	public int selectionsMade = 0;

	void Update () {
		for (int i = 0; i < 4; i++) {
			if (Input.GetKeyDown (playerKeyCodes [i])) {
				gameController.MovePlayer (this, i);
			}
		}

		if (Input.GetKeyDown (playerKeyCodes[4]))
			gameController.SelectNode (this);
	}
}
