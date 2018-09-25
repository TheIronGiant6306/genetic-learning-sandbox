using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView_SelectInput : MonoBehaviour {

    // template checkbox
    public GameObject checkboxTemplate;

    // checkboxes for all possible input
    private List<GameObject> checkboxes;




    void Start() {
        checkboxes = new List<GameObject>();
        int count = 0;

        if (Manager.getControlProblem().Equals(EControlProblem.POLE_BALANCE_SINGLE)) {
            
            // populate with possible inputs
            foreach (EInputAll enumValue in Enum.GetValues(typeof(EInputAll))) {

                // input is not relevant to selected problem, skip it
                if (!Enum.IsDefined(typeof(EInputSinglePoleBalance), enumValue.ToString())) {
                    continue;
                }

                GameObject checkbox = Instantiate(checkboxTemplate) as GameObject;
                checkbox.SetActive(true);

                CheckboxScrollView_SelectInput checkboxScript = checkbox.GetComponent<CheckboxScrollView_SelectInput>();

                checkboxScript.setInput(enumValue);
                checkboxScript.setParentScrollView(this);

                checkbox.GetComponent<RectTransform>().SetParent(checkboxTemplate.GetComponent<Transform>().parent);
                checkbox.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                checkbox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-15.5f, -30 + (-50 * count));
                checkboxes.Add(checkbox);

				// initialize checkbox to last state if it exists, otherwise false
				checkbox.GetComponent<Toggle>().isOn = PlayerPrefUtils.getBool(enumValue.ToString(), false);

                count++;
            }
        }

    }

    public void handleToggle(EInputAll input, bool toggleState) {
        if (toggleState) {
            Manager.addInputToSelected(input);
        } else {
            Manager.removeInputFromSelected(input);
        }

		// save the last toggled state to use next time
		PlayerPrefUtils.setBool(input.ToString(), toggleState);
    }

    public void selectAll() {
        foreach (GameObject checkbox in checkboxes) {
            Toggle checkboxToggle = checkbox.GetComponent<Toggle>();
            checkboxToggle.isOn = true;
        }
    }

    public void deselectAll() {
        foreach (GameObject checkbox in checkboxes) {
            Toggle checkboxToggle = checkbox.GetComponent<Toggle>();
            checkboxToggle.isOn = false;
        }
    }

}