using UnityEngine;

//-----------------------------------
//Artilery type 1 state machine
//-----------------------------------
public class ArtileryStateMachine : StateMachine
{
    public ArtileryStateMachine(ArtileryScript artScript)
    {
        stateDict.Add("IDLE", new ArtileryIdleState(this, artScript));
        stateDict.Add("ATTK", new ArtileryAttackingState(this, artScript));
        stateDict.Add("GAME_OVR", new ArtileryAttackingState(this, artScript));
    }

}

public class ArtileryIdleState : State
{
    ArtileryScript artScript;
    StateMachine stateMachine;
    public ArtileryIdleState(StateMachine artStateMachine, ArtileryScript artileryScript)
    { artScript = artileryScript; stateMachine = artStateMachine; }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (artScript.enemiesInSight.Count > 0)
        {
            stateMachine.ChangeState("ATTK");
        }

    }


}

public class ArtileryAttackingState : State
{
    ArtileryScript artScript;
    StateMachine stateMachine;
    public ArtileryAttackingState(StateMachine artStateMachine, ArtileryScript artileryScript)
    { artScript = artileryScript; stateMachine = artStateMachine; }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (artScript.enemiesInSight.Count > 0)
        {
            artScript.Shoot();
        }
        else
        {
            stateMachine.ChangeState("IDLE");
        }

    }

}

public class ArtileryGameOverState : State
{

}

//-----------------------------------
//Artilery type 2 state machine
//-----------------------------------
public class Artilery_t1StateMachine : StateMachine
{
    public Artilery_t1StateMachine(Artilery_t1Script artScript)
    {
        stateDict.Add("IDLE", new Artilery_t1IdleState(this, artScript));
        stateDict.Add("ATTK", new Artilery_t1AttackingState(this, artScript));
        stateDict.Add("GAME_OVR",new ArtileryGameOverState());
    }

}

public class Artilery_t1IdleState : State
{
    Artilery_t1Script artScript;
    StateMachine stateMachine;
    public Artilery_t1IdleState(StateMachine artStateMachine, Artilery_t1Script artileryScript)
    { artScript = artileryScript; stateMachine = artStateMachine; }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (artScript.enemiesInSight.Count > 0)
        {
            stateMachine.ChangeState("ATTK");
        }

    }
}

public class Artilery_t1AttackingState : State
{
    Artilery_t1Script artScript;
    Transform selfTransform;
    Vector2 dirToTarget;

    public Artilery_t1AttackingState(StateMachine artStateMachine, Artilery_t1Script artileryScript)
    { artScript = artileryScript; stateMachineInstance = artStateMachine;selfTransform = artileryScript.transform; }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (artScript.enemiesInSight.Count > 0)//If there are enemies
        {
            Transform currTarget = artScript.enemiesInSight[0].transform;
            dirToTarget = (currTarget.transform.position - selfTransform.position).normalized;
            TryFaceTowardsDirection();
            TryShoot();
        }
        else
        {
            stateMachineInstance.ChangeState("IDLE");
        }
    }

    void TryFaceTowardsDirection()
    {
        float angle = Vector2.SignedAngle(dirToTarget, artScript.muzzleTransform.up);
        if (Mathf.Abs(angle) > 2)
        {
            if (angle > 0)
            { artScript.MuzzleRotate(1); }
            else { artScript.MuzzleRotate(-1); }
        }
    }

    void TryShoot()
    {
        if (IsFacingTarget(artScript.enemiesInSight[0].transform))
        {
            artScript.Shoot();
        }

    }

    bool IsFacingTarget(Transform targTrans)
    {
        Vector2 dirToT = (targTrans.position - selfTransform.position).normalized;
        float angle = Vector2.Angle(dirToT, artScript.muzzleTransform.up);
        if (angle < 2)
        { return true; }
        return false;
    }


}