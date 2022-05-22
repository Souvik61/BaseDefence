using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public abstract class BaseState
    {
        public string stateName;
        public cmplx_statemachine.CStateMachine stateMachineInstance;

        public BaseState(CStateMachine stateMachine)
        {
            stateMachineInstance = stateMachine;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnPhysicsUpdate();

        public abstract void OnExit();
    }
}