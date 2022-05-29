using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine {

    public class WT_IDLEState : BaseState
    {
        public WT_IDLEState(CStateMachine stM) : base(stM)
        {
            stateName = "IDLE";
        }

        public override void OnEnter()
        {
            //Empty bro!
        }
    }
}