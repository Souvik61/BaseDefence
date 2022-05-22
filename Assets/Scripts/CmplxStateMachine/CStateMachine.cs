using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{

    public abstract class StateMachine
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
}
