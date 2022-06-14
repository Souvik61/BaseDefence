using UnityEngine;

namespace cmplx_statemachine
{
    /// <summary>
    /// Artilery attack state
    /// </summary>
    public class AT_ATTK_State : BaseState
    {
        ArtileryAIScript artAIScript;
        ArtileryController artController;
        Transform selfTransform;
        Vector2 dirToTarget;

        public AT_ATTK_State(ArtAIStateMachine sTM, ArtileryAIScript artAI) : base(sTM)
        {
            stateName = "ATTK";
            artAIScript = artAI;
            artController = artAI.GetComponent<ArtileryController>();
            selfTransform = artAI.transform;
        }

        public override void OnEnter()
        {
            //Empty
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (artAIScript.enemiesInSight.Count > 0)//If there are enemies
            {
                Transform currTarget = artAIScript.enemiesInSight[0].transform;
                dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
                TryFaceTowardsDirection();
                TryShoot();
            }

            CheckStateTransition();

        }

        void TryShoot()
        {
            if (IsFacingTarget(artAIScript.enemiesInSight[0].transform))
            {
                artController.Shoot();
            }

        }

        void TryFaceTowardsDirection()
        {
            float angle = Vector2.SignedAngle(dirToTarget, artController.muzzleTransform.up);
            if (Mathf.Abs(angle) > 2)
            {
                if (angle > 0)
                { artController.MuzzleRotate(1); }
                else { artController.MuzzleRotate(-1); }
            }
        }

        bool IsFacingTarget(Transform targTrans)
        {
            Vector2 dirToT = (targTrans.position - selfTransform.position).normalized;
            float angle = Vector2.Angle(dirToT, artController.muzzleTransform.up);
            if (angle < 2)
            { return true; }
            return false;
        }

        void CheckStateTransition()
        {
            if (artAIScript.enemiesInSight.Count == 0)
            {
                stateMachineInstance.ChangeState("IDLE");
            }
        }
    }

}
