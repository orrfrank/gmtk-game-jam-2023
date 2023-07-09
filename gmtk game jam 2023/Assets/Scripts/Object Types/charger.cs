using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charger : Person
{
    public override void DoInUpdate()
    {
        Person potentialCharge = null;
        int enemyShirt = 1 - shirtColor; //0 if 1, 1 if 0
        foreach(Person obj in BuildingManager.Instance.PeopleInFloor((int)Mathf.Round(transform.position.y))) {
            if (obj.shirtColor == enemyShirt)
            {
                if (potentialCharge != null)
                {
                    potentialCharge = ClosestObj(potentialCharge, obj);
                }
                else
                {
                    potentialCharge = obj;
                }
            }
        }
        StartCharge(potentialCharge.gameObject);
    }
    private Person ClosestObj(Person obj1, Person obj2) {
        if (Mathf.Round(obj1.transform.position.x - transform.position.x) < Mathf.Round(obj2.transform.position.x - transform.position.x))
        {
            return obj1;
        }
        return obj2;
    }
}
