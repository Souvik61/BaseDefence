using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public class TankAIStateMachine : CStateMachine
    {

        public TankAIScript3 tankAIScript;

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
            stateDict.Add("APPR_BASE1", new ApproachingBaseState(this, tankAIScript));

            stateDict.Add("APPR_BASE", new NewApproachingBaseState(this, tankAIScript));
            stateDict.Add("ATTK_ENEM", new AttackingEnemyState(this, tankAIScript));

            stateDict.Add("REAC_BASE1", new ReachedBaseState(this, tankAIScript));

            stateDict.Add("REAC_BASE", new AttackingBaseState(this, tankAIScript));
            stateDict.Add("GAME_OVER", new GameOverState(this, tankAIScript));
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