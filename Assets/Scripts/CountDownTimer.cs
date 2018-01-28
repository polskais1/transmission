using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountDownTimer : MonoBehaviour {
	public GameController gameController;
	public Text text;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
		text.text = "3";
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.gameStartCountdown) {
			text.text = System.Math.Ceiling(gameController.gameStartCountDownTime - gameController.gameStartCountDownTimeElapsed).ToString();
		}
	}
}
