namespace cmplx_statemachine
{
    public class TankAIStateMachine : CStateMachine
    {

        public TankAIScript3 tankAIScript;//Target AI

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

        void AddValidStates()
        {
            //Valid states
            //stateDict.Add("APPR_BASE1", new ApproachingBaseState(this, tankAIScript));

            AddState("NO_TARG", new TNK_NO_TARG_State(this, tankAIScript));//use
            AddState("APPR_BASE", new NewApproachingBaseState(this, tankAIScript));//use
            AddState("ATTK_ENEM", new AttackingEnemyState(this, tankAIScript));//use

            //AddState("REAC_BASE1", new ReachedBaseState(this, tankAIScript));

            AddState("REAC_BASE", new TNK_ATTK_NEAR(this, tankAIScript));//use

            //AddState("REAC_BASE", new AttackingBaseState(this, tankAIScript));//use
            AddState("GAME_OVER", new GameOverState(this, tankAIScript));//use

        }

    }
}