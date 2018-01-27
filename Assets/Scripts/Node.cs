using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	private Material borderMaterial;
	private Material centerMaterial;
	private int colorIndex = 0;
	private bool hasMounted = false;
	private List<Color> hoveringPlayersColors = new List<Color> ();
	private Player owner;

	public GameObject border;
	public GameObject center;
	public float state; // 0 is empty, 0.5 is light, 1 is dark
	public Vector2 position;

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
		hoveringPlayersColors.Add (player.color);
		colorIndex = hoveringPlayersColors.Count - 1;
		if (hasMounted)
			FlashColor ();
	}

	public void RemovePlayerHover (Player player) {
		colorIndex = 0;
		FlashColor ();
		hoveringPlayersColors.Remove (player.color);
	}

	public void AssignPlayer (Player player, float alpha) {
		centerMaterial.color = player.color;
		state = alpha;
	}
}
