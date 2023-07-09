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
    public GameObject chargingTarget;

    public float interpolationSpeed;
    [SerializeField] Group ownerGroup;

    List<int> desireableFloors;

    public Group OwnerGroup { get => ownerGroup;}

    // Start is called before the first frame update
    void Start()
    {

        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        floor = (int)Mathf.Round(targetPosition.y);
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {    
        ManageVelocity();
        if (following != null) { targetPosition = following.position; }
        else { NpcBehavior(); }
        floor = (int)Mathf.Round( targetPosition.y);
        rb.position =Vector2.Lerp(transform.position, targetPosition, interpolationSpeed * Time.deltaTime);
        DoInUpdate();
    }

    protected virtual void ManageVelocity()
    {
        if (chargingTarget == null)
        {
            if (targetPosition.x > borderCanada)
                direction = -1;
            else if (targetPosition.x < borderMexico)
                direction = 1;
        }
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
    public void EnterElevator()
    {
        if ((int)Mathf.Round(targetPosition.y) == floor)
        {
            Follow(ElevatorController.Instance.transform);
            isWaitingForElevator = false;
            isInElevator = true;

        }
    }

    public virtual void ExitElevator()
    {

    }
    public void SetGroup(Group group)
    {
        this.ownerGroup = group;
    }
    public virtual bool ExitConditionMet()
    {
        return (int)Mathf.Round(targetPosition.y) == targetFloor;
    }    

    public void SetFloor(int targetFloor)
    {
        targetPosition = new Vector2(targetPosition.x, targetFloor);
    }
    //====================================
    //        GROUPING UP
    //====================================
    //the code bellow is responsible for the grouping up
    //the idea is to have a group leader and for everyone in the group to follow him

    public void StartCharge(GameObject target)
    {
        velocity = 4;
        chargingTarget = target;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (chargingTarget != null)
        {
            if (collision.gameObject.name == chargingTarget.name)
            {
                (chargingTarget.GetComponent("Person") as MonoBehaviour).enabled = false;
                chargingTarget.transform.position = new Vector3(transform.position.x+0.2f, transform.position.y+0.1f, transform.position.z);
            }
            switch (collision.gameObject.tag)
            {
                case "barrier":
                    //destroy barrier and win
                    break;
                case "pflammble":
                    //turn to flammable
                    break;
                case "wall":
                    //destroy wall and win
                    break; 
            }
        }
    }

    public virtual void DoInUpdate()
    {

    }


    
}