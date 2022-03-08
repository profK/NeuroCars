using UnityEngine;
using System.Collections;

public abstract class AbstractCarAI : MonoBehaviour {

	// These variables allow the script to power the wheels of the car.
	public  WheelCollider FrontLeftWheel;
	public  WheelCollider FrontRightWheel;

	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	public  float[] GearRatio;
	public  int CurrentGear = 0;

	// These variables are just for applying torque to the wheels and shifting gears.
	// using the defined Max and Min Engine RPM, the script can determine what gear the
	// car needs to be in.
	public float EngineTorque  = 600.0f;
	public float MaxEngineRPM  = 3000.0f;
	public float MinEngineRPM  = 1000.0f;

	private  float EngineRPM  = 0.0f;

	

	// input steer and input torque are the values substituted out for the player input. The 
	// "NavigateTowardsWaypoint" function determines values to use for these variables to move the car
	// in the desired direction.
	protected float inputSteer = 0.0f;
	protected float inputTorque  = 0.0f;
	protected float inputBreak = 0.0f;

	void Start () {
		// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
		
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0,-1.5f,0);
	
		SetupAI();
		
		// Call the function to determine the array of waypoints. This sets up the array of points by finding
		// transform components inside of a source container.
		//GetWaypoints();
	}
	
	protected float MaxSpeed(){
		return MaxEngineRPM * GearRatio[5];	
	}
	
	protected float CurrentSpeed(){
		return EngineRPM * GearRatio[CurrentGear];	
	}
	
	void Update () {
	
		// This is to limith the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
		// but it's easy, and it doesn't interfere with the physics processing.
		GetComponent<Rigidbody>().drag = GetComponent<Rigidbody>().velocity.magnitude / 250;
	
		// Call the funtion to determine the desired input values for the car. This essentially steers and
		// applies gas to the engine.
		CalculateInputs();
	
		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
		EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
		ShiftGears();

		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
		// up to twice it's pitch, where it will suddenly drop when it switches gears.
		GetComponent<AudioSource>().pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;
		// this line is just to ensure that the pitch does not reach a value higher than is desired.
		if ( GetComponent<AudioSource>().pitch > 2.0f ) {
			GetComponent<AudioSource>().pitch = 2.0f;
		}
	
		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
		// multiplied by the calculated AI input variable.
		FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * inputTorque;
		FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * inputTorque;
		
		// the steer angle is an arbitrary value multiplied by the calculated AI input.
		FrontLeftWheel.steerAngle = 10 * inputSteer;
		FrontRightWheel.steerAngle = 10 * inputSteer;
		
		// Calculate break torque
		FrontLeftWheel.brakeTorque = 10 * inputBreak;
		FrontRightWheel.brakeTorque = 10 * inputBreak;
	}

    void ShiftGears() {
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		if ( EngineRPM >= MaxEngineRPM ) {
			int AppropriateGear = CurrentGear;
			
			for ( var i = 0; i < GearRatio.Length; i ++ ) {
				if ( FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
		
		if ( EngineRPM <= MinEngineRPM ) {
			int AppropriateGear = CurrentGear;
			
			for ( var j = GearRatio.Length-1; j >= 0; j -- ) {
				if ( FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}
	
	protected abstract void SetupAI();
	protected abstract void CalculateInputs();
	
	
}
