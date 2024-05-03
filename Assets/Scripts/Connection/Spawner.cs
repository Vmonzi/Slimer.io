using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private List<NetworkPlayer>    _playerPrefab = new List<NetworkPlayer>();
                     private List<Transform>        _playerStartPosition = new List<Transform>();

    CharacterInputHandler _characterInputHandler;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (_playerStartPosition != default)
        {
            for (int i = 1; i <= 2; i++)
            {
                var posPlayer = GameObject.Find("Player" + i).transform;

                if (posPlayer)
                {
                    _playerStartPosition.Add(posPlayer);
                }
            }
        }

        StartCoroutine(CheckPlayers(runner));

        if (runner.IsServer)
        {
            Debug.Log($"Player Joined, I'm the server/host, cant players: {runner.SessionInfo.PlayerCount}");
            runner.Spawn(_playerPrefab[runner.SessionInfo.PlayerCount - 1], _playerStartPosition[runner.SessionInfo.PlayerCount - 1].position, Quaternion.Euler(0, 0, 0), player);
        }
        else
        {
            Debug.Log($"Player Joined, I'm not the server/host");
        }
    }

    IEnumerator CheckPlayers(NetworkRunner runner)
    {
        do
        {
            yield return new WaitForEndOfFrame();

        } while (runner.ActivePlayers.Count() != 2);

        runner.ProvideInput = true;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!NetworkPlayer.Local) return;

        if (!_characterInputHandler)
        {
            _characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>(); ;
        }
        else
        {
            input.Set(_characterInputHandler.GetNetworkInputs());
        }
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        runner.Shutdown();
    }

    #region Unused callbacks

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    #endregion
}
