using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public class ReachedBaseState : BaseState
    {

        public ReachedBaseState(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "REAC_BASE";
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}