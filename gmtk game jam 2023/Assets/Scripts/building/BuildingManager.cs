using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int floorCount;
    private List<Person>[] peopleInEachFloor;
    public static BuildingManager Instance { get; private set; }
    private void Start()
    {
        peopleInEachFloor = new List<Person>[floorCount];
        for (int i = 0; i < floorCount; i++)
        {
            peopleInEachFloor[i] = new List<Person>();
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    public void Burn()
    {
        Debug.Log("burn");
    }
    public void AddPerson(Person person, int floorToGet)
    {
        peopleInEachFloor[floorToGet].Add(person);
    }
    public void RemovePerson(Person person, int floorToGet)
    {
        peopleInEachFloor[floorToGet].Remove(person);
    }
    public List<Person> PeopleInFloor(int floorToGet)
    {
        return new List<Person>(peopleInEachFloor[floorToGet]);
    }
}
