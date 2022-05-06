using UnityEngine;
using tank_ai_states;

public class TankAIStateMachine : StateMachine
{
    public TankAIStateMachine(TankAIScript tankAIScript)//State definitions
    {
        stateDict.Add("NO_TARG", new NoTargetState(this, tankAIScript));
        stateDict.Add("APPR_BASE", new ApproachingBaseAndMuzzleAimState(this, tankAIScript));
        stateDict.Add("ATTK_ENEM", new AttackingTroopsState(this, tankAIScript));
        stateDict.Add("ARRIV_BASE", new ArrivedAtTargetBase(this, tankAIScript));
        stateDict.Add("GAME_OVR", new GameOverState(this));
    }
}

public class TankAIStateMachine2 : StateMachine
{
    public TankAIStateMachine2(TankAIScript tankAIScript)//State definitions
    {
        stateDict.Add("NO_TARG", new NoTargetState(this, tankAIScript));
    }
}