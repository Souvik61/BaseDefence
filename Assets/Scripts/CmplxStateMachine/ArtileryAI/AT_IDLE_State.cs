using UnityEngine;

namespace cmplx_statemachine
{
    /// <summary>
    /// Artilery IDLE state do nothing
    /// </summary>
    public class AT_IDLE_State : BaseState
    {
        ArtileryAIScript artileryAI;

        public AT_IDLE_State(ArtAIStateMachine sTM, ArtileryAIScript artAI) : base(sTM)
        {
            stateName = "IDLE";
            artileryAI = artAI;
        }

        public override void OnEnter()
        {
            //Empty
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            CheckStateTransition();
        }

        void CheckStateTransition()
        {
            if (artileryAI.enemiesInSight.Count > 0)
                stateMachineInstance.ChangeState("ATTK");
            
        }
    }
}