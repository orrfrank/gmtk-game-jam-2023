using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAnimator : MonoBehaviour
{
    Transform target;
    [SerializeField] float interpolationSpeed;
    private void Start()
    {
        target = transform.parent;
        transform.parent = null;
    }
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.position, interpolationSpeed * Time.deltaTime);
    }
}
