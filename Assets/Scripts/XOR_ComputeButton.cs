using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class XOR_ComputeButton : MonoBehaviour {
	public XOR network;
	public Text input1Text;
	public Text input2Text;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void compute() {

		int input1 = Int32.Parse (input1Text.text);
		int input2 = Int32.Parse (input2Text.text);

		network.compute (input1, input2);
	}
}
