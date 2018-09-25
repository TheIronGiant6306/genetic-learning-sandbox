using UnityEngine;
using System.Collections;

public abstract class UpdateController : MonoBehaviour {
	public abstract float getNextForce (SystemState state);
}
