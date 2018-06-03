using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour {
    PlayerState ps = null;
    PhotonView pv = null;
    // Use this for initialization
    void Start () {
        pv = transform.root.GetComponent<PhotonView>();
        ps = transform.root.GetComponent<PlayerState>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!pv.isMine)
        {
            return;
        }
        int collisionLayer = other.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Empty"))
        {
            Destroy(other.gameObject);
        }
        if (collisionLayer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(other.gameObject);
            if (ps.currHp > 0)
            {
                Debug.Log("헤드샷");
                pv.RPC("DamageByEnemy", PhotonTargets.All, 0, Constants.NONE);
                pv.RPC("SetPlayerVisible", PhotonTargets.All, false, Constants.DEAD);
            }
        }
    }

    [PunRPC]
    void DamageByEnemy(int myHealth, int playerState)
    {
        ps.currHp = myHealth;
        ps.playerStateNum = playerState;
    }

    [PunRPC]
    void SetPlayerVisible(bool isVisible, int playerState)
    {
        //playerStateNum = playerState;
        if (playerState == Constants.GROGGY)
        {
            foreach (MeshRenderer _renderer in ps.renderers)
            {
                _renderer.enabled = isVisible;
            }
        }
        else if (playerState == Constants.DEAD)
        {
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
            //optionManager.enabled = false;
            ps.CharMove.enabled = false;
            ps.CamCon.enabled = false;
            ps.hpBarObj.SetActive(false);
            ps.capsuleCollider.enabled = false;
            foreach (Canvas _canvas in ps.canvas)
            {
                _canvas.enabled = isVisible;
            }
            foreach (MeshRenderer _renderer in ps.renderers)
            {
                _renderer.enabled = isVisible;
            }
            foreach (SkinnedMeshRenderer _skinRenderers in ps.skinRenderers)
            {
                _skinRenderers.enabled = isVisible;
            }
        }
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
