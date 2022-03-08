using System;
namespace Genetics
{
	
	
	public class LongPopulation 
	{
		
		long[] population;
		int[] fitness;
		static Random rnd = new Random();
		
		public LongPopulation(int popCount){
			population = new long[popCount];
			fitness = new int[popCount];
			
		}
		
		public void Seed(){
			for(int i=0;i<population.Length;i++){
				population[i]=RandomLong();
			}
		}
		
		public long RandomLong(){
			byte[] ba = new byte[8];
			rnd.NextBytes(ba);
			long l = 0;
			for(int i=0;i<8;i++){
				l= l << 8;
				l = l | (((long)ba[i])&0xFFL);
			}
			Console.WriteLine("long="+l);
			return l;
		}
		
		public long this[int index] 
		{
   			get { return population[index]; }
			set { population[index] = value; }
		}
		
		public void NextGeneration(){
			long[] newPop = new long[population.Length];
			for(int i=0;i<population.Length;i+=2){
				int p1Idx = GetAMemberIndex();
				int p2Idx = p1Idx;
				while(p2Idx==p1Idx){
					p2Idx = GetAMemberIndex();
				}
				DoCrossover(population[p1Idx],population[p2Idx], out newPop[i], out newPop[i+1]);
			}
			if ((population.Length % 2) != 0) { // odd population
				newPop[population.Length-1] = population[GetAMemberIndex()];	
			}
			// mutate 
			for(int i=0;i<newPop.Length;i++){
				population[i] = DoMutation(newPop[i]);	
				fitness[i]=int.MinValue; // fitness unknown
			}
			
		}
		
		private int GetAMemberIndex(){
			long acc = 0;
			// find sume of all fitness
			foreach(int fv in fitness){
				acc += fv;	
			}
			// now find a random btw 0 and sum
			long spin = (long)(rnd.NextDouble()*acc);
			// now count down to find whsoe space its in
			for(int idx=0;idx<fitness.Length;idx++){
				spin -= fitness[idx];
				if (spin<=0) {
					return idx;
				}
			}
			// should never ever get here
			throw new ApplicationException("Spin for member outside of member range!");
		}
				                                         
		private void DoCrossover(long p1, long p2, out long n1, out long n2){
			n1 = 0;
			n2 = 0;
			for(int i=0;i<sizeof(long)*8;i++){
				long bitmask = 1L << i;
				if (rnd.NextDouble()>0.7) { // no cross over	
					n1 |= (p1 & bitmask);
					n2 |= (p2 & bitmask);
				} else { //crossover the bits
					n1 |= (p2 & bitmask);
					n2 |= (p1 & bitmask);
				}
			}
			//Console.WriteLine("Crossover: "+p1+","+p2+","+n1+","+n2);
		}
		
		private long DoMutation(long p){
			for(int i=0;i<sizeof(long)*8;i++){
				long bitmask = 1L << i;
				if (rnd.NextDouble() <= 0.001){
					p = p^bitmask;	
				}
			}
			return p;
		}
		
		public int GetIndexOfMostFit(){
			int idx = 0;
			for(int i=1;i<fitness.Length;i++){
				if (fitness[i]>fitness[idx]){
					idx = i;	
				}
			}
			return idx;
		}
		
		public int GetFitnessAtIndex(int idx){
			return fitness[idx];	
		}
		
		public void SetFitnessAtIndex(int idx, int fit){
			fitness[idx] = fit;	
		}
	}
}

