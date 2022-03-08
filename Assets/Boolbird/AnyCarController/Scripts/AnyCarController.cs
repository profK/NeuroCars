using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region CUSTOM DRIVING

[Serializable]
public enum CarDriveType
{
    FrontWheelDrive,
    RearWheelDrive,
    FourWheelDrive
}

[Serializable]
public enum Transmission
{
    auto,
    manual
}

[Serializable]
public enum SpeedType
{
    MPH,
    KPH
}

#endregion

#region WHEELS SETUP

public enum Axl
{
    Front,
    Rear
}
public enum Type
{
    Drive,
    Trailer
}

[Serializable]
public struct CarWheels
{
    public GameObject model;
    public Axl axel;
    public Type type;
}

[Serializable]
public struct CarWheelsCols
{
    public GameObject collider;
    public Axl axel;
    public Type type;
}

#endregion

#region SPEEDOMETER

public enum SpeedometerType
{
    Analog,
    Digital
}

#endregion

#region MOBILE UI
public enum MobileUIType
{
    Arrows,
    Joystick
}

#endregion

#region TYPE OF CAMERA

public enum TypeOfCamera
{
    fixedCamera,
    smoothFollow
}

#endregion

#region COLLISION

[Serializable]
public struct OptionalMeshes
{
    public MeshFilter modelMesh;
    public int loseAftCollisions;
}

#endregion

public class AnyCarController : MonoBehaviour
{
    #region EDITOR

    public int toolbarTab;
    public string currentTab;

    #endregion

    #region SPEEDOMETER

    [SerializeField] public SpeedometerType speedometerType = SpeedometerType.Analog;
    private int typeOfSpeedometer;

    public AnalogSpeedometer analogSpeedometer;

    private Transform analogSpeedometerPrefab;
    private Transform analogSpeedometerObj;
    public GameObject analogSpeedometerObject;

    public DigitalSpeedometer digitalSpeedometer;

    private Transform digitalSpeedometerPrefab;
    private Transform digitalSpeedometerObj;
    public GameObject digitalSpeedometerObject;

    public bool analogSpeedometerCreated;
    public bool digitalSpeedometerCreated;

    #endregion

    #region MOBILE UI

    [SerializeField] private MobileUIType mobileUIType = MobileUIType.Arrows;
    public int typeOfMobileUI;

    public bool MobileUION = false;
    public Transform mobileUIParent;
    public Transform mobileArrowsHorizontalInputsPrefab;
    public Transform mobileArrowsHorizontalInputs;
    public Transform mobileJoystickHorizontalInputsPrefab;
    public Transform mobileJoystickHorizontalInputs;

    public Transform mobileUIPrefab;
    public Transform mobileUIObj;
    public GameObject mobileUIObject;

    #endregion

    #region WHEELS REFERENCES

    public List<CarWheels> extraWheels;
    public List<CarWheelsCols> extraWheelsColList = new List<CarWheelsCols>();
    public CarWheelsCols extraWheelCol;
    public GameObject frontLeft;
    public GameObject frontRight;
    public GameObject backLeft;
    public GameObject backRight;
    public GameObject frontLeftCol;
    public GameObject frontRightCol;
    public GameObject backLeftCol;
    public GameObject backRightCol;

    public float extraWheelRadius;
    private float FLRadius;
    private float FRRadius;
    private float BLRadius;
    private float BRRadius;

    #region WHEELS VALUES

    public float wheelsMass = 20;
    public float forcePoint = 0;
    public float dumpingRate = 0.025f;
    public float suspensionDistance = 0.2f;
    public Vector3 wheelsPosition;
    public Vector3 wheelsRotation;
    [Range(0.1f, 1f)] public float wheelStiffness = 1f;
    public float suspensionSpring = 70000f;
    public float suspensionDamper = 3500f;
    [Range(0.1f, 1)] public float targetPosition = 0.5f;

    [Range(0.5f, 2)] public float wheelsRadius = 1f;

    #endregion

    #endregion

    #region CAR REFERENCES

    public GameObject bodyMesh;
    public bool steeringWheel;
    public GameObject steerWheelReference;
    public GameObject extraBodyCol;
    public GameObject steeringWheelMesh;
    public GameObject steeringWheelPivot;

    #endregion

    #region INPUT REFERENCES

    public float AccelInput { get; private set; }
    public float BrakeInput { get; private set; }

    #endregion

    #region CUSTOM CONTROLS

    public AnimationCurve enginePower;


    public float maximumSteerAngle;
    [Range(0, 1)] public float steerHelper; // 0 = raw physics , 1 grip in the facing direction
    [Range(0, 0.5f)] public float tractionControl;
    [Range(0, 0.5f)] public float slipLimit = 0.3f;

    [SerializeField] public CarDriveType carDriveType = CarDriveType.FourWheelDrive;
    [SerializeField] public SpeedType speedType = SpeedType.KPH;
    [SerializeField] public Transmission transmission = Transmission.auto;

    public Vector3 centerOfMass;
    public float vehicleMass = 1000f;

    public float motorTorque = 2500f;
    public float brakeTorque = 20000f;
    public float reverseTorque = 500f;
    public float handbrakeTorque = 10000000f;
    public float maxSpeed = 200f;

    public int numberOfGears = 5;
    public float downForce = 300f;

    [Range(0, 0.5f)] public float clutch = 0.3f;

    public bool ABS = true;
    public bool skidMarks = true;

    public bool autoTransmission = true;
    public bool smokeOn = false;
    private ParticleSystem smokeParticles;

    #endregion

    #region NOS

    public bool nosON = false;
    public bool nosActive = false;
    [Range(0, 500000f)] public float nosPower;
    public Transform NOSPrefab;
    public Transform NOSGameObj;
    public GameObject NOSParent;

    private float NOSTimeLeft;
    [Range(0, 10f)] public float NOSMaxTime = 5f;
    public Slider NOSBarValue;

    public AudioClip nosAudioClip;
    [Range(0, 1f)] public float nosVolume = 0.5f;

    #endregion

    #region TURBO

    public bool turboON = false;
    public AudioClip turboAudioClip;
    [Range(0, 1f)] public float turboVolume = 0.5f;

    #endregion

    #region EXHAUST

    public Transform exhaustObjectPrefab;
    public Transform exhaustObject;
    public GameObject exhaustObj;
    public bool exhaustFlame;
    public ParticleSystem exhaustVisual;
    public AudioSource exhaustSoundSource;
    public AudioClip exhaustSound;
    [Range(0.01f, 1)] public float exhaustVolume;

    #endregion

    #region UTILITY

    private float oldRotation;
    private Rigidbody rb
    {
        get;
        set;
    }

    public float currentSpeed;
    private float currentTorque;
    private float rpmRange = 1f;
    public int currentGear;
    private float gearFactor;
    public bool reverseGearOn;
    public bool clutchOn;

    public bool changeCamera = false;
    public float RPM { get; private set; }

    private float speedXGear;
    public List<float> maxSpeedsList;

    public GameObject objToUnpack;
    public GameObject modelMeshToUnpack;

    public GameObject frontLights;
    public GameObject rearLights;

    #endregion

    #region COLLISION SYSTEM

    public bool collisionSystem = false;
    public AudioClip collisionSound;
    [Range(0.01f, 1f)] public float collisionVolume;

    public OptionalMeshes[] optionalMeshList;

    [Range(0.01f, 50)] public float demolutionStrenght;
    [Range(0.1f, 500)] public float demolutionRange;
    public bool customMesh = false;
    public bool collisionParticles = false;

    #endregion

    #region AUDIO REFERENCES

    public AudioClip skidSound;
    [Range(0.01f, 1)] public float skidVolume = 0.3f;

    public AudioClip lowAcceleration;
    public AudioClip lowDeceleration;
    public AudioClip highAcceleration;
    public AudioClip highDeceleration;
    [Range(0.01f, 1)] public float engineVolume;



    #endregion

    #region CREATE COLLIDERS

    public void UnpackPrefab()
    {
        Transform _parent = this.transform.parent;
        objToUnpack = _parent.gameObject;        
    }

    public void UnpackModelMesh()
    {
        modelMeshToUnpack = this.transform.Find("ModelParent").GetChild(0).gameObject;
    }

    public void CreateColliders()
    {
        // Create and Set Wheel Colliders
        frontLeft.AddComponent(typeof(SphereCollider));
        frontRight.AddComponent(typeof(SphereCollider));
        backLeft.AddComponent(typeof(SphereCollider));
        backRight.AddComponent(typeof(SphereCollider));

        // Create Wheel Colliders Objects
        frontLeftCol = new GameObject("FLCOL");
        frontRightCol = new GameObject("FRCOL");
        backLeftCol = new GameObject("BLCOL");
        backRightCol = new GameObject("BRCOL");


        // Front Left Wheel
        frontLeftCol.transform.parent = this.transform.GetChild(0);
        frontLeftCol.transform.position = frontLeft.transform.position;
        frontLeftCol.transform.rotation = frontLeft.transform.rotation;

        // Front Right Wheel
        frontRightCol.transform.parent = this.transform.GetChild(0);
        frontRightCol.transform.position = frontRight.transform.position;
        frontRightCol.transform.rotation = frontRight.transform.rotation;

        // Back Left Wheel
        backLeftCol.transform.parent = this.transform.GetChild(0);
        backLeftCol.transform.position = backLeft.transform.position;
        backLeftCol.transform.rotation = backLeft.transform.rotation;

        // Back Right Wheel
        backRightCol.transform.parent = this.transform.GetChild(0);
        backRightCol.transform.position = backRight.transform.position;
        backRightCol.transform.rotation = backRight.transform.rotation;

        // Add Wheel Colliders
        frontLeftCol.AddComponent(typeof(WheelCollider));
        frontRightCol.AddComponent(typeof(WheelCollider));
        backLeftCol.AddComponent(typeof(WheelCollider));
        backRightCol.AddComponent(typeof(WheelCollider));


        
        // Add Skid Marks
        frontLeftCol.AddComponent(typeof(WheelsFX));
        frontRightCol.AddComponent(typeof(WheelsFX));
        backLeftCol.AddComponent(typeof(WheelsFX));
        backRightCol.AddComponent(typeof(WheelsFX));

        // Get Wheel Radius
        FLRadius = frontLeft.GetComponent<SphereCollider>().radius * frontLeft.transform.lossyScale.x;
        FRRadius = frontRight.GetComponent<SphereCollider>().radius * frontLeft.transform.lossyScale.x;
        BLRadius = backLeft.GetComponent<SphereCollider>().radius * frontLeft.transform.lossyScale.x;
        BRRadius = backRight.GetComponent<SphereCollider>().radius * frontLeft.transform.lossyScale.x;

        // Set Wheel Radius
        frontLeftCol.GetComponent<WheelCollider>().radius = Mathf.Abs(FLRadius);
        frontRightCol.GetComponent<WheelCollider>().radius = Mathf.Abs(FRRadius);
        backLeftCol.GetComponent<WheelCollider>().radius = Mathf.Abs(BLRadius);
        backRightCol.GetComponent<WheelCollider>().radius = Mathf.Abs(BRRadius);

        // Destroy Sphere Colliders
        DestroyImmediate(frontLeft.GetComponent<SphereCollider>());
        DestroyImmediate(frontRight.GetComponent<SphereCollider>());
        DestroyImmediate(backLeft.GetComponent<SphereCollider>());
        DestroyImmediate(backRight.GetComponent<SphereCollider>());

        // Create Body Convex Mesh Collider
        if (bodyMesh != null)
        {
            bodyMesh.AddComponent(typeof(MeshCollider));
            bodyMesh.GetComponent<MeshCollider>().convex = true;
        }


        // Extra Wheels
        extraWheelsColList.Clear();
        int c = 1;
        foreach (var wheel in extraWheels)
        {
            wheel.model.AddComponent(typeof(SphereCollider));

            extraWheelCol.collider = new GameObject("extraWheel " + c);
            extraWheelCol.axel = wheel.axel;
            extraWheelCol.type = wheel.type;

            extraWheelCol.collider.transform.parent = this.transform.GetChild(0);
            extraWheelCol.collider.transform.position = wheel.model.transform.position;
            extraWheelCol.collider.transform.rotation = wheel.model.transform.rotation;


            extraWheelCol.collider.AddComponent(typeof(WheelCollider));
            extraWheelCol.collider.AddComponent(typeof(WheelsFX));

            extraWheelRadius = wheel.model.GetComponent<SphereCollider>().radius * wheel.model.transform.lossyScale.x;
            extraWheelCol.collider.GetComponent<WheelCollider>().radius = Mathf.Abs(extraWheelRadius);

            DestroyImmediate(wheel.model.GetComponent<SphereCollider>());

            extraWheelsColList.Add(extraWheelCol);

            c++;
        }
    }

    public void CreateDebugBodyCol()
    {
        extraBodyCol = Instantiate(Resources.Load("BodyCollider"), this.transform.position, Quaternion.identity) as GameObject;
        extraBodyCol.transform.parent = this.transform;
        extraBodyCol.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        extraBodyCol.transform.rotation = this.transform.rotation;
        bodyMesh = extraBodyCol;
    }

    #endregion

    #region CREATE CAMERAS

    public GameObject camerasParent;
    public List<GameObject> cameraObjects = new List<GameObject>();
    public List<TypeOfCamera> typeOfCamera = new List<TypeOfCamera>();
    public List<int> typeOfCameraInt = new List<int>();
    public Transform cameraReference;
    public int currentCamera;

    [Range(0.01f, 20)] public float smoothFollowing;

    public void CreateCameras()
    {
        cameraObjects.Clear();

        if (camerasParent == null)
        {
            camerasParent = new GameObject("Cameras Parent");
            camerasParent.transform.parent = this.transform.GetChild(1);

            if (typeOfCamera.Count > 1)
            {
                for (var i = 0; i < typeOfCamera.Count; ++i)
                {
                    cameraObjects.Add(new GameObject("camPlace " + i));
                    cameraObjects[i].transform.parent = camerasParent.transform;
                    cameraObjects[i].transform.position = this.transform.position;
                }
            }
            else
            {
                cameraObjects.Add(new GameObject("camPlace "));
                cameraObjects[0].transform.parent = camerasParent.transform;
                cameraObjects[0].transform.position = this.transform.position;
            }

        }        
    }

    #endregion

    #region CONTROLS

    public CarInputs carInputs;
    [SerializeField] private TypeOfController controller = TypeOfController.Keyboard;

    [SerializeField] private mainControlKey steeringModeKey = mainControlKey.Standard;
    [SerializeField] private mainControlKey gasModeKey = mainControlKey.Standard;
    [SerializeField] private mainControlKey brakeModeKey = mainControlKey.Standard;

    [SerializeField] private mainControlJoystick steeringModeJoystick = mainControlJoystick.Standard;
    [SerializeField] private mainControlJoystick gasModeJoystick = mainControlJoystick.Standard;
    [SerializeField] private mainControlJoystick brakeModeJoystick = mainControlJoystick.Standard;

    [SerializeField] private ButtonNumber buttonSteeringLeft = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonSteeringRight = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonGas = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonBrake = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonHandBrake = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonNos = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonCamera = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonLights = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonGearUp = ButtonNumber.button0;
    [SerializeField] private ButtonNumber buttonGearDown = ButtonNumber.button0;

    public int typeOfController;

    public int joystickNumber;

    public int keySteeringMode;
    public int keyGasMode;
    public int keyBrakeMode;

    public int joystickSteeringMode;
    public int joystickGasMode;
    public int joystickBrakeMode;

    public int steeringLeftButton;
    public int steeringRightButton;
    public int gasButton;
    public int brakeButton;
    public int handBrakeButton;
    public int nosButton;
    public int cameraButton;
    public int lightsButton;
    public int gearUpButton;
    public int gearDownButton;


    public string steeringLeftKey = "a";
    public string steeringRightKey = "d";
    public string gasKey = "w";
    public string brakeKey = "s";
    public string handBrakeKey = "space";
    public string nosKey = "n";
    public string cameraKey = "c";
    public string lightsKey = "l";
    public string gearUp = "e";
    public string gearDown = "q";  


    #endregion

    void Start()
    {
        #region INITIALIZE COMPONENTS

        rb = GetComponent<Rigidbody>();
        rb.mass = vehicleMass;
        centerOfMass = rb.centerOfMass;
        currentTorque = motorTorque - (tractionControl * motorTorque);
        maxSpeedsList = new List<float>();
        

        #region LIGHTS INITIALIZATION

        frontLights = transform.GetChild(1).GetChild(1).gameObject;
        rearLights = transform.GetChild(1).GetChild(2).gameObject;

        rearLights.SetActive(false);

        #endregion

        #region SPEEDOMETER

        typeOfSpeedometer = (int)speedometerType;

#if false
        

        if (analogSpeedometerCreated || digitalSpeedometerCreated)
        {
            if (typeOfSpeedometer == 0)
            {
                analogSpeedometer = transform.GetChild(2).Find("AnalogSpeedometer(Clone)").GetComponent<AnalogSpeedometer>();
                if (speedType == SpeedType.KPH)
                {
                    analogSpeedometer.speedType.GetComponent<Text>().text = "KPH";
                }
                else
                {
                    analogSpeedometer.speedType.GetComponent<Text>().text = "MPH";
                }
            }
            else
            {
                digitalSpeedometer = transform.GetChild(2).Find("DigitalSpeedometer(Clone)").GetComponent<DigitalSpeedometer>();

                if (speedType == SpeedType.KPH)
                {
                    digitalSpeedometer.speedType.GetComponent<Text>().text = "KPH";
                }
                else
                {
                    digitalSpeedometer.speedType.GetComponent<Text>().text = "MPH";
                }
            }
        }
#endif

        #endregion

        #region NOS

        if (nosON)
        {
            NOSParent = transform.GetChild(2).Find("NOS Parent").gameObject;
            NOSGameObj = NOSParent.transform.GetChild(0);
            NOSBarValue = NOSGameObj.GetComponent<Slider>();

            NOSTimeLeft = NOSMaxTime;
        }

        #endregion

        #region MOBILE UI

        typeOfMobileUI = (int)mobileUIType;

        mobileUIParent = transform.GetChild(2).Find("MobileUI(Clone)");

        if(mobileUIParent != null)
        {
            MobileUION = true;
        }

        #endregion

        #region CAMERAS

        var checkCameraParent = GameObject.Find("Cameras Parent");

        if (checkCameraParent != null)
        {
            if (checkCameraParent.transform.parent = this.transform.GetChild(1))
            {
                camerasParent = transform.GetChild(1).Find("Cameras Parent").gameObject;

                if (cameraReference.GetComponent<FollowPlayer>() == null)
                {
                    cameraReference.gameObject.AddComponent<FollowPlayer>();
                }

                cameraReference.GetComponent<FollowPlayer>().lookTarget = this.transform;
                cameraReference.GetComponent<FollowPlayer>().sSpeed = smoothFollowing;
                cameraReference.GetComponent<FollowPlayer>().cameraTarget = camerasParent.transform.GetChild(0);

                currentCamera = 0;

                foreach (var camera in typeOfCamera)
                {
                    typeOfCameraInt.Add((int)camera);
                }
            }            
        }
        else
        {
            Debug.Log("Create Following Camera Objects");
        }

        #endregion

        #region SMOKE

        smokeParticles = transform.root.GetComponentInChildren<ParticleSystem>();

        if (!smokeOn)
        {
            smokeParticles.Stop();
        }
        else
        {
            smokeParticles.Play();
        }

        #endregion

        #region INPUTS

        carInputs = gameObject.AddComponent<CarInputs>();
        typeOfController = (int)controller;


        keySteeringMode = (int)steeringModeKey;
        keyGasMode = (int)gasModeKey;
        keyBrakeMode = (int)brakeModeKey;

        joystickSteeringMode = (int)steeringModeJoystick;
        joystickGasMode = (int)gasModeJoystick;
        joystickBrakeMode = (int)brakeModeJoystick;

        steeringLeftButton = (int)buttonSteeringLeft;
        steeringRightButton = (int)buttonSteeringRight;
        gasButton = (int)buttonGas;
        brakeButton = (int)buttonBrake;
        handBrakeButton = (int)buttonHandBrake;
        nosButton = (int)buttonNos;
        cameraButton = (int)buttonCamera;
        lightsButton = (int)buttonLights;
        gearUpButton = (int)buttonGearUp;
        gearDownButton = (int)buttonGearDown;

        #endregion

        #region MANUAL GEARS INITIATION

        speedXGear = maxSpeed / numberOfGears;

        for (int i = 0; i < numberOfGears; i++)
        {
            maxSpeedsList.Add(speedXGear * (i + 1));
        }

        #endregion

        #region TYPE OF TRANSMISSION

        switch (transmission)
        {
            case Transmission.auto:
                autoTransmission = true;
                break;
            case Transmission.manual:
                autoTransmission = false;
                break;
        }

        #endregion

        EngineAudio audioScript = gameObject.AddComponent<EngineAudio>();
        DamageSystem damageScript = gameObject.AddComponent<DamageSystem>();


        #region EXHAUST

        if (exhaustFlame)
        {
            if(exhaustObj == null)
            {
                exhaustObj = transform.GetChild(1).Find("ExhaustPipe(Clone)").gameObject;

                exhaustVisual = exhaustObj.GetComponent<ParticleSystem>();

                exhaustSoundSource = exhaustObj.GetComponent<AudioSource>();
                exhaustSoundSource.clip = exhaustSound;
                exhaustSoundSource.volume = exhaustVolume;
            }
        }

        #endregion

        SetWheelsValues();

        #endregion        
    }

    private void Update()
    {
        #region CAMERA

        if (camerasParent != null)
        {
            CameraSwitch();
            CameraScriptON(currentCamera);
        }

        #endregion

        #region NOS

        if (nosON)
        {
            if (!MobileUION)
            {
                if (Input.GetKey(nosKey) || Input.GetKey("joystick " + joystickNumber + " button " + nosButton))
                {
                    nosActive = true;
                }
                else
                {
                    nosActive = false;
                }
            }

            NOSON();
        }

        #endregion

        #region LIGHTS MANAGER

        if (Input.GetKeyDown(lightsKey) || Input.GetKeyDown("joystick " + joystickNumber + " button " + lightsButton))
        {
            if (!frontLights.activeSelf)
            {
                frontLights.SetActive(true);

                if (analogSpeedometerCreated || digitalSpeedometerCreated)
                {
                    if (speedometerType == SpeedometerType.Analog)
                    {
                        analogSpeedometer.lightsImg.gameObject.SetActive(true);
                    }
                    else
                    {
                        digitalSpeedometer.lightsImg.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                frontLights.SetActive(false);

                if (analogSpeedometerCreated || digitalSpeedometerCreated)
                {
                    if (speedometerType == SpeedometerType.Analog)
                    {
                        analogSpeedometer.lightsImg.gameObject.SetActive(false);
                    }
                    else
                    {
                        digitalSpeedometer.lightsImg.gameObject.SetActive(false);
                    }
                }                    
            }
        }

        #endregion

        #region CURRENT SPEED

        switch (speedType)
        {
            case SpeedType.MPH:

                currentSpeed = rb.velocity.magnitude * 2.23693629f;

                break;
            case SpeedType.KPH:

                currentSpeed = rb.velocity.magnitude * 3.6f;

                break;
        }

        #endregion
    }

    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        #region CLUTCH

        if (clutchOn)
        {
            accel = 0;
        }

        #endregion

        AnimateWheels();

        #region GETINPUTS

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        handbrake = Mathf.Clamp(handbrake, 0, 1);

        #endregion

        #region GEARS

        CalculateRPM();

        #region TYPE OF TRANSMISSION

        switch (transmission)
        {
            case Transmission.auto:
                AutoGearSystem();
                break;
            case Transmission.manual:
                ManualGearSystem();
                MinSpeedRequired();
                break;
        }

        #endregion

        #endregion

        #region DRIVING

        Steering(steering);
        
        SteerHelper();

        ApplyDrive(accel, footbrake);

        MaxSpeedReached();

        HandBreaking(handbrake);

        #endregion

        #region AERODYNAMICS

        AddDownForce();

        if(skidMarks == true)
        {
            CheckForWheelSpin();
        }
        
        TractionControl();

        #endregion

        #region STEERING WHEEL

        if (steeringWheelPivot != null)
        {
            SteeringWheelMovement(steering);
        }

        #endregion

        #region REVERSE GEAR CHECK
#if false
        if (analogSpeedometerCreated || digitalSpeedometerCreated)
        {
            if (reverseGearOn)
            {
                if (speedometerType == SpeedometerType.Analog)
                {
                    analogSpeedometer.currentGear.GetComponent<Text>().text = "R";
                }
                else
                {
                    digitalSpeedometer.currentGear.GetComponent<Text>().text = "R";
                }
            }
            else
            {
                if (speedometerType == SpeedometerType.Analog)
                {
                    analogSpeedometer.currentGear.GetComponent<Text>().text = (currentGear + 1).ToString();
                }
                else
                {
                    digitalSpeedometer.currentGear.GetComponent<Text>().text = (currentGear + 1).ToString();
                }
            }
        }
#endif
        #endregion

    }

    #region DRIVING

    private void AnimateWheels()
    {
        Quaternion FLRot;
        Vector3 FLPos;
        Quaternion FRRot;
        Vector3 FRPos;
        Quaternion BLRot;
        Vector3 BLPos;
        Quaternion BRRot;
        Vector3 BRPos;

        // Front Left
        frontLeftCol.GetComponent<WheelCollider>().GetWorldPose(out FLPos, out FLRot);
        FLRot = FLRot * Quaternion.Euler(wheelsRotation);
        frontLeft.transform.position = FLPos;
        frontLeft.transform.rotation = FLRot;

        // Front Right
        frontRightCol.GetComponent<WheelCollider>().GetWorldPose(out FRPos, out FRRot);
        FRRot = FRRot * Quaternion.Euler(wheelsRotation);
        frontRight.transform.position = FRPos;
        frontRight.transform.rotation = FRRot;

        // Back Left
        backLeftCol.GetComponent<WheelCollider>().GetWorldPose(out BLPos, out BLRot);
        BLRot = BLRot * Quaternion.Euler(wheelsRotation);
        backLeft.transform.position = BLPos;
        backLeft.transform.rotation = BLRot;

        // Back Right
        backRightCol.GetComponent<WheelCollider>().GetWorldPose(out BRPos, out BRRot);
        BRRot = BRRot * Quaternion.Euler(wheelsRotation);
        backRight.transform.position = BRPos;
        backRight.transform.rotation = BRRot;

        // Extra Wheels
        int i = 0;
        foreach (var wheelCol in extraWheelsColList)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheelCol.collider.GetComponent<WheelCollider>().GetWorldPose(out _pos, out _rot);
            _rot = _rot * Quaternion.Euler(wheelsRotation);
            extraWheels[i].model.transform.position = _pos;
            extraWheels[i].model.transform.rotation = _rot;

            i++;
        }

    }
    private void Steering(float steering)
    {
        var steerAngle = steering * maximumSteerAngle;
        frontLeftCol.GetComponent<WheelCollider>().steerAngle = Mathf.Lerp(frontLeftCol.GetComponent<WheelCollider>().steerAngle, steerAngle, 0.5f);
        frontRightCol.GetComponent<WheelCollider>().steerAngle = Mathf.Lerp(frontRightCol.GetComponent<WheelCollider>().steerAngle, steerAngle, 0.5f);

        foreach (var wheelCol in extraWheelsColList)
        {
            if (wheelCol.axel == Axl.Front)
            {
                wheelCol.collider.GetComponent<WheelCollider>().steerAngle = Mathf.Lerp(wheelCol.collider.GetComponent<WheelCollider>().steerAngle, steerAngle, 0.5f);
            }
        }
    }

    private void SteerHelper()
    {
        // wheels don't touch ground
        WheelHit wHFL;
        WheelHit wHFR;
        WheelHit wHBL;
        WheelHit wHBR;

        frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wHFL);
        frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out wHFR);
        backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wHBL);
        backRightCol.GetComponent<WheelCollider>().GetGroundHit(out wHBR);

        if (wHFL.normal == Vector3.zero)
            return; 
        if (wHFR.normal == Vector3.zero)
            return; 
        if (wHBL.normal == Vector3.zero)
            return; 
        if (wHBR.normal == Vector3.zero)
            return; 

        // Extra Wheels
        int i = 0;
        foreach (var wheelCol in extraWheelsColList)
        {
            WheelHit wheelhit;
            wheelCol.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; 
            i++;
        }

        // Shift direction debug
        if (Mathf.Abs(oldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - oldRotation) * steerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            rb.velocity = velRotation * rb.velocity;
        }
        oldRotation = transform.eulerAngles.y;
    }

    private void ApplyDrive(float accel, float footbrake)
    {
        float thrustTorque;

        switch (carDriveType)
        {
            case CarDriveType.FourWheelDrive:

                thrustTorque = accel * (currentTorque / 4f) * enginePower.Evaluate(1);

                frontLeftCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                frontRightCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                backLeftCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                backRightCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;

                foreach (var wheelCol in extraWheelsColList)
                {
                    wheelCol.collider.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                }

                break;

            case CarDriveType.FrontWheelDrive:

                thrustTorque = accel * (currentTorque / 2f) * enginePower.Evaluate(1);

                frontLeftCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                frontRightCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;

                foreach (var wheelCol in extraWheelsColList)
                {
                    if (wheelCol.axel == Axl.Front)
                    {
                        wheelCol.collider.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                    }
                }
                
                break;

            case CarDriveType.RearWheelDrive:

                thrustTorque = accel * (currentTorque / 2f) * enginePower.Evaluate(1);

                backLeftCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                backRightCol.GetComponent<WheelCollider>().motorTorque = thrustTorque;

                foreach (var wheelCol in extraWheelsColList)
                {
                    if (wheelCol.axel == Axl.Rear)
                    {
                        wheelCol.collider.GetComponent<WheelCollider>().motorTorque = thrustTorque;
                    }
                }

                break;

        }

        #region FOOTBRAKE

        if (footbrake > 0)
        {
            if (currentSpeed > 5 && Vector3.Angle(transform.forward, rb.velocity) < 50f)
            {
                reverseGearOn = false;
            }
            else
            {
                reverseGearOn = true;
            }
        }
        else
        {
            reverseGearOn = false;
        }

        if (!ABS)
        {
            if (currentSpeed > 5 && Vector3.Angle(transform.forward, rb.velocity) < 50f)
            {
                frontLeftCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
                frontRightCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
                backLeftCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
                backRightCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;

                foreach (var wheelCol in extraWheelsColList)
                {
                    wheelCol.collider.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
                }

                if (footbrake > 0)
                {
                    rearLights.SetActive(true);
                }
                else
                {
                    rearLights.SetActive(false);
                }
            }
            else if (footbrake > 0)
            {
                rearLights.SetActive(false);

                frontLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                frontRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                backLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                backRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                frontLeftCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                frontRightCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                backLeftCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                backRightCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;

                foreach (var wheelCol in extraWheelsColList)
                {
                    wheelCol.collider.GetComponent<WheelCollider>().brakeTorque = 0f;
                    wheelCol.collider.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                }                
            }
        }
        else
        {
            if (currentSpeed > 5 && Vector3.Angle(transform.forward, rb.velocity) < 50f)
            {
                StartCoroutine(ABSCoroutine(footbrake));

                if (footbrake > 0)
                {
                    rearLights.SetActive(true);
                }
                else
                {
                    rearLights.SetActive(false);
                }
            }
            else if (footbrake > 0)
            {
                rearLights.SetActive(false);

                frontLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                frontRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                backLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                backRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
                frontLeftCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                frontRightCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                backLeftCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                backRightCol.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;

                foreach (var wheelCol in extraWheelsColList)
                {
                    wheelCol.collider.GetComponent<WheelCollider>().brakeTorque = 0f;
                    wheelCol.collider.GetComponent<WheelCollider>().motorTorque = -reverseTorque * footbrake;
                }
            }
        }

        #endregion

    }

    private void MaxSpeedReached()
    {
        switch (transmission)
        {
            case Transmission.manual:

                switch (speedType)
                {
                    case SpeedType.MPH:

                        if (currentSpeed > maxSpeedsList[currentGear])
                            rb.velocity = (maxSpeedsList[currentGear] / 2.23693629f) * rb.velocity.normalized;
                        break;

                    case SpeedType.KPH:

                        if (currentSpeed > maxSpeedsList[currentGear])
                            rb.velocity = (maxSpeedsList[currentGear] / 3.6f) * rb.velocity.normalized;
                        break;
                }

                break;

            case Transmission.auto:

                switch (speedType)
                {
                    case SpeedType.MPH:

                        if (currentSpeed > maxSpeed)
                            rb.velocity = (maxSpeed / 2.23693629f) * rb.velocity.normalized;
                        break;

                    case SpeedType.KPH:

                        if (currentSpeed > maxSpeed)
                            rb.velocity = (maxSpeed / 3.6f) * rb.velocity.normalized;
                        break;
                }

                break;
        }
        

    }

    private void MinSpeedRequired()
    {
        if (transmission == Transmission.manual)
        {
            if (currentGear > 2)
            {
                if (currentSpeed < maxSpeedsList[currentGear - 3])
                {
                    currentTorque /= (currentGear - 1);
                }
            }        
        }        
    }

    private void HandBreaking(float handbrake)
    {
        if (handbrake > 0f)
        {
            var hbTorque = handbrake * handbrakeTorque;
            backLeftCol.GetComponent<WheelCollider>().brakeTorque = hbTorque;
            backRightCol.GetComponent<WheelCollider>().brakeTorque = hbTorque;
        }
    }

    IEnumerator ABSCoroutine(float footbrake)
    {
        frontLeftCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
        frontRightCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
        backLeftCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
        backRightCol.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;

        foreach (var wheelCol in extraWheelsColList)
        {
            wheelCol.collider.GetComponent<WheelCollider>().brakeTorque = brakeTorque * footbrake;
        }

        yield return new WaitForSeconds(0.1f);

        frontLeftCol.GetComponent<WheelCollider>().brakeTorque = 0;
        frontRightCol.GetComponent<WheelCollider>().brakeTorque = 0;
        backLeftCol.GetComponent<WheelCollider>().brakeTorque = 0;
        backRightCol.GetComponent<WheelCollider>().brakeTorque = 0;

        foreach (var wheelCol in extraWheelsColList)
        {
            wheelCol.collider.GetComponent<WheelCollider>().brakeTorque = 0;
        }

        yield return new WaitForSeconds(0.1f);
    }

    #endregion

    #region WHEELS VALUES

    private void SetWheelsValues()
    {
        frontLeftCol.GetComponent<WheelCollider>().mass = wheelsMass;
        frontRightCol.GetComponent<WheelCollider>().mass = wheelsMass;
        backLeftCol.GetComponent<WheelCollider>().mass = wheelsMass;
        backRightCol.GetComponent<WheelCollider>().mass = wheelsMass;

        frontLeftCol.GetComponent<WheelCollider>().radius *= wheelsRadius;
        frontRightCol.GetComponent<WheelCollider>().radius *= wheelsRadius;
        backLeftCol.GetComponent<WheelCollider>().radius *= wheelsRadius;
        backRightCol.GetComponent<WheelCollider>().radius *= wheelsRadius;

        frontLeftCol.GetComponent<WheelCollider>().forceAppPointDistance = forcePoint;
        frontRightCol.GetComponent<WheelCollider>().forceAppPointDistance = forcePoint;
        backLeftCol.GetComponent<WheelCollider>().forceAppPointDistance = forcePoint;
        backRightCol.GetComponent<WheelCollider>().forceAppPointDistance = forcePoint;

        frontLeftCol.GetComponent<WheelCollider>().wheelDampingRate = dumpingRate;
        frontRightCol.GetComponent<WheelCollider>().wheelDampingRate = dumpingRate;
        backLeftCol.GetComponent<WheelCollider>().wheelDampingRate = dumpingRate;
        backRightCol.GetComponent<WheelCollider>().wheelDampingRate = dumpingRate;

        frontLeftCol.GetComponent<WheelCollider>().suspensionDistance = suspensionDistance;
        frontRightCol.GetComponent<WheelCollider>().suspensionDistance = suspensionDistance;
        backLeftCol.GetComponent<WheelCollider>().suspensionDistance = suspensionDistance;
        backRightCol.GetComponent<WheelCollider>().suspensionDistance = suspensionDistance;

        frontLeftCol.GetComponent<WheelCollider>().center = wheelsPosition;
        frontRightCol.GetComponent<WheelCollider>().center = wheelsPosition;
        backLeftCol.GetComponent<WheelCollider>().center = wheelsPosition;
        backRightCol.GetComponent<WheelCollider>().center = wheelsPosition;

        var leftCol = frontLeftCol.GetComponent<WheelCollider>().suspensionSpring;
        var rightCol = frontRightCol.GetComponent<WheelCollider>().suspensionSpring;
        var bLeft = backLeftCol.GetComponent<WheelCollider>().suspensionSpring;
        var bRight = backRightCol.GetComponent<WheelCollider>().suspensionSpring;
        leftCol.spring = suspensionSpring;
        rightCol.spring = suspensionSpring;
        bLeft.spring = suspensionSpring;
        bRight.spring = suspensionSpring;
        leftCol.damper = suspensionDamper;
        rightCol.damper = suspensionDamper;
        bLeft.damper = suspensionDamper;
        bRight.damper = suspensionDamper;
        leftCol.targetPosition = targetPosition;
        rightCol.targetPosition = targetPosition;
        bLeft.targetPosition = targetPosition;
        bRight.targetPosition = targetPosition;
        frontLeftCol.GetComponent<WheelCollider>().suspensionSpring = leftCol;
        frontRightCol.GetComponent<WheelCollider>().suspensionSpring = rightCol;
        backLeftCol.GetComponent<WheelCollider>().suspensionSpring = bLeft;
        backRightCol.GetComponent<WheelCollider>().suspensionSpring = bRight;
        
        var fl = frontLeftCol.GetComponent<WheelCollider>().sidewaysFriction;
        var fr = frontRightCol.GetComponent<WheelCollider>().sidewaysFriction;
        var bl = backLeftCol.GetComponent<WheelCollider>().sidewaysFriction;
        var br = backRightCol.GetComponent<WheelCollider>().sidewaysFriction;
        fl.stiffness = wheelStiffness;
        fr.stiffness = wheelStiffness;
        bl.stiffness = wheelStiffness;
        br.stiffness = wheelStiffness;
        frontLeftCol.GetComponent<WheelCollider>().sidewaysFriction = fl;
        frontRightCol.GetComponent<WheelCollider>().sidewaysFriction = fr;
        backLeftCol.GetComponent<WheelCollider>().sidewaysFriction = bl;
        backRightCol.GetComponent<WheelCollider>().sidewaysFriction = br;

        
        var flf = frontLeftCol.GetComponent<WheelCollider>().forwardFriction;
        var frf = frontRightCol.GetComponent<WheelCollider>().forwardFriction;
        var blf = backLeftCol.GetComponent<WheelCollider>().forwardFriction;
        var brf = backRightCol.GetComponent<WheelCollider>().forwardFriction;
        flf.stiffness = wheelStiffness;
        frf.stiffness = wheelStiffness;
        blf.stiffness = wheelStiffness;
        brf.stiffness = wheelStiffness;
        frontLeftCol.GetComponent<WheelCollider>().forwardFriction = flf;
        frontRightCol.GetComponent<WheelCollider>().forwardFriction = frf;
        backLeftCol.GetComponent<WheelCollider>().forwardFriction = blf;
        backRightCol.GetComponent<WheelCollider>().forwardFriction = brf;

        foreach (var wheelCol in extraWheelsColList)
        {
            wheelCol.collider.GetComponent<WheelCollider>().mass = wheelsMass;
            wheelCol.collider.GetComponent<WheelCollider>().radius = extraWheelRadius * wheelsRadius;
            wheelCol.collider.GetComponent<WheelCollider>().forceAppPointDistance = forcePoint;
            wheelCol.collider.GetComponent<WheelCollider>().wheelDampingRate = dumpingRate;
            wheelCol.collider.GetComponent<WheelCollider>().suspensionDistance = suspensionDistance;
            wheelCol.collider.GetComponent<WheelCollider>().center = wheelsPosition;

            var susp = wheelCol.collider.GetComponent<WheelCollider>().suspensionSpring;
            susp.spring = suspensionSpring;
            susp.damper = suspensionDamper;
            susp.targetPosition = targetPosition;
            wheelCol.collider.GetComponent<WheelCollider>().suspensionSpring = susp;

            var stifn = wheelCol.collider.GetComponent<WheelCollider>().sidewaysFriction;
            stifn.stiffness = wheelStiffness;
            wheelCol.collider.GetComponent<WheelCollider>().sidewaysFriction = stifn;
        }
    }

    #endregion

    #region GEAR SYSTEM
    private void AutoGearSystem()
    {        
        float gearRatio = Mathf.Abs(currentSpeed / maxSpeed);

        float gearUp = (1 / (float)numberOfGears) * (currentGear + 1);
        float gearDown = (1 / (float)numberOfGears) * currentGear;

        if (currentGear > 0 && gearRatio < gearDown)
        {
            currentGear--;
        }

        if (gearRatio > gearUp && (currentGear < (numberOfGears - 1)))
        {
            if (!reverseGearOn)
            {
                if (exhaustFlame)
                {
                    ExhaustFX();
                }
                currentGear++;
            }
        }
    }

    private void ManualGearSystem()
    {
        if (Input.GetKeyDown(gearDown) || Input.GetKeyDown("joystick " + joystickNumber + " button " + gearDownButton))
        {
            if (currentGear > 0)
            {
                clutchOn = true;
                currentGear--;
                StartCoroutine(ClutchCoroutine());
            }
        }

        if (Input.GetKeyDown(gearUp) || Input.GetKeyDown("joystick " + joystickNumber + " button " + gearUpButton))
        {
            if (currentGear < (numberOfGears - 1))
            {
                if (!reverseGearOn)
                {
                    clutchOn = true;
                    ExhaustFX();
                    currentGear++;
                    StartCoroutine(ClutchCoroutine());
                }
            }
        }
    }

    // Curved Bias towards 1 for a value between 0-1
    private static float BiasCurve(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // Smooth Lerp with no fixed Boundaries
    private static float SmoothLerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        // Smooth Gear Changing
        float f = (1 / (float)numberOfGears);

        var targetGearFactor = Mathf.InverseLerp(f * currentGear, f * (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));
        gearFactor = Mathf.Lerp(gearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRPM()
    {
        // Calculate engine RPM
        CalculateGearFactor();
        float gearNumFactor;

        if (autoTransmission)
        {
            gearNumFactor = (currentGear / (float)numberOfGears);
        }
        else
        {            
            float curGearRange = (currentGear + 1) / (float)numberOfGears;
            float speedGearRange = Mathf.Abs(currentSpeed / maxSpeed);
            gearNumFactor = speedGearRange / curGearRange;
        }

        var minRPM = SmoothLerp(0f, rpmRange, BiasCurve(gearNumFactor));
        var maxRPM = SmoothLerp(rpmRange, 1f, gearNumFactor);

        RPM = SmoothLerp(minRPM, maxRPM, gearFactor);
    }

    #endregion

    #region AERODYNAMICS
    private void AddDownForce()
    {
        rb.AddForce(-transform.up * downForce * rb.velocity.magnitude);
    }

    private void TractionControl()
    {
        WheelHit wheelHit;
        WheelHit flHit;
        WheelHit frHit;
        WheelHit blHit;
        WheelHit brHit;

        switch (carDriveType)
        {
            case CarDriveType.FourWheelDrive:

                frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out flHit);
                AdjustTorque(flHit.forwardSlip);

                frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out frHit);
                AdjustTorque(frHit.forwardSlip);

                backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out blHit);
                AdjustTorque(blHit.forwardSlip);

                backRightCol.GetComponent<WheelCollider>().GetGroundHit(out brHit);
                AdjustTorque(brHit.forwardSlip);

                foreach (var wheelCol in extraWheelsColList)
                {
                    wheelCol.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                }

                break;

            case CarDriveType.RearWheelDrive:

                backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out blHit);
                AdjustTorque(blHit.forwardSlip);

                backRightCol.GetComponent<WheelCollider>().GetGroundHit(out brHit);
                AdjustTorque(brHit.forwardSlip);

                foreach (var wheelCol in extraWheelsColList)
                {
                    if (wheelCol.axel == Axl.Rear)
                    {
                        wheelCol.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
                        AdjustTorque(wheelHit.forwardSlip);
                    }
                }

                break;

            case CarDriveType.FrontWheelDrive:

                frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out flHit);
                AdjustTorque(flHit.forwardSlip);

                frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out frHit);
                AdjustTorque(frHit.forwardSlip);

                foreach (var wheelCol in extraWheelsColList)
                {
                    if (wheelCol.axel == Axl.Front)
                    {
                        wheelCol.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
                        AdjustTorque(wheelHit.forwardSlip);
                    }
                }

                break;
        }
    }

    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= slipLimit && currentTorque >= 0)
        {
            currentTorque -= 10 * tractionControl;
        }
        else
        {
            currentTorque += 10 * tractionControl;
            if (currentTorque > motorTorque)
            {
                currentTorque = motorTorque;
            }
        }
    }

    #endregion

    #region SKID MANAGER

    private void CheckForWheelSpin()
    {
        WheelHit wheelnHit;
        WheelHit flnHit;
        WheelHit frnHit;
        WheelHit blnHit;
        WheelHit brnHit;

        frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out flnHit);
        frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out frnHit);
        backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out blnHit);
        backRightCol.GetComponent<WheelCollider>().GetGroundHit(out brnHit);

        #region FRONT LEFT SLIP


        if (Mathf.Abs(flnHit.forwardSlip) >= slipLimit || Mathf.Abs(flnHit.sidewaysSlip) >= slipLimit)
        {
            frontLeftCol.GetComponent<WheelsFX>().EmitTyreSmoke();

            if (!AnySkidSoundPlaying())
            {
                frontLeftCol.GetComponent<WheelsFX>().PlayAudio();
            }
        }
        else
        {
            if (frontLeftCol.GetComponent<WheelsFX>().playingAudio)
            {
                frontLeftCol.GetComponent<WheelsFX>().StopAudio();
            }

            frontLeftCol.GetComponent<WheelsFX>().EndSkidTrail();
        }


        #endregion

        #region FRONT RIGHT SLIP

        if (Mathf.Abs(frnHit.forwardSlip) >= slipLimit || Mathf.Abs(frnHit.sidewaysSlip) >= slipLimit)
        {
            frontRightCol.GetComponent<WheelsFX>().EmitTyreSmoke();

            if (!AnySkidSoundPlaying())
            {
                frontRightCol.GetComponent<WheelsFX>().PlayAudio();
            }
        }
        else
        {
            if (frontRightCol.GetComponent<WheelsFX>().playingAudio)
            {
                frontRightCol.GetComponent<WheelsFX>().StopAudio();
            }

            frontRightCol.GetComponent<WheelsFX>().EndSkidTrail();
        }
        

        #endregion

        #region BACK LEFT SLIP

        if (Mathf.Abs(blnHit.forwardSlip) >= slipLimit || Mathf.Abs(blnHit.sidewaysSlip) >= slipLimit)
        {
            backLeftCol.GetComponent<WheelsFX>().EmitTyreSmoke();

            if (!AnySkidSoundPlaying())
            {
                backLeftCol.GetComponent<WheelsFX>().PlayAudio();
            }
        }
        else
        {
            if (backLeftCol.GetComponent<WheelsFX>().playingAudio)
            {
                backLeftCol.GetComponent<WheelsFX>().StopAudio();
            }

            backLeftCol.GetComponent<WheelsFX>().EndSkidTrail();
        }



        #endregion

        #region BACK RIGHT SLIP

        if (Mathf.Abs(brnHit.forwardSlip) >= slipLimit || Mathf.Abs(brnHit.sidewaysSlip) >= slipLimit)
        {
            backRightCol.GetComponent<WheelsFX>().EmitTyreSmoke();

            if (!AnySkidSoundPlaying())
            {
                backRightCol.GetComponent<WheelsFX>().PlayAudio();
            }
        }
        else
        {
            if (backRightCol.GetComponent<WheelsFX>().playingAudio)
            {
                backRightCol.GetComponent<WheelsFX>().StopAudio();
            }

            backRightCol.GetComponent<WheelsFX>().EndSkidTrail();
        }
        
        #endregion

        #region EXTRA WHEELS SLIP

        foreach (var wheelCol in extraWheelsColList)
        {
            wheelCol.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelnHit);

            if (Mathf.Abs(wheelnHit.forwardSlip) >= slipLimit || Mathf.Abs(wheelnHit.sidewaysSlip) >= slipLimit)
            {
                wheelCol.collider.GetComponent<WheelsFX>().EmitTyreSmoke();

                if (!AnySkidSoundPlaying())
                {
                    wheelCol.collider.GetComponent<WheelsFX>().PlayAudio();
                }
                continue;
            }

            
            if (wheelCol.collider.GetComponent<WheelsFX>().playingAudio)
            {
                wheelCol.collider.GetComponent<WheelsFX>().StopAudio();
            }
            
            wheelCol.collider.GetComponent<WheelsFX>().EndSkidTrail();

        }

        #endregion
    }

    private bool AnySkidSoundPlaying()
    {

        if (frontLeftCol.GetComponent<WheelsFX>().playingAudio)
        {
            return true;
        }

        else if (frontRightCol.GetComponent<WheelsFX>().playingAudio)
        {
            return true;
        }

        else if (backLeftCol.GetComponent<WheelsFX>().playingAudio)
        {
            return true;
        }

        else if (backRightCol.GetComponent<WheelsFX>().playingAudio)
        {
            return true;
        }

        else foreach (var wheelCol in extraWheelsColList)
        {
            if (wheelCol.collider.GetComponent<WheelsFX>().playingAudio)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region CLUTCH

    public IEnumerator ClutchCoroutine()
    {
        yield return new WaitForSeconds(clutch);
        clutchOn = false;
    }

    #endregion

    #region EXAUST

    public void ExhaustFX()
    {
        if (exhaustObj != null)
        {
            exhaustSoundSource.Play();
            exhaustVisual.Play();
        }
    }

    public void CreateExhaustGameObj()
    {
        exhaustObjectPrefab = Resources.Load<Transform>("ExhaustPipe");
        exhaustObject = Instantiate(exhaustObjectPrefab);
        exhaustObject.transform.parent = this.transform.GetChild(1);
    }

    public void DestroyExhaustGameObj()
    {
        exhaustObj = transform.GetChild(1).Find("ExhaustPipe(Clone)").gameObject;
        DestroyImmediate(exhaustObj, true);
    }

    #endregion

    #region MOBILE UI

    public void CreateMobileUI()
    {
        mobileUIPrefab = Resources.Load<Transform>("MobileUI");
        mobileUIObj = transform.GetChild(2).Find("MobileUI(Clone)");

        switch (mobileUIType)
        {
            case MobileUIType.Arrows:

                mobileArrowsHorizontalInputsPrefab = Resources.Load<Transform>("MobileArrows");

                break;
            case MobileUIType.Joystick:

                mobileJoystickHorizontalInputsPrefab = Resources.Load<Transform>("MobileJoystick");

                break;
        }

        if (mobileUIObj == null)
        {
            mobileUIObj = Instantiate(mobileUIPrefab);
            mobileUIObj.transform.SetParent(this.transform.GetChild(2));
            mobileUIObj.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            mobileUIObj.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            MobileUION = true;


            switch (mobileUIType)
            {
                case MobileUIType.Arrows:

                    mobileArrowsHorizontalInputs = Instantiate(mobileArrowsHorizontalInputsPrefab);
                    mobileArrowsHorizontalInputs.transform.SetParent(mobileUIObj);
                    mobileArrowsHorizontalInputs.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                    mobileArrowsHorizontalInputs.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    break;
                case MobileUIType.Joystick:

                    mobileJoystickHorizontalInputs = Instantiate(mobileJoystickHorizontalInputsPrefab);
                    mobileJoystickHorizontalInputs.transform.SetParent(mobileUIObj);
                    mobileJoystickHorizontalInputs.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-290, -145, 0);
                    mobileJoystickHorizontalInputs.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    break;
            }

        }
    }

    public void DestroyMobileUI()
    {
        if (MobileUION)
        {
            mobileUIObject = transform.GetChild(2).Find("MobileUI(Clone)").gameObject;
            DestroyImmediate(mobileUIObject, true);
            MobileUION = false;
        }
    }

    #endregion

    #region CAMERAS

    public void CameraSwitch()
    {
        if (Input.GetKeyDown(cameraKey) || Input.GetKeyDown("joystick " + joystickNumber + " button " + cameraButton) || changeCamera)
        {
            if (currentCamera < typeOfCamera.Count - 1)
            {
                currentCamera++;
            }
            else
            {
                currentCamera = 0;
                cameraReference.GetComponent<FollowPlayer>().cameraTarget = camerasParent.transform.GetChild(0);
            }

            cameraReference.GetComponent<FollowPlayer>().cameraTarget = camerasParent.transform.GetChild(currentCamera);

            changeCamera = false;
        }
    }

    public void CameraScriptON(int currentCameraVar)
    {
        if (typeOfCameraInt[currentCameraVar] == 0)
        {
            cameraReference.GetComponent<FollowPlayer>().transform.parent = this.transform;
            cameraReference.GetComponent<FollowPlayer>().transform.position = camerasParent.transform.GetChild(currentCameraVar).position;
            cameraReference.GetComponent<FollowPlayer>().transform.rotation = camerasParent.transform.GetChild(currentCameraVar).rotation;
            cameraReference.GetComponent<FollowPlayer>().enabled = false;
        }
        else if (typeOfCameraInt[currentCameraVar] == 1)
        {
            cameraReference.GetComponent<FollowPlayer>().transform.parent = null;
            cameraReference.GetComponent<FollowPlayer>().enabled = true;
        }
    }

    #endregion

    #region STEERING WHEEL

    public void CreateSteeringWheel()
    {
        if (steeringWheelPivot == null)
        {
            steeringWheelPivot = new GameObject("SteeringWheelPivot");
            steeringWheelPivot.transform.parent = steeringWheelMesh.transform.parent;
            steeringWheelPivot.transform.position = steeringWheelMesh.transform.position;
            steeringWheelPivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
            steeringWheelMesh.transform.parent = steeringWheelPivot.transform;
        }
    }

    public void SteeringWheelMovement(float steering)
    {
        var steerAngle = steering * maximumSteerAngle * 100;
        steeringWheelPivot.transform.localRotation = Quaternion.Euler(steeringWheelPivot.transform.localRotation.x, steerAngle * Time.deltaTime, steeringWheelPivot.transform.localRotation.z);
    }

    #endregion

    #region NOS

    public void CreateNOS()
    {
        if (NOSParent == null)
        {
            NOSParent = new GameObject("NOS Parent");
            NOSParent.transform.parent = this.transform.GetChild(2);
            NOSParent.transform.localPosition = new Vector3(0,0,0);

            NOSPrefab = Resources.Load<Transform>("NOS UI");
            NOSGameObj = Instantiate(NOSPrefab);
            NOSGameObj.transform.SetParent(NOSParent.transform);
            NOSGameObj.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -380, 0);
        }
    }

    public void DestroyNOS()
    {
        NOSParent = transform.GetChild(2).Find("NOS Parent").gameObject;

        if (NOSParent != null)
        {            
            DestroyImmediate(NOSParent, true);
        }
    }

    
    public void NOSON()
    {
        NOSBarValue.value = calculateSliderValue();

        if (NOSTimeLeft >= NOSMaxTime)
        {
            NOSGameObj.gameObject.SetActive(false);
        }
        else
        {
            NOSGameObj.gameObject.SetActive(true);
        }

        if (nosActive == true && NOSTimeLeft != 0)
        {
            if (NOSTimeLeft <= 0)
            {
                NOSTimeLeft = 0;
                nosActive = false;
            }
            else if (NOSTimeLeft > 0)
            {
                NOSTimeLeft -= Time.deltaTime;
                rb.AddRelativeForce(0, 0, nosPower * Time.deltaTime);
            }
        }
        else if (nosActive == false && NOSTimeLeft < NOSMaxTime)
        {
            NOSTimeLeft += (Time.deltaTime / 3);
        }

        float calculateSliderValue()
        {
            return (NOSTimeLeft / NOSMaxTime);
        }
    }

    #endregion

    #region SPEEDOMETER

    public void CreateSpeedometer()
    {
        if (!analogSpeedometerCreated && !digitalSpeedometerCreated)
        {
            switch (speedometerType)
            {
                case SpeedometerType.Analog:

                    analogSpeedometerPrefab = Resources.Load<Transform>("AnalogSpeedometer");
                    analogSpeedometerObj = transform.GetChild(2).Find("AnalogSpeedometer(Clone)");

                    analogSpeedometerObj = Instantiate(analogSpeedometerPrefab);
                    analogSpeedometerObj.transform.SetParent(this.transform.GetChild(2));
                    analogSpeedometerObj.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                    analogSpeedometerObj.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    analogSpeedometerCreated = true;
                    digitalSpeedometerCreated = false;

                    break;

                case SpeedometerType.Digital:

                    digitalSpeedometerPrefab = Resources.Load<Transform>("DigitalSpeedometer");
                    digitalSpeedometerObj = transform.GetChild(2).Find("DigitalSpeedometer(Clone)");

                    digitalSpeedometerObj = Instantiate(digitalSpeedometerPrefab);
                    digitalSpeedometerObj.transform.SetParent(this.transform.GetChild(2));
                    digitalSpeedometerObj.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                    digitalSpeedometerObj.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    analogSpeedometerCreated = false;
                    digitalSpeedometerCreated = true;

                    break;
            }
        }
    }

    public void DestroySpeedometer()
    {
        if (analogSpeedometerCreated)
        {
            analogSpeedometerObject = transform.GetChild(2).Find("AnalogSpeedometer(Clone)").gameObject;

            DestroyImmediate(analogSpeedometerObject, true);
        }
        else if (digitalSpeedometerCreated)
        {
            digitalSpeedometerObject = transform.GetChild(2).Find("DigitalSpeedometer(Clone)").gameObject;
            DestroyImmediate(digitalSpeedometerObject, true);
        }

        analogSpeedometerCreated = false;
        digitalSpeedometerCreated = false;
    }

    #endregion

}
