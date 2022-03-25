using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] private float fireRate;
    private float fireRateTimer;

    [Header("Bullet Props")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform barrelPos;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletPerShot;
    [SerializeField] private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        fireRateTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldFire()) Fire();
    }
    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - barrelPos.position;

        for (int i = 0; i < bulletPerShot; i++)
        {
            GameObject currBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            currBullet.transform.forward = direction.normalized;
            Rigidbody rigidbody = currBullet.GetComponent<Rigidbody>();
            rigidbody.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
        } 
    }
}
