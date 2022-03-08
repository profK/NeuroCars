using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnyCarController))]
public class ControllerTabs : Editor
{
    private AnyCarController myTarget;
    private SerializedObject soTarget;

    #region SETUP

    private SerializedProperty frontLeft;
    private SerializedProperty frontRight;
    private SerializedProperty backLeft;
    private SerializedProperty backRight;
    private SerializedProperty extraWheels;
    private SerializedProperty bodyMesh;
    private SerializedProperty steeringWheelMesh;
    private SerializedProperty mobileUIType;
    private SerializedProperty typeOfCamera;
    private SerializedProperty smoothFollowing;
    private SerializedProperty cameraReference;

    #endregion

    #region WHEELS

    private SerializedProperty wheelsRadius;
    private SerializedProperty wheelsMass;
    private SerializedProperty forcePoint;
    private SerializedProperty dumpingRate;
    private SerializedProperty suspensionDistance;
    private SerializedProperty wheelsPosition;
    private SerializedProperty wheelsRotation;
    private SerializedProperty wheelStiffness;
    private SerializedProperty suspensionSpring;
    private SerializedProperty suspensionDamper;
    private SerializedProperty targetPosition;
    private SerializedProperty maximumSteerAngle;
    private SerializedProperty steerHelper;
    private SerializedProperty slipLimit;

    #endregion

    #region ENGINE

    private SerializedProperty enginePower;
    private SerializedProperty carDriveType;
    private SerializedProperty transmission;
    private SerializedProperty clutch;
    private SerializedProperty tractionControl;
    private SerializedProperty motorTorque;
    private SerializedProperty brakeTorque;
    private SerializedProperty reverseTorque;
    private SerializedProperty handbrakeTorque;
    private SerializedProperty speedType;
    private SerializedProperty maxSpeed;
    private SerializedProperty numberOfGears;
    private SerializedProperty downForce;
    private SerializedProperty vehicleMass;


    #endregion

    #region FEATURES

    private SerializedProperty speedometerType;
    private SerializedProperty turboON;
    private SerializedProperty nosON;
    private SerializedProperty nosPower;
    private SerializedProperty NOSMaxTime;
    private SerializedProperty exhaustFlame;
    private SerializedProperty centerOfMass;
    private SerializedProperty ABS;
    private SerializedProperty skidMarks;
    private SerializedProperty collisionSystem;
    private SerializedProperty collisionParticles;
    private SerializedProperty demolutionStrenght;
    private SerializedProperty demolutionRange;
    private SerializedProperty optionalMeshList;
    private SerializedProperty customMesh;
    private SerializedProperty controller;
    private SerializedProperty smokeOn;

    #region CONTROLS

    private SerializedProperty joystickNumber;

    private SerializedProperty steeringModeKey;
    private SerializedProperty gasModeKey;
    private SerializedProperty brakeModeKey;

    private SerializedProperty steeringModeJoystick;
    private SerializedProperty gasModeJoystick;
    private SerializedProperty brakeModeJoystick;

    private SerializedProperty steeringLeftKey;
    private SerializedProperty steeringRightKey;
    private SerializedProperty gasKey;
    private SerializedProperty brakeKey;

    private SerializedProperty buttonSteeringLeft;
    private SerializedProperty buttonSteeringRight;
    private SerializedProperty buttonGas;
    private SerializedProperty buttonBrake;

    private SerializedProperty handBrakeKey;
    private SerializedProperty nosKey;
    private SerializedProperty cameraKey;
    private SerializedProperty lightsKey;
    private SerializedProperty gearUp;
    private SerializedProperty gearDown;

    private SerializedProperty buttonHandBrake;
    private SerializedProperty buttonNos;
    private SerializedProperty buttonCamera;
    private SerializedProperty buttonLights;
    private SerializedProperty buttonGearUp;
    private SerializedProperty buttonGearDown;

    #endregion

    #endregion

    #region AUDIO

    private SerializedProperty exhaustSound;
    private SerializedProperty exhaustVolume;
    private SerializedProperty skidSound;
    private SerializedProperty skidVolume;
    private SerializedProperty lowAcceleration;
    private SerializedProperty lowDeceleration;
    private SerializedProperty highAcceleration;
    private SerializedProperty highDeceleration;
    private SerializedProperty engineVolume;
    private SerializedProperty collisionSound;
    private SerializedProperty collisionVolume;
    private SerializedProperty nosAudioClip;
    private SerializedProperty nosVolume;
    private SerializedProperty turboAudioClip;
    private SerializedProperty turboVolume;

    #endregion

    #region BUTTON REFERENCES

    public Texture buttonSetUp;
    public Texture buttonWheels;
    public Texture buttonEngine;
    public Texture buttonFeatures;
    public Texture buttonAudio;
    public Texture anyCarControllerLabel;

    public string turboTextButton = "OFF";
    public string nosTextButton = "OFF";

    #endregion


    private void OnEnable()
    {
        myTarget = (AnyCarController)target;
        soTarget = new SerializedObject(target);

        #region BUTTONS

        buttonSetUp = Resources.Load<Texture>("buttonSetUp");
        buttonWheels = Resources.Load<Texture>("buttonWheels");
        buttonEngine = Resources.Load<Texture>("buttonEngine");
        buttonFeatures = Resources.Load<Texture>("buttonFeatures");
        buttonAudio = Resources.Load<Texture>("buttonAudio");
        anyCarControllerLabel = Resources.Load<Texture>("anyCarControllerLabel");

        #endregion

        #region SETUP

        frontLeft = soTarget.FindProperty("frontLeft");
        frontRight = soTarget.FindProperty("frontRight");
        backLeft = soTarget.FindProperty("backLeft");
        backRight = soTarget.FindProperty("backRight");
        extraWheels = soTarget.FindProperty("extraWheels");
        steeringWheelMesh = soTarget.FindProperty("steeringWheelMesh");
        bodyMesh = soTarget.FindProperty("bodyMesh");
        mobileUIType = soTarget.FindProperty("mobileUIType");
        typeOfCamera = soTarget.FindProperty("typeOfCamera");
        smoothFollowing = soTarget.FindProperty("smoothFollowing");
        cameraReference = soTarget.FindProperty("cameraReference");

        #endregion

        #region WHEELS

        wheelsRadius = soTarget.FindProperty("wheelsRadius");
        wheelsMass = soTarget.FindProperty("wheelsMass");
        forcePoint = soTarget.FindProperty("forcePoint");
        dumpingRate = soTarget.FindProperty("dumpingRate");
        suspensionDistance = soTarget.FindProperty("suspensionDistance");
        suspensionDamper = soTarget.FindProperty("suspensionDamper");
        suspensionSpring = soTarget.FindProperty("suspensionSpring");
        targetPosition = soTarget.FindProperty("targetPosition");
        wheelsPosition = soTarget.FindProperty("wheelsPosition");
        wheelsRotation = soTarget.FindProperty("wheelsRotation");
        wheelStiffness = soTarget.FindProperty("wheelStiffness");
        maximumSteerAngle = soTarget.FindProperty("maximumSteerAngle");
        steerHelper = soTarget.FindProperty("steerHelper");
        slipLimit = soTarget.FindProperty("slipLimit");

        #endregion

        #region ENGINE

        enginePower = soTarget.FindProperty("enginePower");
        carDriveType = soTarget.FindProperty("carDriveType");
        transmission = soTarget.FindProperty("transmission");
        clutch = soTarget.FindProperty("clutch");
        tractionControl = soTarget.FindProperty("tractionControl");
        motorTorque = soTarget.FindProperty("motorTorque");
        brakeTorque = soTarget.FindProperty("brakeTorque");
        reverseTorque = soTarget.FindProperty("reverseTorque");
        handbrakeTorque = soTarget.FindProperty("handbrakeTorque");
        speedType = soTarget.FindProperty("speedType");
        maxSpeed = soTarget.FindProperty("maxSpeed");
        numberOfGears = soTarget.FindProperty("numberOfGears");
        downForce = soTarget.FindProperty("downForce");
        vehicleMass = soTarget.FindProperty("vehicleMass");

        #endregion

        #region FEATURES

        speedometerType = soTarget.FindProperty("speedometerType");
        turboON = soTarget.FindProperty("turboON");
        nosON = soTarget.FindProperty("nosON");
        nosPower = soTarget.FindProperty("nosPower");
        NOSMaxTime = soTarget.FindProperty("NOSMaxTime");
        smokeOn = soTarget.FindProperty("smokeOn");
        centerOfMass = soTarget.FindProperty("centerOfMass");
        ABS = soTarget.FindProperty("ABS");
        skidMarks = soTarget.FindProperty("skidMarks");
        optionalMeshList = soTarget.FindProperty("optionalMeshList");
        collisionSystem = soTarget.FindProperty("collisionSystem");
        collisionParticles = soTarget.FindProperty("collisionParticles");
        customMesh = soTarget.FindProperty("customMesh");
        demolutionStrenght = soTarget.FindProperty("demolutionStrenght");
        demolutionRange = soTarget.FindProperty("demolutionRange");
        exhaustFlame = soTarget.FindProperty("exhaustFlame");
        controller = soTarget.FindProperty("controller");

        #region CONTROLS

        joystickNumber = soTarget.FindProperty("joystickNumber");
        steeringModeKey = soTarget.FindProperty("steeringModeKey");
        gasModeKey = soTarget.FindProperty("gasModeKey");
        brakeModeKey = soTarget.FindProperty("brakeModeKey");

        steeringModeJoystick = soTarget.FindProperty("steeringModeJoystick");
        gasModeJoystick = soTarget.FindProperty("gasModeJoystick");
        brakeModeJoystick = soTarget.FindProperty("brakeModeJoystick");


        steeringLeftKey = soTarget.FindProperty("steeringLeftKey");
        steeringRightKey = soTarget.FindProperty("steeringRightKey");
        gasKey = soTarget.FindProperty("gasKey");
        brakeKey = soTarget.FindProperty("brakeKey");

        buttonSteeringLeft = soTarget.FindProperty("buttonSteeringLeft");
        buttonSteeringRight = soTarget.FindProperty("buttonSteeringRight");
        buttonGas = soTarget.FindProperty("buttonGas");
        buttonBrake = soTarget.FindProperty("buttonBrake");

        handBrakeKey = soTarget.FindProperty("handBrakeKey");
        nosKey = soTarget.FindProperty("nosKey");
        cameraKey = soTarget.FindProperty("cameraKey");
        lightsKey = soTarget.FindProperty("lightsKey");
        gearUp = soTarget.FindProperty("gearUp");
        gearDown = soTarget.FindProperty("gearDown");

        buttonHandBrake = soTarget.FindProperty("buttonHandBrake");
        buttonNos = soTarget.FindProperty("buttonNos");
        buttonCamera = soTarget.FindProperty("buttonCamera");
        buttonLights = soTarget.FindProperty("buttonLights");
        buttonGearDown = soTarget.FindProperty("buttonGearDown");
        buttonGearUp = soTarget.FindProperty("buttonGearUp");

        #endregion

        #endregion

        #region AUDIO

        skidSound = soTarget.FindProperty("skidSound");
        skidVolume = soTarget.FindProperty("skidVolume");
        lowAcceleration = soTarget.FindProperty("lowAcceleration");
        lowDeceleration = soTarget.FindProperty("lowDeceleration");
        highAcceleration = soTarget.FindProperty("highAcceleration");
        highDeceleration = soTarget.FindProperty("highDeceleration");
        engineVolume = soTarget.FindProperty("engineVolume");
        collisionVolume = soTarget.FindProperty("collisionVolume");
        collisionSound = soTarget.FindProperty("collisionSound");
        exhaustSound = soTarget.FindProperty("exhaustSound");
        exhaustVolume = soTarget.FindProperty("exhaustVolume");
        nosAudioClip = soTarget.FindProperty("nosAudioClip");
        nosVolume = soTarget.FindProperty("nosVolume");
        turboAudioClip = soTarget.FindProperty("turboAudioClip");
        turboVolume = soTarget.FindProperty("turboVolume");

        #endregion
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Box(anyCarControllerLabel, GUILayout.ExpandWidth(true), GUILayout.Height(55));
        soTarget.Update();

        EditorGUI.BeginChangeCheck();


        myTarget.toolbarTab = GUILayout.Toolbar(myTarget.toolbarTab, new Texture[] { buttonSetUp, buttonWheels, buttonEngine, buttonFeatures, buttonAudio }, GUILayout.Height(30));

        switch (myTarget.toolbarTab)
        {
            case 0:
                myTarget.currentTab = "Set Up";
                break;
            case 1:
                myTarget.currentTab = "Wheels";
                break;
            case 2:
                myTarget.currentTab = "Engine";
                break;
            case 3:
                myTarget.currentTab = "Features";
                break;
            case 4:
                myTarget.currentTab = "Audio";
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }

        EditorGUI.BeginChangeCheck();

        switch (myTarget.currentTab)
        {
            case "Set Up":

                #region SETUP

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("SET UP", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Make references of your car model and attach script.");

                #region MAIN SETUP

                EditorGUILayout.Space();
                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Wheels", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(frontLeft);
                EditorGUILayout.PropertyField(frontRight);
                EditorGUILayout.PropertyField(backLeft);
                EditorGUILayout.PropertyField(backRight);
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.Space(12);
                EditorGUILayout.PropertyField(extraWheels, true);
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Car Body", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(bodyMesh);

                if (GUILayout.Button("Debug BodyCollider"))
                {
                    myTarget.CreateDebugBodyCol();
                }

                GUILayout.EndVertical();
                
                GUI.color = Color.Lerp(Color.white, Color.grey, 0.2f);
                if (GUILayout.Button("ATTACH SCRIPT", GUILayout.MinHeight(40)))
                {
                    myTarget.UnpackPrefab();
                    UnpackPrefab();
                    myTarget.CreateColliders();
                }
                GUI.color = Color.white;
                EditorGUILayout.Space();

                #endregion

                #region CAMERAS

                GUILayout.BeginVertical("", "box");

                EditorGUILayout.LabelField("Create Cameras", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(cameraReference);
                EditorGUILayout.PropertyField(smoothFollowing);
                GUILayout.BeginHorizontal();
                GUILayout.Space(12);
                EditorGUILayout.PropertyField(typeOfCamera, true);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("CREATE CAMERA OBJECTS"))
                {
                    myTarget.CreateCameras();
                }
                GUILayout.EndVertical();

                #endregion

                EditorGUILayout.Space();

                #region STEERING WHEEL

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Steering Wheel Mesh", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space(4);
                EditorGUILayout.PropertyField(steeringWheelMesh, GUIContent.none);
                GUILayout.EndVertical();
                if (GUILayout.Button("CREATE"))
                {
                    myTarget.UnpackModelMesh();
                    UnpackModelMesh();
                    myTarget.CreateSteeringWheel();
                }                
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                #endregion

                EditorGUILayout.Space();

                #region SPEEDOMETER

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Speedometer Type", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space(4);
                EditorGUILayout.PropertyField(speedometerType, GUIContent.none);
                GUILayout.EndVertical();
                if (GUILayout.Button("CREATE"))
                {
                    myTarget.CreateSpeedometer();
                }
                if (GUILayout.Button("DESTROY"))
                {
                    myTarget.DestroySpeedometer();
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                #endregion

                EditorGUILayout.Space();

                #region MOBILE UI

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Mobile UI Type", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space(4);
                EditorGUILayout.PropertyField(mobileUIType, GUIContent.none);
                GUILayout.EndVertical();
                if (GUILayout.Button("CREATE"))
                {
                    myTarget.CreateMobileUI();
                }
                if (GUILayout.Button("DESTROY"))
                {
                    myTarget.DestroyMobileUI();
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                #endregion

                #endregion

                break;
            case "Wheels":

                #region WHEELS

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("WHEELS", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Set wheels physics and customize user preferences.");
                EditorGUILayout.Space();

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Tyres", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(wheelsRadius);
                EditorGUILayout.PropertyField(wheelsMass);
                EditorGUILayout.PropertyField(dumpingRate);
                EditorGUILayout.PropertyField(forcePoint);
                EditorGUILayout.Space();
                GUILayout.EndVertical();
                EditorGUILayout.Space();
                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Suspensions", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(suspensionSpring);
                EditorGUILayout.PropertyField(suspensionDamper);
                EditorGUILayout.PropertyField(suspensionDistance);
                EditorGUILayout.PropertyField(targetPosition);
                EditorGUILayout.Space();
                GUILayout.EndVertical();

                EditorGUILayout.Space();
                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Drift Controls", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(maximumSteerAngle);
                EditorGUILayout.PropertyField(wheelStiffness);
                EditorGUILayout.PropertyField(steerHelper);
                EditorGUILayout.PropertyField(tractionControl);
                EditorGUILayout.PropertyField(slipLimit);
                EditorGUILayout.Space();
                GUILayout.EndVertical();
                EditorGUILayout.Space();
                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Wheels Debug", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(wheelsPosition);
                EditorGUILayout.PropertyField(wheelsRotation);
                EditorGUILayout.Space();
                GUILayout.EndVertical();
                EditorGUILayout.Space();

                #endregion

                break;
            case "Engine":

                #region ENGINE

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ENGINE", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Set engine features and customize user preferences.");
                EditorGUILayout.Space();

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Type of Engine", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(carDriveType);
                EditorGUILayout.PropertyField(transmission);

                if(transmission.intValue == 1)
                {
                    EditorGUILayout.PropertyField(clutch);
                }

                EditorGUILayout.PropertyField(numberOfGears);
                EditorGUILayout.Space();
                GUILayout.EndVertical();

                EditorGUILayout.Space();

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Power Controls", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(enginePower);
                EditorGUILayout.PropertyField(motorTorque);
                EditorGUILayout.PropertyField(brakeTorque);
                EditorGUILayout.PropertyField(reverseTorque);
                EditorGUILayout.PropertyField(handbrakeTorque);
                EditorGUILayout.PropertyField(downForce);
                EditorGUILayout.Space();
                GUILayout.EndVertical();

                EditorGUILayout.Space();

                GUILayout.BeginVertical("", "box");
                GUILayout.Label("Extra Power", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();

                #region TURBO

                GUILayout.Label("Turbo", EditorStyles.boldLabel, GUILayout.Width(50));

                if(turboON.boolValue == true)
                {
                    turboTextButton = "ON";
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    turboTextButton = "OFF";
                    GUI.color = Color.white;
                }

                if (turboON.boolValue == false)
                {
                    if (GUILayout.Button(turboTextButton))
                    {
                        turboON.boolValue = true;
                    }
                }
                else
                {
                    if (GUILayout.Button(turboTextButton))
                    {                        
                        turboON.boolValue = false;
                    }
                }
                GUI.color = Color.white;

                #endregion

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();


                #region NOS

                GUILayout.Label("NOS", EditorStyles.boldLabel, GUILayout.Width(50));

                if (nosON.boolValue == true)
                {
                    nosTextButton = "ON";
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    nosTextButton = "OFF";
                    GUI.color = Color.white;
                }

                if (nosON.boolValue == false)
                {
                    if (GUILayout.Button(nosTextButton))
                    {
                        nosON.boolValue = true;
                        myTarget.CreateNOS();
                    }
                }
                else
                {
                    if (GUILayout.Button(nosTextButton))
                    {
                        nosON.boolValue = false;
                        myTarget.DestroyNOS();
                    }
                }                

                GUI.color = Color.white;

                #endregion

                GUILayout.EndHorizontal();

                #region NOS DROPDOWN

                if(nosON.boolValue == true)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.9f);

                    GUILayout.BeginVertical("", "box");
                    EditorGUILayout.PropertyField(nosPower);
                    EditorGUILayout.PropertyField(NOSMaxTime);
                    GUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.Space();
                }

                GUI.color = Color.white;

                #endregion



                GUILayout.EndVertical();


                EditorGUILayout.Space();
                GUILayout.BeginVertical("", "box");                
                EditorGUILayout.LabelField("Speed Controls", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(speedType);
                EditorGUILayout.PropertyField(maxSpeed);
                EditorGUILayout.PropertyField(vehicleMass);
                EditorGUILayout.PropertyField(centerOfMass);
                EditorGUILayout.Space();
                GUILayout.EndVertical();

                EditorGUILayout.Space();

                #endregion

                break;
            case "Features":

                #region FEATURES

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("OPTIONS", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Set extra features and customize user preferences.");
                EditorGUILayout.Space();


                #region FEATURES BUTTONS

                GUILayout.BeginHorizontal();

                #region SKID MARKS

                if (skidMarks.boolValue == true)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("Skid Marks", GUILayout.MinWidth(120)))
                {
                    if (skidMarks.boolValue == true)
                    {
                        skidMarks.boolValue = false;
                    }
                    else
                    {
                        skidMarks.boolValue = true;
                    }
                }

                GUI.color = Color.white;

                #endregion

                #region EXHAUST

                if (exhaustFlame.boolValue == true)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("Exhaust FX", GUILayout.MinWidth(120)))
                {

                    if (exhaustFlame.boolValue == true)
                    {
                        exhaustFlame.boolValue = false;
                        myTarget.DestroyExhaustGameObj();
                    }
                    else
                    {
                        exhaustFlame.boolValue = true;
                        myTarget.CreateExhaustGameObj();
                    }
                }


                GUI.color = Color.white;

                #endregion


                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();

                #region ABS

                if (ABS.boolValue == true)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("ABS", GUILayout.MinWidth(120)))
                {
                    if (ABS.boolValue == true)
                    {
                        ABS.boolValue = false;
                    }
                    else
                    {
                        ABS.boolValue = true;
                    }
                }

                GUI.color = Color.white;

                #endregion

                #region SMOKE

                if (smokeOn.boolValue == true)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("Smoke", GUILayout.MinWidth(120)))
                {
                    if (smokeOn.boolValue == true)
                    {
                        smokeOn.boolValue = false;
                    }
                    else
                    {
                        smokeOn.boolValue = true;
                    }
                }

                GUI.color = Color.white;

                #endregion


                GUILayout.EndHorizontal();
                EditorGUILayout.Space();

                #endregion

                #region COLLISION

                if (collisionSystem.boolValue == true)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("Collision System"))
                {
                    
                    if (collisionSystem.boolValue == true)
                    {
                        collisionSystem.boolValue = false;
                    }
                    else
                    {
                        collisionSystem.boolValue = true;
                    }
                }

                GUI.color = Color.white;



                if (collisionSystem.boolValue == true)
                {
                    GUILayout.BeginVertical("", "box");

                    EditorGUILayout.PropertyField(demolutionStrenght);
                    EditorGUILayout.PropertyField(demolutionRange);

                    GUILayout.BeginHorizontal();

                    #region CUSTOM MESH BUTTON

                    if (customMesh.boolValue)
                    {
                        GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }

                    if (GUILayout.Button("Custom Mesh"))
                    {
                        if (customMesh.boolValue == true)
                        {
                            customMesh.boolValue = false;
                        }
                        else
                        {
                            customMesh.boolValue = true;
                        }
                    }

                    GUI.color = Color.white;

                    #endregion

                    #region COLLISION PARTICLES

                    if (collisionParticles.boolValue)
                    {
                        GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                    }
                    else
                    {
                        GUI.color = Color.white;
                    }

                    if (GUILayout.Button("Collision Particles"))
                    {
                        if (collisionParticles.boolValue == true)
                        {
                            collisionParticles.boolValue = false;
                        }
                        else
                        {
                            collisionParticles.boolValue = true;
                        }
                    }

                    GUI.color = Color.white;

                    #endregion

                    GUILayout.EndHorizontal();


                    if (customMesh.boolValue)
                    {
                        GUI.color = Color.Lerp(Color.gray, Color.white, .8f);
                        GUILayout.BeginVertical("", "box");                        
                        EditorGUILayout.Space();
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        GUI.color = Color.white;
                        EditorGUILayout.PropertyField(optionalMeshList, true);
                        GUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        GUILayout.EndVertical();
                    }


                    GUILayout.EndVertical();

                }

                #endregion

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("CONTROLS", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Customize controls, keys or user preferences.");


                EditorGUILayout.Space();


                #region CONTROL TYPE BUTTONS

                GUILayout.BeginHorizontal();

                #region KEYBOARD

                if (controller.intValue == 0)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("KEYBOARD"))
                {
                    if (controller.intValue == 0)
                    {
                        controller.intValue = 1;
                    }
                    else
                    {
                        controller.intValue = 0;
                    }
                }


                GUI.color = Color.white;

                #endregion


                #region JOYSTICK

                if (controller.intValue == 1)
                {
                    GUI.color = Color.Lerp(Color.gray, Color.white, 0.5f);
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button("JOYSTICK"))
                {
                    if (controller.intValue == 0)
                    {
                        controller.intValue = 1;
                    }
                    else
                    {
                        controller.intValue = 0;
                    }
                }


                GUI.color = Color.white;

                #endregion

                GUILayout.EndHorizontal();

                #endregion

                GUILayout.BeginVertical("", "box");

                EditorGUILayout.Space();

                switch (controller.intValue)
                {
                    case 0:

                        #region STEERING CONTROLS

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Steering", GUILayout.Width(50));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(steeringModeKey, GUIContent.none, true, GUILayout.MinWidth(40));
                        GUILayout.FlexibleSpace();

                        if (steeringModeKey.intValue == 1)
                        {
                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();

                            GUILayout.Label("Left", GUILayout.Width(35));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(steeringLeftKey, GUIContent.none, true, GUILayout.MinWidth(15));
                            GUILayout.FlexibleSpace();

                            GUILayout.Label("Right", GUILayout.Width(35));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(steeringRightKey, GUIContent.none, true, GUILayout.MinWidth(15));
                            GUILayout.FlexibleSpace();

                            GUILayout.EndHorizontal();

                            
                        }
                        
                        GUILayout.EndHorizontal();


                        #endregion

                        EditorGUILayout.Space();

                        GuiLine();

                        EditorGUILayout.Space();

                        #region GAS CONTROLS

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Gas", GUILayout.Width(50));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(gasModeKey, GUIContent.none, true, GUILayout.MinWidth(40));
                        GUILayout.FlexibleSpace();

                        if (gasModeKey.intValue == 1)
                        {
                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();

                            GUILayout.Label("Gas Key", GUILayout.Width(60));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(gasKey, GUIContent.none, true, GUILayout.MinWidth(10));
                            GUILayout.FlexibleSpace();

                            GUILayout.EndHorizontal();


                        }

                        GUILayout.EndHorizontal();


                        #endregion

                        EditorGUILayout.Space();

                        GuiLine();

                        EditorGUILayout.Space();

                        #region BRAKE CONTROLS

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Brake", GUILayout.Width(50));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(brakeModeKey, GUIContent.none, true, GUILayout.MinWidth(40));
                        GUILayout.FlexibleSpace();

                        if (brakeModeKey.intValue == 1)
                        {
                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();

                            GUILayout.Label("Brake Key", GUILayout.Width(60));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(brakeKey, GUIContent.none, true, GUILayout.MinWidth(10));
                            GUILayout.FlexibleSpace();

                            GUILayout.EndHorizontal();


                        }

                        GUILayout.EndHorizontal();


                        #endregion

                        EditorGUILayout.Space();


                        GuiLine();

                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("HandBrake", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(handBrakeKey, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Lights", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(lightsKey, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("NOS", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(nosKey, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Camera", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(cameraKey, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Gear Up", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(gearUp, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Gear Down", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(gearDown, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        break;

                    case 1:

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Joystick Number", GUILayout.Width(100));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(joystickNumber, GUIContent.none, true, GUILayout.MinWidth(5));
                        GUILayout.FlexibleSpace();

                        GUILayout.Space(50);

                        GUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        GuiLine();

                        #region STEERING CONTROLS

                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Steering", GUILayout.Width(50));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(steeringModeJoystick, GUIContent.none, true, GUILayout.MinWidth(40));
                        GUILayout.FlexibleSpace();

                        if (steeringModeJoystick.intValue == 1)
                        {
                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();

                            GUILayout.Label("Left", GUILayout.Width(35));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(buttonSteeringLeft, GUIContent.none, true, GUILayout.MinWidth(15));
                            GUILayout.FlexibleSpace();

                            GUILayout.Label("Right", GUILayout.Width(35));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(buttonSteeringRight, GUIContent.none, true, GUILayout.MinWidth(15));
                            GUILayout.FlexibleSpace();

                            GUILayout.EndHorizontal();


                        }

                        GUILayout.EndHorizontal();


                        #endregion

                        EditorGUILayout.Space();

                        GuiLine();

                        EditorGUILayout.Space();

                        #region GAS CONTROLS

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Gas", GUILayout.Width(50));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(gasModeJoystick, GUIContent.none, true, GUILayout.MinWidth(40));
                        GUILayout.FlexibleSpace();

                        if (gasModeJoystick.intValue == 1)
                        {
                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();

                            GUILayout.Label("Gas Key", GUILayout.Width(60));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(buttonGas, GUIContent.none, true, GUILayout.MinWidth(10));
                            GUILayout.FlexibleSpace();

                            GUILayout.EndHorizontal();


                        }

                        GUILayout.EndHorizontal();


                        #endregion

                        EditorGUILayout.Space();

                        GuiLine();

                        EditorGUILayout.Space();

                        #region BRAKE CONTROLS

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Brake", GUILayout.Width(50));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(brakeModeJoystick, GUIContent.none, true, GUILayout.MinWidth(40));
                        GUILayout.FlexibleSpace();

                        if (brakeModeJoystick.intValue == 1)
                        {
                            EditorGUILayout.Space();

                            GUILayout.BeginHorizontal();

                            GUILayout.Label("Brake Key", GUILayout.Width(60));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.PropertyField(buttonBrake, GUIContent.none, true, GUILayout.MinWidth(10));
                            GUILayout.FlexibleSpace();

                            GUILayout.EndHorizontal();


                        }

                        GUILayout.EndHorizontal();


                        #endregion

                        EditorGUILayout.Space();


                        GuiLine();

                        EditorGUILayout.Space();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("HandBrake", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(buttonHandBrake, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Lights", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(buttonLights, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("NOS", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(buttonNos, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Camera", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(buttonCamera, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Gear Up", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(buttonGearUp, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Gear Down", GUILayout.Width(70));
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.PropertyField(buttonGearDown, GUIContent.none, true, GUILayout.MinWidth(20));
                        GUILayout.FlexibleSpace();

                        GUILayout.EndHorizontal();                        

                        break;
                }

                EditorGUILayout.Space();
                GUILayout.EndVertical();


                #endregion

                break;

            case "Audio":

                #region AUDIO

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("AUDIO", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Set audio clips and mix volumes");
                EditorGUILayout.Space();

                GUILayout.BeginVertical("", "box");
                EditorGUILayout.LabelField("Engine Audio", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(lowAcceleration);
                EditorGUILayout.PropertyField(lowDeceleration);
                EditorGUILayout.PropertyField(highAcceleration);
                EditorGUILayout.PropertyField(highDeceleration);
                EditorGUILayout.PropertyField(engineVolume);
                EditorGUILayout.Space();
                GUILayout.EndVertical();

                if (nosON.boolValue == true)
                {
                    EditorGUILayout.Space();
                    GUILayout.BeginVertical("", "box");
                    EditorGUILayout.LabelField("NOS Audio", EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(nosAudioClip);
                    EditorGUILayout.PropertyField(nosVolume);
                    EditorGUILayout.Space();
                    GUILayout.EndVertical();
                }

                if (turboON.boolValue == true)
                {
                    EditorGUILayout.Space();
                    GUILayout.BeginVertical("", "box");
                    EditorGUILayout.LabelField("Turbo Audio", EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(turboAudioClip);
                    EditorGUILayout.PropertyField(turboVolume);
                    EditorGUILayout.Space();
                    GUILayout.EndVertical();
                }

                if (skidMarks.boolValue == true)
                {

                    EditorGUILayout.Space();
                    GUILayout.BeginVertical("", "box");
                    EditorGUILayout.LabelField("Skid Audio", EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(skidSound);
                    EditorGUILayout.PropertyField(skidVolume);
                    EditorGUILayout.Space();
                    GUILayout.EndVertical();
                }
                

                if (collisionSystem.boolValue == true)
                {
                    EditorGUILayout.Space();
                    GUILayout.BeginVertical("", "box");
                    EditorGUILayout.LabelField("Collision Audio", EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(collisionSound);
                    EditorGUILayout.PropertyField(collisionVolume);
                    EditorGUILayout.Space();
                    GUILayout.EndVertical();
                }

                if (exhaustFlame.boolValue == true)
                {
                    EditorGUILayout.Space();
                    GUILayout.BeginVertical("", "box");
                    EditorGUILayout.LabelField("Exhaust Audio", EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(exhaustSound);
                    EditorGUILayout.PropertyField(exhaustVolume);
                    EditorGUILayout.Space();
                    GUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                #endregion

                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }

    
    public void UnpackPrefab()
    {
        PrefabUtility.UnpackPrefabInstance(myTarget.objToUnpack, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
    }

    public void UnpackModelMesh()
    {
        if (PrefabUtility.GetPrefabInstanceStatus(myTarget.modelMeshToUnpack).ToString() != "NotAPrefab")
        {
            PrefabUtility.UnpackPrefabInstance(myTarget.modelMeshToUnpack, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
    }

    void GuiLine(int i_height = 1)

    {

        Rect rect = EditorGUILayout.GetControlRect(false, i_height);

        rect.height = i_height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

    }
}
