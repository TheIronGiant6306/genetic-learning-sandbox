using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScrollView_SelectProblem : MonoBehaviour {

    public Text buttonText;
    private EControlProblem controlProblem;
    private ScrollView_SelectProblem parentScrollView;

	public void setControlProblem(EControlProblem controlProblem) {
        this.controlProblem = controlProblem;
        buttonText.text = Enums.toString(controlProblem);
    }

    public EControlProblem getControlProblem() {
        return controlProblem;
    }

    public void setParentScrollView(ScrollView_SelectProblem parentScrollView) {
        this.parentScrollView = parentScrollView;
    }

    public void buttonClick() {
        parentScrollView.handleButtonClick(this.gameObject);
    }
}
