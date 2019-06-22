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
        yield return new WaitForSeconds(dragonSummonWait);

        Quaternion rotation = transform.rotation;
        GameObject dragon = Instantiate(dragonStrikePrefab, transform.position - transform.forward * dragonSummonOffset, rotation);
        GameObject portal = Instantiate(portalPrefab, transform.position, rotation); // - (transform.forward * 5f) + (Vector3.up * 1.2f)

        portal.transform.DOScale(0, .2f).SetEase(Ease.OutSine).From();
        portal.GetComponent<Renderer>().material.DOFloat(1, "_Amount", 4f).SetDelay(8f).OnComplete(() => Destroy(portal));

        ParticleSystem[] portalParticles = portal.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem p in portalParticles)
        {
            p.Play();
        }

        Destroy(gameObject);
    }
}
