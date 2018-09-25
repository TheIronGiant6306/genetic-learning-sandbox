using System;
using UnityEngine;
using UnityEngine.UI;

public class InputField_PopSize : MonoBehaviour {

    // Use this for initialization
    void Start() {

		// set population size to last set value
		gameObject.GetComponent<InputField> ().text = PlayerPrefUtils.getString ("POPULATION_SIZE", "");

        handleInputChange();
    }


    public void handleInputChange() {
        InputField inputField = gameObject.GetComponent<InputField>();
        int value;

		if (!Int32.TryParse (inputField.text, out value) || value <= 0) {
			value = 1;
			inputField.text = "";
		} 

		// save last set population size for later
		PlayerPrefUtils.setString ("POPULATION_SIZE",inputField.text);

        Manager.setPopulationSize(value);
    }

}
