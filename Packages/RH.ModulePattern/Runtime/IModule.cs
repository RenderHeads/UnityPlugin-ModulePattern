using System.Collections;
using System.Collections.Generic;

namespace RenderHeads.Tooling.Core.ModulePattern
{
    /// <summary>
    /// Base interface for all modules, IModuleFactory requires modules inherit from this.
    /// General module usage pattern is:
    /// RAII. All initialization is done through the constructor. After a constructor is run the module should be fully usable.
    /// UpdateModules should be called at regular intervals. The order of module updates should be controlled by the caller to ensure all dependencies have already been updated.
    /// Pass in dependencies through the constructor IOC like.
    /// </summary>
	public interface IModule
    {/// <summary>
     /// Should be called at a regular interval. The order of module updates should be controlled by the caller to ensure all dependencies have already been updated.
     /// </summary>
     /// <param name="delta">Pass in a deltaTime calculated in parent update, if you want to pass that into underlying update event, so thay you don't need to query it within the method</param>
        void UpdateModule(float? delta = null);
    }
}
