using UnityEngine;

namespace cmplx_statemachine
{
    public class ArtAIStateMachine_Lin : ArtAIStateMachine
    {
        public ArtileryControlBase artAIScript;//Target AI

        protected override void AddValidStates()
        {
            //Valid states
            AddState("IDLE", new AT_IDLE_State(this, artAIScript));//use
            AddState("ATTK", new AT_ATTK_State(this, artAIScript));//use
        }
    }
}