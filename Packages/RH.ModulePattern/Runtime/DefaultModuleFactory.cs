﻿//(C) RenderHeads PTY LTD 2021
//Author: Ross Borchers

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RenderHeads.Tooling.Core.ModulePattern
{
	/// <summary>
	/// Default implementation of IModule factory that provides some convenience methods for finding modules in a scene / other scenes.
	/// </summary>
	public class DefaultModuleFactory : IModuleFactory
	{
		/// <summary>
		/// gets set to the build index when you pass a Scene into the constructor
		/// </summary>
		private readonly int _sceneIndex = -1;
		/// <summary>
		/// Caches scene.path in the constructor
		/// </summary>
		private readonly string _scenePath = "";

		/// <summary>
		/// Keeps track of all constructred ModuleFactories
		/// </summary>
		private static readonly  List<IModuleFactory> _all = new List<IModuleFactory>();

		/// <summary>
		/// Cache of all modules known by this module factory.
		/// </summary>
		private readonly List<IModule> _modules = new List<IModule>();

		/// <summary>
		/// Should be constructed when the scene is first loaded, before modules are initialized. The expected use case is to initialize the factory only once.
		/// </summary>
		/// <param name="scene">The scene the module factory belongs to. Used to sort and retrieve modules by scene.</param>
		public DefaultModuleFactory(Scene scene)
		{
			_sceneIndex = scene.buildIndex;
			_scenePath = scene.path;
			_all.Add(this);
		}

		public static IModuleFactory[] GetAll()
		{
			return _all.ToArray();
		}
		
		/// <summary>
		/// [Dangerous] Finds all modules of type in ALL registered default module factories across ALL SCENES. Only use for 'singleton' modules
		/// </summary>
		/// <typeparam name="T">The module you want to search for</typeparam>
		/// <returns>All modules matches the searched type, will return an empty list if there are no matches</returns>       
		public static List<T> FindInAll<T>() where T : IModule
		{
			List<T> found = new List<T>();
			foreach (IModuleFactory fact in _all)
			{
				if (fact.TryGetModule(out T mod))
				{
					found.Add(mod);
				}
			}
			return found;
		}

		/// <summary>
		/// Finds a module of type in ALL registered default module factories across ALL SCENES. Only use for 'singleton' modules.
		/// Consider using TryFindFactoryInScene(Scene scene, out IModuleFactory sceneFactory) instead.
		/// </summary>
		/// <typeparam name="T">The module you want to search for</typeparam>
		/// <param name="first">The first matching module in all module factories</param>
		/// <returns>true if it found something, otherwise false, in which case first will probably be null.</returns>
		public static bool TryFindFirstInAll<T>(out T first) where T : IModule
		{
			List<T> found = FindInAll<T>();
			if (found.Count > 0)
			{
				first = found[0];
				return true;
			}
			else
			{
				first = default;
				return false;
			}
		}

		/// <summary>
		/// Given a scene find the module factory in that scene. Useful for finding modules when there a are multiples of the same type, but only one per scene.
		/// </summary>
		/// <param name="scene">The scene we expect to find the module in.</param>
		/// <param name="sceneFactory">The found factory, if found. else null.</param>
		/// <returns>True if a factory was found in the scene. Else false and sceneFactory will be null.</returns>
		public static bool TryFindFactoryInScene(Scene scene, out IModuleFactory sceneFactory)
		{
			//if asset bundle scene check against scene path
			if (scene.buildIndex == -1)
			{
				foreach (DefaultModuleFactory factory in _all)
				{
					if (factory._scenePath == scene.path)
					{
						sceneFactory = factory;
						return true;
					}
				}
			}
			else //if scene included in build check against scene id
			{
				foreach (DefaultModuleFactory factory in _all)
				{
					if (factory._sceneIndex == scene.buildIndex)
					{
						sceneFactory = factory;
						return true;
					}
				}
			}

			sceneFactory = null;
			return false;
		}

		/// <summary>
		/// Try get a module in the factory. Useful when a MonoBehaviour needs access to a module arbitrarily. This should not be used by other modules.
		/// </summary>
		/// <typeparam name="T">The type of the module to find. If there are multiple of the same type the first one is returned.</typeparam>
		/// <param name="module">The module found. If any.</param>
		/// <returns>True if a module was found otherwise false.</returns>
		public bool TryGetModule<T>(out T module) where T : IModule
		{
			Debug.Assert(typeof(T).IsInterface, "TryGetModule<T> should only be called with an interface type");

			module = default;
			Type requestedInterfaceType = typeof(T);
			foreach (IModule mod in _modules)
			{
				Type modType = mod.GetType();
				if (requestedInterfaceType.IsAssignableFrom(modType))
				{
					module = (T)mod;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Add a module to the module factory. This should only be called when the factory is first created.
		/// </summary>
		/// <typeparam name="T">The type of the module.</typeparam>
		/// <param name="module">An instance of the module.</param>
		/// <returns>The same module as the parameter <paramref name="module"/>.</returns>
		public T AddModule<T>(T module) where T : IModule
		{
			foreach (IModule mod in _modules)
			{
				if (mod.GetType().Name == module.GetType().Name)
				{
					throw new InvalidOperationException("Module of type: " + module.GetType().Name + " already exists");
				}
			}

			_modules.Add(module);
			return module;
		}

		/// <summary>
		/// A factory method which creates Module of type T, and adds it to the factory. Only usable if T has a valid constructor with no parameters.
		/// </summary>
		/// <param name="args"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T CreateModule<T>() where T : IModule, new()
		{
			T module = (T)Activator.CreateInstance(typeof(T));
			AddModule(module);
			return module;
		}

		/// <summary>
		/// A factory method which creates Module of type T, and adds it to the factory.
		/// </summary>
		/// <param name="args"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[System.Obsolete("[IModuleFactory] Use AddModule instead, it now returns the added module, nullifying the advantage of this function, while maintaining type safety.")]
		public T CreateModule<T>(object[] args = null) where T : IModule
		{
			T module = (T)Activator.CreateInstance(typeof(T), args);
			AddModule(module);
			return module;
		}

		/// <summary>
		/// Returns all of the modules this factory has direct access to. Modules in other scenes/factories will not be returned.
		/// </summary>
		/// <returns></returns>
		public List<IModule> GetModules()
		{
			return _modules;
		}
	}
}