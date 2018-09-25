using UnityEngine;
using System.Collections;
using System;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Math;
using AForge.Neuro;


public class Sandbox : MonoBehaviour {

	public PoleBalancingVisualSimulation visualSim;
	public bool powerAdvance = false;
	public int powerAdvanceSize = 100;
	public int totalGenerations;
	public int generationsRemaining;
	public int generationsToAdvance = 1000;
	public bool advance = false;
	public bool loadVisualSim = false;
	public string chromosomePath;
	public bool saveBestChromosome = false;
	public bool loadChromosome = false;

	private Population population;
	private int generationsRemainingInternal;
	private int totalGenerationsInternal;

	private int chromosomeSize = 110;		// multiple of 11
	private int populationSize = 100;

	// Use this for initialization
	void Start () {

		generationsRemainingInternal = generationsToAdvance;
		generationsRemaining = generationsRemainingInternal;
		totalGenerationsInternal = 0;

		IRandomNumberGenerator chromosomeGenerator = new UniformGenerator( new Range( 0, 1 ) );
		IRandomNumberGenerator mutatationMultiplierGenerator = new UniformGenerator( new Range( 0, 1 ) );
		IRandomNumberGenerator mutatationAdditionGenerator = new UniformGenerator( new Range( 0, 1 ) );

		DoubleArrayChromosome chromosome = new DoubleArrayChromosome (chromosomeGenerator,mutatationMultiplierGenerator,mutatationAdditionGenerator, chromosomeSize);

		RankSelection selectionMethod = new RankSelection ();

		IFitnessFunction fitnessFunction = new PoleBalancingFitnessFunction ();

		population = new Population (populationSize, chromosome, fitnessFunction, selectionMethod);



		//System.Random rnd = new System.Random ();
		//IRandomNumberGenerator gaussianRV = new GaussianGenerator (0f,0.3f,rnd.Next());




	}
	
	// Update is called once per frame
	void Update () {
		totalGenerations = totalGenerationsInternal;
		generationsRemaining = generationsRemainingInternal;
	}

	void FixedUpdate() {
		if (advance) {
			if (powerAdvance) {
				if (powerAdvanceSize > generationsRemainingInternal) {
					powerAdvanceSize = generationsRemainingInternal;
				}
				for (int i=0; i<powerAdvanceSize; i++) {
					population.RunEpoch ();
					generationsRemainingInternal--;
					totalGenerationsInternal++;
				}
			} else {
				population.RunEpoch ();
				generationsRemainingInternal--;
				totalGenerationsInternal++;
			}
		} else {
			generationsRemainingInternal = generationsToAdvance;
		}

		if (generationsRemainingInternal <= 0) {
			advance = false;
			Debug.Log ("fitness max: "+population.FitnessMax);
			Debug.Log ("fitness avg: "+population.FitnessAvg);
			Debug.Log ("");
		}

		if (loadVisualSim) {

			DoubleArrayChromosome bestChromosome = (DoubleArrayChromosome)population.BestChromosome;

			int alpha = 2;
			int hiddenNeuronCount = ((DoubleArrayChromosome)bestChromosome).Value.Length / 11;		

			ActivationNetwork bestNetwork = EncodingStrategy.directConnectionEncoding ((DoubleArrayChromosome)bestChromosome,new SigmoidFunction (alpha) , 10, hiddenNeuronCount, 1);

			visualSim.setNetwork (bestNetwork);

			loadVisualSim = false;
		}

		if (saveBestChromosome) {
			DoubleArrayChromosome bestChromosome = (DoubleArrayChromosome)population.BestChromosome;
			SerializableOrganism.serialize (chromosomePath,bestChromosome);
			Debug.Log ("save best chromosome: "+chromosomePath);
			saveBestChromosome = false;
		}

		if (loadChromosome) {
			SerializableOrganism serializableChrom = SerializableOrganism.reconstitute (chromosomePath);

			IRandomNumberGenerator chromosomeGenerator = new UniformGenerator( new Range( 0, 1 ) );
			IRandomNumberGenerator mutatationMultiplierGenerator = new UniformGenerator( new Range( 0, 1 ) );
			IRandomNumberGenerator mutatationAdditionGenerator = new UniformGenerator( new Range( 0, 1 ) );


			DoubleArrayChromosome chromosome = new DoubleArrayChromosome (chromosomeGenerator,mutatationMultiplierGenerator,mutatationAdditionGenerator, serializableChrom.getValues());
			RankSelection selectionMethod = new RankSelection ();
			IFitnessFunction fitnessFunction = new PoleBalancingFitnessFunction ();
			population = new Population (populationSize, chromosome, fitnessFunction, selectionMethod);
			population.RunEpoch();


			Debug.Log ("population regenerated from chromosome: "+chromosomePath);
			loadChromosome = false;
		}

	}
}
