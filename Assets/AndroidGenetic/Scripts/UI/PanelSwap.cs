using UnityEngine;

public class PanelSwap : MonoBehaviour {

    public GameObject parentPanel;
    public GameObject nextPanel;


    public void swapPanel() {
        parentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }

}
