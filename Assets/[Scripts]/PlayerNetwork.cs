using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;

    public Rigidbody rb;

    float horizontalInput;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;
    public bool isGrounded;
    private bool moveLeft, moveRight;
    public float jumpForce = 350f;
    public Animator animator;
    int jumpHash = Animator.StringToHash("Jump");
    int runStateHash = Animator.StringToHash("RunState");

    private Transform spawnObjectTransform;

    //private NetworkVariable<MycustomData> randomNumber = new NetworkVariable<MycustomData>(
    //    new MycustomData
    //    {
    //        _int = 56,
    //        _bool = true,
    //    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //public struct MycustomData: INetworkSerializable 
    //{
    //    public int _int;
    //    public bool _bool;
    //    public string message;

    //    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    //    {
    //        serializer.SerializeValue(ref _int);
    //        serializer.SerializeValue(ref _bool);
    //        serializer.SerializeValue(ref message);
    //    }
    //}

    //public override void OnNetworkSpawn()
    //{
    //    randomNumber.OnValueChanged += (MycustomData previousValue, MycustomData newValue) =>
    //    {
    //        Debug.Log(OwnerClientId + ";  " + newValue._int + "; "+ newValue._bool + "; " + newValue.message);
    //    };
    //}


    private void Update()
    {
        //Debug.Log(OwnerClientId + "; randomNumber: " + randomNumber.Value);

        if (!IsOwner) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        horizontalInput = Input.GetAxis("Horizontal");

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            //Instantiate(spawnedObjectPrefab);
            //TestServerRpc(new ServerRpcParams());
            //randomNumber.Value = new MycustomData
            //{
            //    _int=10,
            //    _bool=false,
            //    message = " All your base are belong to us!"
            //};
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            spawnObjectTransform.GetComponent<NetworkObject>().Despawn(true);
        }

            Vector3 moveDiv = new Vector3(0, 0, 0);

        //if (Input.GetKey(KeyCode.W)) moveDiv.z = +1f;
        //if (Input.GetKey(KeyCode.S)) moveDiv.z = -1f;
        //if (Input.GetKey(KeyCode.A)) moveDiv.x = -1f;
        //if (Input.GetKey(KeyCode.D)) moveDiv.x = +1f;

        if (Input.GetKey(KeyCode.W)) moveDiv.x = -1f;
        if (Input.GetKey(KeyCode.S)) moveDiv.x = +1f;
        if (Input.GetKey(KeyCode.A)) moveDiv.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDiv.z = +1f;

        float moveSpeed = 3f;
        transform.position += moveDiv * moveSpeed * Time.deltaTime;
    }

    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);
        rb.AddForce(Vector3.up * jumpForce);
        animator.SetTrigger(jumpHash);
    }

   

    [ServerRpc]
    private void TestServerRpc (ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc" + OwnerClientId+ "; " + serverRpcParams.Receive.SenderClientId);
    }
}
