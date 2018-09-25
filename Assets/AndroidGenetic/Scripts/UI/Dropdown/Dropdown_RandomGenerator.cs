using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_RandomGenerator : MonoBehaviour {
    public enum EGeneratorMapping { CHROM_GEN, MUTATION_MULT_GEN, MUTATION_ADD_GEN };

    public EGeneratorMapping mapping;
    public GameObject firstNumberInput;
    public GameObject firstNumberPlaceholder;
    public GameObject firstNumberText;
    public GameObject secondNumberInput;
    public GameObject secondNumberPlaceholder;
    public GameObject secondNumberText;
    private List<ERandomGenerator> dropdownOptions;
    private Dropdown dropdown;

    // Use this for initialization
    void Start () {
        dropdownOptions = new List<ERandomGenerator>();
	    dropdown = gameObject.GetComponent<Dropdown>();
        dropdown.options.Clear();

        foreach (ERandomGenerator enumValue in Enum.GetValues(typeof(ERandomGenerator))) {
            dropdown.options.Add(new Dropdown.OptionData() { text = Enums.toString(enumValue) });
            dropdownOptions.Add(enumValue);
        }
			

		// set function to last selected value
		dropdown.value = PlayerPrefUtils.getInt (mapping.ToString () + "_FUNCTION",dropdownOptions.IndexOf (ERandomGenerator.UNIFORM));

		// weird hack to fix label
		if (dropdown.value == 0) {
			dropdown.value = 1;
			dropdown.value = 0;
		}

		handleParamInputChange ();
    }

    public void handleOptionChange() {

        ERandomGenerator selectedValue = dropdownOptions[dropdown.value];
        InputField firstNumberInputField = firstNumberInput.GetComponent<InputField>();
        InputField secondNumberInputField = secondNumberInput.GetComponent<InputField>();
		Text firstNumberTextText = firstNumberText.GetComponent<Text>();
		Text secondNumberTextText = secondNumberText.GetComponent<Text>();
        Text firstNumberPlaceholderText = firstNumberPlaceholder.GetComponent<Text>();
        Text secondNumberPlaceholderText = secondNumberPlaceholder.GetComponent<Text>();

        firstNumberInputField.text = "";
        secondNumberInputField.text = "";

		// save last function selection for later
		PlayerPrefUtils.setInt (mapping.ToString () + "_FUNCTION", dropdown.value);

        switch (mapping) {
            case EGeneratorMapping.CHROM_GEN:
                Manager.setChromosomeGenerator(selectedValue);
                break;
            case EGeneratorMapping.MUTATION_MULT_GEN:
                Manager.setMutationMultiplierGenerator(selectedValue);
                break;
            case EGeneratorMapping.MUTATION_ADD_GEN:
                Manager.setMutationAdditionGenerator(selectedValue);
                break;
            default:
                break;
        }

        switch (selectedValue) {                // float min/max, float mean/stdDev, float rate
		case ERandomGenerator.UNIFORM:
				firstNumberInput.SetActive (true);
				secondNumberInput.SetActive (true);
				firstNumberPlaceholderText.text = PlayerPrefUtils.getString (mapping.ToString () + "_" + dropdown.value + "_FIRSTPARAM", "Lower bound...");
				secondNumberPlaceholderText.text = PlayerPrefUtils.getString (mapping.ToString () + "_" + dropdown.value + "_SECONDPARAM", "Upper bound...");
                break;
            case ERandomGenerator.GAUSSIAN:
                firstNumberInput.SetActive(true);
                secondNumberInput.SetActive(true);
				firstNumberPlaceholderText.text = PlayerPrefUtils.getString (mapping.ToString ()+"_"+dropdown.value+"_FIRSTPARAM","Mean...");
				secondNumberPlaceholderText.text = PlayerPrefUtils.getString (mapping.ToString ()+"_"+dropdown.value+"_SECONDPARAM","Std dev...");
                break;
            case ERandomGenerator.EXPONENTIAL:
                firstNumberInput.SetActive(true);
                secondNumberInput.SetActive(false);
				firstNumberPlaceholderText.text = PlayerPrefUtils.getString (mapping.ToString ()+"_"+dropdown.value+"_FIRSTPARAM","Rate...");
                break;
            default:
                break;
        }

		firstNumberInputField.text = firstNumberPlaceholderText.text;
		secondNumberInputField.text = secondNumberPlaceholderText.text;

		handleParamInputChange ();
    }

    public void handleParamInputChange() {
        InputField firstNumberInputField = firstNumberInput.GetComponent<InputField>();
        InputField secondNumberInputField = secondNumberInput.GetComponent<InputField>();


        float firstParam, secondParam;

        bool result = float.TryParse(firstNumberInputField.text, out firstParam);
        if (!result) {
            firstParam = 0f;
        }
        bool result2 = float.TryParse(secondNumberInputField.text, out secondParam);
        if (!result2) {
            secondParam = 0f;
        }

		if (!result && !result2) {
			return;
		} else {

			// save last set params
			if (result) {
				PlayerPrefUtils.setString (mapping.ToString ()+"_"+dropdown.value+"_FIRSTPARAM", firstNumberInputField.text);
			}

			if (result2) {
				PlayerPrefUtils.setString (mapping.ToString () + "_" + dropdown.value + "_SECONDPARAM", secondNumberInputField.text);
			}
		}
			
        switch (mapping) {
            case EGeneratorMapping.CHROM_GEN:
                Manager.setChromosomeGeneratorParam(firstParam,0);
                Manager.setChromosomeGeneratorParam(secondParam, 1);
                break;
            case EGeneratorMapping.MUTATION_MULT_GEN:
                Manager.setMutationMultGeneratorParam(firstParam, 0);
                Manager.setMutationMultGeneratorParam(secondParam, 1);
                break;
            case EGeneratorMapping.MUTATION_ADD_GEN:
                Manager.setMutationAddGeneratorParam(firstParam, 0);
                Manager.setMutationAddGeneratorParam(secondParam, 1);
                break;
            default:
                break;
        }
    }




    public void setDefaultParams() {

    }

}
