using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Math;
using AForge.Neuro;

public class Manager : MonoBehaviour {

    // population build settings

    // manager singleton
    private static Manager instance = null;

    // chromosome params
    private ERandomGenerator chromosomeGenerator;
    private ERandomGenerator mutationMultiplierGenerator;
    private ERandomGenerator mutationAdditionGenerator;

    private float chromosomeGenerator_firstParam, chromosomeGenerator_secondParam;                          // min/max, mean/stdDev, rate  
    private float mutationMultiplierGenerator_firstParam, mutationMultiplierGenerator_secondParam;
    private float mutationAdditionGenerator_firstParam, mutationAdditionGenerator_secondParam;

    // network encoding params
    private EControlProblem controlProblem;
    private HashSet<EInputAll> selectedInput;
    private Dictionary<EInputAll, int> inputMapping;

    // network topology / activation
    private int[] hiddenLayerNeuronCounts;                  
    private EActivationFunction networkActivationFunction;  
    private float activationFunctionParameter;              

    // population params
    private ESelectionMethod selectionMethod;
    private int populationSize;



    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        selectedInput = new HashSet<EInputAll>();
        hiddenLayerNeuronCounts = new int[] { 1 };
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        
	}



    // finalize parameters
    public static void finalizeParameters() {

        // establish input mapping
        HashSet<EInputAll> inputSet = Manager.getSelectedInput();
        EInputAll[] tempInputArray = new EInputAll[inputSet.Count];
        inputSet.CopyTo(tempInputArray);
        instance.inputMapping = new Dictionary<EInputAll, int>();

        for (int i = 0; i < inputSet.Count; i++) {
            instance.inputMapping[tempInputArray[i]] = i;
        }

    }







    // getters / setters

    // control problem
    public static void setControlProblem(EControlProblem controlProblem) {
        instance.controlProblem = controlProblem;
    }
    public static EControlProblem getControlProblem() {
        return instance.controlProblem;
    }


    // chromosome generators
    public static void setChromosomeGenerator(ERandomGenerator chromosomeGenerator) {
        instance.chromosomeGenerator = chromosomeGenerator;
    }
    public static ERandomGenerator getChromosomeGenerator() {
        return instance.chromosomeGenerator;
    }
    public static void setMutationMultiplierGenerator(ERandomGenerator mutationMultiplierGenerator) {
        instance.mutationMultiplierGenerator = mutationMultiplierGenerator;
    }
    public static ERandomGenerator getMutationMultiplierGenerator() {
        return instance.mutationMultiplierGenerator;
    }
    public static void setMutationAdditionGenerator(ERandomGenerator mutationAdditionGenerator) {
        instance.mutationAdditionGenerator = mutationAdditionGenerator;
    }
    public static ERandomGenerator getMutationAdditionGenerator() {
        return instance.mutationAdditionGenerator;
    }

    // chromosome generator parameters
    public static void setChromosomeGeneratorParam(float value, int param) {
        if (param == 0) {
            instance.chromosomeGenerator_firstParam = value;
        } else if (param == 1) {
            instance.chromosomeGenerator_secondParam = value;
        } 
    }
    public static float getChromosomeGeneratorParam(int param) {
        if (param == 0) {
            return instance.chromosomeGenerator_firstParam;
        } else if (param == 1) {
            return instance.chromosomeGenerator_secondParam;
        }
        else {
            return 0f;
        }
    }
    public static void setMutationMultGeneratorParam(float value, int param) {
        if (param == 0) {
            instance.mutationMultiplierGenerator_firstParam = value;
        } else if (param == 1) {
            instance.mutationMultiplierGenerator_secondParam = value;
        }
    }
    public static float getMutationMultGeneratorParam(int param) {
        if (param == 0) {
            return instance.mutationMultiplierGenerator_firstParam;
        } else if (param == 1) {
            return instance.mutationMultiplierGenerator_secondParam;
        } else {
            return 0f;
        }
    }
    public static void setMutationAddGeneratorParam(float value, int param) {
        if (param == 0) {
            instance.mutationAdditionGenerator_firstParam = value;
        } else if (param == 1) {
            instance.mutationAdditionGenerator_secondParam = value;
        }
    }
    public static float getMutationAddGeneratorParam(int param) {
        if (param == 0) {
            return instance.mutationAdditionGenerator_firstParam;
        } else if (param == 1) {
            return instance.mutationAdditionGenerator_secondParam;
        } else {
            return 0f;
        }
    }

    // selected input 
    public static void setSelectedInput(HashSet<EInputAll> selectedInput) {
        instance.selectedInput = selectedInput;
    }
    public static HashSet<EInputAll> getSelectedInput() {
        return instance.selectedInput;
    }
    public static void addInputToSelected(EInputAll input) {
        instance.selectedInput.Add(input);
    }
    public static void removeInputFromSelected(EInputAll input) {
        instance.selectedInput.Remove(input);
    }
    public static int getSelectedInputCount() {
        return instance.selectedInput.Count;
    }
    public static void setInputMapping(Dictionary<EInputAll,int> inputMapping) {
        instance.inputMapping = inputMapping;
    }
    public static Dictionary<EInputAll,int> getInputMapping() {
        return instance.inputMapping;
    }

    // population parameters
    public static void setPopulationSelectionMethod(ESelectionMethod selectionMethod) {
        instance.selectionMethod = selectionMethod;
    }
    public static ESelectionMethod getPopulationSelectionMethod() {
        return instance.selectionMethod;
    }
    public static void setPopulationSize(int populationSize) {
        instance.populationSize = populationSize;
    }
    public static int getPopulationSize() {
        return instance.populationSize;
    }

    // network topology
    public static void setHiddenLayerNeuronCounts(int[] hiddenLayerNeuronCounts) {
        instance.hiddenLayerNeuronCounts = hiddenLayerNeuronCounts;
    }
    public static int[] getHiddenLayerNeuronCounts() {
        return instance.hiddenLayerNeuronCounts;
    }

    // activation function
    public static void setActivationFunction(EActivationFunction networkActivationFunction) {
        instance.networkActivationFunction = networkActivationFunction;
    }
    public static EActivationFunction getActivationFunction() {
        return instance.networkActivationFunction;
    }
    public static void setActivationFunctionParameter(float activationFunctionParameter) {
        instance.activationFunctionParameter = activationFunctionParameter;
    }
    public static float getActivationFunctionParameter() {
        return instance.activationFunctionParameter;
    }
}
