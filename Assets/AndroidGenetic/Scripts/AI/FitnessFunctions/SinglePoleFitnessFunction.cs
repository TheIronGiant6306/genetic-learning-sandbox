using UnityEngine;
using System.Collections;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Neuro;

public class SinglePoleFitnessFunction : IFitnessFunction {
    private int inputCount;
    private int[] hiddenLayerNeuronCounts;
    private int outputNeuronCount;
    private IActivationFunction activationFunction;
    private Sandbox_SinglePoleBalance parentSandbox;

    public SinglePoleFitnessFunction(int inputCount, int[] hiddenLayerNeuronCounts, int outputNeuronCount, IActivationFunction activationFunction, Sandbox_SinglePoleBalance parentSandbox) {
        this.inputCount = inputCount;
        this.hiddenLayerNeuronCounts = hiddenLayerNeuronCounts;
        this.outputNeuronCount = outputNeuronCount;
        this.activationFunction = activationFunction;
        this.parentSandbox = parentSandbox;
    }

    public double Evaluate(IChromosome chromosome) {

        ActivationNetwork network = Encoding.buildActivationNetwork(inputCount,hiddenLayerNeuronCounts,outputNeuronCount,activationFunction,chromosome);

        TrainingController_SinglePoleBalance simulation = new TrainingController_SinglePoleBalance(network, parentSandbox);

        double fitness = simulation.run();

        return fitness;
    }
}
