using UnityEngine;
using System.Collections.Generic;
using Array=System.Array;


public class AICar_Script : AbstractCarAI {
	
	// Here's all the variables for the AI, the waypoints are determined in the "GetWaypoints" function.
	// the waypoint container is used to search for all the waypoints in the scene, and the current
	// waypoint is used to determine which waypoint in the array the car is aiming for.
	public GameObject waypointContainer ;
	private Transform[] waypoints;
	private int currentWaypoint  = 0;
	
	protected override void SetupAI () {
		// Now, this function basically takes the container object for the waypoints, then finds all of the transforms in it,
		// once it has the transforms, it checks to make sure it's not the container, and adds them to the array of waypoints.
		Transform[] potentialWaypoints = waypointContainer.GetComponentsInChildren<Transform>();
		List<Transform> wpList = new List<Transform> ();
		
		foreach ( Transform potentialWaypoint in potentialWaypoints ) {
			if ( potentialWaypoint != waypointContainer.transform ) {
				wpList.Add(potentialWaypoint);
			}
		}
		waypoints = wpList.ToArray();
	}
	
	protected override void CalculateInputs() {
		// now we just find the relative position of the waypoint from the car transform,
		// that way we can determine how far to the left and right the waypoint is.
		Vector3 RelativeWaypointPosition = transform.InverseTransformPoint( new Vector3( 
													waypoints[currentWaypoint].position.x, 
													transform.position.y, 
													waypoints[currentWaypoint].position.z ) );
																					
																					
		// by dividing the horizontal position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
		inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
		
		// now we do the same for torque, but make sure that it doesn't apply any engine torque when going around a sharp turn...
		if ( Mathf.Abs( inputSteer ) < 0.5 ) {
			inputTorque = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude - Mathf.Abs( inputSteer );
		}else{
			inputTorque = 0.0f;
		}
		
		// this just checks if the car's position is near enough to a waypoint to count as passing it, if it is, then change the target waypoint to the
		// next in the list.
		if ( RelativeWaypointPosition.magnitude < 20 ) {
			currentWaypoint ++;
			
			if ( currentWaypoint >= waypoints.Length ) {
				currentWaypoint = 0;
			}
		}
		
	}
}
