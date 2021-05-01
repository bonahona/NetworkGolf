using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGolfPlayerController : NetworkBehaviour
{
    public bool HazControl;
    public Rigidbody Ball;
    public float ImpulseMultiplier;
    public Material ActiveMaterial;
    public Material InactiveMaterial;
    public Renderer Renderer;

    [ClientRpc]
	public void CanHazControl()
    {
        HazControl = isLocalPlayer;
        CameraDotLookAtThis();
        SetActiveColorOrOutlineOrWhatEver();
        PlayActivationAnimation();
    }

    public void GiefBackControl()
    {
        HazControl = false;
        SetInActiveColorOrOutlineOrWhatEver();
    }

    void CameraDotLookAtThis()
    {
        Camera.main.GetComponentInParent<FollowObject>().Target = Ball.gameObject;
        Camera.main.GetComponentInParent<LookAtObject>().Target = Ball.gameObject;
    }

    void SetInActiveColorOrOutlineOrWhatEver()
    {
        //todo;
        Renderer.material = InactiveMaterial;
    }

    void SetActiveColorOrOutlineOrWhatEver()
	{
        //todo;
        Renderer.material = ActiveMaterial;

    }

    void PlayActivationAnimation()
    {
        Ball.AddForce(Vector3.up * .5f, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!HazControl)
		{
            return;
		}

        if (Input.GetMouseButton(0))
		{
            DrawPlayerArrowUI();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ApplyForce();
            GiefBackControl();
            MiniGolfGameController.Instance.PlayerPlayed(netId);
        }
    }

    Vector3 GetMouseOffset()
    {
        var plane = new Plane(Vector3.up, -Ball.transform.position.y);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float intersectionDistance;
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
        Ball.AddForce(direction * ImpulseMultiplier, ForceMode.VelocityChange);
	}
}
