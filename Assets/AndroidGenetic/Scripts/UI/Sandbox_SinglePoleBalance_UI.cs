using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sandbox_SinglePoleBalance_UI : MonoBehaviour {

    public GameObject trainingPanel;
    public GameObject visualSimPanel;
    public GameObject cart;
    public GameObject pole;
    public GameObject platform;
    public Sandbox_SinglePoleBalance sandbox;
    public VisualController_SinglePoleBalance visualController;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void goToTrainingPanel() {
        cart.SetActive(false);
        pole.SetActive(false);
        platform.SetActive(false);

        visualSimPanel.SetActive(false);
        trainingPanel.SetActive(true);
    }

    public void goToVisualSimPanel() {
        visualController.setNetwork(sandbox.getFittestMember());
        visualController.resetButton();

        cart.SetActive(true);
        pole.SetActive(true);
        platform.SetActive(true);

        trainingPanel.SetActive(false);
        visualSimPanel.SetActive(true);
        
    }

}
