using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine {

    public class WT_IDLEState : BaseState
    {
        WatchTowerAIScript towerAIScript;
        public WT_IDLEState(CStateMachine stM, WatchTowerAIScript towerAI) : base(stM)
        {
            stateName = "IDLE";
            towerAIScript = towerAI;
        }

        public override void OnEnter()
        {
            //Empty bro!
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (towerAIScript.enemiesInSight.Count > 0)
                stateMachineInstance.ChangeState("ATTK");
        }
    }
}