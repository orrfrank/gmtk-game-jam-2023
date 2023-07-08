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
    private Stack<Person> peopleInElevator = new Stack<Person>(); //TODO - add a script where info like floor count is available
    private Queue<Person>[] elevatorEntrenceQueue = new Queue<Person>[10];
    public int floor;

    bool areExitingElevator;

    public UnityEvent ElevatorOpenListener;
    public static ElevatorController Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        elevatorAttributes = GetComponent<ElevatorAttributes>();
        for (int i = 0; i < elevatorEntrenceQueue.Length; i++)
        {
            elevatorEntrenceQueue[i] = new Queue<Person>();
        }
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
        ElevatorOpenListener.Invoke();
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
            StartCoroutine(AddEveryoneToElevator());
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
        elevatorEntrenceQueue[person.floor].Enqueue(person);
    }
    public IEnumerator AddEveryoneToElevator()
    {
        while (elevatorEntrenceQueue.Count() > 0 && !areExitingElevator)
        {
            yield return new WaitForSeconds(0.1f);
            if (elevatorAttributes.IsOpen)
                peopleInElevator.Push(elevatorEntrenceQueue[floor].Dequeue());
            else
                yield break;
        }
    }
    public IEnumerator AllowEveryoneFromBeingRemoved()
    {
        Stack <Person> list = new Stack<Person>();
        //Person[] persons = elevatorEntrenceQueue.ToArray<Person>();
        while (elevatorEntrenceQueue.Count() > 0 && !areExitingElevator)
        {
            while (peopleInElevator.Count() > 0)
            {
                Person p = peopleInElevator.Pop();
                if (p.ExitConditionMet())
                    p.ExitElevator();
                else
                    list.Push
            }
        }
    }
}
