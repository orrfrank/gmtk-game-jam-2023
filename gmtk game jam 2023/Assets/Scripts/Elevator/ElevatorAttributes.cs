using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAttributes : MonoBehaviour
{
    public  int carryCapacity = 6;
    private bool isOpen;

    public bool IsOpen { get => isOpen; set => isOpen = value; }
}
