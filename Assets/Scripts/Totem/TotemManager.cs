using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Fusion;

public class TotemManager : NetworkBehaviour
{
    public static TotemManager Instance { get; private set; }

    [SerializeField] List<NetworkObject> _prefabCollectable = new List<NetworkObject>();
    [SerializeField] List<NetworkObject> _totemObject = new List<NetworkObject>();
    [SerializeField] List<NetworkObject> _sceneCollectable = new List<NetworkObject>();

    public List<NetworkObject> PrefabCollectable { get { return _prefabCollectable; } }

    public override void Spawned()
    {
        Debug.Log("Spawned Executed");
        Debug.Log(Object.HasStateAuthority);


        if (!Instance) Instance = this;
        else Runner.Despawn(Object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority || other.gameObject.layer != 7) return;

        var objectHandler = other.gameObject.GetComponent<PlayerObjectsHandler>();

        if (_prefabCollectable.Count > 0 && objectHandler && objectHandler.CollectedObjects.Contains(_prefabCollectable[0].GetComponent<NetworkObject>()))
        {
            foreach (var player in Runner.ActivePlayers)
            {
                GameManager.Instance.RPC_AddScore(player, objectHandler);
                RPC_OnObjectCollected(player, objectHandler);
            }
        }
    }

    [Rpc]
    public void RPC_OnObjectCollected([RpcTarget] PlayerRef player, PlayerObjectsHandler model)
    {
        _prefabCollectable.RemoveAt(0);

        if (_prefabCollectable.Count <= 0) GameManager.Instance.CheckScores();
        else NextCollectable();
    }

    private void NextCollectable()
    {
        _sceneCollectable.RemoveAt(0);
        _sceneCollectable[0].gameObject.SetActive(true);
        Runner.Despawn(_totemObject[0]);
        _totemObject.RemoveAt(0);
        _totemObject[0].gameObject.SetActive(true);
    }

    public void AddCollectable(NetworkObject food)
    {
        _sceneCollectable[0] = food;
    }
}
