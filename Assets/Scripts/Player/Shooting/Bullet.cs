using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkRigidbody))]
public class Bullet : NetworkRigidbody
{

    [SerializeField] float _speed;
    TickTimer _expireLifeTimer;
    [SerializeField] float _lifeTime;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (!Object.HasStateAuthority) return;
        if (_expireLifeTimer.Expired(Runner))
        {
            DespawnObject();
        }
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            base.Spawned();
            Rigidbody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);
            _expireLifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);
            Debug.Log("sali");
        }
    }
    void DespawnObject()
    {
        _expireLifeTimer = TickTimer.None;
        Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        var objectHandler = other.gameObject.GetComponent<PlayerObjectsHandler>();
        if (objectHandler) objectHandler.RPC_DropObject(); 

        Debug.Log("gudbai gusbaaaaaaaai");
        DespawnObject();
    }
}
