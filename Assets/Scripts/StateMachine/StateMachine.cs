using System.Collections.Generic;

/// <summary>
/// Simulates a state machine, To use the state machine user needs to call its Start, Update, Exit functions
/// For concurrent states use inheritance :-)
/// </summary>
public class StateMachine
{
    protected State currentState;
    protected string currentStateName;
    /// <summary>
    /// Dictionary of all States permitted by this  state machine.
    /// </summary>
    protected Dictionary<string, State> stateDict;

    public StateMachine()
    {
        stateDict = new Dictionary<string, State>();
    }

    public void Initialize(string stateName)
    {
        if (stateDict.ContainsKey(stateName))
        {
            currentStateName = stateName;
            currentState = stateDict[stateName];
            currentState.OnEnter();
        }
    }

    public void Start()
    {

    }

    public void Update()
    {
        currentState.OnUpdate();
    }

    public void Exit()
    {
        currentState.OnExit();
    }

    /*
    public void AddState(State state)
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentStates[i] != null)
            {
                if (currentStates[i].GetType() == state.GetType())
                    return;//State type already exists return
            }
        }
        stackPtr = (stackPtr + 1) % 3;//loop values within 0-2
        currentStates[stackPtr] = state;
        state.OnEnter();
    }

    public void AddState(State state, int index)//Add State to the state stack,Index should be within 2
    {
        currentStates[index] = state;
        state.OnEnter();
    }

    */

    public void ChangeState(State newState)//Change state
    {
        if (currentState.GetType() != newState.GetType())
        {
            currentState.OnExit();
            currentState = newState;
            newState.OnEnter();
        }
    }

    public void ChangeState(string stateName)
    {
        if (stateDict.ContainsKey(stateName) && stateName != currentStateName)
        {
            currentState.OnExit();
            currentStateName = stateName;
            currentState = stateDict[stateName];
            currentState.OnEnter();
        }
    }

    public string GetCurrentStateName()
    { return currentStateName; }

}
