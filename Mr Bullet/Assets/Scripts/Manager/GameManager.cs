using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int enemyCount = 1;

    [HideInInspector]
    public bool isGameOver;

    public int blackBullets = 3;
    public int goldenBullets = 1;

    public GameObject blackBullet, goldenBullet;

    void Awake()
    {
        FindObjectOfType<PlayerController>().ammo = blackBullets + goldenBullets;
        for (int i = 0; i < blackBullets; i++)
        {
            var blackBulletObj = Instantiate(blackBullet);
            blackBulletObj.transform.SetParent(GameObject.Find("Bullets").transform);
        }

        for (int i = 0; i < goldenBullets; i++)
        {
            var goldenBulletObj = Instantiate(goldenBullet);
            goldenBulletObj.transform.SetParent(GameObject.Find("Bullets").transform);
        }

    }

    void Update()
    {
        if (!isGameOver && FindObjectOfType<PlayerController>().ammo <= 0 && enemyCount > 0 &&  GameObject.FindGameObjectsWithTag("Bullet").Length <= 0)
        {
            isGameOver = true;
            GameUI.instance.GameOverScreen();
        }
    }

    public void CheckBullets()
    {
        if (goldenBullets > 0)
        {
            goldenBullets--;
            GameObject.FindGameObjectWithTag("GoldenBullet").SetActive(false);
        }
        else if (blackBullets > 0)
        {
            blackBullets--;
            GameObject.FindGameObjectWithTag("BlackBullet").SetActive(false);
        }
    }


    public void CheckEnemyCount()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCount <= 0)
        {
            GameUI.instance.WinScreen();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
