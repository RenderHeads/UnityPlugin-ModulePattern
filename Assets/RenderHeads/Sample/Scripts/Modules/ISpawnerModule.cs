// (C) RenderHeads PTY LTD
// Author: Shane Marks;

using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Tooling.Core.ModulePattern.Sample
{
    public interface ISpawnerModule : IModule
    {
        /// <summary>
        /// Return total amount of spawn objects since the start of the game
        /// </summary>
        /// <returns>Return total amount of spawn objects since the start of the game</returns>
        int GetSpawnCount();

        /// <summary>
        /// Stop Spawning
        /// </summary>
        void Stop();

        /// <summary>
        /// Start spawning
        /// </summary>
        void Start();

        /// <summary>
        /// Returns a list of spawned objects
        /// </summary>
        /// <returns>Returns a list of spawned objects</returns>
        List<GameObject> GetSpawnedObjects();

        /// <summary>
        /// Returns if the spawner is spawning
        /// </summary>
        /// <returns>Returns if the spawner is spawning</returns>
        bool IsSpawning();
    }
}