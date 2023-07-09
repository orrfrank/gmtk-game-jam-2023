using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Thief : Person
{
    // Start is called before the first frame update
    public override bool ExitConditionMet()//exits if one of diamonds is on said floor
    {
        int thiefFloor = (int)Mathf.Round(targetPosition.y);
        for (int i = 0; i < BuildingManager.Instance.floors.Length; i++)
        {
            if (thiefFloor == i && BuildingManager.Instance.floors[i].gem != null)
            {
                return true;
            }
        }
        return false;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collide");
        if (collision.gameObject.tag == "gem")
        {
            Debug.Log("with gem");
            velocity = 0;
            GetComponent<Rigidbody2D>().position = transform.position;
            Guard.stolenFloor = (int)Mathf.Round(transform.position.y);
            Guard.thief = gameObject;
            StartCoroutine(StealTimer(collision.gameObject));
        }
    }
    IEnumerator StealTimer(GameObject gem)
    {
        yield return new WaitForSeconds(5f);
        Destroy(gem);

        Destroy(this.gameObject);
    }

    
}
