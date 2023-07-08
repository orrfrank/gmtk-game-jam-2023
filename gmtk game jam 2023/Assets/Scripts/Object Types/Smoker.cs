using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : Person
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("flammable"))
        {
            Debug.Log("burn by smoke");
            BuildingManager.Instance.Burn();
        }
    }
    public override void ExitElevator()
    {
        base.ExitElevator();
        foreach (FloorInfo info in BuildingManager.Instance.floors)
        {
            if (info.num == floor)
            {
                if (!info.smokingAllowed && CopInFloor(floor))
                    borderCanada = 2;
                else
                    borderCanada = 13;
            }
        }
    }
    private bool CopInFloor(int floorNum)
    {
        foreach (Person person in BuildingManager.Instance.PeopleInFloor(floorNum))
        {
            if (person is Guard)
                return true;
        }
        return false;
    }
}
