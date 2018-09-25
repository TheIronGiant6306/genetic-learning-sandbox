using UnityEngine;
using System.Collections;
using AForge.Neuro;
using AForge.Genetic;

public static class EncodingStrategy {


	public static ActivationNetwork directConnectionEncoding (DoubleArrayChromosome chromosome, IActivationFunction activationFunction, int inputsCount, params int[] neuronsCount) {
		double[] chromosomeValues = chromosome.Value;

		ActivationNetwork network = new ActivationNetwork (activationFunction, inputsCount, neuronsCount);


		Layer[] layers = network.Layers;


		// count total network weights
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


		// test whether network weight count equals chromosome length
		if (!chromosomeValues.Length.Equals(weightCount)) {
			Debug.Log ("network weight count does not match chromosome length");
			return network;
		}
			

		// map chromosome values to network weights
		int i = 0;
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
			i++;
		}
				
//		int test = 0;
//		foreach (Layer layer in layers) {
//			Neuron[] neurons = layer.Neurons;
//
//			foreach (Neuron neuron in neurons) {
//				double[] weights = neuron.Weights;
//
//				foreach (double weight in weights) {
//					Debug.Log ("networkWeight: "+weight+ " chromosomeValue: "+chromosomeValues[test]);
//					test++;
//				}
//			}
//		}

		return network;
	}

}
