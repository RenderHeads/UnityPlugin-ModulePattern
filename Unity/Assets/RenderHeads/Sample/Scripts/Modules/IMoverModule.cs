using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Tooling.Core.ModulePattern.Sample
{
    public interface IMoverModule : IModule
    {
        /// <summary>
        /// start moving objects
        /// </summary>
        void Start();

        /// <summary>
        /// stop moving objects
        /// </summary>
        void Stop();


        /// <summary>
        /// Returns if objects are moving (started = true)
        /// </summary>
        /// <returns> Returns if objects are moving (started = true)</returns>
        bool IsMoving();
    }
}
