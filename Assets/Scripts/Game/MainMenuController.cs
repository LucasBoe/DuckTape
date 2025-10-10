using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace SS
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] SceneContextDefinition playContext;

        [Button] private void LoadPlayContext ()
        {
            CustomSceneManager.Instance.ChangeScene(playContext, new());
        }
    }
}
