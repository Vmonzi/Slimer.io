using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class PlayerMovement : NetworkCharacterController
{
    [SerializeField] private Vector3 _starterPos;

    protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;

    protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

    public override void Spawned()
    {
        base.Spawned();

        if (HasInputAuthority) CameraControl.Instance.Target = transform;
    }

    public override void Jump(bool ignoreGrounded = false, float? overrideImpulse = null)
    {
        if (IsGrounded)
        {
            Vector3 jumpVector = transform.up * jumpImpulse;
            Controller.Move(jumpVector);

            Velocity    = jumpVector;
            IsGrounded  = Controller.isGrounded;
        }

    }
    public override void Move(Vector3 direction)
    {
        IsGrounded = Controller.isGrounded;

        //Movement
        Vector3 movement = Vector3.ClampMagnitude(direction * acceleration * Runner.DeltaTime, maxSpeed);

        if (!IsGrounded)
        {
            movement.y += gravity * Runner.DeltaTime;
        }

        Controller.Move(movement);

        Velocity = movement;

        Quaternion rotation = Quaternion.LookRotation(direction);

        if(rotation != Quaternion.identity) Controller.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Runner.DeltaTime);
    }

    public override void Dash(Vector3 dir)
    {
        var dashVelocity = dir.normalized * dashSpeed;

        Controller.Move(dashVelocity);

        Velocity    = dashVelocity;
        IsGrounded  = Controller.isGrounded;
    }

    public void ResetPos()
    {
        transform.position = _starterPos + transform.up * 10;
        Debug.Log("mori");

        Velocity = Vector3.zero;
        IsGrounded = Controller.isGrounded;
    }
}
