using SS;
using UnityEngine;

public class GlobalBalancing : SingletonBehaviour<GlobalBalancing>
{
    [SerializeField] GlobalBalancingConfig config;
    public static GlobalBalancingConfig Value => Instance.config;
}