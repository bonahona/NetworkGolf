using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject Target;
    public float Acceleration;
    public float Drag;
    public float DirectionOffset;
    private Vector3 Velocity;

    float Smoothstep(float min, float max, float x)
    {
        x = Mathf.Clamp01((x - min) / (max - min));
        return x * x * (3 - 2 * x);
    }

    void Update()
    {
        if (!Target)
            return;
        var diroffs = Velocity.normalized * Smoothstep(0, 10, Velocity.magnitude) * DirectionOffset;
        Velocity += (Target.transform.position - transform.position + diroffs) * Acceleration * Time.deltaTime - Velocity * Drag * Time.deltaTime;
        transform.position += Velocity * Time.deltaTime;
    }
}
