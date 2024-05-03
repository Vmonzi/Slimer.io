using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ResetSpawn : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null) other.GetComponent<PlayerMovement>().ResetPos();
    }
}
