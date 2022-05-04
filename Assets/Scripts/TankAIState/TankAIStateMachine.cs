using UnityEngine;

public class TankAIStateMachine : StateMachine
{
    public TankAIStateMachine(TankAIScript_pt1 tankAIScript)
    {
        stateDict.Add("NO_TARG", new NoTargetState(this, tankAIScript));
        stateDict.Add("APPR_BASE", new ApproachingBaseAndMuzzleAimState(this, tankAIScript));
        stateDict.Add("ATTK_ENEM", new AttackingTroopsState(this, tankAIScript));
        stateDict.Add("ARRIV_BASE", new ArrivedAtTargetBase(this, tankAIScript));
        stateDict.Add("GAME_OVR", new GameOverState(this));
    }
}
