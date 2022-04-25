using UnityEngine;
using TMPro;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] int damage;
    public float fireRate;
    [SerializeField] float timeBetweenShooting, spread, range, timeBetweenShots;
    [SerializeField] int  bulletPerTap;
    [SerializeField] bool auto;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] LayerMask playerLayerMask;

    private AudioSource sound;

    int bulletsShot;
    bool shooting, readyToShoot;

    public Player player;
    public Transform attackPoint;

    public ParticleSystem muzzleFlash, bulletImpact;

    private float nextTimeToFire;

    private void Awake()
    {
        readyToShoot = true;
        sound = GetComponentInParent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        timeBetweenShooting = 1 / fireRate;
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
        return Physics.CheckSphere(transform.position + (Vector3.up * 1.5f), 15f, playerLayerMask);
    }

    private void Shoot()
    {
        readyToShoot = false;

        // spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 targetPoint = new Vector3(player.transform.position.x + x, player.transform.position.y + 1 + y, player.transform.position.z);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.transform.name.Equals("Player"))
            {
                player.TakeDamage(damage);
            }
        }
        sound.Play();

        muzzleFlash.Emit(1);

        Vector3 direction = targetPoint - attackPoint.position;
        GameObject currBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
        currBullet.transform.forward = direction.normalized;
        Rigidbody rigidbody = currBullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

        Destroy(currBullet.gameObject, 2f);

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
