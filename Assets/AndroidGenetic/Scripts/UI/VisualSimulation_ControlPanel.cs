using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSimulation_ControlPanel : MonoBehaviour {

    public Text startPauseResetButtonText;
    public VisualController_SinglePoleBalance visualController;

	void Awake() {
		//visualController.setVisualControlPanel(this);
	}

	// Use this for initialization
	void Start () {
		updateUIButtonText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void handleButtonPress() {

		if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.READY)) {
			visualController.startButton ();
		} else if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.RUNNING)) {
			visualController.pauseButton ();
		} else if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.PAUSED)) {
			visualController.startButton ();
		} else if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.FAIL_REPORT)) {
			visualController.resetButton ();
		}

	}

    public void updateUIButtonText() {

		if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.READY)) {
			startPauseResetButtonText.text = "Start";
		} else if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.RUNNING)) {
			startPauseResetButtonText.text = "Pause";
		} else if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.PAUSED)) {
			startPauseResetButtonText.text = "Resume";
		} else if (visualController.getState ().Equals (VisualController_SinglePoleBalance.State.FAIL_REPORT)) {
			startPauseResetButtonText.text = "Reset";
		}
			
    }
}
