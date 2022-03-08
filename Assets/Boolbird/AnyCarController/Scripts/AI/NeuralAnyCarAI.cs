using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralAnyCarAI : AbstractAnyCarAI
{
    
    public SingleLayerPerceptron _network;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    public void SetNetwork(SingleLayerPerceptron network)
    {
        _network = network;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Process(float[] inputs, out float[] outputs)
    {
        outputs = _network.Process(inputs);
    }
}
