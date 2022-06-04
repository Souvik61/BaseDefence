

namespace cmplx_statemachine
{
    public class TNK_NO_TARG_State : BaseState
    {
        TankAIScript3 tankAIScript;
        public TNK_NO_TARG_State(TankAIStateMachine sTM, TankAIScript3 tankAI) : base(sTM)
        {
            stateName = "NO_TARG";
            tankAIScript = tankAI;
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CheckStateTransition();
        }

        void CheckStateTransition()
        {
            if (tankAIScript.targetBase != null)
                stateMachineInstance.ChangeState("APPR_BASE");
        }

    }
}