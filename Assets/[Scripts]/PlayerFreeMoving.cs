using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerFreeMoving : NetworkBehaviour
{  
    private void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDiv = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDiv.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDiv.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDiv.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDiv.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDiv * moveSpeed * Time.deltaTime;
    }
}
