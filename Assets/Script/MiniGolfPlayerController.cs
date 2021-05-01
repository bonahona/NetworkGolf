using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGolfPlayerController : NetworkBehaviour
{
    public bool HazControl;
    public int StrikesLeft = 0;
    public Rigidbody Ball;
    public float ImpulseMultiplier;
    public float SqrSlowDownVeloccity;
    public Material ActiveMaterial;
    public Material InactiveMaterial;
    public Renderer Renderer;

    [ClientRpc]
    public void CanHazControl()
    {
        HazControl = isLocalPlayer;
        StrikesLeft = 1;
        CameraDotLookAtThis();
        SetActiveColorOrOutlineOrWhatEver();
        PlayActivationAnimation();
    }

    [ClientRpc]
    public void GiefBackControl()
    {
        SetInActiveColorOrOutlineOrWhatEver();
    }

    [Command]
    public void TellSrvYouAreDone()
    {
        MiniGolfGameController.Instance.PlayerPlayed(netId);
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
    }

    private void FixedUpdate()
    {
        var sqrVelo = Ball.velocity.sqrMagnitude;
        if (sqrVelo < SqrSlowDownVeloccity)
        {
            Ball.AddForce((-Ball.velocity / Mathf.Max(Ball.velocity.magnitude, 1)) * Time.deltaTime * 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!HazControl)
        {
            return;
        }
        if (StrikesLeft > 0)
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
        else if (Ball.velocity.sqrMagnitude < 0.2f)
        {
            HazControl = false;
            GiefBackControl();
            TellSrvYouAreDone();
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
        StrikesLeft -= 1;
        var direction = Ball.transform.position - GetMouseOffset();
        Ball.AddForce(direction * ImpulseMultiplier, ForceMode.VelocityChange);
    }
}
