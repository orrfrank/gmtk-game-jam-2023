using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAttributes : MonoBehaviour
{
    public const int CARRY_CAPACITY = 6;
    private bool isOpen;

    public bool IsOpen { get => isOpen; set => isOpen = value; }
}
