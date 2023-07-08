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
    [SerializeField] private Stack<Person> peopleInElevator = new Stack<Person>(); //TODO - add a script where info like floor count is available
    [SerializeField] private Queue<Person>[] elevatorEntrenceQueue = new Queue<Person>[10];
    public int floor;

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
            elevatorEntrenceQueue[i] = new Queue<Person>();
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
        transform.position = (Vector2)startPos.position + new Vector2(0, Mathf.Round(mousePos.y));
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

    public void AddToEnetrenceQueue(Person person)
    {
        Debug.Log("AddedPersonToEntrenceQueue");
        elevatorEntrenceQueue[person.floor].Enqueue(person);
    }
    public IEnumerator AddEveryoneToElevator()
    {
        while (elevatorEntrenceQueue[floor].Count() > 0 && !areExitingElevator)
        {
            yield return new WaitForSeconds(0.1f);
            if (elevatorAttributes.IsOpen)
            {
                peopleInElevator.Push(elevatorEntrenceQueue[floor].Dequeue());
                Debug.Log($"someone went from entrence queue to elevator, count: {peopleInElevator.Count()}");
            }
            else
                yield break;
        }
    }
    public IEnumerator AllowEveryoneToBeRemoved()
    {
        if (peopleInElevator.Count() == 0)
        {
            yield return null;
            Debug.Log("no one was in the elevator");
        }
        Stack <Person> list = new Stack<Person>();
        Person[] persons = peopleInElevator.ToArray();
        for (int i = persons.Length - 1; i >= 0 ; i--)
        {
            areExitingElevator = true;
            yield return new WaitForSeconds(0.5f);
            Person p = persons[i];
            Debug.Log(p.ExitConditionMet());
            if (p.ExitConditionMet())
            {
                Debug.Log("someone was removed from the elevator");

                p.ExitElevator();
                peopleInElevator.Pop();
            }
            else
            if (!elevatorAttributes.IsOpen)
            {
                while (i >= 0)
                {
                    list.Push(persons[i]);
                    i--;
                }
                peopleInElevator = list;
                areExitingElevator = false;
                yield return null;
            }

        }
        peopleInElevator = list;
        areExitingElevator = false;
    }
}
