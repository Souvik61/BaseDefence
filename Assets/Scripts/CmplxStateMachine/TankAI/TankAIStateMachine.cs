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
            
        }

        private void OnDisable()
        {
            
        }

        private void Awake()
        {
            stateDict.Add("APPR_BASE", new ApproachingBaseState(this, tankAIScript));
            stateDict.Add("ATTK_ENEM", new AttackingEnemyState(this, tankAIScript));
            stateDict.Add("REAC_BASE", new ReachedBaseState(this, tankAIScript));
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

    }
}