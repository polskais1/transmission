using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	private Material borderMaterial;
	private Material centerMaterial;
	private int colorIndex = 0;
	private bool hasMounted = false;
	private List<Color> hoveringPlayersColors = new List<Color> ();

	public GameObject border;
	public GameObject center;
	public Player futureOwner;
	public int futureState;
	public Player owner;
	public Vector2 position;
	public int state = 0; // 0 is empty, 1 is light, 2 is dark

	void Start () {
		borderMaterial = border.GetComponent<Renderer> ().material;
		centerMaterial = center.GetComponent<Renderer> ().material;
		hoveringPlayersColors.Add (new Color (255, 255, 255));
		InvokeRepeating ("FlashColor", 0, 0.3f);
		hasMounted = true;
	}

	void FlashColor () {
		if (colorIndex == hoveringPlayersColors.Count)
			colorIndex = 0;

		borderMaterial.color = hoveringPlayersColors [colorIndex];
		colorIndex++;
	}

	public void SetPosition (Vector2 newPosition) {
		position = newPosition;
		transform.Translate (position);
	}

	public void AssignPlayerHover (Player player) {
		hoveringPlayersColors.Add (player.darkColor);
		colorIndex = hoveringPlayersColors.Count - 1;
		if (hasMounted)
			FlashColor ();
	}

	public void RemovePlayerHover (Player player) {
		colorIndex = 0;
		FlashColor ();
		hoveringPlayersColors.Remove (player.darkColor);
	}

	public void AssignPlayer (Player player, int newState) {
		owner = player;
		centerMaterial.color = newState == 1 ? player.lightColor : player.darkColor;
		state = newState;
	}
}
