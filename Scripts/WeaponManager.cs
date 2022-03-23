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
    [SerializeField] private Transform aimPos;

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
        if (Input.GetKeyDown(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aimPos);

        for (int i = 0; i < bulletPerShot; i++)
        {
            GameObject currBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rigidbody = currBullet.GetComponent<Rigidbody>();
            rigidbody.AddForce(barrelPos.forward * bulletSpeed, ForceMode.Impulse);
        } 
    }
}
