using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float range = 100f;
    public Camera fpsCam;
    public GameObject impactEffect;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;
    public LineRenderer line;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Start()
    {
        muzzleFlash.gameObject.SetActive(false);
    }

    void HideMuzzle()
    {
        muzzleFlash.gameObject.SetActive(false);
    }

    void Shoot()
    {
        muzzleFlash.gameObject.SetActive(true);
        muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        muzzleFlash.Play();
        Invoke("HideMuzzle", 0.05f);
        gunSound.PlayOneShot(gunSound.clip);

        RaycastHit hit;
        Vector3 endPosition;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            endPosition = hit.point;

            Debug.Log("Hit " + hit.transform.name);

            if (hit.transform.CompareTag("Target"))
            {
                Destroy(hit.transform.gameObject);
            }

            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
        else
        {
            endPosition = fpsCam.transform.position + fpsCam.transform.forward * range;
        }

        StartCoroutine(ShootLine(endPosition));
    }

    IEnumerator ShootLine(Vector3 end)
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, end);
        line.enabled = true;

        yield return new WaitForSeconds(0.05f);

        line.enabled = false;
    }
}