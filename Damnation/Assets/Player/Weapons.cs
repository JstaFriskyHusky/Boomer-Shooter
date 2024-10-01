using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapons : MonoBehaviour
{
    //gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazine, roundsPerTrigger;
    public bool automatic;
    int rounds, spentRounds;

    //bools
    bool shooting, readyToShoot, reloading;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    

    private void Awake()
    {
        rounds = magazine;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

    }

    private void MyInput()
    {
        if (automatic) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && spentRounds < magazine && !reloading) Reload();

        //shoot
        if (readyToShoot && shooting && !reloading && rounds > 0) {
            spentRounds = roundsPerTrigger;
            Shoot();
        }

        void Shoot()
        {
            readyToShoot = false;

            //spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //calculate direction with spread
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            //raycast
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
            {
                Debug.Log(rayHit.collider.name);

                
            }
            rounds--;
            spentRounds--;

            Invoke("ResetShot", timeBetweenShooting);

            if (spentRounds > 0 && rounds > 0)
            Invoke("Shoot", timeBetweenShots);
        }
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
        rounds = magazine;
        reloading = false;
    }
    

}
