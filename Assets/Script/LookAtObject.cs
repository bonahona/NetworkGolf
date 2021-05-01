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
        var targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
