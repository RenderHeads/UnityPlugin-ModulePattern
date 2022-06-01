// (C) RenderHeads PTY LTD
// Author: Shane Marks

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Tooling.Core.ModulePattern.Sample
{
    public class ConsistentSpawnModule : ISpawnerModule
    {

        /// <summary>
        /// Reference to gameobject to spawn
        /// </summary>
        private GameObject _toSpawn;


        /// <summary>
        /// The last time a game object was spawned, will update when a new object to spawned
        /// </summary>
        private DateTime _lastSpawnTime;

        /// <summary>
        /// How long to wait between spawns
        /// </summary>
        private int _spawnDistanceInSeconds = 1;

        /// <summary>
        /// Will only spawn objects if this is true
        /// </summary>
        private bool _canSpawn = false;

        /// <summary>
        /// A list of spawned objects
        /// </summary>
        private List<GameObject> _spawnedObjects;

        /// <summary>
        /// Constructor for our module
        /// </summary>
        /// <param name="toSpawn">The object to spawn</param>
        /// <param name="spawnImmediately">If true set canSpawn = true so it starts spawning as soon as its constructred</param>
        public ConsistentSpawnModule(GameObject toSpawn, bool spawnImmediately)
        {
            _toSpawn = toSpawn;
            _lastSpawnTime = DateTime.Now;
            _spawnedObjects = new List<GameObject>();
            _canSpawn = spawnImmediately;
        }

        public int GetSpawnCount()
        {
            return _spawnedObjects.Count;
        }

        public List<GameObject> GetSpawnedObjects()
        {
            return _spawnedObjects;
        }

        public bool IsSpawning()
        {
            return _canSpawn;
        }

        public void Start()
        {
            _canSpawn = true;
            _lastSpawnTime = DateTime.Now;
        }

        public void Stop()
        {
            _canSpawn = false;
        }

        public void UpdateModule(float? delta)
        {
            if (!_canSpawn)
            {
                return;
            }
            if (_lastSpawnTime.AddSeconds(_spawnDistanceInSeconds) < DateTime.Now)
            {
                _spawnedObjects.Add(GameObject.Instantiate(_toSpawn, new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)), Quaternion.identity));
                _lastSpawnTime = DateTime.Now;
            }
        }
    }
}