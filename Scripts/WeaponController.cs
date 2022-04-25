using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    [SerializeField] int damage;
    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDrop;
    [SerializeField] float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    [SerializeField] int magazineSize, bulletPerTap, ammoSize;
    [SerializeField] bool auto;
    [SerializeField] TrailRenderer tracer;

    [SerializeField] CameraController recoilCam;

    int bulletsLeft, bulletsShot, bulletShotCount;
    bool shooting, readyToShoot, reloading;
    List<Bullet> bullets = new List<Bullet>();

    public Player player;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemyLayer;

    public Camera cam;
    public ParticleSystem muzzleFlash, bulletImpact;

    public TextMeshProUGUI text;

    private AudioSource sound;

    private Vector3 GetBulletPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    private Bullet CreateBullet(Vector3 velocity, Vector3 position)
    {
        Bullet bullet = new Bullet();
        bullet.initialVelocity = velocity;
        bullet.initialPosition = position;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracer, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    private void UpdateBullet(float deltaTime)
    {
        bullets.ForEach(bullet => {
            Vector3 p0 = GetBulletPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetBulletPosition(bullet);
            SimulateBullet(p0, p1, bullet);
        });
    }

    private void SimulateBullet(Vector3 start, Vector3 end, Bullet bullet)
    {

    }

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        sound = GetComponent<AudioSource>();
        bulletsLeft = magazineSize;
        ammoSize = (transform.name.Equals("Pistol")) ? player.totalPistolAmmo : player.totalRifleAmmo;
        readyToShoot = true;
        timeBetweenShooting = 1 / fireRate;
        
    }

    private void Update()
    {
        ammoSize = (transform.name.Equals("Pistol")) ? player.totalPistolAmmo : player.totalRifleAmmo;
        GetInput();
        UpdateDisplay();
        /*UpdateBullet(Time.deltaTime);*/
    }

    private void UpdateDisplay()
    {
        text.SetText(bulletsLeft + " / " + ammoSize);
    }
    private void GetInput()
    {
        if (auto) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if ((Input.GetKeyDown(KeyCode.R) || bulletsLeft <= 0) && bulletsLeft < magazineSize && ammoSize >= magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletPerTap;
            Shoot();
        }
            
    }

    private void Shoot()
    {
        readyToShoot = false;

        // spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f + x, 0.5f + y, 0));
        Vector3 targetPoint;

        recoilCam.AddRecoil();
        sound.Play();

        if (Physics.Raycast(ray, out rayHit, range, enemyLayer))
        {
            targetPoint = rayHit.point;
            if (rayHit.transform.CompareTag("Enemy"))
            {
                rayHit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
            else if (rayHit.transform.CompareTag("MilitaryTarget"))
            {
                MilitaryTargetController.AddHitCount();
            }
            else if (rayHit.transform.CompareTag("Boss"))
            {
                rayHit.transform.GetComponent<Boss>().TakeDamage(damage);
            }
        }
        else
        {
            targetPoint = ray.GetPoint(range);
        }


        /*Vector3 velocity = (targetPoint - attackPoint.position).normalized * bulletSpeed;
        Bullet bullet = CreateBullet(velocity, attackPoint.position);
        bullets.Add(bullet);*/

        TrailRenderer bullettracer = Instantiate(tracer, attackPoint.position, Quaternion.identity);
        bullettracer.AddPosition(attackPoint.position);

        muzzleFlash.Emit(1);

        bullettracer.transform.position = targetPoint;
        ParticleSystem impact = Instantiate(bulletImpact, targetPoint, Quaternion.LookRotation(rayHit.normal));
        bulletImpact.Emit(1);


        Destroy(impact.gameObject, 1f);
        Destroy(bullettracer.gameObject, 1f);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (GameManager.Instance.current.id == 4 && transform.name.Equals("Rifle")) {
            if (bulletShotCount < 50)
            {
                GameManager.Instance.ChangeMissionDisplay(++bulletShotCount);
            }

            if (bulletShotCount >= 50 && !GameManager.Instance.current.done)
            {
                GameManager.Instance.MissionDone();
            }
        } 


        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        ammoSize -= (magazineSize - bulletsLeft);
        if (transform.name.Equals("Pistol"))
        {
            player.totalPistolAmmo = ammoSize;
        }
        else if (transform.name.Equals("Rifle"))
        {
            player.totalRifleAmmo = ammoSize;
        }
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
