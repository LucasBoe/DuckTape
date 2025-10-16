using Unity.Cinemachine;
using UnityEngine;

public class StationVirtualCameraController : MonoBehaviour
{
    [SerializeField] private StationEnvironment station;
    [SerializeField] private CinemachineCamera virtualCamera;
    private void OnEnable()
    {
        station.OnEnter.AddListener(OnEnter);
        station.OnExit.AddListener(OnExit);
    }    private void OnDisable()
    {
        station.OnEnter.RemoveListener(OnEnter);
        station.OnExit.RemoveListener(OnExit);
    }
    private void OnEnter()
    {
        virtualCamera.enabled = true;
    }
    private void OnExit()
    {
        virtualCamera.enabled = false;
    }
}