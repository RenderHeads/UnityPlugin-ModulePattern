// (C) RenderHeads PTY LTD
// Author: Shane Marks
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Tooling.Core.ModulePattern.Sample
{
    public class GameView : MonoBehaviour
    {
        ISpawnerModule _spawnerModule;
        IMoverModule _moverModule;

        [SerializeField] private Text _countLabel = null;
        [SerializeField] private Button _spawnButton = null;
        [SerializeField] private Text _spawnButtonText = null;
        [SerializeField] private Button _moverButton = null;
        [SerializeField] private Text _moveButtonText = null;

        public event Action OnSpawnerClick;

        public event Action OnMoverClick;

        void Start()
        {
            if (!DefaultModuleFactory.TryFindFirstInAll(out _spawnerModule))
            {
                throw new System.Exception("[GameView] Could not find SpawnerModule, was it declared in game manager Awake?");
            }

            if (!DefaultModuleFactory.TryFindFirstInAll(out _moverModule))
            {
                throw new System.Exception("[GameView] Could not find MoverModule, was it declared in game manager Awake?");
            }

            _spawnButton.onClick.AddListener(() => { OnSpawnerClick.Invoke(); });
            _moverButton.onClick.AddListener(() => { OnMoverClick.Invoke(); });
        }

       

        // Update is called once per frame
        void Update()
        {
            _countLabel.text = $"Total Spawned: {_spawnerModule.GetSpawnCount()}";
            _spawnButtonText.text = $"{(_spawnerModule.IsSpawning() ? "Stop" : "Start")} spawning";
            _moveButtonText.text = $"{(_moverModule.IsMoving() ? "Stop" : "Start")} moving";



        }
    }
}