using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Person
{
    public bool triggered = false;
    GameObject chasingAfter;
    protected override void NpcBehavior()
    {
        if(triggered)
        {
            Debug.Log("guard trigger");
            base.NpcBehavior();
        }
    }
    public void TriggerGuard(GameObject criminal)
    {
        triggered = false;
        StartCharge(criminal);
    }
}
