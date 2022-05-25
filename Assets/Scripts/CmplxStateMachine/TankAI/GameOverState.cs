using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public class GameOverState : BaseState
    {

        public GameOverState(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "GAME_OVER";
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}