using UnityEngine;

namespace RenderHeads.Tooling.Core.ModulePattern.Sample
{
    public class MoverModule : IMoverModule
    {
        /// <summary>
        /// Reference to the spawner implementation we will use in this module
        /// </summary>
        private ISpawnerModule _spawnerModule;

        /// <summary>
        /// if true we will be allowed to move our spawned objects
        /// </summary>
        private bool _canMove = false;

        /// <summary>
        /// stores total deltatime passed while _canMove = true
        /// </summary>
        private float _delta;

        /// <summary>
        /// We put the Ispawner module in the constructor, because the mover module depends on it
        /// </summary>
        /// <param name="spawnerModule">An implementation of the spawner module</param>
        public MoverModule (ISpawnerModule spawnerModule)
        {
            _spawnerModule = spawnerModule;
        }

        public void Start()
        {
            _canMove = true;
        }

        public void Stop()
        {
            _canMove = false;
        }
        public void UpdateModule(float? delta)
        {
            if (!_canMove)
            {
                return;
            }

            _delta += Time.deltaTime;
            
            foreach (GameObject go in _spawnerModule.GetSpawnedObjects())
            {
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + Mathf.Sin(_delta)*0.1f, go.transform.position.z);
            }
        }
        public bool IsMoving()
        {
            return _canMove;
        }
    }
}