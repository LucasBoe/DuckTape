using System;
using UnityEngine;

public class CargoTrainWagon : TrainWagonBase
{
    protected override int CalculateWeight()
    {
        //add logic for contained cargo
        return base.CalculateWeight();
    }
}