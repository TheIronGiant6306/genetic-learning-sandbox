using UnityEngine;
using System.Collections;

public static class MathUtils {

	public static float dotProduct(float[] vector1,float[] vector2) {
		if (vector1.Length != vector2.Length) {
			Debug.LogError ("Cannot compute dot product; vector length not equal");
		}

		float partialSum = 0f;

		for (int i=0; i<vector1.Length; i++) {
			partialSum += vector1 [i] * vector2 [i];
		}

		return partialSum;
	}

}
