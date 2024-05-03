using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _firingTransform;

    float _coolDown = 0.2f;
    float _lastShoot;

    //[Networked(OnChanged = nameof(OnFiringChange))]
    bool IsFiring { get; set; }

    public override void FixedUpdateNetwork()
    {

        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFirePressed)
            {
                Shoot();
            }
        }
    }

   public void Shoot()
    {
       if (Time.time - _lastShoot < _coolDown) return;

        _lastShoot = Time.time;
        StartCoroutine(FiringCoroutine());

        Runner.Spawn(_bulletPrefab, _firingTransform.transform.position, transform.rotation);
        //Rigidbody projectileRb = _bulletPrefab.GetComponent<Rigidbody>();

        Runner.LagCompensation.Raycast(origin: _firingTransform.position,
                                       direction: _firingTransform.forward,
                                       length: 100,
                                       player: Object.InputAuthority,
                                       hit: out var hitInfo);
        Debug.Log("pium pium");

    }

    IEnumerator FiringCoroutine()
    {
        IsFiring = true;

        yield return new WaitForSeconds(_coolDown);

        IsFiring = false;
    }

    //static void OnFiringChange(Changed<PlayerShoot> changed)
    //{
    //    bool isFiringCurrent = changed.Behaviour.IsFiring;

    //    changed.LoadOld();

    //    bool isFiringBefore = changed.Behaviour.IsFiring;
    //}
}
