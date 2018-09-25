using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Math;
using AForge.Neuro;

public class Sandbox_SinglePoleBalance : MonoBehaviour {
    public enum ETrainingParamMode { STATIC, GAUSSIAN };

    // live params
    private Population population;
    private TrainingUIController uiController;
    private bool advance;
    private int generationCount;

    // training param mode
    private ETrainingParamMode initPoleAngleDegrees_Mode;
    private ETrainingParamMode initPoleVelocity_Mode;
    private ETrainingParamMode initPoleAcceleration_Mode;
    private ETrainingParamMode initCartPosition_Mode;
    private ETrainingParamMode initCartVelocity_Mode;
    private ETrainingParamMode initCartAcceleration_Mode;
    private ETrainingParamMode gravitationalAcceleration_Mode;
    private ETrainingParamMode cartMass_Mode;
    private ETrainingParamMode poleMass_Mode;
    private ETrainingParamMode poleLength_Mode;
    private ETrainingParamMode trackLimit_Mode;
    private ETrainingParamMode poleFailureAngle_Mode;
    private ETrainingParamMode forceMagnitude_Mode;
    private ETrainingParamMode successThreshold_Mode;

    // training param - static value
    private float initPoleAngleDegrees_Static;
    private float initPoleVelocity_Static;
    private float initPoleAcceleration_Static;
    private float initCartPosition_Static;
    private float initCartVelocity_Static;
    private float initCartAcceleration_Static;
    private float gravitationalAcceleration_Static;
    private float cartMass_Static;
    private float poleMass_Static;
    private float poleLength_Static;
    private float trackLimit_Static;
    private float poleFailureAngle_Static;
    private float forceMagnitude_Static;
    private float successThreshold_Static;

    // training param - gaussian mean
    private float initPoleAngleDegrees_Mean;
    private float initPoleVelocity_Mean;
    private float initPoleAcceleration_Mean;
    private float initCartPosition_Mean;
    private float initCartVelocity_Mean;
    private float initCartAcceleration_Mean;
    private float gravitationalAcceleration_Mean;
    private float cartMass_Mean;
    private float poleMass_Mean;
    private float poleLength_Mean;
    private float trackLimit_Mean;
    private float poleFailureAngle_Mean;
    private float forceMagnitude_Mean;
    private float successThreshold_Mean;

    // training param - gaussian std dev
    private float initPoleAngleDegrees_StdDev;
    private float initPoleVelocity_StdDev;
    private float initPoleAcceleration_StdDev;
    private float initCartPosition_StdDev;
    private float initCartVelocity_StdDev;
    private float initCartAcceleration_StdDev;
    private float gravitationalAcceleration_StdDev;
    private float cartMass_StdDev;
    private float poleMass_StdDev;
    private float poleLength_StdDev;
    private float trackLimit_StdDev;
    private float poleFailureAngle_StdDev;
    private float forceMagnitude_StdDev;
    private float successThreshold_StdDev;

    // network members
    IRandomNumberGenerator chromosomeGenerator;
    IRandomNumberGenerator mutatationMultiplierGenerator;
    IRandomNumberGenerator mutatationAdditionGenerator;
    DoubleArrayChromosome seedChromosome;
    int chromosomeLength;
    int inputCount;
    int outputCount;
    int[] hiddenLayerNeuronCounts;
    ISelectionMethod selectionMethod;
    IFitnessFunction fitnessFunction;
    IActivationFunction activationFunction;

    void Awake() {
        // initialize population
        buildPopulation();
        advance = false;
    }

    // Use this for initialization
    void Start () {
        
        generationCount = 0;

        // initialize training parameters to defaults
        initPoleAngleDegrees_Static = 0f;
        initPoleVelocity_Static = 0f;                                                   
        initPoleAcceleration_Static = 0f;
        initCartPosition_Static = 0f;
        initCartVelocity_Static = 0f;
        initCartAcceleration_Static = 0f;
        gravitationalAcceleration_Static = -9.81f;
        cartMass_Static = 1f;
        poleMass_Static = 0.1f;
        poleLength_Static = 0.5f;
        trackLimit_Static = 2.4f;
        poleFailureAngle_Static = 12f;
        forceMagnitude_Static = 10f;
        successThreshold_Static = 20f;

        initPoleAngleDegrees_Mean = 0f;
        initPoleVelocity_Mean = 0f;
        initPoleAcceleration_Mean = 0f;
        initCartPosition_Mean = 0f;
        initCartVelocity_Mean = 0f;
        initCartAcceleration_Mean = 0f;
        gravitationalAcceleration_Mean = -9.81f;
        cartMass_Mean = 1f;
        poleMass_Mean = 0.1f;
        poleLength_Mean = 0.5f;
        trackLimit_Mean = 2.4f;
        poleFailureAngle_Mean = 12f;
        forceMagnitude_Mean = 10f;
        successThreshold_Mean = 20f;

        initPoleAngleDegrees_StdDev = 5f;
        initPoleVelocity_StdDev = 4f;
        initPoleAcceleration_StdDev = 2f;
        initCartPosition_StdDev = 0.6f;                       
        initCartVelocity_StdDev = 0.6f;
        initCartAcceleration_StdDev = 0.2f;
        gravitationalAcceleration_StdDev = 2f;                    
        cartMass_StdDev = 1f;                                     
        poleMass_StdDev = 0.2f;                                   
        poleLength_StdDev = 0.3f;                                 
        trackLimit_StdDev = 0.8f;                                 
        poleFailureAngle_StdDev = 2f;                             
        forceMagnitude_StdDev = 2f;
        successThreshold_StdDev = 2f;


    }


    // Update is called once per frame
    void Update () {
        if (advance) {

            population.RunEpoch();
            generationCount++;

            // update ui display componenents - generation count, best network fitness, avg fitness
            uiController.setGenerationCount(generationCount);
            uiController.setBestFitness(population.FitnessMax);
            uiController.setAvgFitness(population.FitnessAvg);

            //advance = false;
        }
    }

    void FixedUpdate() {

    }

    // TODO: add functionality to train population

    private void buildPopulation() {
        chromosomeGenerator = null;
        mutatationMultiplierGenerator = null;
        mutatationAdditionGenerator = null;
        seedChromosome = null;
        chromosomeLength = 0;
        inputCount = Manager.getSelectedInputCount();
        hiddenLayerNeuronCounts = Manager.getHiddenLayerNeuronCounts();
        outputCount = 0;
        selectionMethod = null;
        fitnessFunction = null;
        activationFunction = null;



        // chromosome generator
        switch (Manager.getChromosomeGenerator()) {
            case ERandomGenerator.UNIFORM:
                float lowerBound = Manager.getChromosomeGeneratorParam(0);
                float upperBound = Manager.getChromosomeGeneratorParam(1);
                chromosomeGenerator = new UniformGenerator(new Range(lowerBound, upperBound));
                break;
            case ERandomGenerator.GAUSSIAN:
                float mean = Manager.getChromosomeGeneratorParam(0);
                float stdDev = Manager.getChromosomeGeneratorParam(1);
                chromosomeGenerator = new GaussianGenerator(mean, stdDev);
                break;
            case ERandomGenerator.EXPONENTIAL:
                float rate = Manager.getChromosomeGeneratorParam(0);
                chromosomeGenerator = new ExponentialGenerator(rate);
                break;
            default:
                break;
        }

        // mutation multiplier generator
        switch (Manager.getMutationMultiplierGenerator()) {
            case ERandomGenerator.UNIFORM:
                float lowerBound = Manager.getMutationMultGeneratorParam(0);
                float upperBound = Manager.getMutationMultGeneratorParam(1);
                mutatationMultiplierGenerator = new UniformGenerator(new Range(lowerBound, upperBound));
                break;
            case ERandomGenerator.GAUSSIAN:
                float mean = Manager.getMutationMultGeneratorParam(0);
                float stdDev = Manager.getMutationMultGeneratorParam(1);
                mutatationMultiplierGenerator = new GaussianGenerator(mean, stdDev);
                break;
            case ERandomGenerator.EXPONENTIAL:
                float rate = Manager.getMutationMultGeneratorParam(0);
                mutatationMultiplierGenerator = new ExponentialGenerator(rate);
                break;
            default:
                break;
        }

        // mutation addition generator
        switch (Manager.getMutationAdditionGenerator()) {
            case ERandomGenerator.UNIFORM:
                float lowerBound = Manager.getMutationAddGeneratorParam(0);
                float upperBound = Manager.getMutationAddGeneratorParam(1);
                mutatationAdditionGenerator = new UniformGenerator(new Range(lowerBound, upperBound));
                break;
            case ERandomGenerator.GAUSSIAN:
                float mean = Manager.getMutationAddGeneratorParam(0);
                float stdDev = Manager.getMutationAddGeneratorParam(1);
                mutatationAdditionGenerator = new GaussianGenerator(mean, stdDev);
                break;
            case ERandomGenerator.EXPONENTIAL:
                float rate = Manager.getMutationAddGeneratorParam(0);
                mutatationAdditionGenerator = new ExponentialGenerator(rate);
                break;
            default:
                break;
        }


        // activation function
        switch (Manager.getActivationFunction()) {
            case EActivationFunction.SIGMOID:
                activationFunction = new SigmoidFunction(Manager.getActivationFunctionParameter());
                break;
            case EActivationFunction.BIPOLAR_SIGMOID:
                activationFunction = new BipolarSigmoidFunction(Manager.getActivationFunctionParameter());
                break;
            case EActivationFunction.THRESHOLD:
                activationFunction = new ThresholdFunction();
                break;
            default:
                break;
        }


        // selection method
        switch (Manager.getPopulationSelectionMethod()) {
            case ESelectionMethod.RANK:
                selectionMethod = new RankSelection();
                break;
            case ESelectionMethod.ELITE:
                selectionMethod = new EliteSelection();
                break;
            case ESelectionMethod.ROULETTE_WHEEL:
                selectionMethod = new RouletteWheelSelection();
                break;
            default:
                break;
        }


        // problem dependent params - outputCount, fitness function
        switch (Manager.getControlProblem()) {
            case EControlProblem.POLE_BALANCE_SINGLE:
                outputCount = 1;
                fitnessFunction = new SinglePoleFitnessFunction(inputCount,hiddenLayerNeuronCounts,outputCount,activationFunction,this);
                break;
            default:
                break;
        }


        // calculate chromosome length
        chromosomeLength += (inputCount * hiddenLayerNeuronCounts[0]);
        chromosomeLength += (hiddenLayerNeuronCounts[hiddenLayerNeuronCounts.Length-1] * outputCount);
        for (int i=0; i<hiddenLayerNeuronCounts.Length-1; i++) {
            chromosomeLength += hiddenLayerNeuronCounts[i] * hiddenLayerNeuronCounts[i + 1];
        }

        seedChromosome = new DoubleArrayChromosome(chromosomeGenerator, mutatationMultiplierGenerator, mutatationAdditionGenerator, chromosomeLength);
        population = new Population(Manager.getPopulationSize(), seedChromosome, fitnessFunction, selectionMethod);
    }





    public void setInitPoleAngleDegrees_Mode(ETrainingParamMode initPoleAngleDegrees_Mode) {
        this.initPoleAngleDegrees_Mode = initPoleAngleDegrees_Mode;
    }
    public void setInitPoleVelocity_Mode(ETrainingParamMode initPoleVelocity_Mode) {
        this.initPoleVelocity_Mode = initPoleVelocity_Mode;
    }
    public void setInitPoleAcceleration_Mode(ETrainingParamMode initPoleAcceleration_Mode) {
        this.initPoleAcceleration_Mode = initPoleAcceleration_Mode;
    }
    public void setInitCartPosition_Mode(ETrainingParamMode initCartPosition_Mode) {
        this.initCartPosition_Mode = initCartPosition_Mode;
    }
    public void setInitCartVelocity_Mode(ETrainingParamMode initCartVelocity_Mode) {
        this.initCartVelocity_Mode = initCartVelocity_Mode;
    }
    public void setInitCartAcceleration_Mode(ETrainingParamMode initCartAcceleration_Mode) {
        this.initCartAcceleration_Mode = initCartAcceleration_Mode;
    }
    public void setGravitationalAcceleration_Mode(ETrainingParamMode gravitationalAcceleration_Mode) {
        this.gravitationalAcceleration_Mode = gravitationalAcceleration_Mode;
    }
    public void setCartMass_Mode(ETrainingParamMode cartMass_Mode) {
        this.cartMass_Mode = cartMass_Mode;
    }
    public void setPoleMass_Mode(ETrainingParamMode poleMass_Mode) {
        this.poleMass_Mode = poleMass_Mode;
    }
    public void setPoleLength_Mode(ETrainingParamMode poleLength_Mode) {
        this.poleLength_Mode = poleLength_Mode;
    }
    public void setTrackLimit_Mode(ETrainingParamMode trackLimit_Mode) {
        this.trackLimit_Mode = trackLimit_Mode;
    }
    public void setPoleFailureAngle_Mode(ETrainingParamMode poleFailureAngle_Mode) {
        this.poleFailureAngle_Mode = poleFailureAngle_Mode;
    }
    public void setForceMagnitude_Mode(ETrainingParamMode forceMagnitude_Mode) {
        this.forceMagnitude_Mode = forceMagnitude_Mode;
    }
    public void setSuccessThreshold_Mode(ETrainingParamMode successThreshold_Mode) {
        this.successThreshold_Mode = successThreshold_Mode;
    }

    public ETrainingParamMode getInitPoleAngleDegrees_Mode() {
        return initPoleAngleDegrees_Mode;
    }
    public ETrainingParamMode getInitPoleVelocity_Mode() {
        return initPoleVelocity_Mode;
    }
    public ETrainingParamMode getInitPoleAcceleration_Mode() {
        return initPoleAcceleration_Mode;
    }
    public ETrainingParamMode getInitCartPosition_Mode() {
        return initCartPosition_Mode;
    }
    public ETrainingParamMode getInitCartVelocity_Mode() {
        return initCartVelocity_Mode;
    }
    public ETrainingParamMode getInitCartAcceleration_Mode() {
        return initCartAcceleration_Mode;
    }
    public ETrainingParamMode getGravitationalAcceleration_Mode() {
        return gravitationalAcceleration_Mode;
    }
    public ETrainingParamMode getCartMass_Mode() {
        return cartMass_Mode;
    }
    public ETrainingParamMode getPoleMass_Mode() {
        return poleMass_Mode;
    }
    public ETrainingParamMode getPoleLength_Mode() {
        return poleLength_Mode;
    }
    public ETrainingParamMode getTrackLimit_Mode() {
        return trackLimit_Mode;
    }
    public ETrainingParamMode getPoleFailureAngle_Mode() {
        return poleFailureAngle_Mode;
    }
    public ETrainingParamMode getForceMagnitude_Mode() {
        return forceMagnitude_Mode;
    }
    public ETrainingParamMode getSuccessThreshold_Mode() {
        return successThreshold_Mode;
    }

    public void setInitPoleAngleDegrees_Static(float initPoleAngleDegrees_Static) {
        this.initPoleAngleDegrees_Static = initPoleAngleDegrees_Static;
    }
    public void setInitPoleVelocity_Static(float initPoleVelocity_Static) {
        this.initPoleVelocity_Static = initPoleVelocity_Static;
    }
    public void setInitPoleAcceleration_Static(float initPoleAcceleration_Static) {
        this.initPoleAcceleration_Static = initPoleAcceleration_Static;
    }
    public void setInitCartPosition_Static(float initCartPosition_Static) {
        this.initCartPosition_Static = initCartPosition_Static;
    }
    public void setInitCartVelocity_Static(float initCartVelocity_Static) {
        this.initCartVelocity_Static = initCartVelocity_Static;
    }
    public void setInitCartAcceleration_Static(float initCartAcceleration_Static) {
        this.initCartAcceleration_Static = initCartAcceleration_Static;
    }
    public void setGravitationalAcceleration_Static(float gravitationalAcceleration_Static) {
        this.gravitationalAcceleration_Static = gravitationalAcceleration_Static;
    }
    public void setCartMass_Static(float cartMass_Static) {
        this.cartMass_Static = cartMass_Static;
    }
    public void setPoleMass_Static(float poleMass_Static) {
        this.poleMass_Static = poleMass_Static;
    }
    public void setPoleLength_Static(float poleLength_Static) {
        this.poleLength_Static = poleLength_Static;
    }
    public void setTrackLimit_Static(float trackLimit_Static) {
        this.trackLimit_Static = trackLimit_Static;
    }
    public void setPoleFailureAngle_Static(float poleFailureAngle_Static) {
        this.poleFailureAngle_Static = poleFailureAngle_Static;
    }
    public void setForceMagnitude_Static(float forceMagnitude_Static) {
        this.forceMagnitude_Static = forceMagnitude_Static;
    }
    public void setSuccessThreshold_Static(float successThreshold_Static) {
        this.successThreshold_Static = successThreshold_Static;
    }

    public float getInitPoleAngleDegrees_Static() {
        return initPoleAngleDegrees_Static;
    }
    public float getInitPoleVelocity_Static() {
        return initPoleVelocity_Static;
    }
    public float getInitPoleAcceleration_Static() {
        return initPoleAcceleration_Static;
    }
    public float getInitCartPosition_Static() {
        return initCartPosition_Static;
    }
    public float getInitCartVelocity_Static() {
        return initCartVelocity_Static;
    }
    public float getInitCartAcceleration_Static() {
        return initCartAcceleration_Static;
    }
    public float getGravitationalAcceleration_Static() {
        return gravitationalAcceleration_Static;
    }
    public float getCartMass_Static() {
        return cartMass_Static;
    }
    public float getPoleMass_Static() {
        return poleMass_Static;
    }
    public float getPoleLength_Static() {
        return poleLength_Static;
    }
    public float getTrackLimit_Static() {
        return trackLimit_Static;
    }
    public float getPoleFailureAngle_Static() {
        return poleFailureAngle_Static;
    }
    public float getForceMagnitude_Static() {
        return forceMagnitude_Static;
    }
    public float getSuccessThreshold_Static() {
        return successThreshold_Static;
    }

    public void setInitPoleAngleDegrees_Mean(float initPoleAngleDegrees_Mean) {
        this.initPoleAngleDegrees_Mean = initPoleAngleDegrees_Mean;
    }
    public void setInitPoleVelocity_Mean(float initPoleVelocity_Mean) {
        this.initPoleVelocity_Mean = initPoleVelocity_Mean;
    }
    public void setInitPoleAcceleration_Mean(float initPoleAcceleration_Mean) {
        this.initPoleAcceleration_Mean = initPoleAcceleration_Mean;
    }
    public void setInitCartPosition_Mean(float initCartPosition_Mean) {
        this.initCartPosition_Mean = initCartPosition_Mean;
    }
    public void setInitCartVelocity_Mean(float initCartVelocity_Mean) {
        this.initCartVelocity_Mean = initCartVelocity_Mean;
    }
    public void setInitCartAcceleration_Mean(float initCartAcceleration_Mean) {
        this.initCartAcceleration_Mean = initCartAcceleration_Mean;
    }
    public void setGravitationalAcceleration_Mean(float gravitationalAcceleration_Mean) {
        this.gravitationalAcceleration_Mean = gravitationalAcceleration_Mean;
    }
    public void setCartMass_Mean(float cartMass_Mean) {
        this.cartMass_Mean = cartMass_Mean;
    }
    public void setPoleMass_Mean(float poleMass_Mean) {
        this.poleMass_Mean = poleMass_Mean;
    }
    public void setPoleLength_Mean(float poleLength_Mean) {
        this.poleLength_Mean = poleLength_Mean;
    }
    public void setTrackLimit_Mean(float trackLimit_Mean) {
        this.trackLimit_Mean = trackLimit_Mean;
    }
    public void setPoleFailureAngle_Mean(float poleFailureAngle_Mean) {
        this.poleFailureAngle_Mean = poleFailureAngle_Mean;
    }
    public void setForceMagnitude_Mean(float forceMagnitude_Mean) {
        this.forceMagnitude_Mean = forceMagnitude_Mean;
    }
    public void setSuccessThreshold_Mean(float successThreshold_Mean) {
        this.successThreshold_Mean = successThreshold_Mean;
    }

    public float getInitPoleAngleDegrees_Mean() {
        return initPoleAngleDegrees_Mean;
    }
    public float getInitPoleVelocity_Mean() {
        return initPoleVelocity_Mean;
    }
    public float getInitPoleAcceleration_Mean() {
        return initPoleAcceleration_Mean;
    }
    public float getInitCartPosition_Mean() {
        return initCartPosition_Mean;
    }
    public float getInitCartVelocity_Mean() {
        return initCartVelocity_Mean;
    }
    public float getInitCartAcceleration_Mean() {
        return initCartAcceleration_Mean;
    }
    public float getGravitationalAcceleration_Mean() {
        return gravitationalAcceleration_Mean;
    }
    public float getCartMass_Mean() {
        return cartMass_Mean;
    }
    public float getPoleMass_Mean() {
        return poleMass_Mean;
    }
    public float getPoleLength_Mean() {
        return poleLength_Mean;
    }
    public float getTrackLimit_Mean() {
        return trackLimit_Mean;
    }
    public float getPoleFailureAngle_Mean() {
        return poleFailureAngle_Mean;
    }
    public float getForceMagnitude_Mean() {
        return forceMagnitude_Mean;
    }
    public float getSuccessThreshold_Mean() {
        return successThreshold_Mean;
    }

    public void setInitPoleAngleDegrees_StdDev(float initPoleAngleDegrees_StdDev) {
        this.initPoleAngleDegrees_StdDev = initPoleAngleDegrees_StdDev;
    }
    public void setInitPoleVelocity_StdDev(float initPoleVelocity_StdDev) {
        this.initPoleVelocity_StdDev = initPoleVelocity_StdDev;
    }
    public void setInitPoleAcceleration_StdDev(float initPoleAcceleration_StdDev) {
        this.initPoleAcceleration_StdDev = initPoleAcceleration_StdDev;
    }
    public void setInitCartPosition_StdDev(float initCartPosition_StdDev) {
        this.initCartPosition_StdDev = initCartPosition_StdDev;
    }
    public void setInitCartVelocity_StdDev(float initCartVelocity_StdDev) {
        this.initCartVelocity_StdDev = initCartVelocity_StdDev;
    }
    public void setInitCartAcceleration_StdDev(float initCartAcceleration_StdDev) {
        this.initCartAcceleration_StdDev = initCartAcceleration_StdDev;
    }
    public void setGravitationalAcceleration_StdDev(float gravitationalAcceleration_StdDev) {
        this.gravitationalAcceleration_StdDev = gravitationalAcceleration_StdDev;
    }
    public void setCartMass_StdDev(float cartMass_StdDev) {
        this.cartMass_StdDev = cartMass_StdDev;
    }
    public void setPoleMass_StdDev(float poleMass_StdDev) {
        this.poleMass_StdDev = poleMass_StdDev;
    }
    public void setPoleLength_StdDev(float poleLength_StdDev) {
        this.poleLength_StdDev = poleLength_StdDev;
    }
    public void setTrackLimit_StdDev(float trackLimit_StdDev) {
        this.trackLimit_StdDev = trackLimit_StdDev;
    }
    public void setPoleFailureAngle_StdDev(float poleFailureAngle_StdDev) {
        this.poleFailureAngle_StdDev = poleFailureAngle_StdDev;
    }
    public void setForceMagnitude_StdDev(float forceMagnitude_StdDev) {
        this.forceMagnitude_StdDev = forceMagnitude_StdDev;
    }
    public void setSuccessThreshold_StdDev(float successThreshold_StdDev) {
        this.successThreshold_StdDev = successThreshold_StdDev;
    }

    public float getInitPoleAngleDegrees_StdDev() {
        return initPoleAngleDegrees_StdDev;
    }
    public float getInitPoleVelocity_StdDev() {
        return initPoleVelocity_StdDev;
    }
    public float getInitPoleAcceleration_StdDev() {
        return initPoleAcceleration_StdDev;
    }
    public float getInitCartPosition_StdDev() {
        return initCartPosition_StdDev;
    }
    public float getInitCartVelocity_StdDev() {
        return initCartVelocity_StdDev;
    }
    public float getInitCartAcceleration_StdDev() {
        return initCartAcceleration_StdDev;
    }
    public float getGravitationalAcceleration_StdDev() {
        return gravitationalAcceleration_StdDev;
    }
    public float getCartMass_StdDev() {
        return cartMass_StdDev;
    }
    public float getPoleMass_StdDev() {
        return poleMass_StdDev;
    }
    public float getPoleLength_StdDev() {
        return poleLength_StdDev;
    }
    public float getTrackLimit_StdDev() {
        return trackLimit_StdDev;
    }
    public float getPoleFailureAngle_StdDev() {
        return poleFailureAngle_StdDev;
    }
    public float getForceMagnitude_StdDev() {
        return forceMagnitude_StdDev;
    }
    public float getSuccessThreshold_StdDev() {
        return successThreshold_StdDev;
    }

    public void setAdvance(bool advance) {
        this.advance = advance;
    }
    public bool getAdvance() {
        return advance;
    }
    public void setUITrainingController(TrainingUIController uiController) {
        this.uiController = uiController;
    }
    public int getPopulationSize() {
        return population.Size;
    }
    public int getGenerationCount() {
        return generationCount;
    }
    public double getBestFitness() {
        return population.FitnessMax;
    }
    public double getAvgFitness() {
        return population.FitnessAvg;
    }
    public ActivationNetwork getFittestMember() {
        return Encoding.buildActivationNetwork(inputCount,hiddenLayerNeuronCounts,outputCount,activationFunction,population.BestChromosome);
    }
}
