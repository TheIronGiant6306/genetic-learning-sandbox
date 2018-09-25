using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Math.Random;

public class TrainingController_SinglePoleBalance {

    private Sandbox_SinglePoleBalance parentSandbox;
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


    public TrainingController_SinglePoleBalance(ActivationNetwork network, Sandbox_SinglePoleBalance parentSandbox) {
        this.network = network;
        this.parentSandbox = parentSandbox;


        System.Random rnd = new System.Random();                                                           // should be a system param??
        IRandomNumberGenerator gaussianRV;

        // initialize start state
        if (parentSandbox.getInitPoleAngleDegrees_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            poleAngleDegrees = parentSandbox.getInitPoleAngleDegrees_Static();
        } else if (parentSandbox.getInitPoleAngleDegrees_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getInitPoleAngleDegrees_Mean();
            float stdDev = parentSandbox.getInitPoleAngleDegrees_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            poleAngleDegrees = gaussianRV.Next();
        }
        poleAngleRadians = poleAngleDegrees * Mathf.Deg2Rad;

        if (parentSandbox.getInitPoleVelocity_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            poleVelocity = parentSandbox.getInitPoleVelocity_Static();
        } else if (parentSandbox.getInitPoleVelocity_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getInitPoleVelocity_Mean();
            float stdDev = parentSandbox.getInitPoleVelocity_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            poleVelocity = gaussianRV.Next();
        }

        if (parentSandbox.getInitPoleAcceleration_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            poleAcceleration = parentSandbox.getInitPoleAcceleration_Static();
        } else if (parentSandbox.getInitPoleAcceleration_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getInitPoleAcceleration_Mean();
            float stdDev = parentSandbox.getInitPoleAcceleration_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            poleAcceleration = gaussianRV.Next();
        }

        if (parentSandbox.getInitCartPosition_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            cartPosition = parentSandbox.getInitCartPosition_Static();
        } else if (parentSandbox.getInitCartPosition_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getInitCartPosition_Mean();
            float stdDev = parentSandbox.getInitCartPosition_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            cartPosition = gaussianRV.Next();
        }

        if (parentSandbox.getInitCartVelocity_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            cartVelocity = parentSandbox.getInitCartVelocity_Static();
        } else if (parentSandbox.getInitCartVelocity_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getInitCartVelocity_Mean();
            float stdDev = parentSandbox.getInitCartVelocity_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            cartVelocity = gaussianRV.Next();
        }

        if (parentSandbox.getInitCartAcceleration_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            cartAcceleration = parentSandbox.getInitCartAcceleration_Static();
        } else if (parentSandbox.getInitCartAcceleration_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getInitCartAcceleration_Mean();
            float stdDev = parentSandbox.getInitCartAcceleration_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            cartAcceleration = gaussianRV.Next();
        }

        if (parentSandbox.getGravitationalAcceleration_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            gravitationalAcceleration = parentSandbox.getGravitationalAcceleration_Static();
        } else if (parentSandbox.getGravitationalAcceleration_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getGravitationalAcceleration_Mean();
            float stdDev = parentSandbox.getGravitationalAcceleration_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            gravitationalAcceleration = gaussianRV.Next();
        }

        if (parentSandbox.getCartMass_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            cartMass = parentSandbox.getCartMass_Static();
        } else if (parentSandbox.getCartMass_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getCartMass_Mean();
            float stdDev = parentSandbox.getCartMass_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            cartMass = gaussianRV.Next();
        }

        if (parentSandbox.getPoleMass_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            poleMass = parentSandbox.getPoleMass_Static();
        } else if (parentSandbox.getPoleMass_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getPoleMass_Mean();
            float stdDev = parentSandbox.getPoleMass_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            poleMass = gaussianRV.Next();
        }

        if (parentSandbox.getPoleLength_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            poleLength = parentSandbox.getPoleLength_Static();
        } else if (parentSandbox.getPoleLength_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getPoleLength_Mean();
            float stdDev = parentSandbox.getPoleLength_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            poleLength = gaussianRV.Next();
        }

        if (parentSandbox.getTrackLimit_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            trackLimit = parentSandbox.getTrackLimit_Static();
        } else if (parentSandbox.getTrackLimit_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getTrackLimit_Mean();
            float stdDev = parentSandbox.getTrackLimit_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            trackLimit = gaussianRV.Next();
        }

        if (parentSandbox.getPoleFailureAngle_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            poleFailureAngle = parentSandbox.getPoleFailureAngle_Static();
        } else if (parentSandbox.getPoleFailureAngle_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getPoleFailureAngle_Mean();
            float stdDev = parentSandbox.getPoleFailureAngle_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            poleFailureAngle = gaussianRV.Next();
        }

        if (parentSandbox.getForceMagnitude_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            forceMagnitude = parentSandbox.getForceMagnitude_Static();
        } else if (parentSandbox.getForceMagnitude_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getForceMagnitude_Mean();
            float stdDev = parentSandbox.getForceMagnitude_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            forceMagnitude = gaussianRV.Next();
        }

        if (parentSandbox.getSuccessThreshold_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC)) {
            successThreshold = parentSandbox.getSuccessThreshold_Static();
        } else if (parentSandbox.getSuccessThreshold_Mode().Equals(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN)) {
            float mean = parentSandbox.getSuccessThreshold_Mean();
            float stdDev = parentSandbox.getSuccessThreshold_StdDev();
            gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            successThreshold = gaussianRV.Next();
        }

        time = 0f;
        timeStepSize = 0.02f;


        // bounds check
        if (trackLimit <= 0) {
            trackLimit = 0.1f;
        }
        if (Mathf.Abs(cartPosition) > trackLimit ) {
            cartPosition = 0f;
        }
        if (gravitationalAcceleration > 0) {
            gravitationalAcceleration = -gravitationalAcceleration;
        }
        if (cartMass < 0) {
            cartMass = -cartMass;
        }
        if (poleMass < 0) {
            poleMass = -poleMass;
        }
        if (poleLength < 0) {
            poleMass = -poleMass;
        }
        if (trackLimit < 0) {
            trackLimit = -trackLimit;
        }
        if (poleFailureAngle < 0) {
            poleFailureAngle = -poleFailureAngle;
        }
        if (forceMagnitude < 0) {
            forceMagnitude = -forceMagnitude;
        }
    }                                                                       



    public double run() {

        bool running = true;

        while (running) {
            if (failureTest()) {
                running = false;
            } else if (time > successThreshold) {
                running = false;
            }

            advanceState();
        }

        return System.Convert.ToDouble(time);
    }


    private void advanceState() {

        // get next output from network
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


    // return true if fail
    private bool failureTest() {

        if (Mathf.Abs(poleAngleDegrees) > poleFailureAngle) {
            return true;
        } else if (Mathf.Abs(cartPosition) > trackLimit) {
            return true;
        } else {
            return false;
        }
    }

}
