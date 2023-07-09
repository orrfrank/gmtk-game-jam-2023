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

    public float readOnlyVelocityOnX;

    float lerpDuration = 0;

    Transform followX;

    public float interpolationSpeed;
    [SerializeField] Group ownerGroup;

    List<int> desireableFloors;

    public Animator anim;
    public Group OwnerGroup { get => ownerGroup;}

    Vector3 lastPos;


    // Start is called before the first frame update
    void Start()
    {

        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        floor = (int)Mathf.Round(targetPosition.y);
        rb = GetComponent<Rigidbody2D>();
        lastPos = transform.position;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
        ManageVelocity();
        if (ownerGroup.InitialMember1 == this)
        {
            if (followX != null) { targetPosition.Set(followX.position.x, targetPosition.y); }
            if (following != null) { targetPosition = following.position; }
            else { NpcBehavior(); }
        }
        else
        {
            targetPosition = ownerGroup.InitialMember1.transform.position;
        }
        floor = (int)Mathf.Round(targetPosition.y);
        rb.position =Vector2.Lerp(transform.position, targetPosition, interpolationSpeed * Time.deltaTime);
        
    }
    private void FixedUpdate()
    {
        readOnlyVelocityOnX = Mathf.Abs((transform.position.x - lastPos.x) * 10) / Time.deltaTime;
        lastPos = transform.position;
        anim.SetFloat("xVel", readOnlyVelocityOnX);
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
        following = follow;
    }
    public void FollowX(Transform follow)
    {
        followX = follow;
    }
    public void StopFollowingX()
    {
        followX = null;
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
    public void Charge(GameObject target)
    {
        //(target.GetComponent("Person") as MonoBehaviour).enabled = false;
    }

    //====================================
    //        GROUPING UP
    //====================================
    //the code bellow is responsible for the grouping up
    //the idea is to have a group leader and for everyone in the group to follow him

    /*
    public void StartCharge(GameObject target)
    {
        //(target.GetComponent("Person") as MonoBehaviour).enabled = false;
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

    */
    
}
