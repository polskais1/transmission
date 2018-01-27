using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public int gridX;
	public int gridY;
	public Node NodePrefab;
	public float padding;

	private Node[,] nodes;

	void Start () {
		SetupGame ();
	}

	void Update () {
	}

	void SetupGame () {
		nodes = new Node[gridX, gridY];
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Node newNode = Instantiate (NodePrefab);
				newNode.transform.Translate (new Vector2 ((x - (gridX + (gridX - 1) * padding) / 2) + (x * padding), (y - (gridY + (gridY - 1) * padding) / 2) + (y * padding)));
				nodes [x, y] = newNode;
			}
		}
	}
}
