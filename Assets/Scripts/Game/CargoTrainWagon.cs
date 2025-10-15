using System;
using UnityEngine;

public class CargoTrainWagon : TrainWagonBase
{
    public override int CalculateWeight()
    {
        //add logic for contained cargo
        return base.CalculateWeight();
    }
}