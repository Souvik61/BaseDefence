using UnityEngine;

namespace cmplx_statemachine
{
    public class ArtAIStateMachine : CStateMachine
    {

        public ArtileryAIScript artAIScript;//Target AI

        private void Awake()
        {
            AddValidStates();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected virtual void AddValidStates()
        {
            //Valid states
            AddState("IDLE", new AT_IDLE_State(this, artAIScript));//use
            AddState("ATTK", new AT_ATTK_State(this, artAIScript));//use
        }

    }

}