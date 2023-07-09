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
    public float velocity = 3;
    //INTERNAL VARIABLES
    Rigidbody2D rb;
    bool isWaitingForElevator = false;
    bool isInElevator;
    [SerializeField] protected Vector2 targetPosition;
    public int borderMexico = 1;
    public int borderCanada = 13;
    int direction = 1;
    public bool isGroupOwner;
    [SerializeField] bool isInGroup;
    List<Person> groupMembers;
    [SerializeField] Person groupOwner;
    List<int> desireableFloors;
    public GameObject chargingTarget;
    // Start is called before the first frame update
    void Start()
    {

        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        floor = (int)Mathf.Round(targetPosition.y);
        rb = GetComponent<Rigidbody2D>();
        BuildingManager.Instance.AddPersonToFloor(this, floor);
        CheckForGroupers();
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
        if (targetPosition.x > borderCanada)
            direction = -1;
        else if (targetPosition.x < borderMexico)
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

    public virtual void ExitElevator()
    {
        Debug.Log((int)Mathf.Round(targetPosition.y) == targetFloor);
        isInElevator = false;
        StopFollowing(ElevatorController.Instance.transform);
        Debug.Log((int)Mathf.Round(targetPosition.y));
        BuildingManager.Instance.AddPersonToFloor(this, (int)Mathf.Round(targetPosition.y));
        CheckForGroupers();
        if (isGroupOwner && !ExitConditionMet())
            RequestEnterElevator();
    }
    public virtual bool ExitConditionMet()
    {
        if (!isGroupOwner)
        {
            //Debug.Log(groupMembers.Count);
            //Debug.Log((int)Mathf.Round(targetPosition.y) == targetFloor);
            Debug.Log(targetFloor);
            return (int)Mathf.Round(targetPosition.y) == targetFloor;
        }
        Debug.Log("group owner part");
        bool exit = false;
        try
        {
            foreach (Person member in groupMembers)
            {
                if (member != this)
                    exit |= member.ExitConditionMet();
            }
        }
        catch { Debug.Log("error lol"); }
        
        return exit;
    }    


    //====================================
    //        GROUPING UP
    //====================================
    //the code bellow is responsible for the grouping up
    //the idea is to have a group leader and for everyone in the group to follow him
    public void StartCharge(GameObject target)
    {
        yield return new WaitUntil(chargingTarget.transform.position.x - transform.position.x > 0);
        velocity = 4;
        chargingTarget = target;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (chargingTarget != null)
        {
            if (chargingTarget.transform.position.x - transform.position.x < 0.1)
            {
                (chargingTarget.GetComponent("Person") as MonoBehaviour).enabled = false;
                chargingTarget.transform.position = new Vector3(transform.position.x+0.2f, transform.position.y+0.1f, transform.position.z);
            }
        }
    }


    void CheckForGroupers()
    {

        if (shirtColor > 2)
            return;
        if (!isGroupOwner && isInGroup)
            return;
        Debug.Log("shirt color: " + shirtColor);
        List<Person> people = BuildingManager.Instance.PeopleInFloor((int)Mathf.Round(targetPosition.y));
        foreach (Person person in people )
        {
            if (person.shirtColor == shirtColor  && person != this)
            {
                if (isGroupOwner)
                {
                    TransferOwnership(person);

                }
                person.AddToGroup(this);
                groupOwner = person;
                isInGroup = true;
                return;
            }
        }
        CreateGroup();
    }
    public void CreateGroup()
    {

        isGroupOwner = true;
        isInGroup = true;
        groupMembers = new List<Person>();
        groupMembers.Add(this);
        desireableFloors = new List<int>();
        if (floor != targetFloor)
            desireableFloors.Add(targetFloor);

    }
    public void AddToGroup(Person member)
    {

        groupMembers.Add(member);
        member.Follow(this.transform);
        isInGroup = true;
        if (!isWaitingForElevator)
        {
            foreach (Person member2 in groupMembers)
            {
                if (!member2.ExitConditionMet())
                {
                    RequestEnterElevator();
                }
                    
            }
        }
    }
    public int ExtraPassengers()
    {

        if (isInGroup && !isGroupOwner)
        {
            Debug.Log($"Extra In Group {groupOwner.ExtraPassengers()}");
            return groupOwner.ExtraPassengers();
        }
        try
        {
            return groupMembers.Count;
        }
        catch
        {
            
            return 0;
        }
    }
    public void AbandonGroup()
    {

        if (shirtColor > 2)
            return;

        StopFollowing();
        try
        {
            groupOwner.groupMembers.Remove(this);

        }
        catch
        { }
        groupOwner = null;
        isInGroup = false;
        
    }
    public void TransferOwnership(Person newOwner)
    {

        if (shirtColor > 2)
            return;

        isGroupOwner = false;
        foreach (Person member in groupMembers)
        {
            member.AbandonGroup();
            newOwner.AddToGroup(member);

        }
    }
    public void RequestGroupElevatorEntrence()
    {


        groupOwner.RequestEnterElevator();
    }
}
