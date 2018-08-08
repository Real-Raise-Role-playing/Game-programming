using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkCharacterMove : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private PhotonView pv = null;
    Animator anim = null;
    OptionManager om = null;
    CharacterMove cm = null;
    // Use this for initialization
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>(); //Anim
    }
    void Start()
    {
        if (pv.isMine)
        {
            om = GetComponent<OptionManager>();
            cm = GetComponent<CharacterMove>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 3.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 3.0f);
        }
        else
        {
            if (!om.InventoryOn)
            {
                anim.SetBool("run", cm.run);
                anim.SetBool("isWalk", cm.isWalk);
                anim.SetFloat("inputH", cm.x);
                anim.SetFloat("inputV", cm.z);
            }
            else
            {
                anim.SetBool("run", false);
                anim.SetBool("isWalk", false);
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetBool("run"));
            stream.SendNext(anim.GetBool("isWalk"));
            stream.SendNext(anim.GetFloat("inputH"));
            stream.SendNext(anim.GetFloat("inputV"));
        }
        else
        {
            // Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            anim.SetBool("run", (bool)stream.ReceiveNext());
            anim.SetBool("isWalk", (bool)stream.ReceiveNext());
            anim.SetFloat("inputH", (float)stream.ReceiveNext());
            anim.SetFloat("inputV", (float)stream.ReceiveNext());
        }
    }
}
