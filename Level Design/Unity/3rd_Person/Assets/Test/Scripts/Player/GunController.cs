using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public BulletController bullet;
    public float bulletSpeed;
    public bool isFiring;
    public float timeBetweenShots;
    private float shotCounter;
    public int currentAmo = 10;
    public int MaxAmmo = 10;
    public Text ammoCounter;

    public Transform firePoint;

    void Start()
    {
        MaxAmmo = currentAmo;
    }

    public void Update()
    {
        if (isFiring && currentAmo > 0)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                BulletController newShot = Instantiate(bullet, firePoint.position, firePoint.rotation);
                newShot.speed = bulletSpeed;
                currentAmo -= 1;
            }
        }
        else
        {
            shotCounter = 0;
        }

        ammoCounter.text = "Ammo: "+  currentAmo.ToString() + "/" + MaxAmmo.ToString();
    }
}
