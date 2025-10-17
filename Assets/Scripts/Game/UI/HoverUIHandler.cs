using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using SS;
using TMPro;
using UnityEngine;

public class HoverUIHandler : MonoBehaviour
{
    private HoverUINaked[] dummys;
    private List<HoverUINaked> instances = new();
    
    [ShowNativeProperty] private int dummyCount => dummys.Length;
    [Button]
    private void OnValidate()
    {
        dummys = GetComponentsInChildren<HoverUINaked>();
    }
    private void Awake()
    {
        foreach (var dummy in dummys)
        {
            dummy.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        HoverHandler.Instance.BeginHoverEvent.AddListener(CheckBeginHover);
        HoverHandler.Instance.EndHoverEvent.AddListener(CheckEndHover);
    }
    private void OnDisable()
    {
        HoverHandler.Instance.BeginHoverEvent.RemoveListener(CheckBeginHover);
        HoverHandler.Instance.EndHoverEvent.RemoveListener(CheckEndHover);
    }
    private void CheckEndHover(HoverableMonoBehaviour behaviour)
    {
        for (var index = instances.Count - 1; index >= 0; index--)
        {
            var instance = instances[index];
            instances.RemoveAt(index);
            instance.transform.DOScale(0, .2f).SetEase(Ease.OutSine);
            Destroy(instance.gameObject, 3f);
        }
    }
    private void CheckBeginHover(HoverableMonoBehaviour behaviour)
    {
        foreach (var dummy in dummys)
        {
            if (dummy.CheckSourceTypeMatch(behaviour))
            {
                var instance = dummy.CreateInstance(behaviour);
                instance.transform.localScale = Vector3.zero;
                instance.gameObject.SetActive(true);
                instance.transform.DOScale(1, .2f).SetEase(Ease.OutBack);
                instances.Add(instance);
                break;
            }
        }
    }
}
