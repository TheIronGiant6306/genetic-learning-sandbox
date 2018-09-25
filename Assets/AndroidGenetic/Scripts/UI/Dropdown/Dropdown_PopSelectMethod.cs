using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_PopSelectMethod : MonoBehaviour {

    private List<ESelectionMethod> dropdownOptions;
    private Dropdown dropdown;

    // Use this for initialization
    void Start () {
        dropdownOptions = new List<ESelectionMethod>();
        dropdown = gameObject.GetComponent<Dropdown>();
        dropdown.options.Clear();

        foreach (ESelectionMethod enumValue in Enum.GetValues(typeof(ESelectionMethod))) {
            dropdown.options.Add(new Dropdown.OptionData() { text = Enums.toString(enumValue) });
            dropdownOptions.Add(enumValue);
        }

		// set function to last selected value
		dropdown.value = PlayerPrefUtils.getInt ("POP_SELECT_METHOD",dropdownOptions.IndexOf (ESelectionMethod.RANK));

		// weird hack to fix label
		if (dropdown.value == 0) {
			dropdown.value = 1;
			dropdown.value = 0;
		}

    }

    public void handleOptionChange() {
        ESelectionMethod selectedValue = dropdownOptions[dropdown.value];
        Manager.setPopulationSelectionMethod(selectedValue);

		// save last function selection for later
		PlayerPrefUtils.setInt ("POP_SELECT_METHOD", dropdown.value);
    }
}
