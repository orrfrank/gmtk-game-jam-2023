using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public List<GameObject> diamonds = new List<GameObject>();
    public static BuildingManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void AddDiamond(GameObject obj)
    {
       diamonds.Add(obj);
    }

    public void Burn()
    {
        Debug.Log("burn");
    }
}
