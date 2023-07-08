using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Person : MonoBehaviour
{
    public float weight;
    public int shirtColor;
    public bool gender;
    public bool hair;
    public int floor = 0;
    public int targetFloor = 1;
    public Transform following = null;
    public float velocity;
    //INTERNAL VARIABLES
    Rigidbody2D rb;
    bool isWaitingForElevator = false;
    bool isInElevator;
    [SerializeField] protected Vector2 targetPosition;
    const int BORDER_MEXICO = 1;
    const int BORDER_CANADA = 13;
    int direction = 1;
    public bool isGroupOwner;
    bool isInGroup;
    List<Person> groupMembers;
    Person groupOwner;
    List<int> desireableFloors;
    // Start is called before the first frame update
    void Start()
    {
        floor = (int)Mathf.Round(targetPosition.y);
        BuildingManager.Instance.AddPersonToFloor(this, floor);
        CheckForGroupers();
        rb = GetComponent<Rigidbody2D>();
        if (!ExitConditionMet())
        {
            RequestEnterElevator();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        ManageVelocity();
        if (following != null) { targetPosition = following.position; }
        else { NpcBehavior(); }
        floor = (int)Mathf.Round( targetPosition.y);
        rb.position = targetPosition;
    }
    protected virtual void ManageVelocity()
    {
        if (targetPosition.x > BORDER_CANADA)
            direction = -1;
        else if (targetPosition.x < BORDER_MEXICO)
            direction = 1;
    }
    protected virtual void NpcBehavior()
    {
        targetPosition = targetPosition + new Vector2(velocity * Time.deltaTime * direction, 0);
    }
    public void Follow(Transform follow)
    {
        following = follow.transform;
    }
    public void StopFollowing(Transform follow)
    {
        if (following == follow)
            following = null;
    }
    public void StopFollowing()
    {
        following = null;
    }
    public void RequestEnterElevator()
    {
        
        if (ElevatorController.Instance.elevatorAttributes.IsOpen)
            EnterElevator();
        else
        {
            ElevatorController.Instance.AddToEnetrenceQueue(this);
            isWaitingForElevator = true;
        }
    }
    public void EnterElevator()
    {
        if ((int)Mathf.Round(targetPosition.y) == floor)
        {
            Follow(ElevatorController.Instance.transform);
            isWaitingForElevator = false;
            isInElevator = true;
            BuildingManager.Instance.RemovePersonFromFloor(this, (int)Mathf.Round(targetPosition.y));

        }
    }

    public void ExitElevator()
    {
        Debug.Log((int)Mathf.Round(targetPosition.y) == targetFloor);
        isInElevator = false;
        StopFollowing(ElevatorController.Instance.transform);
        Debug.Log((int)Mathf.Round(targetPosition.y));
        BuildingManager.Instance.AddPersonToFloor(this, (int)Mathf.Round(targetPosition.y));
        CheckForGroupers();
    }
    public virtual bool ExitConditionMet()
    {
        return (int)Mathf.Round(targetPosition.y) == targetFloor;
    }    


    //====================================
    //        GROUPING UP
    //====================================
    //the code bellow is responsible for the grouping up
    //the idea is to have a group leader and for everyone in the group to follow him

    void CheckForGroupers()
    {
        Debug.Log("CheckForGroupers");
        List<Person> people = BuildingManager.Instance.PeopleInFloor((int)Mathf.Round(targetPosition.y));
        Debug.Log(people.Count);
        foreach (Person person in people )
        {
            if (person.shirtColor == shirtColor && shirtColor < 2 && person != this)
            {
                if (isGroupOwner)
                {
                    TransferOwnership(person);

                }
                person.AddToGroup(this);
                return;
            }
        }
        CreateGroup();
    }
    public void CreateGroup()
    {
        Debug.Log("CreateGroup");
        isGroupOwner = true;
        isInGroup = true;
        groupMembers = new List<Person>();
        desireableFloors = new List<int>();
        if (floor != targetFloor)
            desireableFloors.Add(targetFloor);

    }
    public void AddToGroup(Person member)
    {
        Debug.Log("AddToGroup");
        groupMembers.Add(member);
        member.Follow(this.transform);
        isInGroup = true;
    }
    public void AbandonGroup()
    {
        Debug.Log("AbandonGroup");
        StopFollowing();
        groupOwner = null;
        isInGroup = false;
    }
    public void TransferOwnership(Person newOwner)
    {
        Debug.Log("TransferOwnership");
        isGroupOwner = false;
        foreach (Person member in groupMembers)
        {
            member.AbandonGroup();
            newOwner.AddToGroup(member);

        }
    }
    public void RequestGroupElevatorEntrence(int floor)
    {

    }
}
