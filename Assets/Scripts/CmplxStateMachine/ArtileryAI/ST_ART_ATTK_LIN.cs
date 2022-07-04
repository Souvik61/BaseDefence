using UnityEngine;

namespace cmplx_statemachine
{
    public class ST_ART_ATTK_LIN : BaseState
    {
        ArtileryAIScript artAIScript;
        ArtileryControlBase artController;
        Transform selfTransform;
        Vector2 dirToTarget;

        public ST_ART_ATTK_LIN(ArtAIStateMachine sTM, ArtileryAIScript artAI) : base(sTM)
        {
            stateName = "ATTK";
            artAIScript = artAI;
            artController = artAI.GetComponent<ArtileryControlBase>();
            selfTransform = artAI.transform;
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
                TryShoot();
            }

            CheckStateTransition();

        }

        void TryShoot()
        {
            artController.Shoot();
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
