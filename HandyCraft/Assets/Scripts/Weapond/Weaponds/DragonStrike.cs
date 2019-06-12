using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DragonStrike : MonoBehaviour
{

    private new Rigidbody rigidbody;

    public float dragonSummonWait = 1f;
    public float dragonSummonOffset = 10f;
    public GameObject dragonStrikePrefab;
    public GameObject portalPrefab;

    private bool launched;

    // Start is called before the first frame update
    private void Awake()
    {

        rigidbody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        if (launched && rigidbody.velocity != Vector3.zero)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }
    }

    public void Launch()
    {
        launched = true;
        StartCoroutine(UltimateCourotine());
    }

    IEnumerator UltimateCourotine()
    {
        ThrowArrow();

        //Time for Dragon to summon
        yield return new WaitForSeconds(dragonSummonWait);

        Quaternion rotation = transform.rotation;
        GameObject dragon = Instantiate(dragonStrikePrefab, transform.position - transform.forward * dragonSummonOffset, rotation);
        GameObject portal = Instantiate(portalPrefab, transform.position, rotation); // - (transform.forward * 5f) + (Vector3.up * 1.2f)

        //Show Portal
        portal.transform.DOScale(0, .2f).SetEase(Ease.OutSine).From();
        portal.GetComponent<Renderer>().material.DOFloat(1, "_Amount", 4f).SetDelay(8f).OnComplete(() => Destroy(portal));

        //Particles
        ParticleSystem[] portalParticles = portal.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem p in portalParticles)
        {
            p.Play();
        }

        Destroy(gameObject);
        //Extras
        /*
        arrow.GetComponent<TrailRenderer>().emitting = false;
        arrow.GetComponent<Renderer>().enabled = false;
        arrow.parent = transform.GetChild(0);
        arrow.GetComponent<Renderer>().enabled = true;
        arrow.localPosition = arrowLocalPos;
        arrow.localEulerAngles = arrowLocalRot;
        rigidbody.isKinematic = true;
        arrow.GetComponent<Renderer>().material.SetFloat("_GlowPower", 1);
        arrow.GetComponent<Renderer>().material.DOFloat(0, "_GlowPower", .5f);
        */
    }

    private void ThrowArrow()
    {
        //GetComponent<TrailRenderer>().enabled = true;

        //GetComponent<Renderer>().material.SetFloat("_GlowPower", 0);

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            p.Play();
        }
    }
}
