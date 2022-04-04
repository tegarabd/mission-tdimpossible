using UnityEngine;
using TMPro;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timeBetweenShooting, spread, range, timeBetweenShots;
    [SerializeField] int  bulletPerTap;
    [SerializeField] bool auto;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;

    int bulletsShot;
    bool shooting, readyToShoot;

    public Player player;
    public Transform attackPoint;

    public ParticleSystem muzzleFlash, bulletImpact;

    private void Awake()
    {
        readyToShoot = true;
    }

    private void Update()
    {
        shooting = IsPlayerInRange();

        if (readyToShoot && shooting)
        {
            bulletsShot = bulletPerTap;
            Shoot();
        }

    }

    private bool IsPlayerInRange()
    {
        return GetComponentInChildren<Grid>().NodeFromWorldPosition(player.transform.position) != null;
    }

    private void Shoot()
    {
        readyToShoot = false;

        // spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 targetPoint = new Vector3(player.transform.position.x + x, player.transform.position.y + 1 + y, player.transform.position.z);

        muzzleFlash.Emit(1);

        Vector3 direction = targetPoint - attackPoint.position;
        GameObject currBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
        currBullet.transform.forward = direction.normalized;
        currBullet.GetComponent<BulletController>().damage = damage;
        Rigidbody rigidbody = currBullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

        transform.GetComponent<Rigidbody>().AddForce(-direction.normalized * 0.5f, ForceMode.Impulse);

        Instantiate(bulletImpact, player.transform.position, Quaternion.identity);
        bulletImpact.transform.position = targetPoint;
        bulletImpact.Emit(1);

        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

}
