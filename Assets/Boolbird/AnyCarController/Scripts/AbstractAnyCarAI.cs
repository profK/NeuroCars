using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public abstract class AbstractAnyCarAI : MonoBehaviour
{
    protected int carIndex;
    protected int lastFlag=-1;
    protected float LastTimeMoved=0;
    protected NeuroCarCreator mgr;
    
    
    // Start is called before the first frame update
    void Start()
    {
        lastFlag = -1;
        LastTimeMoved = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIndex(int idx)
    {
        carIndex = idx;
    }
    
    public void SetCreator(NeuroCarCreator manager)
    {
        mgr = manager;
    }
    
    protected abstract void Process(float[] inputs,out float[] outputs);

    public void GetControls(float currentSpeed,float maxSpeed,
        out float xValue, out float yValue, out float handBrakeValue)
    {

        CarRayCaster caster = GetComponentInChildren<CarRayCaster>();
        RaycastHit leftHit = caster.LEFT_HIT;
        RaycastHit rightHit = caster.RIGHT_HIT;    
        
        float[] inputs = new float[2];
        inputs[0] = ((leftHit.distance/(leftHit.distance+rightHit.distance))*2)-1;
        if (float.IsNaN(inputs[0])) { //up against a wall
            xValue = 0;
            yValue = 0;
            handBrakeValue = 0;
            return;
        }
        inputs[1] = (currentSpeed/maxSpeed*2)-1;
        // do speed
        float[] outputs = new float[3]; 
        Process(inputs,out outputs);
        yValue = outputs[0];
        xValue = outputs[1];
        handBrakeValue = 0;
    }
    
    public void PassedFlag(int number){
        if (number > lastFlag)
        {
            mgr.CarPassedFlag(carIndex, number);
            lastFlag = number;
        } 
        else if (number<lastFlag)  {
            if (number == 0)
            {
                mgr.CarFinished(carIndex);
            } else {
                mgr.CarTurnedAround(carIndex);
            }
        } 
      
        
		
    }
}
