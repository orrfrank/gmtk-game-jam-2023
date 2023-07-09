using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    // Start is called before the first frame update
    public int floorCount;
    private List<Group>[] peopleInEachFloor;
    public FloorInfo[] floors;


    public static BuildingManager Instance { get; private set; }
    private void Start()
    {
        
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        peopleInEachFloor = new List<Group>[floorCount];
        for (int i = 0; i < floorCount; i++)
        {
            peopleInEachFloor[i] = new List<Group>();
        }

        floorCount = transform.childCount;
        floors = new FloorInfo[floorCount];
        for (int i = 0; i < floors.Length; i++)
        {
            floors[i] = transform.GetChild(i).GetComponent<FloorInfo>();
            floors[i].num = i;
            for (int j = 0; j < floors[i].transform.childCount; j++)
            {
                if (floors[i].transform.GetChild(j).tag == "sign")
                {
                    Debug.Log("work?");
                    floors[i].smokingProhibited = true;
                }
                else if (floors[i].transform.GetChild(j).tag == "gem")
                {
                    Debug.Log("work??");
                    floors[i].gem = floors[i].transform.GetChild(j).gameObject;
                }
            }
        }
    }

    public void Burn()
    {
        Debug.Log("burn");
    }
    public void AddGroupToFloor(Group group, int floorToGet)
    {
        peopleInEachFloor[floorToGet].Add(group);
    }
    public void RemoveGroupFromFloor(Group group, int floorToGet)
    {
        peopleInEachFloor[floorToGet].Remove(group);
    }
    public List<Group> GroupInFloor(int floorToGet)
    {
        return new List<Group>(peopleInEachFloor[floorToGet]);
    }
    public List<Person> PeopleInFloor(int floorToGet)
    {
        List<Person> persons = new List<Person>();
        foreach (Group group in peopleInEachFloor[floorToGet])
        {
            persons = new List<Person>(group.GetMembers());
        }
        return persons;
    }
}
