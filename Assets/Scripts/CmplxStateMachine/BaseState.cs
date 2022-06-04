namespace cmplx_statemachine
{
    public abstract class BaseState
    {
        public string stateName;
        public cmplx_statemachine.CStateMachine stateMachineInstance;

        public BaseState(CStateMachine stateMachine)
        {
            stateMachineInstance = stateMachine;
        }

        public abstract void OnEnter();

        public virtual void OnUpdate() { }

        public virtual void OnPhysicsUpdate() { }

        public virtual void OnExit() { }
    }
}