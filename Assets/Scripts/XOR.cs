using UnityEngine;
using System.Collections;
using AForge.Neuro;
using AForge.Neuro.Learning;

public class XOR : MonoBehaviour {

	public int hiddenNeuronCount = 4;
	public double sigmoidAlpha = 2;

	ActivationNetwork network;
	BackPropagationLearning teacher;
	double[][] input;
	double[][] output;
	int epochs;

	// Use this for initialization
	void Start () {

		epochs = 0;

		// inputs : array of observations, each observation is array of features
		input = new double[4][]; 

		input [0] = new double[] { 0, 0 };
		input [1] = new double[] { 0, 1 };
		input [2] = new double[] { 1, 0 };
		input [3] = new double[] { 1, 1 };



		// outputs : array of corresponding outputs, in this case only one
		output = new double[4][];

		output [0] = new double[] { 0 };
		output [1] = new double[] { 1 };
		output [2] = new double[] { 1 };
		output [3] = new double[] { 0 };



		// network
		network = new ActivationNetwork (new SigmoidFunction (sigmoidAlpha) , 2, hiddenNeuronCount, 1);			// sigmoid activation function, 2 inputs, 2 hidden neurons (one layer), 1 output 


		// network teacher
		teacher = new BackPropagationLearning(network);

	}

	public void train(int count) {
		// run learning epoch loop
		double error = 1;
		for (int i=0; i<count; i++) {
			epochs++;
			error = teacher.RunEpoch(input, output);
		}
		print ("epoch: "+epochs+" error: "+error);
	}


	public void compute(int input1, int input2) {
		double[] input = new double[] { input1, input2 };

		double[] output = network.Compute (input);
		print ("input1: "+input1+" input2: "+input2+" output: "+output[0]);
	}



	// Update is called once per frame
	void Update () {

	}


}
