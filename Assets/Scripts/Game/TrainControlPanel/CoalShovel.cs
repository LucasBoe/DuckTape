using UnityEngine;

public class CoalShovel : SectionSpecficUI
{
    [SerializeField] private GameObject ShovelEffect;
    public void ShovelCoal()
    {
        Instantiate(ShovelEffect);
        DriveHandler.Instance.Shovel();
    }
}
