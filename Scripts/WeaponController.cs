using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    [SerializeField] int magazineSize, bulletPerTap;
    [SerializeField] bool auto;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;

    int bulletsLeft, bulletsShot, bulletShotCount;
    bool shooting, readyToShoot, reloading;

    public Player player;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemyLayer;

    public Camera cam;
    public ParticleSystem muzzleFlash, bulletImpact;

    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        GetInput();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void GetInput()
    {
        if (auto) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

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

        if (Physics.Raycast(ray, out rayHit, range, enemyLayer))
        {
            targetPoint = rayHit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(range);
        }


        muzzleFlash.Emit(1);

        Vector3 direction = targetPoint - attackPoint.position;
        GameObject currBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
        currBullet.transform.forward = direction.normalized;
        currBullet.GetComponent<BulletController>().damage = damage;
        Rigidbody rigidbody = currBullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

        transform.GetComponent<Rigidbody>().AddForce(-direction.normalized * 0.5f, ForceMode.Impulse);

        Instantiate(bulletImpact, rayHit.point, Quaternion.identity);
        bulletImpact.transform.position = targetPoint;
        bulletImpact.transform.forward = rayHit.normal;
        bulletImpact.Emit(1);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (GameManager.Instance.current.id == 4 && transform.name.Equals("M4_Carbine")) {
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
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
