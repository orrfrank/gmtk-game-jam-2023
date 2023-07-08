using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] protected bool isStatic = true;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected bool isBreakable = false;
    [SerializeField] protected float maxWeight = 3;
    [SerializeField] protected float breakTime = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        if (isStatic)
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BreakSelf()
    {
        Destroy(gameObject);
    }
    public void GetNearbyEntities()
    {

    }
}
