using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_PauseState : FSMState
{
    // Constructor
    public GM_PauseState()
    {
        stateType = FSMStateType.Pause;
    }

    public override void EnterStateInit()
    {
        Debug.Log("Pause State Entered.");
    }

    public override void Reason(Transform player, Transform gm)
    {

    }

    public override void Act(Transform player, Transform npc)
    {
        Time.timeScale = 0;
    }
}
