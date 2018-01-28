using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Add in controller support
// TODO Add in more players

public class GameController : MonoBehaviour {
	// Prefabs
	public Node NodePrefab;
	public Player PlayerPrefab;
	public Text Prompt;
	public Slider TimerSlider;
	public Text Title;
	public Text WinnerText;

	// Game Settings
	public bool gameInProgress = false;
	public int gridX;
	public int gridY;
	public float padding;
	public float timeElapsed; // Since last turn
	public float turnTime;

	private float breakCountdown;
	private float breakTime = 2.0f;
	private bool gameOver = false;
	private int globalCount = 0;
	private bool inBreak = false;
	private Node[,] nodes;
	private Player player1;
	private Node[] player1TokensRemaining;
	private Player player2;
	private Node[] player2TokensRemaining;
	private float playArea = 7f;
	private float winScreenTime = 3.0f;
	private float winScreenCountdown;

	void Update () {
		if (inBreak) {
			breakCountdown += Time.deltaTime;
			if (breakCountdown >= breakTime)
				BeginNextTurn ();
		} else if (gameInProgress) {
			if (timeElapsed >= turnTime) {
				EndTurn ();
				return;
			}
			timeElapsed += Time.deltaTime;
		} else if (gameOver) {
			winScreenCountdown += Time.deltaTime;
			if (winScreenCountdown >= winScreenTime)
				ReturnToMenu ();
		} else if (!gameOver && Input.anyKey) {
			SetupGame ();
		}
	}

	void ReturnToMenu () {
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Destroy (nodes [x, y].gameObject);
			}
		}
		Destroy (player1.gameObject);
		Destroy (player2.gameObject);
		gameOver = false;
		WinnerText.text = "";
		WinnerText.color = new Color (1, 1, 1);
		Title.gameObject.SetActive (true);
		Prompt.gameObject.SetActive (true);
	}

	void SetupGame () {
		// Hide UI
		TimerSlider.gameObject.SetActive (true);
		Title.gameObject.SetActive (false);
		Prompt.gameObject.SetActive (false);

		// Setup game board
		nodes = new Node[gridX, gridY];
		float totalSize = gridX + (padding * (gridX - 1));
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Node newNode = Instantiate (NodePrefab);
				// TODO add in scale as well
//				float scaleX = gridX / playArea;
//				float scaleY = gridY / playArea;
//				newNode.transform.localScale = new Vector2 (scaleX, scaleY);

				float posX = ((1 + padding) * x) + 0.5f - (totalSize / 2);
				float posY = ((1 + padding) * y) - (totalSize / 2);
				newNode.SetPosition (new Vector2 (posX, posY));
				nodes [x, y] = newNode;
			}
		}

		// Setup players
		player1 = Instantiate (PlayerPrefab);
		player1.gameController = this;
		player1.id = 1;
		List<KeyCode> player1KeyCodes = new List<KeyCode> ();
		player1KeyCodes.Add (KeyCode.UpArrow);
		player1KeyCodes.Add (KeyCode.RightArrow);
		player1KeyCodes.Add (KeyCode.DownArrow);
		player1KeyCodes.Add (KeyCode.LeftArrow);
		player1KeyCodes.Add (KeyCode.Return);
		player1.playerKeyCodes = player1KeyCodes;
		player1.darkColor = new Color (1.0f, 0.0f, 0.0f); // red
		player1.lightColor = new Color (1.0f, 0.4f, 0.4f); // red
		nodes [0, 0].AssignPlayerHover (player1);
		player1.currentPosition = new int[2] { 0, 0 };
		player1.Tokens = new Node[] { Instantiate (NodePrefab), Instantiate (NodePrefab) };
		player1.Tokens [0].SetPosition (new Vector2 (-8f, -4f));
		player1.Tokens [0].AssignPlayerToEntireNode (player1);
		player1.Tokens [1].SetPosition (new Vector2 (-6.5f, -4f));
		player1.Tokens [1].AssignPlayerToEntireNode (player1);

		player2 = Instantiate (PlayerPrefab);
		player2.gameController = this;
		player2.id = 2;
		List<KeyCode> player2KeyCodes = new List<KeyCode> ();
		player2KeyCodes.Add (KeyCode.W);
		player2KeyCodes.Add (KeyCode.D);
		player2KeyCodes.Add (KeyCode.S);
		player2KeyCodes.Add (KeyCode.A);
		player2KeyCodes.Add (KeyCode.Space);
		player2.playerKeyCodes = player2KeyCodes;
		player2.darkColor = new Color (0.0f, 0.0f, 1.0f); // blue
		player2.lightColor = new Color (0.4f, 0.4f, 1.0f); // blue
		nodes [gridX - 1, gridY - 1].AssignPlayerHover (player2);
		player2.currentPosition = new int[2] { gridX - 1, gridY - 1 };
		player2.Tokens = new Node[] { Instantiate (NodePrefab), Instantiate (NodePrefab) };
		player2.Tokens [0].SetPosition (new Vector2 (8f, -4f));
		player2.Tokens [0].AssignPlayerToEntireNode (player2);
		player2.Tokens [1].SetPosition (new Vector2 (6.5f, -4f));
		player2.Tokens [1].AssignPlayerToEntireNode (player2);

		timeElapsed = 0;
		winScreenCountdown = 0.0f;

		gameInProgress = true;
	}

	void EndTurn () {
		ProgressNodes ();
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
			if (targetNode.state != 2) {
				targetNode.AssignPlayer (player, 2);
				targetNode.futureState = 2;
				player.MakeSelection ();
			}
		}
	}

	void ProgressNodes () {
		int count = 0;
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Node node = nodes [x, y];
				if (node.state != 2) {
					Player advantage = CheckAdvantage (x, y);
					if (advantage != null) {
						node.shouldAnimate = true;
						if (node.owner != null && advantage.id == node.owner.id) {
							node.futureState = 2;
						} else {
							node.futureOwner = advantage;
							node.futureState = 1;
						}
					}
				} else {
					count += 1;
				}
			}
		}
		if (count == gridX * gridY)
			EndGame ();
		else
			BeginBreak ();
	}

	Player CheckAdvantage (int x, int y) {
		int player1Score = 0;
		int player2Score = 0;

		if (y < gridY - 1) {
			Node topNode = nodes [x, y + 1];
			if (topNode.owner != null && topNode.state == 2) {
				if (topNode.owner.id == player1.id) {
					player1Score++;
				} else {
					player2Score++;
				}
			}
		}

		if (x < gridX - 1) {
			Node rightNode = nodes [x + 1, y];
			if (rightNode.owner != null && rightNode.state == 2) {
				if (rightNode.owner.id == player1.id) {
					player1Score++;
				} else {
					player2Score++;
				}
			}
		}

		if (y > 0) {
			Node botNode = nodes [x, y - 1];
			if (botNode.owner != null && botNode.state == 2) {
				if (botNode.owner.id == player1.id) {
					player1Score++;
				} else {
					player2Score++;
				}
			}
		}

		if (x > 0) {
			Node leftNode = nodes [x - 1, y];
			if (leftNode.owner != null && leftNode.state == 2) {
				if (leftNode.owner.id == player1.id) {
					player1Score++;
				} else {
					player2Score++;
				}
			}
		}

		if (player1Score > player2Score)
			return player1;
		if (player1Score < player2Score)
			return player2;
		
		return null;
	}

	void BeginNextTurn () {
		inBreak = false;
		timeElapsed = 0;
		player1.EnableAllTokens ();
		player2.EnableAllTokens ();
	}

	void BeginBreak () {
		breakCountdown = 0.0f;
		inBreak = true;

		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Node node = nodes [x, y];
				if (node.futureOwner != null) {
					node.AssignPlayerWithAnimation (node.futureOwner, node.futureState);
				}
				if (node.futureState != null)
					node.state = node.futureState;
			}
		}
	}

	void EndGame () {
		TimerSlider.gameObject.SetActive (false);
		gameInProgress = false;
		timeElapsed = 0;
		Player winner = DetermineWinner ();
		if (winner != null) {
			WinnerText.text = "Player " + winner.id + " Wins!";
			WinnerText.color = winner.lightColor;
		} else
			WinnerText.text = "Draw";

		player1.DestroyTokens ();
		player2.DestroyTokens ();
		gameOver = true;
	}

	Player DetermineWinner () {
		int player1Score = 0;
		int player2Score = 0;
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				if (nodes [x, y].owner.id == 1)
					player1Score++;
				else
					player2Score++;
			}
		}
		if (player1Score > player2Score)
			return player1;
		if (player1Score < player2Score)
			return player2;
		return null;
	}
}
