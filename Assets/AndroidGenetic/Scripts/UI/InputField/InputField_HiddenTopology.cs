using System;
using UnityEngine;
using UnityEngine.UI;

public class InputField_HiddenTopology : MonoBehaviour {

    // Use this for initialization
    void Start() {

		// set input field to previous value or default
		gameObject.GetComponent<InputField>().text = PlayerPrefUtils.getString ("HIDDEN_TOPOLOGY","100,50");
        handleInputChange();
    }


    public void handleInputChange() {
        InputField inputField = gameObject.GetComponent<InputField>();
        string rawText = inputField.text.Trim();

        char[] delimiterChars = { ' ', ',', '.', ':', '\t', '_' };

        string[] tokens = rawText.Split(delimiterChars);
        int[] topology = new int[tokens.Length];
        
        for(int i=0; i<tokens.Length; i++) {
            string token = tokens[i];
            int value;

            if (!Int32.TryParse(token, out value) || value <= 0) {
                inputField.text = "";
                Manager.setHiddenLayerNeuronCounts(new int[] { 1 });
                return;
            }

            topology[i] = value;
        }

		// save input field for later
		PlayerPrefUtils.setString ("HIDDEN_TOPOLOGY",rawText);

        Manager.setHiddenLayerNeuronCounts(topology);
    }
}
