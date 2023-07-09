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
    public Person InitialMember1 { get => InitialMember; }
    private void Start()
    {
        floor = (int)Mathf.Round(InitialMember.transform.position.y);
        groupMembers.Add(InitialMember);
        color = InitialMember.shirtColor;
        BuildingManager.Instance.AddGroupToFloor(this, floor);
        CheckForGroupUp();
        CheckIfElevatorNeeded();
        
    }
    private void Update()
    {
        
    }
    void CheckForGroupUp()
    {
        if (color >= 2)
            return;
        List<Group> groups = BuildingManager.Instance.GroupInFloor(floor);
        int count = groups.Count;
        for (int i = 0; i < count; i++)
        {
            Merge(groups[i]);
        }
    }
    private void CheckIfElevatorNeeded()
    {
        if (!AllExitConditionsMet() && !isWaitingForElevator)
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
            p.Follow(InitialMember.transform);
        }
        BuildingManager.Instance.RemoveGroupFromFloor(other, floor);
        ElevatorController.Instance.RemoveFromEntrenceQueue(other);
        Destroy(other);
        isWaitingForElevator = false;
        CheckIfElevatorNeeded();
    }
    public bool AnyExitCondition()
    {
        foreach (Person person in groupMembers)
        {
            if (person.ExitConditionMet())
            {
                Debug.Log("true");
                return true;
            }
        }
        Debug.Log("false");
        return false;
    }
    public bool AllExitConditionsMet()
    {
        bool ret = true;
        foreach (Person person in groupMembers)
        {
            ret &= person.ExitConditionMet();
        }
        return ret;
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
        CheckForGroupUp();
        CheckIfElevatorNeeded();
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
