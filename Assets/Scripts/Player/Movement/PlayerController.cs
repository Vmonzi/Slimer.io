using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShoot))]
public class PlayerController : NetworkBehaviour
{
    PlayerMovement _playerMovement;
    NetworkInputData _networkInput;
    PlayerShoot _playerShoot;

    NetworkMecanimAnimator _myAnimator;


    public override void Spawned()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShoot = GetComponent<PlayerShoot>();
    }


    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData networkInputData)) return;

        //Movement
        Vector3 moveDirection = networkInputData.move;
        _playerMovement.Move(moveDirection);

        //Shoot
        if (networkInputData.isFirePressed)
        {
            _playerShoot.Shoot();
        }

        //Jump
        if (networkInputData.isJumpPressed)
        {
            _playerMovement.Jump();
        }

        if (networkInputData.isDashPressed)
        {
            _playerMovement.Dash(transform.forward);
        }
    }
}
