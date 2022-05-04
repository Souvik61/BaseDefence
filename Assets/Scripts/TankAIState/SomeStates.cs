using UnityEngine;

//Tank behavoiur states

/// <summary>
/// Approaching base state
/// </summary>
public class ApproachingBaseState : State
{
    protected TankAIScript tankAIScript;
    protected TankScript tankController;
    protected Transform selfTransform;

    float targetDistanceTolerance = 7;
    Vector2 dirToTarget;
    ArmyBaseScript targetBase;


    public ApproachingBaseState(StateMachine machine, TankAIScript tankAIScript)
    {
        name = "APPR_BASE";
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<TankScript>();
        targetBase = tankAIScript.targetBase;
    }
 
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        DriveTank();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    bool DriveTank()
    {
        if (targetBase != null)
        {
            bool isMoving = false;
            float distanceToTarget = Vector2.Distance(selfTransform.position, targetBase.transform.position);

            //If target too far
            if (distanceToTarget > targetDistanceTolerance)
            {
                dirToTarget = (targetBase.transform.position - selfTransform.position).normalized;
                float dotProd = Vector2.Dot(selfTransform.up, dirToTarget);

                //move forward?
                if (dotProd > 0)
                {
                    tankController.Move(1, 0);
                }
                //move backward?
                else
                {
                    if (distanceToTarget > 2.5f)
                    {
                        //Too far to reverse
                        tankController.Move(1, 0);
                    }
                    else
                    {
                        tankController.Move(-1, 0);
                    }
                }

                isMoving = true;
            }
            else//Reached target
            {
            }
            return isMoving;
        }
        return false;
    }
    
}

/// <summary>
/// Muzzle aim state
/// </summary>
public class MuzzleAimState : State
{
    protected TankAIScript tankAIScript;
    TankScript tankController;
    Transform selfTransform;
    Vector2 faceDir, dirToTarget;

    public MuzzleAimState(StateMachine machine, TankAIScript tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<TankScript>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Transform currTarget = tankAIScript.targetBase.transform;
        dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
        TryFaceTowardsDirection();
    }

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
        if (angle > 0)
        { tankController.MuzzleRotate(1); }
        else { tankController.MuzzleRotate(-1); }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}

public class ApproachingBaseAndMuzzleAimState : ApproachingBaseState
{
    Vector2 faceDir, dirToTarget;
    Transform targetBaseTransform;
    
    public ApproachingBaseAndMuzzleAimState(StateMachine stateMachine, TankAIScript tankAIScript) : base(stateMachine, tankAIScript)
    {
        targetBaseTransform = tankAIScript.targetBase != null ? tankAIScript.targetBase.transform : null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        dirToTarget = (targetBaseTransform.position - selfTransform.position).normalized;
        TryFaceTowardsDirection();

        CheckForEnemy();

        CheckDistance();
    }

    void CheckForEnemy()
    {
        if (tankAIScript.enemiesInSight.Count > 0)
        {
            stateMachineInstance.ChangeState("ATTK_ENEM");
        }
    
    }

    void CheckDistance()
    {
        if (Vector2.Distance(selfTransform.position, targetBaseTransform.position) < 8)
        {
            //reached target base 
            stateMachineInstance.ChangeState("ARRIV_BASE");
        
        }
    
    }

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
        if (Mathf.Abs(angle) > 7)
        {
            if (angle > 0)
            { tankController.MuzzleRotate(1); }
            else { tankController.MuzzleRotate(-1); }
        }
    }

}

/// <summary>
/// Attacking enemy tanks
/// </summary>
public class AttackingTroopsState : State
{
    private TankAIScript tankAIScript;
    TankScript tankController;
    Transform selfTransform;
    Vector2 faceDir, dirToTarget;

    public AttackingTroopsState(StateMachine machine, TankAIScript tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<TankScript>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (tankAIScript.enemiesInSight.Count > 0)//If there are enemies
        {
            Transform currTarget = tankAIScript.enemiesInSight[0].transform;
            dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
            TryFaceTowardsDirection();
            TryShoot();
        }
        else
        {
            if (tankAIScript.targetBase == null)
            {
                stateMachineInstance.ChangeState("NO_TARG");
            }
            else
            {
                stateMachineInstance.ChangeState("APPR_BASE");
            }
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

    public override void OnExit()
    {
        base.OnExit();
    }

}

/// <summary>
/// Arrived at base state
/// </summary>
public class ArrivedAtTargetBase : State
{
    TankAIScript tankAIScript;
    Transform selfTransform;
    TankScript tankController;
    Vector2 dirToTarget;

    public ArrivedAtTargetBase(StateMachine machine, TankAIScript tankAIScript)
    {
        stateMachineInstance = machine;
        this.tankAIScript = tankAIScript;
        selfTransform = tankAIScript.transform;
        tankController = tankAIScript.GetComponent<TankScript>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Transform currTarget = tankAIScript.targetBase.transform;
        dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
        TryFaceTowardsDirection();
        TryShootAtBase();

        if (tankAIScript.targetBase.isDestroyed)
        {
            stateMachineInstance.ChangeState("GAME_OVR");
        }
    }

    void TryShootAtBase()
    {
        if (IsFacingTarget(tankAIScript.targetBase.transform))
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

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
        if (angle > 0)
        { tankController.MuzzleRotate(1); }
        else { tankController.MuzzleRotate(-1); }
    }

    public override void OnExit()
    {
        base.OnExit();
    }


}

public class GameOverState : State
{
    public GameOverState(TankAIStateMachine stateMachine)
    { }

}

public class DestroyedState :State
{
   

}

public class NoTargetState : State
{
    TankAIScript tankAIScript;
    public NoTargetState(StateMachine machine, TankAIScript tankScript)
    {
        stateMachineInstance = machine;
        tankAIScript = tankScript; 
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        CheckForBase();
        CheckForEnemy();
    }

    void CheckForBase()
    {
        if (tankAIScript.targetBase != null)
            stateMachineInstance.ChangeState("APPR_BASE");
    
    }

    void CheckForEnemy()
    {
        if (tankAIScript.enemiesInSight.Count > 0)
        {
            stateMachineInstance.ChangeState("ATTK_ENEM");
        }

    }

}
