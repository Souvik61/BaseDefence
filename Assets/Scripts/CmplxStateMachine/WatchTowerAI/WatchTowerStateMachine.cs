using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine {
    public class WatchTowerStateMachine : CStateMachine
    {
        public WatchTowerAIScript towerAIScript;

        private void OnEnable()
        {
            AllEventsScript.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            AllEventsScript.OnGameOver -= OnGameOver;
        }

        private void Awake()
        {
            stateDict.Add("IDLE", new WT_IDLEState(this));
            stateDict.Add("ATTK", new WT_ATTACKState(this, towerAIScript));
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

        }

        void OnGameOver()
        {
            ChangeState("GAME_OVER");
        }
    }
}