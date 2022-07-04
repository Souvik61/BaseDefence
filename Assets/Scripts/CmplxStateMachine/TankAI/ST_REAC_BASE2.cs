using UnityEngine;

namespace cmplx_statemachine
{
    public class ST_REAC_BASE2 : ST_REAC_BASE
    {
        enum Substate { ATTK_CC, ATTK_ART };
        Substate substate;

        public ST_REAC_BASE2(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM,tankAIScript)
        {
      
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            AnalyzeSituation();

            AttackLogic();

            CheckForTransition();
        }

        void AnalyzeSituation()
        {
            if (tankAI.enemiesInSight.Count > 0)
            {
                substate = Substate.ATTK_ART;
            }
            else
                substate = Substate.ATTK_CC;
        }

        protected override void AttackLogic()
        {
            if (substate == Substate.ATTK_CC)
            { CCAttackLogic(); }
            else
            {
                ArtAttackLogic();
            }
        }

        void CCAttackLogic()
        {
            if (substate == Substate.ATTK_CC)
            {
                Transform target = CCTransform;
                dirToTarget = (target.position - selfTransform.position).normalized;

                TryFaceMuzzleTowardsDirection(dirToTarget);

                if (IsMuzzleFacingTowardDir(dirToTarget, 3))
                {
                    tankController.Shoot();
                }
            }
        }

        void ArtAttackLogic()
        {
            if (tankAI.enemiesInSight.Count > 0)
            {
                Transform target = tankAI.enemiesInSight[0].transform;
                dirToTarget = (target.position - selfTransform.position).normalized;

                TryFaceMuzzleTowardsDirection(dirToTarget);

                if (IsMuzzleFacingTowardDir(dirToTarget, 3))
                {
                    tankController.Shoot();
                }
            }
        }

        void CheckForTransition()
        {




        }

    }
}