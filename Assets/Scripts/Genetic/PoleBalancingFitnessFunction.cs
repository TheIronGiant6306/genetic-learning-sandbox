using UnityEngine;
using System.Collections;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Neuro;

public class PoleBalancingFitnessFunction : IFitnessFunction {

	public double Evaluate(IChromosome chromosome) {

		// encode chromosome to neural network
		int alpha = 2;

		int hiddenNeuronCount = ((DoubleArrayChromosome)chromosome).Value.Length / 11;		


		ActivationNetwork network = EncodingStrategy.directConnectionEncoding ((DoubleArrayChromosome)chromosome,new SigmoidFunction (alpha) , 10, hiddenNeuronCount, 1);
		PoleBalancingSimulation simulation = new PoleBalancingSimulation (network); 
		double fitness = simulation.run();

		return fitness;
	}

}
