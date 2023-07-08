using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Thief : Person
{
    // Start is called before the first frame update
    public override bool ExitConditionMet()//exits if one of diamonds is on said floor
    {
        int thiefPos = (int)Mathf.Round(targetPosition.y);
        foreach (GameObject obj in BuildingManager.Instance.diamonds) 
        {
            if (thiefPos == (int)Mathf.Round(obj.transform.position.y))
            {
                return true;
            }
        }
        return false;
    }
}
