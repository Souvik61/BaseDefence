using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{

    public class WT_ATTACKState : BaseState
    {

        WatchTowerAIScript towerAI;

        Transform selfTransform;
        Vector2 dirToTarget;

        public WT_ATTACKState(CStateMachine stM, WatchTowerAIScript tAI) : base(stM)
        {
            stateName = "ATTK";
            towerAI = tAI;
            selfTransform = tAI.GetComponent<Transform>();
        }

        public override void OnEnter()
        {
            //Empty bro!
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (towerAI.enemiesInSight.Count > 0)//If there are enemies
            {
                Transform currTarget = towerAI.enemiesInSight[0].transform;
                dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
                
                TryShoot();
            }
        }

        void TryShoot()
        {
            //Shoot!
        }
    }
}