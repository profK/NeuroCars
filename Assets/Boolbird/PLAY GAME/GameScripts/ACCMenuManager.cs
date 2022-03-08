using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ACCMenuManager : MonoBehaviour
{
    #region REFERENCES

    public GameObject menuTyresDumpingRateObj;
    public GameObject menuTyresForcePointObj;

    public GameObject menuSuspensionSpringObj;
    public GameObject menuSuspensionDumperObj;
    public GameObject menuSuspensionDistanceObj;
    public GameObject menuSuspensionTargetPositionObj;

    public GameObject menuSteeringAngleObj;
    public GameObject menuWheelStiffnessObj;
    public GameObject menuSteerHelperObj;
    public GameObject menuTractionControlObj;
    public GameObject menuSlipLimitObj;

    public GameObject menuFourWheelsDriveObj;
    public GameObject menuFrontWheelsDriveObj;
    public GameObject menuRearWheelsDriveObj;
    public GameObject menuManualTransmissionObj;
    public GameObject menuAutoTransmissionObj;

    public GameObject menuNumberOfGearsObj;
    public GameObject menuNumberOfGearsSliderObj;
    public GameObject menuMotorTorqueObj;
    public GameObject menuBrakeTorqueObj;
    public GameObject menuReverseTorqueObj;
    public GameObject menuHandBrakeTorqueObj;

    public GameObject menuTurboONObj;
    public GameObject menuNOSONObj;

    public GameObject menuMaxSpeedObj;
    public GameObject menuMaxSpeedSliderObj;
    public GameObject menuVehicleMassObj;

    public GameObject menuAnalogSpeedometerObj;
    public GameObject menuDigitalSpeedometerObj;
    public GameObject menuSkidMarksObj;
    public GameObject menuExhaustObj;
    public GameObject menuABSObj;
    public GameObject menuCollisionSystemObj;


    public GameObject controlPanelObj;

    #endregion

    public void Start()
    {
        controlPanelObj.gameObject.SetActive(false);

        #region CAR DRIVE

        ACCGameSettings.menuCarDrive = 2;
        menuFourWheelsDriveObj.GetComponent<Image>().color = Color.black;
        menuFrontWheelsDriveObj.GetComponent<Image>().color = Color.gray;
        menuRearWheelsDriveObj.GetComponent<Image>().color = Color.gray;

        #endregion

        #region TRANSMISSION

        ACCGameSettings.menuTransmission = 0;
        menuAutoTransmissionObj.GetComponent<Image>().color = Color.black;
        menuManualTransmissionObj.GetComponent<Image>().color = Color.gray;

        #endregion

        #region NOS & TURBO

        ACCGameSettings.menuNOSON = false;
        menuNOSONObj.GetComponent<Image>().color = Color.gray;

        ACCGameSettings.menuTurboON = false;
        menuTurboONObj.GetComponent<Image>().color = Color.gray;

        #endregion

        #region CUSTOM CONTROLS

        ACCGameSettings.menuSkidMarks = false;
        menuSkidMarksObj.GetComponent<Image>().color = Color.gray;

        ACCGameSettings.menuCollisionSystem = false;
        menuCollisionSystemObj.GetComponent<Image>().color = Color.gray;

        ACCGameSettings.menuABS = false;
        menuABSObj.GetComponent<Image>().color = Color.gray;

        ACCGameSettings.menuExhaust = false;
        menuExhaustObj.GetComponent<Image>().color = Color.gray;

        ACCGameSettings.menuSpeedometer = 2;
        menuAnalogSpeedometerObj.GetComponent<Image>().color = Color.gray;
        menuDigitalSpeedometerObj.GetComponent<Image>().color = Color.gray;

        #endregion
    }

    public void Update()
    {
        #region WHEELS

        ACCGameSettings.menuTyresDumpingRate = menuTyresDumpingRateObj.GetComponent<Slider>().value;
        ACCGameSettings.menuTyresForcePoint = menuTyresForcePointObj.GetComponent<Slider>().value;

        ACCGameSettings.menuSuspensionSpring = menuSuspensionSpringObj.GetComponent<Slider>().value;
        ACCGameSettings.menuSuspensionDumper = menuSuspensionDumperObj.GetComponent<Slider>().value;
        ACCGameSettings.menuSuspensionDistance = menuSuspensionDistanceObj.GetComponent<Slider>().value;
        ACCGameSettings.menuSuspensionTargetPosition = menuSuspensionTargetPositionObj.GetComponent<Slider>().value;

        ACCGameSettings.menuSteeringAngle = menuSteeringAngleObj.GetComponent<Slider>().value;
        ACCGameSettings.menuWheelStiffness = menuWheelStiffnessObj.GetComponent<Slider>().value;
        ACCGameSettings.menuSteerHelper = menuSteerHelperObj.GetComponent<Slider>().value;
        ACCGameSettings.menuTractionControl = menuTractionControlObj.GetComponent<Slider>().value;
        ACCGameSettings.menuSlipLimit = menuSlipLimitObj.GetComponent<Slider>().value;

        #endregion

        #region NUMBER OF GEARS

        ACCGameSettings.menuNumberOfGears = menuNumberOfGearsSliderObj.GetComponent<Slider>().value;
        menuNumberOfGearsObj.GetComponent<Text>().text = menuNumberOfGearsSliderObj.GetComponent<Slider>().value.ToString();

        #endregion

        #region ENGINE

        ACCGameSettings.menuMotorTorque = menuMotorTorqueObj.GetComponent<Slider>().value;
        ACCGameSettings.menuBrakeTorque = menuBrakeTorqueObj.GetComponent<Slider>().value;
        ACCGameSettings.menuReverseTorque = menuReverseTorqueObj.GetComponent<Slider>().value;
        ACCGameSettings.menuHandBrakeTorque = menuHandBrakeTorqueObj.GetComponent<Slider>().value;

        ACCGameSettings.menuMaxSpeed = menuMaxSpeedSliderObj.GetComponent<Slider>().value;
        menuMaxSpeedObj.GetComponent<Text>().text = menuMaxSpeedSliderObj.GetComponent<Slider>().value.ToString();
        ACCGameSettings.menuVehicleMass = menuVehicleMassObj.GetComponent<Slider>().value;

        #endregion
    }

    #region CAR DRIVE

    public void FourWheelsButton()
    {
        if (ACCGameSettings.menuCarDrive != 2)
        {
            ACCGameSettings.menuCarDrive = 2;
            menuFourWheelsDriveObj.GetComponent<Image>().color = Color.black;
            menuFrontWheelsDriveObj.GetComponent<Image>().color = Color.gray;
            menuRearWheelsDriveObj.GetComponent<Image>().color = Color.gray;
        }
    }

    public void FrontWheelsButton()
    {
        if (ACCGameSettings.menuCarDrive != 0)
        {
            ACCGameSettings.menuCarDrive = 0;
            menuFourWheelsDriveObj.GetComponent<Image>().color = Color.gray;
            menuFrontWheelsDriveObj.GetComponent<Image>().color = Color.black;
            menuRearWheelsDriveObj.GetComponent<Image>().color = Color.gray;
        }
    }

    public void RearWheelsButton()
    {
        if (ACCGameSettings.menuCarDrive != 1)
        {
            ACCGameSettings.menuCarDrive = 1;
            menuFourWheelsDriveObj.GetComponent<Image>().color = Color.gray;
            menuFrontWheelsDriveObj.GetComponent<Image>().color = Color.gray;
            menuRearWheelsDriveObj.GetComponent<Image>().color = Color.black;
        }
    }

    #endregion

    #region TRANSMISSION

    public void AutoTransmissionButton()
    {
        if (ACCGameSettings.menuTransmission != 0)
        {
            ACCGameSettings.menuTransmission = 0;
            menuAutoTransmissionObj.GetComponent<Image>().color = Color.black;
            menuManualTransmissionObj.GetComponent<Image>().color = Color.gray;
        }
    }

    public void ManualTransmissionButton()
    {
        if (ACCGameSettings.menuTransmission != 1)
        {
            ACCGameSettings.menuTransmission = 1;
            menuAutoTransmissionObj.GetComponent<Image>().color = Color.gray;
            menuManualTransmissionObj.GetComponent<Image>().color = Color.black;
        }
    }

    #endregion

    #region NOS & TURBO

    public void NOSButton()
    {
        if (ACCGameSettings.menuNOSON)
        {
            ACCGameSettings.menuNOSON = false;
            menuNOSONObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuNOSON = true;
            menuNOSONObj.GetComponent<Image>().color = Color.black;
        }
    }

    public void TurboButton()
    {
        if (ACCGameSettings.menuTurboON)
        {
            ACCGameSettings.menuTurboON = false;
            menuTurboONObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuTurboON = true;
            menuTurboONObj.GetComponent<Image>().color = Color.black;
        }
    }

    #endregion

    #region CUSTOM CONTROLS

    public void SkidMarksON()
    {
        if (ACCGameSettings.menuSkidMarks)
        {
            ACCGameSettings.menuSkidMarks = false;
            menuSkidMarksObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuSkidMarks = true;
            menuSkidMarksObj.GetComponent<Image>().color = Color.black;
        }
    }

    public void ExhaustFXON()
    {
        if (ACCGameSettings.menuExhaust)
        {
            ACCGameSettings.menuExhaust = false;
            menuExhaustObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuExhaust = true;
            menuExhaustObj.GetComponent<Image>().color = Color.black;
        }
    }

    public void ABSON()
    {
        if (ACCGameSettings.menuABS)
        {
            ACCGameSettings.menuABS = false;
            menuABSObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuABS = true;
            menuABSObj.GetComponent<Image>().color = Color.black;
        }
    }

    public void AnalogSpeedometerON()
    {
        if (ACCGameSettings.menuSpeedometer != 0)
        {
            ACCGameSettings.menuSpeedometer = 0;
            menuAnalogSpeedometerObj.GetComponent<Image>().color = Color.black;
            menuDigitalSpeedometerObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuSpeedometer = 2;
            menuAnalogSpeedometerObj.GetComponent<Image>().color = Color.gray;
            menuDigitalSpeedometerObj.GetComponent<Image>().color = Color.gray;
        }
    }

    public void DigitalSpeedometerON()
    {
        if (ACCGameSettings.menuSpeedometer != 1)
        {
            ACCGameSettings.menuSpeedometer = 1;
            menuAnalogSpeedometerObj.GetComponent<Image>().color = Color.gray;
            menuDigitalSpeedometerObj.GetComponent<Image>().color = Color.black;
        }
        else
        {
            ACCGameSettings.menuSpeedometer = 2;
            menuAnalogSpeedometerObj.GetComponent<Image>().color = Color.gray;
            menuDigitalSpeedometerObj.GetComponent<Image>().color = Color.gray;
        }
    }

    public void CollisionSystemON()
    {
        if (ACCGameSettings.menuCollisionSystem)
        {
            ACCGameSettings.menuCollisionSystem = false;
            menuCollisionSystemObj.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            ACCGameSettings.menuCollisionSystem = true;
            menuCollisionSystemObj.GetComponent<Image>().color = Color.black;
        }
    }

    #endregion

    #region CONTROLS PANEL

    public void OpenControlsPanel()
    {
        controlPanelObj.gameObject.SetActive(true);
    }

    public void CloseControlsPanel()
    {
        controlPanelObj.gameObject.SetActive(false);
    }

    #endregion

    public void StartGameScene()
    {
        SceneManager.LoadScene("GameLevelScene");
    }
}
