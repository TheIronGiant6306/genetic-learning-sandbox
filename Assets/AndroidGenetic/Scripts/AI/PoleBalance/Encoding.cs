using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Math;
using AForge.Neuro;

public static class Encoding {

    public static ActivationNetwork buildActivationNetwork(int inputCount, int[] hiddenLayerNeuronCounts, int outputNeuronCount, IActivationFunction activationFunction, IChromosome chromosome) {

        int[] networkTopology = new int[hiddenLayerNeuronCounts.Length + 1];

        for (int i = 0; i < networkTopology.Length - 1; i++) {
            networkTopology[i] = hiddenLayerNeuronCounts[i];
        }

        networkTopology[networkTopology.Length - 1] = outputNeuronCount;

        ActivationNetwork network = new ActivationNetwork(activationFunction, inputCount, networkTopology);
        Layer[] layers = network.Layers;
        double[] chromosomeValues = ((DoubleArrayChromosome)chromosome).Value;

        // count synapses to verify compatability
        int weightCount = 0;
        foreach (Layer layer in layers) {
            Neuron[] neurons = layer.Neurons;
            foreach (Neuron neuron in neurons) {
                double[] weights = neuron.Weights;
                foreach (double weight in weights) {
                    weightCount++;
                }
            }
        }

        if (weightCount != ((DoubleArrayChromosome)chromosome).Value.Length) {
            Debug.LogError("network topology not compatable with chromosome! weights: " + weightCount + " chromosome length: " + ((DoubleArrayChromosome)chromosome).Value.Length);
            return network;
        }

        // map chromosome values to network weights
        int index = 0;
        weightCount = 0;
        foreach (Layer layer in layers) {
            Neuron[] neurons = layer.Neurons;

            int j = 0;
            foreach (Neuron neuron in neurons) {
                double[] weights = neuron.Weights;

                int k = 0;
                foreach (double weight in weights) {
                    weights[k] = chromosomeValues[weightCount];

                    weightCount++;
                    k++;
                }
                j++;
            }
            index++;
        }

        return network;
    }
}
