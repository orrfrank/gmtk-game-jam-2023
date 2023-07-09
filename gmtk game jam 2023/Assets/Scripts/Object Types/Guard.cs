using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Person
{
    static public int stolenFloor = -1;
    static public GameObject thief = null;
    protected override void NpcBehavior()
    {
        if(stolenFloor != -1)
        {
            if (targetFloor == stolenFloor)
            {
                StartCharge(thief);
            }
            else
            {
                targetFloor = stolenFloor;
            }
            Debug.Log("guard trigger");
            base.NpcBehavior();
        }
    }
    public override void DoInUpdate()
    {

    }
}
