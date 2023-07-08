using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Person
{
    bool triggered = false;
    GameObject chasingAfter;
    protected override void NpcBehavior()
    {
        if(triggered)
        {
            base.NpcBehavior();
        }
    }
    public void TriggerGuard(GameObject criminal)
    {
        chasingAfter = criminal;
        triggered = true;
    }
}
