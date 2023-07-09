using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    // Start is called before the first frame update
    public int floorCount;
    private List<Group>[] peopleInEachFloor;
    public FloorInfo[] floors;

    public List<GameObject> diamonds = new List<GameObject>();

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
    }

    public void AddDiamond(GameObject obj)
    {
       diamonds.Add(obj);
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
