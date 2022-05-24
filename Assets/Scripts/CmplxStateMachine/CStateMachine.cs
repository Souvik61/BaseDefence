using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{

    public abstract class CStateMachine : MonoBehaviour
    {
        protected BaseState currentState;
        public string currentStateName;

        /// <summary>
        /// Dictionary of all States permitted by this  state machine.
        /// </summary>
        protected Dictionary<string, cmplx_statemachine.BaseState> stateDict;


        bool initialized;

        public CStateMachine()
        {
            stateDict = new Dictionary<string, BaseState>();
        }

        /// <summary>
        /// State machine starts only after we call this method
        /// </summary>
        /// <param name="stateName"></param>
        public void Initialize(string stateName)
        {
            if (stateDict.ContainsKey(stateName))
            {
                currentStateName = stateName;
                currentState = stateDict[stateName];
                initialized = true;
                currentState.OnEnter();
            }
        }

        protected virtual void Start()
        {
            //currentState.OnEnter();
        }

        protected virtual void Update()
        {
            if (initialized)
                currentState.OnUpdate();
        }

        public void Exit()
        {
            currentState.OnExit();
        }


        public void ChangeState(BaseState newState)//Change state
        {
            if (currentState.GetType() != newState.GetType())
            {
                currentState.OnExit();
                currentState = newState;
                currentStateName = newState.stateName;
                newState.OnEnter();
            }
        }

        public void ChangeState(string stateName)
        {
            if (stateDict.ContainsKey(stateName) && stateName != currentStateName)
            {
                ChangeState(stateDict[stateName]);
                return;
            }
            Debug.Log("State does not exists");
        }

        public string GetCurrentStateName()
        { return currentStateName; }
    }
}
