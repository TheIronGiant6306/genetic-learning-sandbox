using UnityEngine;
using System.Collections;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Math.Random;

public class PoleBalancingSimulation {

	private ActivationNetwork network;

	private float poleAngleDegrees;
	private float poleAngleRadians;
	private float poleVelocity;
	private float poleAcceleration;
	private float cartPosition;
	private float cartVelocity;
	private float cartAcceleration;
	private float gravitationalAcceleration;
	private float cartMass;
	private float poleMass;
	private float poleLength;
	private float trackLimit;
	private float poleFailureAngle;
	private float timeStepSize;
	private float forceMagnitude;
	private float successThreshold;

	private float time;


	public PoleBalancingSimulation(ActivationNetwork network) {
		this.network = network;


		System.Random rnd = new System.Random ();
		IRandomNumberGenerator gaussianRV = new GaussianGenerator (0f,5f,rnd.Next());

		// initialize start state
		poleAngleDegrees = gaussianRV.Next();
		poleAngleRadians = poleAngleDegrees * Mathf.Deg2Rad;
		poleVelocity = 0f;
		poleAcceleration = 0f;
		cartPosition = 0f;
		cartVelocity = 0f;
		cartAcceleration = 0f;
		gravitationalAcceleration = -9.81f;
		cartMass = 1f;
		poleMass = 0.1f;
		poleLength = 0.5f;
		trackLimit = 2.4f;
		poleFailureAngle = 12f;
		timeStepSize = 0.02f;
		forceMagnitude = 10f;
		successThreshold = 20f;
		time = 0f;

	}

	public double run() {

		bool running = true;

		while (running) {
			if (failureTest()) {
				running = false;
			} else if (time > successThreshold) {
				running = false;
			}

			advanceState ();
		}

		return System.Convert.ToDouble (time);
	}
		

	private void advanceState() {

		// get next output from network
		double[] input = stateToNetworkInput();
		double[] output = network.Compute (input);
		float force;

		if (output [0] > 0) {
			force = forceMagnitude;
		} else {
			force = -forceMagnitude;
		}


		// compute intermediate calculations for reuse
		float sinPoleAngle = Mathf.Sin(poleAngleRadians);
		float cosPoleAngle = Mathf.Cos(poleAngleRadians);
		float poleVelocitySquared = poleVelocity * poleVelocity;
		float massSum = cartMass + poleMass;

		// compute new pole acceleration 
		float internalNumFraction = (-force - (poleMass * poleLength * poleVelocitySquared * sinPoleAngle)) / massSum;
		float nextPoleAccelNum = (gravitationalAcceleration * sinPoleAngle) + (cosPoleAngle * internalNumFraction); 
		float nextPoleAccelDenom = poleLength * ((4/3)-((poleMass * cosPoleAngle * cosPoleAngle) / massSum));
		float nextPoleAcceleration = nextPoleAccelNum / nextPoleAccelDenom;

		// compute new cart acceleration
		float nextCartAcceleration = (force +
			(poleMass * poleLength * ((poleVelocitySquared * sinPoleAngle) - (poleAcceleration * cosPoleAngle)))) / massSum;

		// compute new pole angle and velocity
		float nextPoleAngleRadians = poleAngleRadians - (timeStepSize * poleVelocity);	// why is this like this
		float nextPoleVelocity = poleVelocity + (timeStepSize * poleAcceleration);

		// compute new cart position and velocity
		float nextCartPosition = cartPosition + (timeStepSize * cartVelocity);
		float nextCartVelocity = cartVelocity + (timeStepSize * cartAcceleration);


		// update state
		cartPosition = nextCartPosition;
		cartVelocity = nextCartVelocity;
		cartAcceleration = nextCartAcceleration;
		poleAngleRadians = nextPoleAngleRadians;
		poleAngleDegrees = nextPoleAngleRadians * Mathf.Rad2Deg;
		poleVelocity = nextPoleVelocity;
		poleAcceleration = nextPoleAcceleration;

		time += timeStepSize;
	}


	private double[] stateToNetworkInput() {
		double[] networkInput = new double[10];
		networkInput [0] = System.Convert.ToDouble (poleAngleRadians);
		networkInput [1] = System.Convert.ToDouble (poleVelocity);
		networkInput [2] = System.Convert.ToDouble (poleAcceleration);
		networkInput [3] = System.Convert.ToDouble (cartPosition);
		networkInput [4] = System.Convert.ToDouble (cartVelocity);
		networkInput [5] = System.Convert.ToDouble (cartAcceleration);
		networkInput [6] = System.Convert.ToDouble (gravitationalAcceleration);
		networkInput [7] = System.Convert.ToDouble (cartMass);
		networkInput [8] = System.Convert.ToDouble (poleMass);
		networkInput [9] = System.Convert.ToDouble (poleLength);

		return networkInput;
	}


	// return true if fail
	private bool failureTest() {

		if (Mathf.Abs (poleAngleDegrees) > poleFailureAngle) {
			return true;
		} else if (Mathf.Abs (cartPosition) > trackLimit) {
			return true;
		} else {
			return false;
		}
	}


}
