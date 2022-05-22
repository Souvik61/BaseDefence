using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public abstract class BaseState
    {
        public string stateName;
        public cmplx_statemachine.StateMachine stateMachineInstance;

        public BaseState(StateMachine stateMachine)
        {
            stateMachineInstance = stateMachine;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnPhysicsUpdate();

        public abstract void OnExit();
    }
}