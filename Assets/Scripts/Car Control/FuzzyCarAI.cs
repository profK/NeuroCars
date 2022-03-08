using UnityEngine;
using System.Collections.Generic;
using Genetics;


public class FuzzyCarAI : AbstractCarAI {
	int[] K = new int[6];
	const float Smax = 2;
	int gidx;
	FuzzyCarCreator mgr;
	private int lastFlag = -1;
	
	public void SetGenome(FuzzyCarCreator mgr, int genomeIndex, LongPopulation  pop){
		this.mgr = mgr;
		gidx = genomeIndex;
		long g = pop[genomeIndex];
		for(int i=0;i<6;i++){
			K[i] = (int)(g&0x3FF); // 10 bits
			g = g >> 10;
		}
	}

	protected override void SetupAI () {
		lastFlag = -1;
	}
	
	static float lastInputSteer=0;
	
	protected override void CalculateInputs() {
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
		//Debug.Log("LeftHitD = "+leftHit.distance);
		//Debug.Log("rigthHitD = "+rightHit.distance);
				
			inputSteer =(((leftHit.distance/(leftHit.distance+rightHit.distance))*2)-1);
			inputSteer = inputSteer *(10*K[0]/0x3FF);
			if (inputSteer<0){
				inputSteer = Mathf.Max(inputSteer,-2.0f);
			} else {
				inputSteer = Mathf.Min(inputSteer,2.0f);	
			}
		
		//Debug.Log("inputSteering="+inputSteer);
		// calculate engine torque
		float inputSteerDelta = Mathf.Abs(inputSteer-lastInputSteer);
		float cornerEnter = Mathf.Abs(inputSteer);
		float cornerExit = 1.0f - cornerEnter;
		//Debug.Log("cornerEnter = "+cornerEnter);
		float fast = CurrentSpeed()/MaxSpeed();
		//Debug.Log("fast="+fast);
		float slow = 1.0f - fast;
		inputTorque = ((cornerExit*K[2])+(slow*K[3]))/(K[2]+K[3]);
		//Debug.Log("inputTorque="+inputTorque);
		inputBreak = ((cornerEnter*K[4])+(fast*K[5]))/(K[4]+K[5]);
		//Debug.Log("inputBreak="+inputBreak);
	}
	
	void PassedFlag(int number){
		if (lastFlag>=0){ // skip start line
			mgr.CarPassedFlag(gidx,number);
		} else if (number<lastFlag){
			mgr.CarTurnedAround(gidx);
		}
		lastFlag = number;
		
	}
}
