using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
enum TypeOfController
{
    Keyboard,
    Joystick
}

[Serializable]
enum ButtonNumber
{
    button0,
    button1,
    button2,
    button3,
    button4,
    button5,
    button6,
    button7,
    button8,
    button9,
    button10,
}

[Serializable]
enum mainControlKey
{
    Standard,
    Keys
}

[Serializable]
enum mainControlJoystick
{
    Standard,
    Buttons
}

public class CarInputs : MonoBehaviour
{
    [HideInInspector]
    public AnyCarController ACC;
    private float xValue;
    private float yValue;
    private float handBrakeValue;
    private AbstractAnyCarAI AI;

    #region MOBILE UI BUTTONS

    [HideInInspector]
    public Transform mobileUIParent;
    [HideInInspector]
    public Transform mobileArrowsParent;
    [HideInInspector]
    public Transform mobileJoystickParent;

    #region BOOLS

    private bool isAccelerating = false;
    private bool isBraking = false;
    private bool isHandbraking = false;
    private bool isTurningLeft = false;
    private bool isTurningRight = false;

    #endregion

    #region BUTTON REFERENCES

    [HideInInspector]
    public Transform acceleratorButton;
    [HideInInspector]
    public Transform brakeButton;
    [HideInInspector]
    public Transform handBrakeButton;
    [HideInInspector]
    public Transform turnLeftButton;
    [HideInInspector]
    public Transform turnRightButton;
    [HideInInspector]
    public Transform gearUpButton;
    [HideInInspector]
    public Transform gearDownButton;
    [HideInInspector]
    public Transform lightsButton;
    [HideInInspector]
    public Transform nosButton;
    [HideInInspector]
    public Transform cameraButton;
    [HideInInspector]
    public Transform eventSystemPrefab;
    [HideInInspector]
    public Transform eventSystemObj;
    [HideInInspector]
    public FixedMobileJoystick mobileFixedJoystick;


    #endregion

    #endregion

    private void Start()
    {
        ACC = this.gameObject.GetComponent<AnyCarController>();
        AI = this.gameObject.GetComponent<AbstractAnyCarAI>();
        #region MOBILE UI
        
        if (ACC.MobileUION)
        {
            mobileUIParent = ACC.mobileUIParent;

            switch (ACC.typeOfMobileUI)
            {
                case 0:

                    mobileArrowsParent = ACC.mobileArrowsHorizontalInputs;

                    turnLeftButton = mobileArrowsParent.Find("TurnLeftButton");
                    turnRightButton = mobileArrowsParent.Find("TurnRightButton");

                    break;
                case 1:

                    mobileJoystickParent = ACC.mobileJoystickHorizontalInputs;

                    mobileFixedJoystick = mobileJoystickParent.GetComponent<FixedMobileJoystick>();

                    break;
            }

            EventSystemCreation();

            acceleratorButton = mobileUIParent.Find("AccellerateButton");
            brakeButton = mobileUIParent.Find("BrakeButton");
            handBrakeButton = mobileUIParent.Find("HandBrakeButton");
            nosButton = mobileUIParent.Find("NOSButton");
            lightsButton = mobileUIParent.Find("LightSwitchButton");
            gearUpButton = mobileUIParent.Find("GearUpButton");
            gearDownButton = mobileUIParent.Find("GearDownButton");
            cameraButton = mobileUIParent.Find("CameraButton");

            MobileUI();
        }

        #endregion
    }

    private void Update()
    {
        #region GET INPUTS

        if (AI != null)
        {
            AIInputs();
        }
        else if (ACC.MobileUION)
        {
            MobileInputs();
        }
        else
        {
            DesktopInputs();
        }

        #endregion
    }

    private void FixedUpdate()
    {
        ACC.Move(xValue, yValue, yValue, handBrakeValue);
    }
    
    #region AI INPUTS
    
    public void AIInputs()
    {
        AI.GetControls(ACC.currentSpeed,ACC.maxSpeed,out xValue, out yValue, out handBrakeValue);
    }
    
    #endregion

    #region DESKTOP INPUTS

    public void DesktopInputs()
    {
        SteeringGetInputs();
        GasGetInputs();
        BrakeGetInputs();
        HandBrakeGetInputs();
    }

    public void SteeringGetInputs()
    {
        switch (ACC.typeOfController)
        {
            case 0:

                switch (ACC.keySteeringMode)
                {
                    case 0:

                        xValue = Input.GetAxis("Horizontal");

                        break;

                    case 1:

                        if (Input.GetKey(ACC.steeringLeftKey))
                        {
                            xValue = -1;
                        }
                        else if (Input.GetKey(ACC.steeringRightKey))
                        {
                            xValue = 1;
                        }
                        else
                        {
                            xValue = 0;
                        }

                        break;
                }

                break;

            case 1:

                switch (ACC.joystickSteeringMode)
                {
                    case 0:

                        xValue = Input.GetAxis("Horizontal");

                        break;

                    case 1:

                        if (Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.steeringLeftButton))
                        {
                            xValue = -1;
                        }
                        else if (Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.steeringRightButton))
                        {
                            xValue = 1;
                        }
                        else
                        {
                            xValue = 0;
                        }

                        break;
                }
                break;
        }
    }

    public void GasGetInputs()
    {
        switch (ACC.typeOfController)
        {
            case 0:

                switch (ACC.keyGasMode)
                {
                    case 0:

                        if (Input.GetAxis("Vertical") > 0)
                        {
                            yValue = Input.GetAxis("Vertical");
                        }

                        break;

                    case 1:

                        if (Input.GetKey(ACC.gasKey))
                        {
                            yValue = 1;
                        }
                        else if (!Input.GetKey(ACC.brakeKey))
                        {
                            yValue = 0;
                        }

                        break;
                }

                break;

            case 1:
                switch (ACC.joystickGasMode)
                {
                    case 0:

                        if (Input.GetAxis("Vertical") > 0)
                        {
                            yValue = Input.GetAxis("Vertical");
                        }

                        break;

                    case 1:

                        if (Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.gasButton))
                        {
                            yValue = 1;
                        }
                        else if (!Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.brakeButton))
                        {
                            yValue = 0;
                        }

                        break;
                }
                break;
        }
    }

    public void BrakeGetInputs()
    {
        switch (ACC.typeOfController)
        {
            case 0:

                switch (ACC.keyBrakeMode)
                {
                    case 0:

                        if (Input.GetAxis("Vertical") < 0)
                        {
                            yValue = Input.GetAxis("Vertical");
                        }
                        else if (Input.GetAxis("Vertical") == 0)
                        {
                            yValue = 0;
                        }

                        break;

                    case 1:

                        if (Input.GetKey(ACC.brakeKey))
                        {
                            yValue = -1;
                        }
                        else if (!Input.GetKey(ACC.gasKey))
                        {
                            yValue = 0;
                        }

                        break;
                }

                break;

            case 1:

                switch (ACC.joystickBrakeMode)
                {
                    case 0:

                        if (Input.GetAxis("Vertical") < 0)
                        {
                            yValue = Input.GetAxis("Vertical");
                        }
                        else if (Input.GetAxis("Vertical") == 0)
                        {
                            yValue = 0;
                        }

                        break;

                    case 1:

                        if (Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.brakeButton))
                        {
                            yValue = -1;
                        }
                        else if (!Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.gasButton))
                        {
                            yValue = 0;
                        }

                        break;
                }
                break;
        }
    }

    public void HandBrakeGetInputs()
    {
        switch (ACC.typeOfController)
        {
            case 0:

                if (ACC.handBrakeKey == "space")
                {
                    handBrakeValue = Input.GetAxis("Jump");
                }
                else
                {
                    if (Input.GetKey(ACC.handBrakeKey))
                    {
                        handBrakeValue = 1;
                    }
                    else
                    {
                        handBrakeValue = 0;
                    }
                }

                break;

            case 1:

                if (Input.GetKey("joystick " + ACC.joystickNumber + " button " + ACC.handBrakeButton))
                {
                    handBrakeValue = 1;
                }
                else
                {
                    handBrakeValue = 0;
                }

                break;
        }
    }

    #endregion


    #region MOBILE INPUTS

    #region EVENT SYSTEM

    private void EventSystemCreation()
    {
        EventSystem myEventSystem = GameObject.FindObjectOfType<EventSystem>();

        if (myEventSystem == null)
        {
            eventSystemPrefab = Resources.Load<Transform>("EventSystem");
            eventSystemObj = Instantiate(eventSystemPrefab);
        }
    }

    #endregion

    public void MobileUI()
    {
        if (ACC.typeOfMobileUI == 0)
        {
            if (turnLeftButton != null)
            {
                InitiateTurnLeftButton();
            }

            if (turnRightButton != null)
            {
                InitiateTurnRightButton();
            }        
        }

        if (acceleratorButton != null)
        {
            InitiateAccelerateButton();
        }

        if (brakeButton != null)
        {
            InitiateBrakeButton();
        }

        if (handBrakeButton != null)
        {
            InitiateHandBrakeButton();
        }

        if (lightsButton != null)
        {
            InitiateLightsButton();
        }

        if (cameraButton != null)
        {
            InitiateCameraButton();
        }
        
        if (!ACC.autoTransmission)
        {
            if (gearUpButton != null && gearDownButton != null)
            {
                InitiateGearDownButton();
                InitiateGearUpButton();
            }
        }
        else
        {
            gearUpButton = mobileUIParent.Find("GearUpButton");
            gearDownButton = mobileUIParent.Find("GearDownButton");
            gearDownButton.gameObject.SetActive(false);
            gearUpButton.gameObject.SetActive(false);
        }

        if (ACC.nosON)
        {
            if (nosButton != null)
            {
                InitiateNOSButton();
            }
        }
        else
        {
            nosButton = mobileUIParent.Find("NOSButton");
            nosButton.gameObject.SetActive(false);
        }
    }


    private void MobileInputs()
    {
        #region ACCELERATING

        if (isAccelerating)
        {
            yValue = 1;
        }
        else
        {
            yValue = 0;
        }

        #endregion

        #region BRAKING

        if (isBraking)
        {
            yValue = -1;
        }
        else if (!isAccelerating)
        {
            yValue = 0;
        }

        #endregion

        #region HANDBRAKING

        if (isHandbraking)
        {
            handBrakeValue = 1;
        }
        else
        {
            handBrakeValue = 0;
        }

        #endregion

        switch (ACC.typeOfMobileUI)
        {
            case 0:

                #region TURNING LEFT

                if (isTurningLeft)
                {
                    xValue = -1;
                }
                else if (!isTurningRight && !isTurningLeft)
                {
                    xValue = 0;
                }

                #endregion

                #region TURNING RIGHT

                if (isTurningRight)
                {
                    xValue = 1;
                }
                else if (!isTurningRight && !isTurningLeft)
                {
                    xValue = 0;
                }

                #endregion

                break;

            case 1:

                #region MOBILE FIXED JOYSTICK

                MobileFixedJoystickMovement();

                #endregion

                break;
        }
    }

    #region ARROWS

    private void InitiateTurnLeftButton()
    {
        turnLeftButton.gameObject.AddComponent<EventTrigger>();

        var turnLeftPointerDown = new EventTrigger.Entry();
        var TurnLeftPointerUp = new EventTrigger.Entry();

        turnLeftPointerDown.eventID = EventTriggerType.PointerDown;
        TurnLeftPointerUp.eventID = EventTriggerType.PointerUp;

        turnLeftPointerDown.callback.AddListener((e) => isTurningLeft = true);
        TurnLeftPointerUp.callback.AddListener((e) => isTurningLeft = false);

        turnLeftButton.GetComponent<EventTrigger>().triggers.Add(turnLeftPointerDown);
        turnLeftButton.GetComponent<EventTrigger>().triggers.Add(TurnLeftPointerUp);
    }

    private void InitiateTurnRightButton()
    {
        turnRightButton.gameObject.AddComponent<EventTrigger>();

        var turnRightPointerDown = new EventTrigger.Entry();
        var TurnRightPointerUp = new EventTrigger.Entry();

        turnRightPointerDown.eventID = EventTriggerType.PointerDown;
        TurnRightPointerUp.eventID = EventTriggerType.PointerUp;

        turnRightPointerDown.callback.AddListener((e) => isTurningRight = true);
        TurnRightPointerUp.callback.AddListener((e) => isTurningRight = false);

        turnRightButton.GetComponent<EventTrigger>().triggers.Add(turnRightPointerDown);
        turnRightButton.GetComponent<EventTrigger>().triggers.Add(TurnRightPointerUp);
    }

    #endregion

    #region JOYSTICK

    private void MobileFixedJoystickMovement()
    {
        xValue = mobileFixedJoystick.Horizontal;
    }

    #endregion

    private void InitiateAccelerateButton()
    {
        acceleratorButton.gameObject.AddComponent<EventTrigger>();

        var acceleratePointerDown = new EventTrigger.Entry();
        var acceleratePointerUp = new EventTrigger.Entry();

        acceleratePointerDown.eventID = EventTriggerType.PointerDown;
        acceleratePointerUp.eventID = EventTriggerType.PointerUp;

        acceleratePointerDown.callback.AddListener((e) => isAccelerating = true);
        acceleratePointerUp.callback.AddListener((e) => isAccelerating = false);

        acceleratorButton.GetComponent<EventTrigger>().triggers.Add(acceleratePointerDown);
        acceleratorButton.GetComponent<EventTrigger>().triggers.Add(acceleratePointerUp);
    }

    private void InitiateBrakeButton()
    {
        brakeButton.gameObject.AddComponent<EventTrigger>();

        var brakePointerDown = new EventTrigger.Entry();
        var brakePointerUp = new EventTrigger.Entry();

        brakePointerDown.eventID = EventTriggerType.PointerDown;
        brakePointerUp.eventID = EventTriggerType.PointerUp;

        brakePointerDown.callback.AddListener((e) => isBraking = true);
        brakePointerUp.callback.AddListener((e) => isBraking = false);

        brakeButton.GetComponent<EventTrigger>().triggers.Add(brakePointerDown);
        brakeButton.GetComponent<EventTrigger>().triggers.Add(brakePointerUp);
    }

    private void InitiateHandBrakeButton()
    {
        handBrakeButton.gameObject.AddComponent<EventTrigger>();

        var handBrakePointerDown = new EventTrigger.Entry();
        var handBrakePointerUp = new EventTrigger.Entry();

        handBrakePointerDown.eventID = EventTriggerType.PointerDown;
        handBrakePointerUp.eventID = EventTriggerType.PointerUp;

        handBrakePointerDown.callback.AddListener((e) => isHandbraking = true);
        handBrakePointerUp.callback.AddListener((e) => isHandbraking = false);

        handBrakeButton.GetComponent<EventTrigger>().triggers.Add(handBrakePointerDown);
        handBrakeButton.GetComponent<EventTrigger>().triggers.Add(handBrakePointerUp);
    }

    private void InitiateNOSButton()
    {
        nosButton.gameObject.AddComponent<EventTrigger>();

        var nosPointerDown = new EventTrigger.Entry();
        var nosPointerUp = new EventTrigger.Entry();

        nosPointerDown.eventID = EventTriggerType.PointerDown;
        nosPointerUp.eventID = EventTriggerType.PointerUp;

        nosPointerDown.callback.AddListener((e) => ACC.nosActive = true);
        nosPointerUp.callback.AddListener((e) => ACC.nosActive = false);

        nosButton.GetComponent<EventTrigger>().triggers.Add(nosPointerDown);
        nosButton.GetComponent<EventTrigger>().triggers.Add(nosPointerUp);
    }

    private void InitiateLightsButton()
    {
        lightsButton.GetComponent<Button>().onClick.AddListener(delegate () { if (!ACC.frontLights.activeSelf) { ACC.frontLights.SetActive(true); } else { ACC.frontLights.SetActive(false); } });
    }

    private void InitiateGearUpButton()
    {
        gearUpButton.GetComponent<Button>().onClick.AddListener(delegate () 
        {
            if (ACC.currentGear < (ACC.numberOfGears - 1))
            {
                if (!ACC.reverseGearOn)
                {
                    ACC.clutchOn = true;
                    ACC.ExhaustFX();
                    ACC.currentGear++;
                    StartCoroutine(ACC.ClutchCoroutine());
                }
            }
        });
    }

    private void InitiateGearDownButton()
    {
        gearDownButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (ACC.currentGear > 0)
            {
                ACC.clutchOn = true;
                ACC.currentGear--;
                StartCoroutine(ACC.ClutchCoroutine());
            }
        });
    }

    private void InitiateCameraButton()
    {
        cameraButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ACC.changeCamera = true;
        });
    }

    #endregion
}
