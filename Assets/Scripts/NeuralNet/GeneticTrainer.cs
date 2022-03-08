using UnityEngine;
using System.Collections;

public class GeneticTrainer
{
	static private System.Random rnd = new System.Random();	
	private static bool arithmaticMutation = true;
	
	static public void Train(SingleLayerPerceptron[] networks,int [] scores,float mutationRate){
		float[][] genomes;
		genomes = ExtractGenomes(networks);
		float[][] newGenomes = new float[genomes.GetLength(0)][];
		int highScoreIndex=0;
		int secondHighestIndex=0;
		for (int i = 0; i < scores.Length; i++)
		{
			if (scores[highScoreIndex] < scores[i])
			{
				secondHighestIndex = highScoreIndex;
				highScoreIndex = i;
			} else if (scores[secondHighestIndex] < scores[i])
			{
				secondHighestIndex = i;
			}
		}

		if (scores[highScoreIndex] == 0)
		{
			highScoreIndex =ChooseAGenome(scores);
		}
		
		if (scores[secondHighestIndex] == 0)
		{
			secondHighestIndex =ChooseAnotherGenome(highScoreIndex,scores);
		}

		newGenomes[0] = genomes[highScoreIndex];
		newGenomes[1] = genomes[secondHighestIndex];
		for(int i=1;i<newGenomes.GetLength(0)/2;i++){ 
			Debug.Log("Calculating genomes "+(i*2)+","+(i*2+1));
			int idx = ChooseAGenome(scores);
			int idx2 = ChooseAnotherGenome(idx,scores);
			float[][] newG = Mutate(Crossover(genomes[idx],genomes[idx2]),mutationRate);
			newGenomes[i*2] = newG[0];
			newGenomes[(i*2)+1] = newG[1];     
		}
		SetGenomes(networks,newGenomes);	                        
	}
		
	static private float[][] ExtractGenomes(SingleLayerPerceptron[] networks){
		float[][] g = new float[networks.GetLength(0)][];
		for(int i=0;i<g.GetLength(0);i++){
			g[i] = networks[i].GetWeights();
		}
		return g;
	}
	
	static private void SetGenomes(SingleLayerPerceptron[] networks, float[][]genomes){
		for(int i=0;i<genomes.GetLength(0);i++){
			networks[i].SetWeights(genomes[i]);
		}	
	}
	
	static private float[][] Crossover(float[] g1,float[]g2){
		float[][] output = new float[2][];
		output[0] = new float[g1.GetLength(0)];
		output[1] = new float[g2.GetLength(0)];
		for(int i=0;i<g1.GetLength(0);i++){
			double a = rnd.NextDouble();
			if (arithmaticMutation){
				output[0][i] = (float)((g1[i]*a)+((1-a)*g2[i]));
				output[1][i] = (float)((g2[i]*a)+((1-a)*g1[i]));
			} else {
				if (a>.7){
					output[0][i] =g1[i];
					output[1][i] =g2[i];
				} else {
					output[0][i] =g2[i];
					output[1][i] =g1[i];
				}
			}
		}
		return output;
	}
	
	static private float[][] Mutate(float[][] genomes,float mutationRate){
		for(int i=0;i<genomes[0].GetLength(0);i++){
			if (rnd.NextDouble()<=mutationRate){ // .01% mutate chance
				genomes[0][i] += (float)(rnd.NextDouble()-0.5);	
			}
			if (rnd.NextDouble()<=mutationRate){ // 1% mutate chance
				genomes[1][i] += (float)(rnd.NextDouble()-0.5);	
			}
		}
		return genomes;
	}
	
	static private int ChooseAGenome(int[] scores){
		int acc = 0;
		for(int i=0;i<scores.GetLength(0);i++){
			acc+= scores[i];	
		}

		if (acc == 0)
		{
			Debug.LogWarning("No scores yet!");
			return (int) (rnd.NextDouble() * scores.Length);
		}
		int spin = (int)(rnd.NextDouble()*acc);
		for(int i=0;i<scores.GetLength(0);i++){
			spin -= scores[i];
			if (spin<=0){
				return i;
			}
		}
		throw new System.InvalidOperationException("Spin outside of range.");
	}
	
	static private int ChooseAnotherGenome(int idx1,int[] scores){
		int idx2 = idx1;
		while(idx1==idx2){
			idx2 = ChooseAGenome(scores);	
		}
		return idx2;
	}
}
