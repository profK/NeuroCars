using UnityEngine;
using System.Collections;

public class SingleLayerPerceptron
{
	static System.Random rnd = new System.Random();
	
	public float[,] hiddenWeights;
	public float[,] outputWeights;
	public float[] hiddenLayerBiases;
	public float[] outputNodeBiases;
	//used for calculation
	float[] values;
	float[] output;
	
	public SingleLayerPerceptron (int inputNodes,int hiddenLayerNodes, int outputNodes)
	{
		hiddenWeights = new float[inputNodes,hiddenLayerNodes];
		outputWeights = new float[hiddenLayerNodes,outputNodes];
		hiddenLayerBiases = new float[hiddenLayerNodes];
		outputNodeBiases = new float[outputNodes];
		values = new float[hiddenLayerNodes];
		output = new float[outputNodes];
		Randomize();
	}
	
	public void Randomize(){
		for(int i=0;i<hiddenWeights.GetLength(0);i++){
			for(int j=0;j<hiddenWeights.GetLength(1);j++){
				hiddenWeights[i,j] = ((float)rnd.NextDouble()-0.5f)*2;	
				
			}
		}
		for(int i=0;i<outputWeights.GetLength(0);i++){
			for(int j=0;j<outputWeights.GetLength(1);j++){
				outputWeights[i,j] = ((float)rnd.NextDouble()-0.5f)*2;	
				Debug.Log("ow["+i+","+j+"] = "+outputWeights[i,j]);
			}
		}
		for(int i=0;i<hiddenLayerBiases.GetLength(0);i++){
			hiddenLayerBiases[i] = 	((float)rnd.NextDouble()*2)-1;
		}
		for(int i=0;i<outputNodeBiases.GetLength(0);i++){
			outputNodeBiases[i] = 	((float)rnd.NextDouble()*2)-1;	
		}
	}
	
	public float[] Process(float[] input){
		//float[] values = new float[hiddenLayerBiases.GetLength(0)];
		for(int i=0;i<input.GetLength(0);i++){
			if (float.IsNaN(input[i])){
				Debug.LogError("Input["+i+"] is NaN!");
			}
		}
		// do hidden layer
		for(int j=0;j<hiddenWeights.GetLength(1);j++){
			values[j] = hiddenLayerBiases[j];
			//values[j] = 0;
			for(int i=0;i<hiddenWeights.GetLength(0);i++){
				values[j] += (input[i] * hiddenWeights[i,j]);	
			}
			values[j] = Threshold(values[j]);
			if (float.IsNaN(values[j])){
				Debug.LogError("Values["+j+"] is NaN!");	
			}
		}
		// do output
		
		for(int j=0;j<outputWeights.GetLength(1);j++){
			output[j] = outputNodeBiases[j];
			for(int i=0;i<outputWeights.GetLength(0);i++){
				output[j] += values[i] * outputWeights[i,j];	
			}
			output[j] = Threshold(values[j]);
			if (float.IsNaN(output[j])){
				Debug.LogError("Output["+j+"] is NaN!");	
			}
		}
		Debug.Log("Network output: "+output[0]+","+output[1]+"'"+output[2]);
		return output;
	}
	
	private float Threshold(float inp){
		return (float)(2 / (1 + System.Math.Exp(-2 * inp)) - 1);
	}
	
	public float[] GetWeights(){
		float[] w = new float[(hiddenWeights.GetLength(0)*hiddenWeights.GetLength(1))+
		                          (outputWeights.GetLength(0)*outputWeights.GetLength(1))+
		                          outputNodeBiases.GetLength(0)+hiddenLayerBiases.GetLength(0)];
		int idx=0;
		for(int i=0;i<hiddenWeights.GetLength(0);i++){
			for(int j=0;j<hiddenWeights.GetLength(1);j++){
				w[idx++] = hiddenWeights[i,j];	
			}
		}
		for(int i=0;i<outputWeights.GetLength(0);i++){
			for(int j=0;j<outputWeights.GetLength(1);j++){
				w[idx++] = outputWeights[i,j];	
			}
		}
		for(int i=0;i<outputNodeBiases.GetLength(0);i++){
			w[idx++] = outputNodeBiases[i];	
		}
		for(int i=0;i<hiddenLayerBiases.GetLength(0);i++){
			w[idx++] = hiddenLayerBiases[i];	
		}
		return w;
	}
	
	public void SetWeights(float[] w){
		int idx=0;
		for(int i=0;i<hiddenWeights.GetLength(0);i++){
			for(int j=0;j<hiddenWeights.GetLength(1);j++){
				hiddenWeights[i,j] = w[idx++];
			}
		}
		for(int i=0;i<outputWeights.GetLength(0);i++){
			for(int j=0;j<outputWeights.GetLength(1);j++){
				outputWeights[i,j] = w[idx++] ;
			}
		}
		for(int i=0;i<outputNodeBiases.GetLength(0);i++){
			outputNodeBiases[i]=w[idx++];	
		}
		for(int i=0;i<hiddenLayerBiases.GetLength(0);i++){
			hiddenLayerBiases[i]=w[idx++];	
		}
	}
}

