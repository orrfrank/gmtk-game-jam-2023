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
    bool isWaitingForElevator = false;
    bool isInElevator;
    [SerializeField] Vector2 targetPosition;
    const int BORDER_MEXICO = 1;
    const int BORDER_CANADA = 13;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("deez");
            RequestEnterElevator();
        }
        ManageVelocity();
        if (following != null) { targetPosition = following.position; }
        else { NpcBehavior(); }
        floor = (int)Mathf.Round( targetPosition.y);
        transform.position = targetPosition;
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
    public void RequestEnterElevator()
    {
        if (ElevatorController.Instance.elevatorAttributes.IsOpen)
            EnterElevator();
        else
        {
            Debug.Log("nuts");

            ElevatorController.Instance.ElevatorOpenListener.AddListener(EnterElevator);
            isWaitingForElevator = true;
        }
    }
    public void EnterElevator()
    {
        if ((int)Mathf.Round(targetPosition.y) == floor)
        {
            ElevatorController.Instance.ElevatorOpenListener.RemoveListener(EnterElevator);
            Follow(ElevatorController.Instance.transform);
            isWaitingForElevator = false;
            isInElevator = true;
            RequestElevatorExit();
        }
    }
    public void RequestElevatorExit()
    {
        ElevatorController.Instance.ElevatorOpenListener.AddListener(ExitElevator);

    }
    public void ExitElevator()
    {
        Debug.Log((int)Mathf.Round(targetPosition.y) == targetFloor);
        if ((int)Mathf.Round(targetPosition.y) == targetFloor)
        {
            ElevatorController.Instance.ElevatorOpenListener.RemoveListener(ExitElevator);
            isInElevator = false;
            StopFollowing(ElevatorController.Instance.transform);
            Debug.Log("exited");
        }
    }
    public virtual bool ExitConditionMet()
    {
        return (int)Mathf.Round(targetPosition.y) == targetFloor;
    }    
}
