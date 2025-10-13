using UnityEngine;

public class Cargo : MonoBehaviour
{
    [SerializeField] private CargoConfigBase cargoConfig;
    public CargoConfigBase Config => cargoConfig;
}
