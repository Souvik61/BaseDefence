using UnityEngine;

namespace cmplx_statemachine
{
    public class AttackingEnemyState : BaseState
    {
        public TankAIScript3 tankAIScript;
        Transform selfTransform;
        TankScript tankController;
        Vector2 dirToTarget;

        public AttackingEnemyState(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "ATTK_ENEM";
            this.tankAIScript = tankAIScript;
            selfTransform = tankAIScript.transform;
            tankController = tankAIScript.GetComponent<TankScript>();
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            if (tankAIScript.enemiesInSight.Count > 0)//If there are enemies
            {
                Transform currTarget = tankAIScript.enemiesInSight[0].transform;
                dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
                TryFaceTowardsDirection();
                TryShoot();
            }

            CheckStateTransition();

        }

        void TryFaceTowardsDirection()
        {
            float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
            if (Mathf.Abs(angle) > 2)
            {
                if (angle > 0)
                { tankController.MuzzleRotate(1); }
                else { tankController.MuzzleRotate(-1); }
            }
        }

        void TryShoot()
        {
            if (IsFacingTarget(tankAIScript.enemiesInSight[0].transform))
            {
                tankController.Shoot();
            }
        }

        bool IsFacingTarget(Transform targTrans)
        {
            Vector2 dirToT = (targTrans.position - selfTransform.position).normalized;
            float angle = Vector2.Angle(dirToT, tankController.muzzleTransform.up);
            if (angle < 2)
            { return true; }
            return false;
        }

        void CheckStateTransition()
        {
            if (tankAIScript.enemiesInSight.Count == 0)
            {
                stateMachineInstance.ChangeState("APPR_BASE");
            }
        }

    }
}