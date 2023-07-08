using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Group : MonoBehaviour
{
    List<Person> groupMembers = new List<Person>();
    [SerializeField] Person InitialMember;
    public int color;
    [SerializeField] private int floor;
    Vector2 groupPosition;
    public int Floor { get => floor;}

    private void Start()
    {
        groupMembers.Add(InitialMember);
        BuildingManager.Instance.AddGroupToFloor(this, floor);
    }
    public void Merge(Group other)
    {
        foreach (Person person in other.groupMembers) {
            person.SetGroup(this);
            groupMembers.Add(person);
        }
    }
    public bool AnyExitCondition()
    {
        foreach (Person person in groupMembers)
        {
            if (person.ExitConditionMet())
                return true;
        }
        return false;
    }
    public int GroupSize()
    {
        return groupMembers.Count;
    }
    public void SetFloor(int floor)
    {
        this.floor = floor;
        foreach (Person person in groupMembers)
        {
            person.SetFloor(floor);
        }
    }
    public void EnterElevator()
    {
        BuildingManager.Instance.RemoveGroupFromFloor(this, floor);
    }
    public void ExitElevator()
    {
        BuildingManager.Instance.AddGroupToFloor(this, floor);
    }
    private void RequestElevatorEntrence()
    {
        ElevatorController.Instance.AddToEnetrenceQueue(this);
    }
}
