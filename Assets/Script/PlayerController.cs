using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody Ball;
    public float ImpulseMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        var isOwner = GetComponentInParent<Mirror.NetworkIdentity>().isLocalPlayer;
		if (isOwner)
        {
            Camera.main.GetComponentInParent<FollowObject>().Target = Ball.gameObject;
            Camera.main.GetComponentInParent<LookAtObject>().Target = Ball.gameObject;
        }
		else
		{
            Destroy(gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0))
		{
            DrawPlayerArrowUI();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ApplyForce();
        }
    }

    Vector3 GetMouseOffset()
    {
        var plane = new Plane(Vector3.up, -Ball.transform.position.y);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var intersectionDistance = 0f;
        if (plane.Raycast(ray, out intersectionDistance))
		{
            return ray.GetPoint(intersectionDistance);
		}
        return Vector3.zero;
	}

    void DrawPlayerArrowUI()
    {
        //todo;
        Debug.DrawLine(Ball.transform.position, GetMouseOffset());
    }

    void ApplyForce()
	{
        var direction = Ball.transform.position - GetMouseOffset();
        Debug.LogError(GetMouseOffset());
        Ball.AddForce(direction * ImpulseMultiplier, ForceMode.VelocityChange);
	}
}
