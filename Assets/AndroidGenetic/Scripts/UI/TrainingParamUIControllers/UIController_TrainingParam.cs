using UnityEngine;
using UnityEngine.UI;

public class UIController_TrainingParam : MonoBehaviour {

    public enum ETrainingParam { INIT_POLE_ANGLE_DEG, INIT_POLE_VELOCITY, INIT_POLE_ACCEL,
                                    INIT_CART_POS, INIT_CART_VELOCITY, INIT_CART_ACCEL, GRAV_ACCEL,
                                    CART_MASS, POLE_MASS, POLE_LENGTH, TRACK_LIMIT, POLE_FAILURE_ANGLE, FORCE_MAGNITUDE, SUCCESS_THRESHOLD};



    public ETrainingParam trainingParamMapping;
    public Sandbox_SinglePoleBalance sandbox;
    public GameObject panel_static;
    public GameObject panel_gaussian;
    public GameObject button_static;
    public GameObject button_gaussian;
    public GameObject input_static;
    public GameObject input_mean;
    public GameObject input_stdDev;

    public float staticDefault;
    public float meanDefault;
    public float stdDevDefault;

	// Use this for initialization
	void Start () {
        handleStaticButton();
        handleParamChange();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void handleStaticButton() {

        // set button colors
        ColorBlock colors = button_static.GetComponent<Button>().colors;
        colors.normalColor = new Color32(60, 60, 60, 237);
        colors.highlightedColor = new Color32(60, 60, 60, 237);
        button_static.GetComponent<Button>().colors = colors;

        colors = button_gaussian.GetComponent<Button>().colors;
        colors.normalColor = new Color32(60, 60, 60, 100);
        colors.highlightedColor = new Color32(60, 60, 60, 100);
        button_gaussian.GetComponent<Button>().colors = colors;

        // set panel colors
        panel_static.GetComponent<Image>().color = new Color32(255,255,255,10);
        panel_gaussian.GetComponent<Image>().color = new Color32(255, 255, 255, 0);

        // set parameter mode in sandbox
        


        switch (trainingParamMapping) {
            case ETrainingParam.INIT_POLE_ANGLE_DEG:
                sandbox.setInitPoleAngleDegrees_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.INIT_POLE_VELOCITY:
                sandbox.setInitPoleVelocity_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.INIT_POLE_ACCEL:
                sandbox.setInitPoleAcceleration_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.INIT_CART_POS:
                sandbox.setInitCartPosition_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.INIT_CART_VELOCITY:
                sandbox.setInitCartVelocity_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.INIT_CART_ACCEL:
                sandbox.setInitCartAcceleration_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.GRAV_ACCEL:
                sandbox.setGravitationalAcceleration_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.CART_MASS:
                sandbox.setCartMass_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.POLE_MASS:
                sandbox.setPoleMass_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.POLE_LENGTH:
                sandbox.setPoleLength_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.TRACK_LIMIT:
                sandbox.setTrackLimit_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.POLE_FAILURE_ANGLE:
                sandbox.setPoleFailureAngle_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.FORCE_MAGNITUDE:
                sandbox.setForceMagnitude_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            case ETrainingParam.SUCCESS_THRESHOLD:
                sandbox.setSuccessThreshold_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.STATIC);
                break;
            default:
                break;
        }


    }

    public void handleGaussianButton() {

        // set button colors
        ColorBlock colors = button_gaussian.GetComponent<Button>().colors;
        colors.normalColor = new Color32(60, 60, 60, 237);
        colors.highlightedColor = new Color32(60, 60, 60, 237);
        button_gaussian.GetComponent<Button>().colors = colors;

        colors = button_static.GetComponent<Button>().colors;
        colors.normalColor = new Color32(60, 60, 60, 100);
        colors.highlightedColor = new Color32(60, 60, 60, 100);
        button_static.GetComponent<Button>().colors = colors;

        // set panel colors
        panel_gaussian.GetComponent<Image>().color = new Color32(255, 255, 255, 10);
        panel_static.GetComponent<Image>().color = new Color32(255, 255, 255, 0);

        // set parameter mode in sandbox

        switch (trainingParamMapping) {
            case ETrainingParam.INIT_POLE_ANGLE_DEG:
                sandbox.setInitPoleAngleDegrees_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.INIT_POLE_VELOCITY:
                sandbox.setInitPoleVelocity_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.INIT_POLE_ACCEL:
                sandbox.setInitPoleAcceleration_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.INIT_CART_POS:
                sandbox.setInitCartPosition_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.INIT_CART_VELOCITY:
                sandbox.setInitCartVelocity_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.INIT_CART_ACCEL:
                sandbox.setInitCartAcceleration_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.GRAV_ACCEL:
                sandbox.setGravitationalAcceleration_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.CART_MASS:
                sandbox.setCartMass_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.POLE_MASS:
                sandbox.setPoleMass_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.POLE_LENGTH:
                sandbox.setPoleLength_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.TRACK_LIMIT:
                sandbox.setTrackLimit_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.POLE_FAILURE_ANGLE:
                sandbox.setPoleFailureAngle_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.FORCE_MAGNITUDE:
                sandbox.setForceMagnitude_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            case ETrainingParam.SUCCESS_THRESHOLD:
                sandbox.setSuccessThreshold_Mode(Sandbox_SinglePoleBalance.ETrainingParamMode.GAUSSIAN);
                break;
            default:
                break;
        }
    }

    public void handleParamChange() {
        InputField staticInputField = input_static.GetComponent<InputField>();
        InputField meanInputField = input_mean.GetComponent<InputField>();
        InputField stdDevInputField = input_stdDev.GetComponent<InputField>();
        float value_static;
        float value_mean;
        float value_stdDev;

        if (!float.TryParse(staticInputField.text,out value_static)) {
            value_static = staticDefault;
            staticInputField.text = staticDefault+"";
        }

        if (!float.TryParse(meanInputField.text, out value_mean)) {
            value_mean = meanDefault;
            meanInputField.text = meanDefault + "";
        }
        
        if (!float.TryParse(stdDevInputField.text, out value_stdDev)) {
            value_stdDev = stdDevDefault;
            stdDevInputField.text = stdDevDefault + "";
        }
        

        switch (trainingParamMapping) {
            case ETrainingParam.INIT_POLE_ANGLE_DEG:
                sandbox.setInitPoleAngleDegrees_Static(value_static);
                sandbox.setInitPoleAngleDegrees_Mean(value_mean);
                sandbox.setInitPoleAngleDegrees_StdDev(value_stdDev);
                break;
            case ETrainingParam.INIT_POLE_VELOCITY:
                sandbox.setInitPoleVelocity_Static(value_static);
                sandbox.setInitPoleVelocity_Mean(value_mean);
                sandbox.setInitPoleVelocity_StdDev(value_stdDev);
                break;
            case ETrainingParam.INIT_POLE_ACCEL:
                sandbox.setInitPoleAcceleration_Static(value_static);
                sandbox.setInitPoleAcceleration_Mean(value_mean);
                sandbox.setInitPoleAcceleration_StdDev(value_stdDev);
                break;
            case ETrainingParam.INIT_CART_POS:
                sandbox.setInitCartPosition_Static(value_static);
                sandbox.setInitCartPosition_Mean(value_mean);
                sandbox.setInitCartPosition_StdDev(value_stdDev);
                break;
            case ETrainingParam.INIT_CART_VELOCITY:
                sandbox.setInitCartVelocity_Static(value_static);
                sandbox.setInitCartVelocity_Mean(value_mean);
                sandbox.setInitCartVelocity_StdDev(value_stdDev);
                break;
            case ETrainingParam.INIT_CART_ACCEL:
                sandbox.setInitCartAcceleration_Static(value_static);
                sandbox.setInitCartAcceleration_Mean(value_mean);
                sandbox.setInitCartAcceleration_StdDev(value_stdDev);
                break;
            case ETrainingParam.GRAV_ACCEL:
                sandbox.setGravitationalAcceleration_Static(value_static);
                sandbox.setGravitationalAcceleration_Mean(value_mean);
                sandbox.setGravitationalAcceleration_StdDev(value_stdDev);
                break;
            case ETrainingParam.CART_MASS:
                sandbox.setCartMass_Static(value_static);
                sandbox.setCartMass_Mean(value_mean);
                sandbox.setCartMass_StdDev(value_stdDev);
                break;
            case ETrainingParam.POLE_MASS:
                sandbox.setPoleMass_Static(value_static);
                sandbox.setPoleMass_Mean(value_mean);
                sandbox.setPoleMass_StdDev(value_stdDev);
                break;
            case ETrainingParam.POLE_LENGTH:
                sandbox.setPoleLength_Static(value_static);
                sandbox.setPoleLength_Mean(value_mean);
                sandbox.setPoleLength_StdDev(value_stdDev);
                break;
            case ETrainingParam.TRACK_LIMIT:
                sandbox.setTrackLimit_Static(value_static);
                sandbox.setTrackLimit_Mean(value_mean);
                sandbox.setTrackLimit_StdDev(value_stdDev);
                break;
            case ETrainingParam.POLE_FAILURE_ANGLE:
                sandbox.setPoleFailureAngle_Static(value_static);
                sandbox.setPoleFailureAngle_Mean(value_mean);
                sandbox.setPoleFailureAngle_StdDev(value_stdDev);
                break;
            case ETrainingParam.FORCE_MAGNITUDE:
                sandbox.setForceMagnitude_Static(value_static);
                sandbox.setForceMagnitude_Mean(value_mean);
                sandbox.setForceMagnitude_StdDev(value_stdDev);
                break;
            case ETrainingParam.SUCCESS_THRESHOLD:
                sandbox.setSuccessThreshold_Static(value_static);
                sandbox.setSuccessThreshold_Mean(value_mean);
                sandbox.setSuccessThreshold_StdDev(value_stdDev);
                break;
            default:
                break;
        }
    }

}
