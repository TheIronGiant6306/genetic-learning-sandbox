using UnityEngine;
using UnityEngine.SceneManagement;

public class Util : MonoBehaviour {

    public void loadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void loadSandboxScene() {
        switch (Manager.getControlProblem()) {
            case EControlProblem.POLE_BALANCE_SINGLE:
                SceneManager.LoadScene("Sandbox_SinglePole");
                break;
            default:
                break;
        }
    }

    public void exitApplication() {
        Application.Quit();
    }

    public void finalizeManagerParamaters() {
        Manager.finalizeParameters();
    }
}
