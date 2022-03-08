using UnityEngine;
using System.Collections;

public class FlagLineScript : MonoBehaviour {
	public int FlagNumber;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
    void OnTriggerEnter(Collider other) {
       GameObject car = other.gameObject;
       AbstractAnyCarAI ai = car.GetComponentInParent<AbstractAnyCarAI>();
       ai.PassedFlag(FlagNumber);
    }

}
