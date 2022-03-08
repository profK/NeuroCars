using UnityEngine;
using System.Collections.Generic;
using Genetics;

public class FuzzyCarCreator : MonoBehaviour {
	public float RaceLength = 90.0f;
	public GameObject CarPrototype;
	public int numCars = 6;
	private LongPopulation population;
	private GameObject[] cars;
	private float[] startTime;
	private int carCount;
	private List<FitnessRec> fitnessScoreList = new List<FitnessRec>();
	private Vector2 scrollPosition;
	int idx;
	float lastTime=0;
	bool racing;
	private int bestFitness=0;
	private int worstFitness=0;
	private float averageFitness = 0f;
	private int generationCount;
	
	// Use this for initialization
	void Start () {
		population = new LongPopulation(numCars);
		population.Seed();
		cars = new GameObject[numCars];
		startTime = new float[numCars];
		generationCount=0;
		StartRace();
		
	}
	
	private void StartRace(){
		lastTime=0;
		idx =0;
		racing = true;
		carCount=0;
		generationCount++;
	}
	
	// Update is called once per frame
	void Update () {
		if (!racing){
			return;	
		}
		if (idx<cars.Length){ // still starting all cars
			if (Time.time-lastTime> 3.0f){	
				int i= idx%3;
				cars[idx] = (GameObject)Instantiate (CarPrototype,new Vector3((5*i)-5,5,10),new Quaternion());
				cars[idx].GetComponent<FuzzyCarAI>().SetGenome(this,idx,population);
				startTime[idx] = Time.time;
				population.SetFitnessAtIndex(idx,0); // reset so we can accumuate a fitenss score as we go
				idx++;
				lastTime = Time.time;
				carCount++;
			}
		} else if (Time.time> startTime[startTime.Length-1]+RaceLength) { // time for last car expired
			CleanupRace();
		}
	}
	
	
	void OnGUI() {
		//create a StringBuilder to build the output in
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append("Generation ");
		sb.Append(generationCount);
		sb.Append("\n");
		if (idx < cars.Length) {			
			sb.Append("Starting car ");
			sb.Append(idx);
		}else {
			if (racing) {
				sb.Append("Time Left: ");
				sb.Append((int)(startTime[startTime.Length-1]+RaceLength-Time.time));
			} else {
				sb.Append("Race Over");
			}
		}
		GUI.Box(new Rect(10,10,150,50),sb.ToString());
		sb = new System.Text.StringBuilder();
		// Output stats on last race
		sb.Append("Best Fitness: ");
		sb.Append(bestFitness);
		sb.Append("\n");
		sb.Append("Worst Fitness: ");
		sb.Append(worstFitness);
		sb.Append("\n");
		sb.Append("Average Fitness: ");
		sb.Append(averageFitness);
		
		GUI.Box(new Rect(200,10,200,60),sb.ToString());
	}
    
	
	public void CarTurnedAround(int carNum){
		RemoveCar(carNum);
	}
	
	public void CarPassedFlag(int carNum,int flagNum){
		if (flagNum==0) {// full circuit	
			population.SetFitnessAtIndex(carNum,1000+(int)((RaceLength-(Time.time - startTime[carNum]))*100));
			RemoveCar(carNum);      
		} else {
			population.SetFitnessAtIndex(carNum,flagNum*100);
		}
	}

	private void RemoveCar(int idx){
		Destroy(cars[idx]);	
		carCount--;
		if (carCount <=0) {
			CleanupRace();	
		}
	}
	
	private struct FitnessRec : System.IComparable<FitnessRec> {
		int index;
		int fitness;
		public FitnessRec(int idx,int fit){
			index = idx;
			fitness = fit;
		}
		
		public int CompareTo (FitnessRec other) {
			return (other.fitness - fitness); // invert so list has highest at top
		}

		public override bool Equals (object obj)
		{
			return fitness == ((FitnessRec)obj).fitness;
		}

		public override string ToString ()
		{
			return string.Format ("[FitnessRec : index= "+index+" fitness= "+fitness+"]");
		}
		
		
	}
	private void CleanupRace(){
		if (!racing){
			return;
		}
		racing = false;
		for(int i=0;i<cars.Length;i++){
			if (cars[i]!=null){
				Destroy(cars[i]);
			}
		}
		// calculate stats
		bestFitness = int.MinValue;
		worstFitness = int.MaxValue;
		averageFitness = 0f;
		for(int i=0;i<cars.Length;i++){
			int fit = population.GetFitnessAtIndex(i);
			bestFitness = (fit>bestFitness)?fit:bestFitness;
			worstFitness = (fit<worstFitness)?fit:worstFitness;
			averageFitness += fit;
		}
		averageFitness = averageFitness/(cars.Length);
		//geneate and restart
		population.NextGeneration();
		StartRace();
	}
}
