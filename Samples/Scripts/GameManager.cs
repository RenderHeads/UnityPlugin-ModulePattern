// (C) RenderHeads PTY LTD
// Author: Shane Marks

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Tooling.Core.ModulePattern.Sample
{
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// reference to our spawner module that will contain our spawning business logic
		/// </summary>        
		private ISpawnerModule _spawnerModule;

		/// <summary>
		/// reference to our mover module which will move our spawned objects
		/// </summary>
		private IMoverModule _moverModule;
		/// <summary>
		/// The object we will spawn
		/// </summary>
		[SerializeField]
		private GameObject _spawnPrefab = null;
		
		/// <summary>
		/// Reference to our game view to setup some callbacks
		/// </summary>
		[SerializeField]
		private GameView _gameView = null;
		
		/// <summary>
		/// We want to use AWAKE so our modules initialize before everything else.
		/// </summary>

		void Awake()
		{
			//Create a new defaultmodule factors and pass in the actice scene as a reference, so this particular module can be scoped to the scene
			DefaultModuleFactory _factory = new DefaultModuleFactory(gameObject.scene);

			//declare our spawner module for spawning objects in the scene, and set spawn immediately = true
			_spawnerModule = new ConsistentSpawnModule(_spawnPrefab, true);

			//declare our mover module for moving objects around
			_moverModule = new MoverModule(_spawnerModule);


			//add our modules to the factory so they can be accessed outside this scope.
			_factory.AddModule(_spawnerModule);
			_factory.AddModule(_moverModule);

			SetupViewCallbacks();
		}

		/// <summary>
		/// Setps the business logic for the buttons on the view to change game behaviour.
		/// </summary>
		private void SetupViewCallbacks()
		{
			_gameView.OnSpawnerClick += () =>
			{
				if (_spawnerModule.IsSpawning())
				{
					_spawnerModule.Stop();
				}
				else
				{
					_spawnerModule.Start();
				}
			};

			_gameView.OnMoverClick += () =>
			{
				if (_moverModule.IsMoving())
				{
					_moverModule.Stop();
				}
				else
				{
					_moverModule.Start();
				}
			};
		}

		/// <summary>
		///  we will manually call the UpdateModule for each module that we have defined, in the order we want, so we can be guarenteed of update order.
		/// </summary>
		void Update()
		{
			_spawnerModule.UpdateModule();
			//update our move module after the spawning has taken place
			_moverModule.UpdateModule();
		}
	}
}