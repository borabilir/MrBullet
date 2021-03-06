using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioClip deathSound;
    
    void Death()
    {
        gameObject.tag = "Untagged";

        FindObjectOfType<GameManager>().CheckEnemyCount();
        SoundManager.instance.PlaySoundFX(deathSound, 0.75f);

        foreach (Transform obj in transform)
        {
            obj.GetComponent<Rigidbody2D>().gravityScale = 1;
        }

    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Bullet")
        {
            Vector2 direction = transform.position - target.transform.position;
            if (transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale < 1)
                Death();

            GetComponent<Rigidbody2D>().AddForce(new Vector2((direction.x > 0 ? 1 : -1) * 10
                , direction.y > 0 ? .3f : -3f), ForceMode2D.Impulse);
        }
        if (target.tag == "Plank" || target.tag == "BoxPlank")
        {
            if (target.GetComponent<Rigidbody2D>().velocity.magnitude > 1.5f)
                Death();
        }

        if (target.tag == "Ground")
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude > 2)
                Death();
        }
    }
}
