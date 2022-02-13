using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float rotateSpeed = 100, bulletSpeed = 100;
    public int ammo = 3;

    public Transform handPosition;
    private Transform firePosition1, firePosition2;

    private LineRenderer lineRenderer;

    public GameObject bullet;

    private GameObject crosshair;

    public AudioClip shotSound;

    void Awake()
    {
        crosshair = GameObject.Find("Crosshair");
        crosshair.SetActive(false); 
        firePosition1 = GameObject.Find("FirePos1").transform;
        firePosition2 = GameObject.Find("FirePos2").transform;
        lineRenderer = GameObject.Find("Gun").GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!IsMouseOverUI())
        {
            if (Input.GetMouseButton(0))
            {
                Aim();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (ammo > 0)
                    Shoot();
                else
                {
                    lineRenderer.enabled = false;
                    crosshair.SetActive(false);
                }
            }
        }
        
    }

    void Aim()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - handPosition.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        handPosition.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition1.position);
        lineRenderer.SetPosition(1, firePosition2.position);
        crosshair.SetActive(true);
        crosshair.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
    }

    void Shoot()
    {
        crosshair.SetActive(false);
        lineRenderer.enabled = false;

        GameObject newBullet = Instantiate(bullet, firePosition1.position, Quaternion.identity);

        if (transform.localScale.x > 0)
            newBullet.GetComponent<Rigidbody2D>().AddForce(firePosition1.right * bulletSpeed, ForceMode2D.Impulse);
        else
            newBullet.GetComponent<Rigidbody2D>().AddForce(-firePosition1.right * bulletSpeed, ForceMode2D.Impulse);

        ammo--;
        FindObjectOfType<GameManager>().CheckBullets();
        SoundManager.instance.PlaySoundFX(shotSound, 0.3f);

        Destroy(newBullet, 2);
    }

    bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
