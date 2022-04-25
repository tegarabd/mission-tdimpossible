using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public static bool timeToShooting;

    [SerializeField] int damage;
    [SerializeField] float fireRate;
    [SerializeField] float spread, range, timeBetweenShots;
    [SerializeField] int bulletPerTap;

    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDrop;

    int bulletsShot;
    bool readyToShoot;

    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask playerMask;

    public ParticleSystem muzzleFlash, bulletImpact;
    public TrailRenderer tracer;

    private void Start()
    {
        timeToShooting = true;
        readyToShoot = true;
    }

    private void Update()
    {
        if (timeToShooting && readyToShoot)
        {
            bulletsShot = bulletPerTap;
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = transform.forward + new Vector3(x, y, 0);
        Ray ray = new Ray(transform.position, direction);

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out rayHit, range, playerMask))
        {
            targetPoint = rayHit.point;
            if (rayHit.transform.name.Equals("Player"))
            {
                rayHit.transform.GetComponent<Player>().TakeDamage(damage);
            }
        }
        else
        {
            targetPoint = ray.GetPoint(range);
        }

        TrailRenderer bullettracer = Instantiate(tracer, attackPoint.position, Quaternion.identity);
        bullettracer.AddPosition(attackPoint.position);

        muzzleFlash.Emit(1);

        bullettracer.transform.position = targetPoint;
        ParticleSystem impact = Instantiate(bulletImpact, targetPoint, (rayHit.normal == Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(rayHit.normal));
        bulletImpact.Emit(1);


        Destroy(impact.gameObject, 1f);
        Destroy(bullettracer.gameObject, 1f);

        bulletsShot--;

        StartCoroutine(ResetShot());

        if (bulletsShot > 0)
            StartCoroutine(Shoot());
    }

    private IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(1f / fireRate);
        readyToShoot = true;
    }
}
