using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CollectableFood : NetworkBehaviour
{
    private TotemManager _collectionManager;

    private void Start()
    {
        _collectionManager = TotemManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority || other.gameObject.layer != 7) return;

        other.GetComponent<PlayerObjectsHandler>().AddCollectedObject(GetComponent<NetworkObject>());

        Runner.Despawn(Object);
    }
}