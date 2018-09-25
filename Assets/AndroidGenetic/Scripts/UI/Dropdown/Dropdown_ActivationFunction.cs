using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_ActivationFunction : MonoBehaviour {

    public GameObject paramInput;
    public GameObject paramInputPlaceholder;
    public GameObject paramInputText;

    private List<EActivationFunction> dropdownOptions;
    private Dropdown dropdown;

    // Use this for initialization
    void Start() {
        dropdownOptions = new List<EActivationFunction>();
        dropdown = gameObject.GetComponent<Dropdown>();
        dropdown.options.Clear();

        foreach (EActivationFunction enumValue in Enum.GetValues(typeof(EActivationFunction))) {
            dropdown.options.Add(new Dropdown.OptionData() { text = Enums.toString(enumValue) });
            dropdownOptions.Add(enumValue);
        }
			

		// set alpha to last selected value
		paramInput.GetComponent<InputField>().text = PlayerPrefUtils.getString ("ACTIVATION_ALPHA","2");

		// set function to last selected value
		dropdown.value = PlayerPrefUtils.getInt ("ACTIVATION_FUNCTION",dropdownOptions.IndexOf (EActivationFunction.BIPOLAR_SIGMOID));

		// weird hack to fix label
		if (dropdown.value == 0) {
			dropdown.value = 1;
			dropdown.value = 0;
		}

		handleParamInputChange ();
    }

    public void handleOptionChange() {

        EActivationFunction selectedValue = dropdownOptions[dropdown.value];
        InputField paramInputField = paramInput.GetComponent<InputField>();
        Text paramPlaceholderText = paramInputPlaceholder.GetComponent<Text>();

        //paramInputField.text = "";

        Manager.setActivationFunction(selectedValue);

		// save last selected value
		PlayerPrefUtils.setInt ("ACTIVATION_FUNCTION",dropdown.value);

        switch (selectedValue) {            
            case EActivationFunction.SIGMOID:
                paramInput.SetActive(true);
                break;
            case EActivationFunction.BIPOLAR_SIGMOID:
                paramInput.SetActive(true);
                break;
            case EActivationFunction.THRESHOLD:
                paramInput.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void handleParamInputChange() {
        InputField paramInputField = paramInput.GetComponent<InputField>();

        float param;

        bool result = float.TryParse(paramInputField.text, out param);
        if (!result) {
            param = 1f;
            paramInputField.text = "1";
        }

		// save last set alpha for later
		PlayerPrefUtils.setString ("ACTIVATION_ALPHA",param.ToString ());
        

		Manager.setActivationFunctionParameter(param);
    }
}
