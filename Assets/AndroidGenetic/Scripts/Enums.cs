
public enum ERandomGenerator { UNIFORM, GAUSSIAN, EXPONENTIAL };        // float min/max, float mean/stdDev, float rate   
public enum ESelectionMethod { RANK, ELITE, ROULETTE_WHEEL };
public enum EControlProblem { POLE_BALANCE_SINGLE };

// inputs
public enum EInputAll {
    POLE_ANGLE_RAD, POLE_VELOCITY, POLE_ACCELERATION,
    CART_POSITION, CART_VELOCITY, CART_ACCELERATION, GRAVITATIONAL_ACCELERATION,
    CART_MASS, POLE_MASS, POLE_LENGTH, TRACK_LIMIT, POLE_FAILURE_ANGLE
}

public enum EInputSinglePoleBalance {
    POLE_ANGLE_RAD, POLE_VELOCITY, POLE_ACCELERATION,
    CART_POSITION, CART_VELOCITY, CART_ACCELERATION, GRAVITATIONAL_ACCELERATION,
    CART_MASS, POLE_MASS, POLE_LENGTH, TRACK_LIMIT, POLE_FAILURE_ANGLE
}




public enum EActivationFunction { SIGMOID, BIPOLAR_SIGMOID, THRESHOLD }


public static class Enums {

    public static string toString(ERandomGenerator enumValue) {
        switch (enumValue) {
            case ERandomGenerator.UNIFORM:
                return "Uniform";
            case ERandomGenerator.GAUSSIAN:
                return "Gaussian";
            case ERandomGenerator.EXPONENTIAL:
                return "Exponential";
            default:
                return "";
        }
    }

    public static string toString(ESelectionMethod enumValue) {
        switch (enumValue) {
            case ESelectionMethod.RANK:
                return "Rank Selection";
            case ESelectionMethod.ELITE:
                return "Elite Selection";
            case ESelectionMethod.ROULETTE_WHEEL:
                return "Roulette Wheel Selection";
            default:
                return "";
        }
    }

    public static string toString(EControlProblem enumValue) {
        switch (enumValue) {
            case EControlProblem.POLE_BALANCE_SINGLE:
                return "Single Pole Balance";
            default:
                return "";
        }
    }

    public static string toString(EInputAll enumValue) {
        switch (enumValue) {
            case EInputAll.POLE_ANGLE_RAD:
                return "Pole Angle";
            case EInputAll.POLE_VELOCITY:
                return "Pole Velocity";
            case EInputAll.POLE_ACCELERATION:
                return "Pole Acceleration";
            case EInputAll.CART_POSITION:
                return "Cart Position";
            case EInputAll.CART_VELOCITY:
                return "Cart Velocity";
            case EInputAll.CART_ACCELERATION:
                return "Cart Acceleration";
            case EInputAll.GRAVITATIONAL_ACCELERATION:
                return "Gravity";
            case EInputAll.CART_MASS:
                return "Cart Mass";
            case EInputAll.POLE_MASS:
                return "Pole Mass";
            case EInputAll.POLE_LENGTH:
                return "Pole Length";
            case EInputAll.TRACK_LIMIT:
                return "Track Limit";
            case EInputAll.POLE_FAILURE_ANGLE:
                return "Pole Failure Angle";
            default:
                return "";
        }
    }

    public static string toString(EActivationFunction enumValue) {
        switch (enumValue) {
            case EActivationFunction.SIGMOID:
                return "Sigmoid";
            case EActivationFunction.BIPOLAR_SIGMOID:
                return "Bipolar Sigmoid";
            case EActivationFunction.THRESHOLD:
                return "Threshold";
            default:
                return "";
        }
    }
}