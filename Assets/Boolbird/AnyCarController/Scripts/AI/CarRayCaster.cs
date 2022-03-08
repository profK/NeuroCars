using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRayCaster : MonoBehaviour
{
    // Start is called before the first frame update
    public RaycastHit LEFT_HIT { get; private set; }
    public RaycastHit RIGHT_HIT { get; private set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform xform = GetComponent<Transform>();
        Vector3 startPt = xform.position;
        Vector3 forwardVec = xform.forward;
        Vector3 leftDir = xform.TransformDirection(new Vector3(1.0f,0f,1.0f));
        Vector3 rightDir = new Vector3(-leftDir.z,0,leftDir.x);
        RaycastHit leftHit;
        RaycastHit rightHit;
		
        Physics.Raycast(startPt,leftDir,out leftHit);
        Physics.Raycast(startPt,rightDir,out rightHit);
        Debug.DrawLine(startPt,leftHit.point,Color.green);
        Debug.DrawLine(startPt,rightHit.point,Color.green);
        LEFT_HIT = leftHit;
        RIGHT_HIT = rightHit;
    }
}
