using System.Collections;
using System.Collections.Generic;
namespace RenderHeads.Tooling.Core.ModulePattern
{
    /// <summary>
    /// Interface for a module factory. These are responsible for storing and potentially updating modules in a scene. You may want to create your own version to encapsulate module related behaviour but for the most part its ok to use <see cref="DefaultModuleFactory"/>.
    /// </summary>
	public interface IModuleFactory
	{
		/// <summary>
		/// Try get a module in the factory. Useful when a MonoBehaviour needs access to a module arbitrarily. This should not be used by other modules.
		/// </summary>
		/// <typeparam name="T">The type of the module to find. If there are multiple of the same type the first one is returned.</typeparam>
		/// <param name="module">The module found. If any.</param>
		/// <returns>True if a module was found otherwise false.</returns>
		bool TryGetModule<T>(out T module) where T : IModule;

		/// <summary>
		/// Add a module to the module factory. This should only be called when the factory is first created.
		/// </summary>
		/// <typeparam name="T">The type of the module.</typeparam>
		/// <param name="module">An instance of the module.</param>
		T AddModule<T>(T module) where T : IModule;


		/// <summary>
		/// A factory method which creates Module of type T, and adds it to the factory. Only usable if T has a valid constructor with no parameters.
		/// </summary>
		/// <param name="args"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T CreateModule<T>() where T : IModule, new();

	
	}
}