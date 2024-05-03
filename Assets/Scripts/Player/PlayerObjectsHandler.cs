using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerObjectsHandler : NetworkBehaviour
{
    [SerializeField] private Transform _shootPos;

    private List<NetworkObject> _collectedObjects = new List<NetworkObject>();
    public List<NetworkObject> CollectedObjects { get { return _collectedObjects; } }

    public void AddCollectedObject(NetworkObject obj)
    {
        string path = "Prefabs/PowerUps/" + obj.name;
        if (path.Contains("(Clone)")) path = path.Replace("(Clone)", "");

        Debug.Log(path);
        _collectedObjects.Add(TotemManager.Instance.PrefabCollectable[0]);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_DropObject()
    {
        if (_collectedObjects.Count <= 0 || TotemManager.Instance.PrefabCollectable[0].name != _collectedObjects[_collectedObjects.Count - 1].name) return;

        Debug.LogWarning("DropFood");

        var food = Runner.Spawn(_collectedObjects[_collectedObjects.Count - 1], _shootPos.position + transform.forward * 2, Quaternion.identity);
        _collectedObjects.RemoveAt(_collectedObjects.Count - 1);

        TotemManager.Instance.AddCollectable(food);
    }
}
