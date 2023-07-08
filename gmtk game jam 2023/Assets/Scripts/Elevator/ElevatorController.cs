using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorController : MonoBehaviour
{
    private ElevatorAttributes elevatorAttributes;
    Camera mainCamera;
    Vector2 mousePos;
    [SerializeField] public Transform startPos;
    public bool isOpen = false;
    public GameObject elevatorDoorObject;

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
            OpenDoor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CloseDoor();
            elevatorAttributes.IsOpen = false;
        }
    }
}
