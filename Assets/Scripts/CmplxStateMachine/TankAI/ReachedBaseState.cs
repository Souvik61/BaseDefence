using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public class ReachedBaseState : BaseState
    {
        Transform selfTransform;
        TankScript tankController;
        ArmyBaseScript targetBase;
        Vector2 dirToTarget;

        public ReachedBaseState(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "REAC_BASE";
            targetBase= tankAIScript.targetBase;
            selfTransform = tankAIScript.transform;
            tankController = tankAIScript.GetComponent<TankScript>();
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            Vector2 dirToTarget = (targetBase.transform.position - selfTransform.position).normalized;
            
            TryFaceMuzzleTowardsDirection(dirToTarget);

            TryShootAtBase();

        }

        void TryShootAtBase()
        {
            if (IsMuzzleFacingTarget(targetBase.transform))
            {
                tankController.Shoot();
            }

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

        void TryFaceMuzzleTowardsDirection(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, tankController.muzzleTransform.up);
            if (Mathf.Abs(angle) > 2)
            {
                if (angle > 0)
                { tankController.MuzzleRotate(1); }
                else { tankController.MuzzleRotate(-1); }
            }
        }

        bool IsMuzzleFacingTarget(Transform target)
        {
            Vector2 dirToT = (target.position - selfTransform.position).normalized;
            float angle = Vector2.SignedAngle(dirToT, tankController.muzzleTransform.up);

            return Mathf.Abs(angle) <= 2;
        }
    }
}