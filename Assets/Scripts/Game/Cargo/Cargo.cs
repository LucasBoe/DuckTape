using UnityEngine;
using UnityEngine.Serialization;

public class Cargo : MonoBehaviour
{
    [FormerlySerializedAs("CargoConfig")] public CargoConfigBase Config;
    public int OriginStationID = -1;
}