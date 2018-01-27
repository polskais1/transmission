using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	// Prefabs
	public Node NodePrefab;
	public Player PlayerPrefab;

	// Game Settings
	public int gridX;
	public int gridY;
	public float padding;
	public float timeElapsed; // Since last turn
	public float turnTime;

	private Node[,] nodes;
	private Player player1;
	private Player player2;

	void Start () {
		SetupGame ();
	}

	void Update () {
		if (timeElapsed >= turnTime) {
			EndTurn ();
			return;
		}
		timeElapsed += Time.deltaTime;
	}

	void SetupGame () {
		// Setup game board
		nodes = new Node[gridX, gridY];
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Node newNode = Instantiate (NodePrefab);
				newNode.SetPosition (new Vector2 ((x - (gridX + (gridX - 1) * padding) / 2) + (x * padding), (y - (gridY + (gridY - 1) * padding) / 2) + (y * padding)));
				nodes [x, y] = newNode;
			}
		}

		// Setup players
		player1 = Instantiate (PlayerPrefab);
		player1.gameController = this;
		List<KeyCode> player1KeyCodes = new List<KeyCode> ();
		player1KeyCodes.Add (KeyCode.UpArrow);
		player1KeyCodes.Add (KeyCode.RightArrow);
		player1KeyCodes.Add (KeyCode.DownArrow);
		player1KeyCodes.Add (KeyCode.LeftArrow);
		player1KeyCodes.Add (KeyCode.Return);
		player1.playerKeyCodes = player1KeyCodes;
		player1.color = new Color (255, 0, 0); // red
		nodes [0, 0].AssignPlayerHover (player1);
		player1.currentPosition = new int[2] { 0, 0 };

		player2 = Instantiate (PlayerPrefab);
		player2.gameController = this;
		List<KeyCode> player2KeyCodes = new List<KeyCode> ();
		player2KeyCodes.Add (KeyCode.W);
		player2KeyCodes.Add (KeyCode.D);
		player2KeyCodes.Add (KeyCode.S);
		player2KeyCodes.Add (KeyCode.A);
		player2KeyCodes.Add (KeyCode.Space);
		player2.playerKeyCodes = player2KeyCodes;
		player2.color = new Color (0, 0, 255); // blue
		nodes [gridX - 1, gridY - 1].AssignPlayerHover (player2);
		player2.currentPosition = new int[2] { gridX - 1, gridY - 1 };

		timeElapsed = 0;
	}

	void EndTurn () {
//		ProgressNodes ();
		timeElapsed = 0;
		player1.selectionsMade = 0;
		player2.selectionsMade = 0;
	}

	public void MovePlayer (Player player, int movementIndex) {
		int newPosX = player.currentPosition [0];
		int newPosY = player.currentPosition [1];
		Node targetNode;

		switch (movementIndex) {
		case(0): // up
			newPosY += 1;
			break;
		case(1): // right
			newPosX += 1;
			break;
		case(2): // down
			newPosY -= 1;
			break;
		case(3): // left
			newPosX -= 1;
			break;
		}

		if (newPosX >= 0 && newPosY >= 0 && newPosX < gridX && newPosY < gridY) {
			targetNode = nodes [newPosX, newPosY];
			nodes [player.currentPosition [0], player.currentPosition [1]].RemovePlayerHover (player);
			player.currentPosition = new int[2] { newPosX, newPosY };
			targetNode.AssignPlayerHover (player);
		}
	}

	public void SelectNode (Player player) {
		if (player.selectionsMade < 2) {
			Node targetNode = nodes [player.currentPosition [0], player.currentPosition [1]];
			if (targetNode.state != 1.0f) {
				targetNode.AssignPlayer (player, 1.0f);
				player.selectionsMade++;
			}
		}
	}
}
