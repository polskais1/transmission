using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	public GameController gameController;
	public Slider slider;

	void Start () {
		slider = gameObject.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		slider.value = gameController.timeElapsed / gameController.turnTime;
	}
}
