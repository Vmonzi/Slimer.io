using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;

    [SerializeField] private Text _myScoreText;
    [SerializeField] private Text _enemyScoreText;

    private int _myScore;
    private int _enemyScore;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
    }

    [Rpc]
    public void RPC_AddScore([RpcTarget] PlayerRef player, PlayerObjectsHandler objectsHandler)
    {
        if (objectsHandler.HasInputAuthority) _myScore += 1;
        else _enemyScore += 1;

        _myScoreText.text = "My Score: " + _myScore;
        _enemyScoreText.text = "Enemy Score: " + _enemyScore;

        Debug.Log($"MyScore: {_myScore}");
        Debug.Log($"EnemyScore: {_enemyScore}");
    }

    public void CheckScores()
    {
        if (_myScore < _enemyScore) Lose();
        else Win();
    }

    public void Win()
    {
        Debug.Log("You Win");
        _winScreen.SetActive(true);
    }

    public void Lose()
    {
        Debug.Log("You Lose");
        _loseScreen.SetActive(true);
    }
}
