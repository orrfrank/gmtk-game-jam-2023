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
    public Person InitialMember1 { get => InitialMember;}


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
            ElevatorController.Instance.AddToEnetrenceQueue(this);
            InitialMember.FollowX(ElevatorController.Instance.AssignWaitingPositions(floor));
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
            Debug.Log("Merging....");
            Person p = other.groupMembers[i];
            p.SetGroup(this);
            this.groupMembers.Add(p);
            p.Follow(InitialMember.transform);
        }
        if (other.isWaitingForElevator)
        {
            if (!isWaitingForElevator)
            {
                this.isWaitingForElevator = true;
                ElevatorController.Instance.ReplaceGroupInQueue(other, this, floor);
            }
            else
            {
                ElevatorController.Instance.RemoveFromEntrenceQueue(other);
            }
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
            {
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
        InitialMember.StopFollowingX();
        InitialMember.Follow(ElevatorController.Instance.transform);
        isWaitingForElevator = false;
        BuildingManager.Instance.RemoveGroupFromFloor(this, floor);
    }
    public void ExitElevator()
    {
        BuildingManager.Instance.AddGroupToFloor(this, floor);
        CheckForGroupUp();
        CheckIfElevatorNeeded();
        InitialMember.StopFollowing();
        foreach (Person member in groupMembers)
        {
            if (member != InitialMember)
                member.Follow(InitialMember.transform);
        }
    }
    private void RequestElevatorEntrence()
    {
        ElevatorController.Instance.AddToEnetrenceQueue(this);
    }
    public List<Person> GetMembers()
    {
        return new List<Person>(groupMembers);
    }
    public void EveryoneFollowD(Transform member)
    {
        foreach (Person person in groupMembers)
        {
            if (member != person.transform)
                person.Follow(member.transform);
        }
    }
}
