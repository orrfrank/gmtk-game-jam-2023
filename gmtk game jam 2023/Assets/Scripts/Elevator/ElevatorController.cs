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
        int floorUpdate = (int)Mathf.Round(mousePos.y);
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
        StartCoroutine(AddEveryoneToElevator());
        elevatorDoorObject.transform.localScale = new Vector2(1f, 0);
    }
    public void CloseDoor()
    {
        elevatorDoorObject.transform.localScale = new Vector2(1f, 1);

    }
    void ManageElevatorOpennes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            elevatorAttributes.IsOpen = true;
            
            OpenDoor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CloseDoor();
            elevatorAttributes.IsOpen = false;
        }
    }

    public void AddToEnetrenceQueue(Group group)
    {
        elevatorEntrenceQueue[group.Floor].Add(group);
    }
    public IEnumerator AddEveryoneToElevator()
    {
        foreach (Group group in elevatorEntrenceQueue[floor])
        {
            if (PeopleInElevator() + group.GroupSize() < elevatorAttributes.carryCapacity)
            {
                group.EnterElevator();
                elevatorEntrenceQueue[floor].Remove(group);
                peopleInElevator.Add(group);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    public IEnumerator AllowEveryoneToBeRemoved()
    {
        foreach (Group group in peopleInElevator)
        {
            group.ExitElevator();
            elevatorEntrenceQueue[floor].Add(group);
            peopleInElevator.Remove(group);
            yield return new WaitForSeconds(0.3f);
        }
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
