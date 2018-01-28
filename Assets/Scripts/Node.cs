using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	private Material borderMaterial;
	private Material centerMaterial;
	private Color colorA;
	private Color colorB;
	private float colorAnimationCountdown = 1.0f;
	private float colorAnimationTime = 1.0f;
	private int colorIndex = 0;
	private bool hasMounted = false;
	private List<Color> hoveringPlayersColors = new List<Color> ();

	public GameObject border;
	public GameObject center;
	public Player futureOwner;
	public int futureState;
	public Player owner;
	public Vector2 position;
	public bool shouldAnimate = false;
	public int state = 0; // 0 is empty, 1 is light, 2 is dark

	void Start () {
		borderMaterial = border.GetComponent<Renderer> ().material;
		centerMaterial = center.GetComponent<Renderer> ().material;
		hoveringPlayersColors.Add (new Color (1, 1, 1));
		InvokeRepeating ("FlashColor", 0, 0.3f);
		hasMounted = true;
	}

	void Update () {
		if (colorAnimationCountdown < colorAnimationTime && shouldAnimate) {
			centerMaterial.color = Color.Lerp (colorA, colorB, colorAnimationCountdown);
			colorAnimationCountdown += Time.deltaTime;
		} else if (shouldAnimate) {
			shouldAnimate = false;
		}
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

	public void AssignPlayerToEntireNode (Player player) {
		if (centerMaterial == null)
			centerMaterial = center.GetComponent<Renderer> ().material;
		if (borderMaterial == null)
			borderMaterial = border.GetComponent<Renderer> ().material;
		centerMaterial.color = player.darkColor;
		hoveringPlayersColors = new List<Color> ();
		hoveringPlayersColors.Add (player.darkColor);
	}

	public void AssignPlayerWithAnimation (Player player, int newState) {
		if (newState == 1) {
			colorA = owner != null ? owner.lightColor : new Color (1, 1, 1);
			colorB = player.lightColor;
		} else {
			colorA = player.lightColor;
			colorB = player.darkColor;
		}
		owner = player;
		colorAnimationCountdown = 0.0f;
		state = newState;
	}
}
