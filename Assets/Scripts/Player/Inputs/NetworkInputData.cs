using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 move;
    public NetworkBool isJumpPressed;
    public NetworkBool isFirePressed;
    public NetworkBool isDashPressed;
}
