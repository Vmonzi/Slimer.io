using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

[RequireComponent(typeof(CharacterInputHandler))]
public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    NicknameText _myNickname;
    [SerializeField] string _nickname;

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    NetworkString<_16> Nickname { get; set; }

    public event Action OnLeft = delegate { };

    public override void Spawned()
    {
        _myNickname = NicknameHandler.Instance.AddNickname(this);


        ////Si tengo autoridad de Input
        if (Object.HasInputAuthority)
        {
            Local = this;


            RPC_SetNickname(_nickname + UnityEngine.Random.Range(1, 1001));
        }

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetNickname(string newNick)
    {
        Nickname = newNick;
    }

    public static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.UpdateNickname(changed.Behaviour.Nickname.ToString());
    }

    void UpdateNickname(string newNick)
    {
        _myNickname.UpdateNickname(newNick);
    }

    public CharacterInputHandler GetInputHandler()
    {
        return GetComponent<CharacterInputHandler>();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
