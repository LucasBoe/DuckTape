using UnityEngine;

namespace SS
{
    public class CurrentContextProvider : MonoBehaviour
    {
        public SceneContextDefinition Current { get; private set; }
        internal void Define(SceneContextDefinition definition) => Current = definition;
    }
}
