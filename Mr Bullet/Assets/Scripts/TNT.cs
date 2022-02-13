using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    public GameObject explosionPrefab;

    public float radius = 1;
    public float power = 5;

    void Explode()
    {
        Vector2 explosionPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, radius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Rigidbody2D>() != null)
            {
                var explodeDirection = hit.GetComponent<Rigidbody2D>().position - explosionPosition;
                
                hit.GetComponent<Rigidbody2D>().gravityScale = 1;
                hit.GetComponent<Rigidbody2D>().AddForce(power * explodeDirection, ForceMode2D.Impulse);
            }

            if (hit.tag == "Enemy")
                hit.tag = "Untagged";
        }

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, radius);
    //}


    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Bullet")
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            Explode();
            Destroy(explosion, .8f);
            Destroy(gameObject);
        }
    }

}
