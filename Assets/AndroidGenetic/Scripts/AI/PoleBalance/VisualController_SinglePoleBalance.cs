using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Math.Random;

public class VisualController_SinglePoleBalance : MonoBehaviour {

    public enum State { READY, RUNNING, PAUSED, FAIL_REPORT }

    // initial parameters
    public GameObject cart;
    public GameObject pole;
    public GameObject platform;

    // initial parameter input fields
    public InputField initPoleAngleDegreesInput;
    public InputField initPoleVelocityInput;
    public InputField initPoleAccelerationInput;
    public InputField initCartPositionInput;
    public InputField initCartVelocityInput;
    public InputField initCartAccelerationInput;
    public InputField gravitationalAccelerationInput;
    public InputField cartMassInput;
    public InputField poleMassInput;
    public InputField poleLengthInput;
    public InputField trackLimitInput;
    public InputField poleFailureAngleInput;

    // initial parameters
    private float initPoleAngleDegrees;
    private float initPoleVelocity;
    private float initPoleAcceleration;
    private float initCartPosition;
    private float initCartVelocity;
    private float initCartAcceleration;
    private float gravitationalAcceleration;
    private float cartMass;
    private float poleMass;
    private float poleLength;
    private float trackLimit;
    private float poleFailureAngle;
    private float timeStepSize;
    private float forceMagnitude;

    // live state
    private float poleAngleDegrees;                                 
    private float poleAngleRadians;
    private float poleVelocity;
    private float poleAcceleration;
    private float cartPosition;
    private float cartVelocity;
    private float cartAcceleration;


	private State state;
    private ActivationNetwork network;
    private float time;

    // visual control panel
    public VisualSimulation_ControlPanel visualControlPanel;

    void Start() {
        time = 0f;

        setInitPoleAngleDegrees(initPoleAngleDegreesInput.text);
        setInitPoleVelocity(initPoleVelocityInput.text);
        setInitPoleAcceleration(initPoleAccelerationInput.text);
        setInitCartPosition(initCartPositionInput.text);
        setInitCartVelocity(initCartVelocityInput.text);
        setInitCartAcceleration(initCartAccelerationInput.text);
        setGravitationalAcceleration(gravitationalAccelerationInput.text);
        setCartMass(cartMassInput.text);
        setPoleMass(poleMassInput.text);
        setPoleLength(poleLengthInput.text);
        setTrackLimit(trackLimitInput.text);
        setPoleFailureAngle(poleFailureAngleInput.text);

        timeStepSize = 0.02f;       // possible ui inputs later
        forceMagnitude = 10f;


		changeState (State.READY);
    }


    void Update() {
        if (state.Equals(State.RUNNING)) {
			Transform cartTransform = cart.GetComponent<Transform>();
			Transform poleTransform = pole.GetComponent<Transform>();
			Transform platformTransform = platform.GetComponent<Transform>();

            // set cart and pole postion
            cartTransform.localPosition = new Vector3(cartPosition, 0f, 0f);
            poleTransform.localPosition = new Vector3(cartPosition, cartTransform.localScale.y, 0f);

            // set pole rotatation
            poleTransform.rotation = Quaternion.Euler(0f, 0f, poleAngleDegrees);
        } 
    }
		

    void FixedUpdate() {
		if (state.Equals(State.RUNNING)) {
			if (failureTest()) {
				changeState (State.FAIL_REPORT);
                
            } else {
                advanceState();
            }
        }
    }



	void changeState(State nextState) {
		state = nextState;
		visualControlPanel.updateUIButtonText();

		if (nextState.Equals(State.READY)) {
			synchronizeView ();
		} else if (nextState.Equals(State.FAIL_REPORT)) {
			
		}
	}



	public void startButton() {
		if (network == null) {
			return;
		}
		changeState (State.RUNNING);
	}
	public void pauseButton() {
		changeState (State.PAUSED);
	}
	public void resetButton() {
		changeState (State.READY);
	}



    // return true if fail
    private bool failureTest() {
        if (Mathf.Abs(poleAngleDegrees) > poleFailureAngle) {
            print("pole failure angle exceeded: " + poleAngleDegrees);
            return true;
        } else if (Mathf.Abs(cartPosition) > trackLimit) {
            print("cart position limit exceeded: " + cartPosition);
            return true;
        } else {
            return false;
        }
    }




	void synchronizeView() {
		if (state.Equals(State.READY)) {
			
			Transform cartTransform = cart.GetComponent<Transform>();
			Transform poleTransform = pole.GetComponent<Transform>();
			Transform platformTransform = platform.GetComponent<Transform>();

			// set cart and pole postion
			cartTransform.localPosition = new Vector3(initCartPosition, 0f, 0f);
			poleTransform.localPosition = new Vector3(initCartPosition, cartTransform.localScale.y, 0f);

			// set pole rotatation
			poleTransform.rotation = Quaternion.Euler(0f, 0f, initPoleAngleDegrees);

			// set pole length (2l)
			poleTransform.localScale = new Vector3(poleTransform.localScale.x, 2 * poleLength, poleTransform.localScale.z);

			// set platform location and width
			platformTransform.localPosition = new Vector3(0f, -0.1f, 0f);
			platformTransform.localScale = new Vector3( 2 * trackLimit , 0.1f, 1f);

			time = 0f;
			poleAngleDegrees = initPoleAngleDegrees;
			poleAngleRadians = initPoleAngleDegrees * Mathf.Deg2Rad;
			poleVelocity = initPoleVelocity;
			poleAcceleration = initPoleAcceleration;
			cartPosition = initCartPosition;
			cartVelocity = initCartVelocity;
			cartAcceleration = initCartAcceleration;

		} if (state.Equals (State.RUNNING)) {
			
			Transform cartTransform = cart.GetComponent<Transform>();
			Transform poleTransform = pole.GetComponent<Transform>();
			Transform platformTransform = platform.GetComponent<Transform>();

			// set cart and pole postion
			cartTransform.localPosition = new Vector3(cartPosition, 0f, 0f);
			poleTransform.localPosition = new Vector3(cartPosition, cartTransform.localScale.y, 0f);

			// set pole rotatation
			poleTransform.rotation = Quaternion.Euler(0f, 0f, poleAngleDegrees);
		
		}
	}

    private void advanceState() {
        // get applied force from network based on current state
        double[] input = stateToNetworkInput();
        double[] output = network.Compute(input);
        float force;

        if (output[0] > 0) {
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
        float nextPoleAccelDenom = poleLength * ((4 / 3) - ((poleMass * cosPoleAngle * cosPoleAngle) / massSum));
        float nextPoleAcceleration = nextPoleAccelNum / nextPoleAccelDenom;

        // compute new cart acceleration
        float nextCartAcceleration = (force +
            (poleMass * poleLength * ((poleVelocitySquared * sinPoleAngle) - (poleAcceleration * cosPoleAngle)))) / massSum;

        // compute new pole angle and velocity
        float nextPoleAngleRadians = poleAngleRadians - (timeStepSize * poleVelocity);  // why is this like this
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

        // add elapsed time to accumulator 
        time += timeStepSize;
    }



    private double[] stateToNetworkInput() {

        Dictionary<EInputAll, int> inputMapping = Manager.getInputMapping();
        double[] networkInput = new double[Manager.getSelectedInputCount()];

        if (inputMapping.ContainsKey(EInputAll.POLE_ANGLE_RAD)) {
            networkInput[inputMapping[EInputAll.POLE_ANGLE_RAD]] = System.Convert.ToDouble(poleAngleRadians);
        }
        if (inputMapping.ContainsKey(EInputAll.POLE_VELOCITY)) {
            networkInput[inputMapping[EInputAll.POLE_VELOCITY]] = System.Convert.ToDouble(poleVelocity);
        }
        if (inputMapping.ContainsKey(EInputAll.POLE_ACCELERATION)) {
            networkInput[inputMapping[EInputAll.POLE_ACCELERATION]] = System.Convert.ToDouble(poleAcceleration);
        }
        if (inputMapping.ContainsKey(EInputAll.CART_POSITION)) {
            networkInput[inputMapping[EInputAll.CART_POSITION]] = System.Convert.ToDouble(cartPosition);
        }
        if (inputMapping.ContainsKey(EInputAll.CART_VELOCITY)) {
            networkInput[inputMapping[EInputAll.CART_VELOCITY]] = System.Convert.ToDouble(cartVelocity);
        }
        if (inputMapping.ContainsKey(EInputAll.CART_ACCELERATION)) {
            networkInput[inputMapping[EInputAll.CART_ACCELERATION]] = System.Convert.ToDouble(cartAcceleration);
        }
        if (inputMapping.ContainsKey(EInputAll.GRAVITATIONAL_ACCELERATION)) {
            networkInput[inputMapping[EInputAll.GRAVITATIONAL_ACCELERATION]] = System.Convert.ToDouble(gravitationalAcceleration);
        }
        if (inputMapping.ContainsKey(EInputAll.CART_MASS)) {
            networkInput[inputMapping[EInputAll.CART_MASS]] = System.Convert.ToDouble(cartMass);
        }
        if (inputMapping.ContainsKey(EInputAll.POLE_MASS)) {
            networkInput[inputMapping[EInputAll.POLE_MASS]] = System.Convert.ToDouble(poleMass);
        }
        if (inputMapping.ContainsKey(EInputAll.POLE_LENGTH)) {
            networkInput[inputMapping[EInputAll.POLE_LENGTH]] = System.Convert.ToDouble(poleLength);
        }
        if (inputMapping.ContainsKey(EInputAll.TRACK_LIMIT)) {
            networkInput[inputMapping[EInputAll.TRACK_LIMIT]] = System.Convert.ToDouble(trackLimit);
        }
        if (inputMapping.ContainsKey(EInputAll.POLE_FAILURE_ANGLE)) {
            networkInput[inputMapping[EInputAll.POLE_FAILURE_ANGLE]] = System.Convert.ToDouble(poleFailureAngle);
        }

        return networkInput;
    }



    public void setNetwork(ActivationNetwork network) {
        this.network = network;
    }

    public void setInitPoleAngleDegrees(float initPoleAngleDegrees) {
        this.initPoleAngleDegrees = initPoleAngleDegrees;
		synchronizeView ();
    }
    public void setInitPoleVelocity(float initPoleVelocity) {
        this.initPoleVelocity = initPoleVelocity;
		synchronizeView ();
    }
    public void setInitPoleAcceleration(float initPoleAcceleration) {
		this.initPoleAcceleration = initPoleAcceleration;
		synchronizeView ();
    }
    public void setInitCartPosition(float initCartPosition) {
        this.initCartPosition = initCartPosition;
		synchronizeView ();
    }
    public void setInitCartVelocity(float initCartVelocity) {
        this.initCartVelocity = initCartVelocity;
		synchronizeView ();
    }
    public void setInitCartAcceleration(float initCartAcceleration) {
        this.initCartAcceleration = initCartAcceleration;
		synchronizeView ();
    }
    public void setGravitationalAcceleration(float gravitationalAcceleration) {
        this.gravitationalAcceleration = gravitationalAcceleration;
		synchronizeView ();
    }
    public void setCartMass(float cartMass) {
        this.cartMass = cartMass;
		synchronizeView ();
    }
    public void setPoleMass(float poleMass) {
        this.poleMass = poleMass;
		synchronizeView ();
    }
    public void setPoleLength(float poleLength) {
        this.poleLength = poleLength;
		synchronizeView ();
    }
    public void setTrackLimit(float trackLimit) {
        this.trackLimit = trackLimit;
		synchronizeView ();
    }
    public void setPoleFailureAngle(float poleFailureAngle) {
        this.poleFailureAngle = poleFailureAngle;
		synchronizeView ();
    }
    public void setTimeStepSize(float timeStepSize) {
        this.timeStepSize = timeStepSize;
		synchronizeView ();
    }
    public void setForceMagnitude(float forceMagnitude) {
        this.forceMagnitude = forceMagnitude;
		synchronizeView ();
    }



    public void setInitPoleAngleDegrees(string initPoleAngleDegrees) {
        this.initPoleAngleDegrees = float.Parse(initPoleAngleDegrees);
		synchronizeView ();
    }
    public void setInitPoleVelocity(string initPoleVelocity) {
        this.initPoleVelocity = float.Parse(initPoleVelocity);
		synchronizeView ();
    }
    public void setInitPoleAcceleration(string initPoleAcceleration) {
        this.initPoleAcceleration = float.Parse(initPoleAcceleration);
		synchronizeView ();
    }
    public void setInitCartPosition(string initCartPosition) {
        this.initCartPosition = float.Parse(initCartPosition);
		synchronizeView ();
    }
    public void setInitCartVelocity(string initCartVelocity) {
        this.initCartVelocity = float.Parse(initCartVelocity);
		synchronizeView ();
    }
    public void setInitCartAcceleration(string initCartAcceleration) {
        this.initCartAcceleration = float.Parse(initCartAcceleration);
		synchronizeView ();
    }
    public void setGravitationalAcceleration(string gravitationalAcceleration) {
        this.gravitationalAcceleration = float.Parse(gravitationalAcceleration);
		synchronizeView ();
    }
    public void setCartMass(string cartMass) {
        this.cartMass = float.Parse(cartMass);
		synchronizeView ();
    }
    public void setPoleMass(string poleMass) {
        this.poleMass = float.Parse(poleMass);
		synchronizeView ();
    }
    public void setPoleLength(string poleLength) {
        this.poleLength = float.Parse(poleLength);
		synchronizeView ();
    }
    public void setTrackLimit(string trackLimit) {
        this.trackLimit = float.Parse(trackLimit);
		synchronizeView ();
    }
    public void setPoleFailureAngle(string poleFailureAngle) {
        this.poleFailureAngle = float.Parse(poleFailureAngle);
		synchronizeView ();
    }
    public void setTimeStepSize(string timeStepSize) {
        this.timeStepSize = float.Parse(timeStepSize);
		synchronizeView ();
    }
    public void setForceMagnitude(string forceMagnitude) {
        this.forceMagnitude = float.Parse(forceMagnitude);
		synchronizeView ();
    }

    public void setVisualControlPanel(VisualSimulation_ControlPanel visualControlPanel) {
        this.visualControlPanel = visualControlPanel;
    }

	public State getState() {
		return this.state;
	}
}
