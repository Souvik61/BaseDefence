using UnityEngine;

namespace cmplx_statemachine
{
    public class ArtAIStateMachine_Lin : ArtAIStateMachine
    {
        //public ArtileryAIScript artAIScript;//Target AI

        protected override void AddValidStates()
        {
            //Valid states
            AddState("IDLE", new AT_IDLE_State(this, artAIScript));//use
            AddState("ATTK", new ST_ART_ATTK_LIN(this, artAIScript));//use
        }
    }
}