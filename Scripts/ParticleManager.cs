using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Photon.MonoBehaviour
{
    public ParticleSystem[] particles;

    public ParticleSystem BodyParticle;
    public ParticleSystem LFootParticle;
    public ParticleSystem RFootParticle;
    public ParticleSystem LCalfParticle;
    public ParticleSystem RCalfParticle;
    public ParticleSystem HeadParticle;
    private PhotonView pv;
    // Use this for initialization
    void Awake () {
        pv = GetComponent<PhotonView>();
        particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem item in particles)
        {
            item.Stop();
        }
	}

    public void Action(string actionName) {
        pv.RPC("ParticleSystemControl", PhotonTargets.All, actionName);
    }

    [PunRPC]
    void ParticleSystemControl(string colliderName)
    {
        if (colliderName == "HeadCollider")
        {
            HeadParticle.Play();
        }
        else if (colliderName == "RFootCollider")
        {
            RFootParticle.Play();
        }
        else if (colliderName == "LFootCollider")
        {
            LFootParticle.Play();
        }
        else if (colliderName == "LCalfCollider")
        {
            LCalfParticle.Play();
        }
        else if (colliderName == "RCalfCollider")
        {
            RCalfParticle.Play();
        }
        else if (colliderName == "BodyCollider")
        {
            BodyParticle.Play();
        }
    }
}
