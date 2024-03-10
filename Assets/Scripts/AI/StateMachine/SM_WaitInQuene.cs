using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_WaitInQuene : StateMachine
{
    public SM_WaitInQuene(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        //sm_name = "Wait "
    }
}
