using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Group : MonoBehaviour
{
    List<Person> groupMembers = new List<Person>();
    [SerializeField] Person InitialMember;
    int color;
    [SerializeField] private int floor;
    Vector2 groupPosition;
    public int Floor { get => floor;}

    public bool isHappy;
    bool isWaitingForElevator;
    public bool IsWaitingForElevator { get => isWaitingForElevator;}
    private void Start()
    {
        floor = (int)Mathf.Round(InitialMember.transform.position.y);
        groupMembers.Add(InitialMember);
        color = InitialMember.shirtColor;
        BuildingManager.Instance.AddGroupToFloor(this, floor);
        //CheckIfElevatorNeeded();
        
    }
    private void Update()
    {
        //isHappy = AnyExitCondition();
        if (Input.GetKeyDown(KeyCode.G))
        {
            List<Group> groups = BuildingManager.Instance.GroupInFloor(floor);
            int count = groups.Count;
            for (int i = 0; i < count; i++)
            {
                Merge(groups[i]);
            }
        }
    }
    private void CheckIfElevatorNeeded()
    {
        if (!AnyExitCondition())
        {
            Debug.Log("entered elevator queue");
            ElevatorController.Instance.AddToEnetrenceQueue(this);
            isWaitingForElevator = true;
        }
            
    }
    public void Merge(Group other)
    {
        if (other == this || other == null)
            return;
        int count = other.groupMembers.Count;
        for (int i = 0; i < count; i++)
        {
            Person p = other.groupMembers[i];
            p.SetGroup(this);
            this.groupMembers.Add(p);
        }
        BuildingManager.Instance.RemoveGroupFromFloor(other, floor);
        ElevatorController.Instance.RemoveFromEntrenceQueue(other);
        Destroy(other);
        CheckIfElevatorNeeded();
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
        Debug.Log(groupMembers.Count);
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
        isWaitingForElevator = false;
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
    public List<Person> GetMembers()
    {
        return new List<Person>(groupMembers);
    }
}
