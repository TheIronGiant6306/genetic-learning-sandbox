using UnityEngine;
using System.Collections;

public class ManualController : UpdateController {
	public float manualForce = 0f;

	public override float getNextForce (SystemState state) {
		return manualForce;
	}
}
