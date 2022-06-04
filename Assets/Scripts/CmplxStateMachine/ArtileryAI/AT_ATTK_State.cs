using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    /// <summary>
    /// Artilery attack state
    /// </summary>
    public class AT_ATTK_State : BaseState
    {
        ArtileryAIScript artAIScript;
        ArtileryController artControl;
        Transform selfTransform;
        Vector2 dirToTarget;

        public AT_ATTK_State(ArtAIStateMachine sTM, ArtileryAIScript artAI) : base(sTM)
        {
            stateName = "ATTK";
            artAIScript = artAI;
            artControl = artAI.GetComponent<ArtileryController>();
        }

        public override void OnEnter()
        {
            //Empty
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (artAIScript.enemiesInSight.Count > 0)//If there are enemies
            {
                Transform currTarget = artAIScript.enemiesInSight[0].transform;
                dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
                TryFaceTowardsDirection();
                TryShoot();
            }

            CheckStateTransition();

        }

        void TryShoot()
        {
        
        
        }

        void TryFaceTowardsDirection()
        { 
            
        
        }

        void CheckStateTransition()
        {
            if (artAIScript.enemiesInSight.Count == 0)
            {
                stateMachineInstance.ChangeState("IDLE");
            }
        }
    }

}
