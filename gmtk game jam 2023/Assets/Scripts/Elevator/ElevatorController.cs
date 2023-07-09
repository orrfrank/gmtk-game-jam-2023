using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorController : MonoBehaviour
{
    
    public ElevatorAttributes elevatorAttributes;
    Camera mainCamera;
    Vector2 mousePos;
    [SerializeField] public Transform startPos;
    public GameObject elevatorDoorObject;
    [SerializeField] private List<Group> peopleInElevator = new List<Group>();
    [SerializeField] private List<Group>[] elevatorEntrenceQueue = new List<Group>[10];
    public int floor = 0;
    private int elevatorCountOffset ;

    public Transform[] waitingPositions;

    public Animator elevatorAnimator;

    [SerializeField] bool areExitingElevator;
    public static ElevatorController Instance { get; private set; }


    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        elevatorAttributes = GetComponent<ElevatorAttributes>();
        for (int i = 0; i < elevatorEntrenceQueue.Length; i++)
        {
            elevatorEntrenceQueue[i] = new List<Group>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVariables(); 
        ManageElevatorOpennes();
        UpdatePosition();
    }
    void UpdatePosition()
    {
        if (elevatorAttributes.IsOpen)
            return;
        int floorUpdate = (int)Mathf.Clamp( Mathf.Round(mousePos.y), 0, BuildingManager.Instance.floorCount);
        transform.position = (Vector2)startPos.position + new Vector2(0, floorUpdate);
        foreach (Group group in peopleInElevator )
        {
            group.SetFloor(floorUpdate);
        }
    }
    void UpdateVariables()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        floor = (int)Mathf.Round(mousePos.y);
    }
    public void OpenDoor()
    {
        StartCoroutine(AllowEveryoneToBeRemoved());
        
        //elevatorDoorObject.transform.localScale = new Vector2(1f, 0);
    }
    public void CloseDoor()
    {
        //elevatorDoorObject.transform.localScale = new Vector2(1f, 1);

    }
    void ManageElevatorOpennes()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //StartCoroutine(ElevatorOpening());
            elevatorAnimator.SetBool("isOpen", true);
            elevatorAttributes.IsOpen = true;
            OpenDoor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            elevatorAttributes.IsOpen = false;
            CloseDoor();
            elevatorAnimator.SetBool("isOpen", false);
        }
    }
    IEnumerator ElevatorOpening()
    {
        elevatorAnimator.SetBool("isOpen", true);
        yield return new WaitForSeconds(0.1f);
        elevatorAttributes.IsOpen = true;
        OpenDoor();
    }
    public void AddToEnetrenceQueue(Group group)
    {
        elevatorEntrenceQueue[group.Floor].Add(group);
    }
    public Transform AssignWaitingPositions(int floorT)
    {
        return waitingPositions[elevatorEntrenceQueue[floorT].Count()];
    }
    public void RemoveFromEntrenceQueue(Group group)
    {
        try
        {
            elevatorEntrenceQueue[group.Floor].Remove(group);

        }
        catch { }
    }
    public IEnumerator AddEveryoneToElevator()
    {
        List<Group> entrenceQueueDupe = elevatorEntrenceQueue[floor].ToList();
        int count = entrenceQueueDupe.Count;
        for (int i = 0; i < count; i++)
        {  
            Group group = entrenceQueueDupe[i];
            if (!group.IsWaitingForElevator)
            {
                elevatorEntrenceQueue[group.Floor].Remove(group);
                continue;
            }
            if (PeopleInElevator() + group.GroupSize() < elevatorAttributes.carryCapacity)
            {
                if (elevatorAttributes.IsOpen == false)
                    yield break;

                group.EnterElevator();
                elevatorEntrenceQueue[group.Floor].Remove(group);
                peopleInElevator.Add(group);
                Console.WriteLine(group);
                yield return new WaitForSeconds(0.3f);
            }
        }

    }
    public void ReplaceGroupInQueue(Group oldGroup,Group newGroup, int floorT)
    {
        elevatorEntrenceQueue[floorT][elevatorEntrenceQueue[floorT].IndexOf(oldGroup)] = newGroup;
    }

    public IEnumerator AllowEveryoneToBeRemoved()
    {
        bool executed = false;
        List<Group> temp = new List<Group>( peopleInElevator);
        foreach (Group group in temp)
        {
            if (elevatorAttributes.IsOpen == false)
                yield break;
            if (group.AnyExitCondition())
            {
                executed = true;
                group.ExitElevator();
                elevatorEntrenceQueue[floor].Add(group);
                peopleInElevator.Remove(group);

                yield return new WaitForSeconds(0.3f);
            }
        }
        if (executed)
            yield return new WaitForSeconds(0.6f);
        if (elevatorAttributes.IsOpen == false)
            yield break;
        StartCoroutine(AddEveryoneToElevator());
        yield break;
    }
    public int PeopleWaitingForElevatorInFloor(int floor)
    {
        return elevatorEntrenceQueue[floor].Count;
    }
    public int PeopleInElevator()
    {
        return peopleInElevator.Count;
    }
    public void ModifyFloorOffset(int floorToEdit, int amountToModify)
    {
        elevatorCountOffset += amountToModify;
    }
}
