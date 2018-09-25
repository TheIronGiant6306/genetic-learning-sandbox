using UnityEngine;
using System.Collections;

public class NullController : UpdateController {

	public override float getNextForce (SystemState state) {
		return 0f;
	}
}
