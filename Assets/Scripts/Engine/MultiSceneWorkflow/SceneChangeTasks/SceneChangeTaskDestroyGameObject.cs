using System.Collections;
using UnityEngine;

namespace SS
{
    public class SceneChangeTaskDestroyGameObject : ISceneChangeTask
    {
        private GameObject go;

        public SceneChangeTaskDestroyGameObject(GameObject _go)
        {
            go = _go;
        }

        public IEnumerator Execute(SceneChangeContext _context)
        {
            GameObject.Destroy(go);
            yield break;
        }
    }
}
