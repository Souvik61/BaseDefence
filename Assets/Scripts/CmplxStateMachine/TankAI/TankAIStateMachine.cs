namespace cmplx_statemachine
{
    public class TankAIStateMachine : CStateMachine
    {

        public TankAIScript3 tankAIScript;//Target AI

        private void Awake()
        {
            //Valid states
            stateDict.Add("APPR_BASE1", new ApproachingBaseState(this, tankAIScript));

            stateDict.Add("APPR_BASE", new NewApproachingBaseState(this, tankAIScript));//use
            stateDict.Add("ATTK_ENEM", new AttackingEnemyState(this, tankAIScript));//use

            stateDict.Add("REAC_BASE1", new ReachedBaseState(this, tankAIScript));

            stateDict.Add("REAC_BASE", new AttackingBaseState(this, tankAIScript));//use
            stateDict.Add("GAME_OVER", new GameOverState(this, tankAIScript));//use
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}