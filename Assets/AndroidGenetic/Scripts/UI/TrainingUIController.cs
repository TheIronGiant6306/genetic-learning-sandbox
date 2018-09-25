using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingUIController : MonoBehaviour {
    public Sandbox_SinglePoleBalance sandbox;
    public InputField generations;
    public InputField bestFitness;
    public InputField avgFitness;
    public InputField popSize;
    public Text trainButtonText;

    private bool trainingInProgress;

	// Use this for initialization
	void Start () {
        sandbox.setUITrainingController(this);
        popSize.text = sandbox.getPopulationSize().ToString();
        generations.text = sandbox.getGenerationCount().ToString();
        bestFitness.text = sandbox.getBestFitness().ToString();
        avgFitness.text = sandbox.getAvgFitness().ToString();
        trainingInProgress = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void handleTrainingButton() {
        if (trainingInProgress) {
            trainButtonText.text = "Train";
            sandbox.setAdvance(false);
            trainingInProgress = false;
        } else {
            trainButtonText.text = "Stop";
            sandbox.setAdvance(true);
            trainingInProgress = true;
        }
    }

    public void setGenerationCount(int generationCount) {
        generations.text = generationCount.ToString();
    }
    public void setBestFitness(double fitness) {
        bestFitness.text = fitness.ToString();
    }
    public void setAvgFitness(double fitness) {
        avgFitness.text = fitness.ToString();
    }
}
