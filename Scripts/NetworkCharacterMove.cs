using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacterMove : Photon.MonoBehaviour {
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    PhotonView pv = null;
	// Use this for initialization
	void Start () {
        pv = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!pv.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
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
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
