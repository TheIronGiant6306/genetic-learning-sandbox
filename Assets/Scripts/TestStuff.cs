using UnityEngine;
using System.Collections;
using System;
using AForge;
using AForge.Genetic;
using AForge.Math.Random;
using AForge.Math;
using AForge.Neuro;

public class TestStuff : MonoBehaviour {

    public float mean = 0f;
    public float stdDev = 0f;

    public bool generate = false;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
        if (generate) {

            System.Random rnd = new System.Random();                                                           // should be a system param??
            IRandomNumberGenerator gaussianRV = new GaussianGenerator(mean, stdDev, rnd.Next());
            print(gaussianRV.Next());

            generate = false;
        }

	}
}
