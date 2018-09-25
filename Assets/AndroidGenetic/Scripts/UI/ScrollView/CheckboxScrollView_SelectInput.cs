using UnityEngine;
using UnityEngine.UI;

public class CheckboxScrollView_SelectInput : MonoBehaviour {
    public Text checkboxText;
    private EInputAll input;
    private ScrollView_SelectInput parentScrollView;

    public void setInput(EInputAll input) {
        this.input = input;
        checkboxText.text = Enums.toString(input);
    }

    public EInputAll getInput() {
        return input;
    }

    public void setParentScrollView(ScrollView_SelectInput parentScrollView) {
        this.parentScrollView = parentScrollView;
    }

    public void toggle(bool state) {                                                     
        parentScrollView.handleToggle(input,state);
    }
}
