using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView_SelectProblem : MonoBehaviour {

    // template button
    public GameObject buttonTemplate;

    // buttons for all control problems
    private List<GameObject> buttons;

    


    void Start () {
        buttons = new List<GameObject>();
        int count = 1;

        // populate with possible control problems
        foreach (EControlProblem enumValue in Enum.GetValues(typeof(EControlProblem))) {

            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            ButtonScrollView_SelectProblem buttonScript = button.GetComponent<ButtonScrollView_SelectProblem>();
            buttonScript.setControlProblem(enumValue);
            buttonScript.setParentScrollView(this);
            button.GetComponent<RectTransform>().SetParent(buttonTemplate.GetComponent<Transform>().parent);
            button.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -60 * count);
            buttons.Add(button);
            count++;
        }

        // preselect first button
        handleButtonClick(buttons[0]);
	}

    public void handleButtonClick(GameObject clickedButton) {
        ButtonScrollView_SelectProblem buttonScript = clickedButton.GetComponent<ButtonScrollView_SelectProblem>();

        // update the state
        Manager.setControlProblem(buttonScript.getControlProblem());

        // set all button colors off except this one
        foreach (GameObject button in buttons) {
            if (button.Equals(clickedButton)) {
                ColorBlock colors = button.GetComponent<Button>().colors;
                colors.normalColor = new Color32(60,60,60,237);
                colors.highlightedColor = new Color32(60, 60, 60, 237);
                button.GetComponent<Button>().colors = colors;
            } else {
                ColorBlock colors = button.GetComponent<Button>().colors;
                colors.normalColor = new Color32(60, 60, 60, 100);
                colors.highlightedColor = new Color32(60, 60, 60, 100);
                button.GetComponent<Button>().colors = colors;
            }
        }
    }
	
}
