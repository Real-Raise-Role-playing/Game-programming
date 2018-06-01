using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

//public class NetworkCharacterMove : Photon.MonoBehaviour {
//    private Vector3 correctPlayerPos = Vector3.zero;
//    private Quaternion correctPlayerRot = Quaternion.identity;
//    private PhotonView pv = null;
//	// Use this for initialization
//	void Awake () {
//        pv = GetComponent<PhotonView>();
//    }

//	// Update is called once per frame
//	void Update () {
//        if (!pv.isMine)
//        {
//            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 3.0f);
//            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 3.0f);
//        }
//    }
//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.isWriting)
//        {
//            // We own this player: send the others our data
//            stream.SendNext(transform.position);
//            stream.SendNext(transform.rotation);
//        }
//        else
//        {
//            // Network player, receive data
//            correctPlayerPos = (Vector3)stream.ReceiveNext();
//            correctPlayerRot = (Quaternion)stream.ReceiveNext();
//        }
//    }
//}

public class NetworkCharacterMove : Photon.MonoBehaviour
{

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Rigidbody rb = null;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private PhotonView pv = null;
    // Use this for initialization
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            SyncedMovement();
            //transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 3.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 3.0f);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            // Network player, receive data
            //syncEndPosition = (Vector3)stream.ReceiveNext();
            //syncStartPosition = transform.position;
            //correctPlayerRot = (Quaternion)stream.ReceiveNext();
            //syncTime = 0f;
            //syncDelay = Time.time - lastSynchronizationTime;
            //lastSynchronizationTime = Time.time;

            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            syncEndPosition = syncPosition + syncVelocity * Time.deltaTime;
            syncStartPosition = transform.position;
        }
    }
    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);

    }
}
