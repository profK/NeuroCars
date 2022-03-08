using UnityEngine;


public class NeuroCarAI : AbstractCarAI {
	const int MAXBREAK = 3000;
	
	private NeuroCarCreator mgr;
	private int carIndex;
	private int lastFlag;
	private float LastTimeMoved=0;
	
	
	public SingleLayerPerceptron _network;
	
	
	protected override void SetupAI () {
		lastFlag = -1;
		LastTimeMoved=Time.time;
	}
	
	public void SetNetwork(SingleLayerPerceptron network, NeuroCarCreator mgr, int carIndex){
		_network = network;
		this.mgr = mgr;
		this.carIndex = carIndex;
	}
	

	protected override void CalculateInputs ()
	{
		if (GetComponent<Rigidbody>().velocity.magnitude!=0){
			LastTimeMoved = Time.time;
		}
		if (Time.time - LastTimeMoved >=5){ // sat stil lfor 5 seconds
			mgr.CarStopped(carIndex);
			return;
		}
		// calculate steering angle
		Vector3 forwardVec = new Vector3(0.0f,0.0f,1.0f);
		Quaternion quat = Quaternion.AngleAxis(GetComponent<Rigidbody>().transform.rotation.eulerAngles.y,new Vector3(0,1,0));
		forwardVec = quat * forwardVec;
		Vector3 startPt = GetComponent<Rigidbody>().position + (forwardVec*gameObject.GetComponent<Renderer>().bounds.size.y);
		Vector3 leftDir = new Vector3(1.0f,0f,1.0f);
		leftDir = quat * leftDir;
		Vector3 rightDir = new Vector3(-leftDir.z,0,leftDir.x);
		RaycastHit leftHit;
		RaycastHit rightHit;
		
		Physics.Raycast(startPt,leftDir,out leftHit);
		Physics.Raycast(startPt,rightDir,out rightHit);
		Debug.DrawLine(startPt,leftHit.point,Color.green);
		Debug.DrawLine(startPt,rightHit.point,Color.green);
		float[] inputs = new float[2];
		inputs[0] = ((leftHit.distance/(leftHit.distance+rightHit.distance))*2)-1;
		if (float.IsNaN(inputs[0])) { //up against a wall
			// not goign anywhere
			return;
		}
		inputs[1] = (((float)CurrentSpeed())/MaxSpeed()*2)-1;
		// do speed
		float[] outputs = _network.Process(inputs);
		inputSteer = outputs[0]; 
		Debug.Log("outputs[0].inoputSteer = "+outputs[0]+","+inputSteer);
		inputTorque = (outputs[1]+1)/2; // map from -1 to +1  => 0 to 1
		inputBreak = (outputs[2]+1)/2;// map from -1 to +1  => 0 to 1
		Debug.Log("stter,torque,break = "+inputSteer+","+inputTorque+","+inputBreak);
	}
	
	void PassedFlag(int number){
		if (number==0){
			if (lastFlag>0){
				mgr.CarPassedFlag(carIndex,number);
			} else if (lastFlag==0) {
				mgr.CarTurnedAround(carIndex);
			}
		} else if (number>lastFlag) {
			mgr.CarPassedFlag(carIndex,number);
		} else {
				mgr.CarTurnedAround(carIndex);
		}
		
		lastFlag = number;
		
	}
	
	
	
}
