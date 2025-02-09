using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_PlayState : FSMState
{
    // Constructor
    public GM_PlayState()
    {
        stateType = FSMStateType.Play;
    }

    public override void EnterStateInit()
    {
        Debug.Log("Play State Entered.");
    }

    public override void Reason(Transform player, Transform gm)
    {

    }

    public override void Act(Transform player, Transform gm)
    {
        Time.timeScale = 1;
    }
}
