using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Rigidbody rb;
    private bool targetHit;

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.collider.CompareTag("MilitaryTarget"))
        {
            if (targetHit) return;
            else targetHit = true;

            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;

            transform.SetParent(collision.transform);

            GameManager.Instance.addMilitaryTargetHitCount();
        }

        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(this);
    }


}
