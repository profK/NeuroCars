using UnityEngine;
using UnityEngine.SceneManagement;

public class ACCLevelScene : MonoBehaviour
{
    public AnyCarController ACC;
    public GameObject pauseMenu;

    void Start()
    {
        pauseMenu.gameObject.SetActive(false);

        ACC.dumpingRate = ACCGameSettings.menuTyresDumpingRate;
        ACC.forcePoint = ACCGameSettings.menuTyresForcePoint;

        ACC.suspensionSpring = ACCGameSettings.menuSuspensionSpring;
        ACC.suspensionDamper = ACCGameSettings.menuSuspensionDumper;
        ACC.suspensionDistance = ACCGameSettings.menuSuspensionDistance;
        ACC.targetPosition = ACCGameSettings.menuSuspensionTargetPosition;

        ACC.maximumSteerAngle = ACCGameSettings.menuSteeringAngle;
        ACC.wheelStiffness = ACCGameSettings.menuWheelStiffness;
        ACC.steerHelper = ACCGameSettings.menuSteerHelper;
        ACC.tractionControl = ACCGameSettings.menuTractionControl;
        ACC.slipLimit = ACCGameSettings.menuSlipLimit;

        
        switch (ACCGameSettings.menuCarDrive)
        {
            case 0:

                ACC.carDriveType = CarDriveType.FrontWheelDrive;

                break;

            case 1:

                ACC.carDriveType = CarDriveType.RearWheelDrive;

                break;

            case 2:

                ACC.carDriveType = CarDriveType.FourWheelDrive;

                break;
        }

        if (ACCGameSettings.menuTransmission == 0)
        {
            ACC.transmission = Transmission.auto;
        }
        else
        {
            ACC.transmission = Transmission.manual;
        }

        ACC.numberOfGears = (int)ACCGameSettings.menuNumberOfGears;
        ACC.motorTorque = ACCGameSettings.menuMotorTorque;
        ACC.brakeTorque = ACCGameSettings.menuBrakeTorque;
        ACC.reverseTorque = ACCGameSettings.menuReverseTorque;
        ACC.handbrakeTorque = ACCGameSettings.menuHandBrakeTorque;

        ACC.turboON = ACCGameSettings.menuTurboON;
        
        if (ACCGameSettings.menuNOSON)
        { 
            ACC.CreateNOS();
            ACC.nosON = true;
        }

        ACC.maxSpeed = ACCGameSettings.menuMaxSpeed;
        ACC.vehicleMass = ACCGameSettings.menuVehicleMass;

        ACC.skidMarks = ACCGameSettings.menuSkidMarks;

        if (ACCGameSettings.menuExhaust)
        {
            ACC.CreateExhaustGameObj();
            ACC.exhaustFlame = true;
        }

        ACC.ABS = ACCGameSettings.menuABS;
        ACC.collisionSystem = ACCGameSettings.menuCollisionSystem;
    }

    public void Awake()
    {
        ACC.speedType = SpeedType.KPH;

        if (ACCGameSettings.menuSpeedometer == 0)
        {
            ACC.speedometerType = SpeedometerType.Analog;
            ACC.CreateSpeedometer();
        }
        else if (ACCGameSettings.menuSpeedometer == 1)
        {
            ACC.speedometerType = SpeedometerType.Digital;
            ACC.CreateSpeedometer();
        }
    }

    public void ACCPauseButton()
    {
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
    }

    public void ResumeACCGame()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
    }

    public void GoToACCMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GAME MENU");
    }
}
