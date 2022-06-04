using UnityEngine;

namespace cmplx_statemachine
{
    /// <summary>
    /// Artilery IDLE state do nothing
    /// </summary>
    public class AT_IDLE_State : BaseState
    {
        public AT_IDLE_State(ArtAIStateMachine sTM, ArtileryAIScript artAI) : base(sTM)
        {
            stateName = "IDLE";
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }
    }
}