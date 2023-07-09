using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLineRenderer : MonoBehaviour
{
    public Transform t1;
    public Transform t2;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, t1.position);
        lineRenderer.SetPosition(1, t2.position);
    }
}
