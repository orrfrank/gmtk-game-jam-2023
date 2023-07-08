using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    // Start is called before the first frame update
    public int floorCount;
    private List<Person>[] peopleInEachFloor;
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
        peopleInEachFloor = new List<Person>[floorCount];
        for (int i = 0; i < floorCount; i++)
        {
            peopleInEachFloor[i] = new List<Person>();
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
    public void AddPersonToFloor(Person person, int floorToGet)
    {
        peopleInEachFloor[floorToGet].Add(person);
    }
    public void RemovePersonFromFloor(Person person, int floorToGet)
    {
        peopleInEachFloor[floorToGet].Remove(person);
    }
    public List<Person> PeopleInFloor(int floorToGet)
    {
        return new List<Person>(peopleInEachFloor[floorToGet]);
    }
}
