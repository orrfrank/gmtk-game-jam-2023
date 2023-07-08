using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoker : Person
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("flammable"))
        {
            Debug.Log("burn by smoke");
            BuildingManager.Instance.Burn();
        }
    }
}
