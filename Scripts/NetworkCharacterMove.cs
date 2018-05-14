using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacterMove : Photon.MonoBehaviour {
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private PhotonView pv = null;
	// Use this for initialization
	void Awake () {
        pv = GetComponent<PhotonView>();
        correctPlayerPos = transform.position;
        correctPlayerRot = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (!pv.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
        //else
        //{
        //    correctPlayerPos = transform.position;
        //    correctPlayerRot = transform.rotation;
        //}
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
