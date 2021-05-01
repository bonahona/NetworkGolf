using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public GameObject Target;
    void Start()
    {
        
    }

    void Update()
    {
        if (!Target)
            return;
        transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position, Vector3.up);
    }
}
