using UnityEngine;
using System.Collections;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Math.Random;

public class PoleBalancingVisualSimulation : MonoBehaviour {

	public enum SystemStateLabel { STOPPED,RUNNING }

	// initial parameters
	public GameObject cart;
	public GameObject pole;
	public float initPoleAngleDegrees = 0f;
	public float initPoleVelocity = 0f;
	public float initPoleAcceleration = 0f;
	public float initCartPosition = 0f;
	public float initCartVelocity = 0f;
	public float initCartAcceleration = 0f;
	public float gravitationalAcceleration = -9.81f;
	public float cartMass = 1f;
	public float poleMass = 0.1f;
	public float poleLength = 0.5f;
	public float trackLimit = 2.4f;
	public float poleFailureAngle = 12f;
	public float timeStepSize = 0.02f;
	public float forceMagnitude = 10f;

	// one shots
	public bool reset = false;
	public bool start = false;
	public bool stop = false;

	private SystemStateLabel stateLabel;
	private SystemState state;
	private ActivationNetwork network;
	//private float time;
	//private float lastForce;


	void Start () {
		stateLabel = SystemStateLabel.STOPPED;
		resetSystem ();
	}


	void Update () {
		if (stateLabel.Equals(SystemStateLabel.RUNNING)) {
			Transform cartTransform = cart.GetComponent<Transform> ();
			Transform poleTransform = pole.GetComponent<Transform> ();

			// set cart and pole postion
			cartTransform.localPosition = new Vector3(state.cartPosition,0f,0f);
			poleTransform.localPosition = new Vector3(state.cartPosition,cartTransform.localScale.y,0f);

			// set pole rotatation
			poleTransform.rotation = Quaternion.Euler(0f,0f,state.poleAngleDegrees);
		}
	}

	void FixedUpdate () {
		if (stateLabel.Equals(SystemStateLabel.STOPPED)) {
			if (start) {
				if (network == null) {
					start = false;
					return;
				}
				stateLabel = SystemStateLabel.RUNNING;
				print ("start");
				start = false;
			} else if (reset) {
				resetSystem ();
				//advanceState ();
				print ("manual reset, end simulation");
				reset = false;
			} else if (stop) {
				stateLabel = SystemStateLabel.STOPPED;
				stop = false;
			}
		} else if (stateLabel.Equals(SystemStateLabel.RUNNING)) {
			if (stop) {
				stateLabel = SystemStateLabel.STOPPED;
				print ("stop");
				stop = false;
			} else if (reset) {
				resetSystem ();
				print ("manual reset, end simulation");
				reset = false;
			} else if (failureTest ()) {
				resetSystem ();
			} else {
				// simulation is running and conditions are valid
				advanceState();
			}
		}
	}

	public void setNetwork(ActivationNetwork network) {
		this.network = network;
	}


	// return true if fail
	private bool failureTest() {

		if (Mathf.Abs (state.poleAngleDegrees) > poleFailureAngle) {
			print ("pole failure angle exceeded: "+state.poleAngleDegrees);
			return true;
		} else if (Mathf.Abs (state.cartPosition) > trackLimit) {
			print ("cart position limit exceeded: "+state.cartPosition);
			return true;
		} else {
			return false;
		}
	}




	public void resetSystem() {
		stateLabel = SystemStateLabel.STOPPED;

		//time = 0f;
		//lastForce = 0f;
		Time.fixedDeltaTime = timeStepSize;

		state = new SystemState ();
		state.poleAngleDegrees = initPoleAngleDegrees;
		state.poleAngleRadians = initPoleAngleDegrees * Mathf.Deg2Rad;
		state.poleVelocity = initPoleVelocity;
		state.poleAcceleration = initPoleAcceleration;
		state.cartPosition = initCartPosition;
		state.cartVelocity = initCartVelocity;
		state.cartAcceleration = initCartAcceleration;

		Transform cartTransform = cart.GetComponent<Transform> ();
		Transform poleTransform = pole.GetComponent<Transform> ();

		// set pole length (2l)
		poleTransform.localScale = new Vector3(poleTransform.localScale.x,2*poleLength,poleTransform.localScale.z);

		// set cart and pole initial postion
		cartTransform.localPosition = new Vector3(state.cartPosition,0f,0f);
		poleTransform.localPosition = new Vector3(state.cartPosition,cartTransform.localScale.y,0f);

		// set pole rotatation
		poleTransform.rotation = Quaternion.Euler(0f,0f,state.poleAngleDegrees);
	}


	private void advanceState() {
		// get applied force from network based on current state
		double[] input = stateToNetworkInput();
		double[] output = network.Compute (input);
		float force;

		if (output [0] > 0) {
			force = forceMagnitude;
		} else {
			force = -forceMagnitude;
		}
			

		// compute intermediate calculations for reuse
		float sinPoleAngle = Mathf.Sin(state.poleAngleRadians);
		float cosPoleAngle = Mathf.Cos(state.poleAngleRadians);
		float poleVelocitySquared = state.poleVelocity * state.poleVelocity;
		float massSum = cartMass + poleMass;

		// compute new pole acceleration 
		float internalNumFraction = (-force - (poleMass * poleLength * poleVelocitySquared * sinPoleAngle)) / massSum;
		float nextPoleAccelNum = (gravitationalAcceleration * sinPoleAngle) + (cosPoleAngle * internalNumFraction); 
		float nextPoleAccelDenom = poleLength * ((4/3)-((poleMass * cosPoleAngle * cosPoleAngle) / massSum));
		float nextPoleAcceleration = nextPoleAccelNum / nextPoleAccelDenom;

		// compute new cart acceleration
		float nextCartAcceleration = (force +
			(poleMass * poleLength * ((poleVelocitySquared * sinPoleAngle) - (state.poleAcceleration * cosPoleAngle)))) / massSum;


		//state.cartAcceleration = nextCartAcceleration;
		//state.poleAcceleration = nextPoleAcceleration;


		// compute new pole angle and velocity
		float nextPoleAngleRadians = state.poleAngleRadians - (timeStepSize * state.poleVelocity);	// why is this like this
		float nextPoleVelocity = state.poleVelocity + (timeStepSize * state.poleAcceleration);

		// compute new cart position and velocity
		float nextCartPosition = state.cartPosition + (timeStepSize * state.cartVelocity);
		float nextCartVelocity = state.cartVelocity + (timeStepSize * state.cartAcceleration);


		// update state
		state.cartPosition = nextCartPosition;
		state.cartVelocity = nextCartVelocity;
		state.cartAcceleration = nextCartAcceleration;
		state.poleAngleRadians = nextPoleAngleRadians;
		state.poleAngleDegrees = nextPoleAngleRadians * Mathf.Rad2Deg;
		state.poleVelocity = nextPoleVelocity;
		state.poleAcceleration = nextPoleAcceleration;
	}


	private double[] stateToNetworkInput() {
		double[] networkInput = new double[10];
		networkInput [0] = System.Convert.ToDouble (state.poleAngleRadians);
		networkInput [1] = System.Convert.ToDouble (state.poleVelocity);
		networkInput [2] = System.Convert.ToDouble (state.poleAcceleration);
		networkInput [3] = System.Convert.ToDouble (state.cartPosition);
		networkInput [4] = System.Convert.ToDouble (state.cartVelocity);
		networkInput [5] = System.Convert.ToDouble (state.cartAcceleration);
		networkInput [6] = System.Convert.ToDouble (gravitationalAcceleration);
		networkInput [7] = System.Convert.ToDouble (cartMass);
		networkInput [8] = System.Convert.ToDouble (poleMass);
		networkInput [9] = System.Convert.ToDouble (poleLength);

		return networkInput;
	}
}
