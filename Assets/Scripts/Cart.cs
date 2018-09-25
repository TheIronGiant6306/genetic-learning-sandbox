using UnityEngine;
using System.Collections;

public class Cart : MonoBehaviour {


	private bool m_left;
	private bool m_right;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_left) {
			m_left = Input.GetKeyDown (KeyCode.LeftArrow);
		}

		if (!m_right) {
			m_right = Input.GetKeyDown (KeyCode.RightArrow);
		}

	}

	void FixedUpdate () {
		if (m_left) {
			print ("left");
			applyLeftForce ();
			m_left = false;
		}
		if (m_right) {
			print ("right");
			applyRightForce ();
			m_right = false;
		}

	}

	void applyLeftForce() {
		Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();

		rigidbody.AddForce (new Vector2(-50,0));
	}

	void applyRightForce() {
		Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D> ();

		rigidbody.AddForce (new Vector2(50,0));
	}

}
