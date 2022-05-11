using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityTemplateProjects;

public class SpaceShipShootController : MonoBehaviour
{
    [Header("--> Spaceship Settings")] 
    [SerializeField] private SpaceShipMovement spaceShip;
    
    [Header("--> Hardpoint Settings")] 
    [SerializeField] private Transform[] hardpoints;
    [SerializeField] private Transform hardpointMiddle;
    [SerializeField] private LayerMask shootableMask;
    [SerializeField] private float hardpointRange = 100f;
    [SerializeField] private bool targetInRange = false;

    [Header("--> Laser Settings")] 
    [SerializeField] private LineRenderer[] lasers;
    [SerializeField] private ParticleSystem laserHitParticles;
    [SerializeField] private float laserDamage = 5f;
    [SerializeField] private float timeBetweenDamageApplication = .25f;
    [SerializeField] private float currentTimeBetweenDamageApplication;
    [SerializeField] private float laserHeatThreshold = 2f;
    [SerializeField] private float laserHeatRate = 0.25f;
    [SerializeField] private float laserCoolRate = 0.5f;
    [SerializeField] private float currentLaserHeat = 0.25f;
    [SerializeField] private bool overHeated = false;
    [SerializeField] private bool firing = false;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleLaserFiring();
    }
    private void HandleLaserFiring()
    {
        if (firing && !overHeated)
        {
            FireLaser();
        }
        else
        {
            foreach (var laser in lasers)
            {
                laser.gameObject.SetActive(false);
            }

            CoolLaser();
        }
    }
    private void FireLaser()
    {
        if (TargetInfo.IsTargetInRange(hardpointMiddle.transform.position,hardpointMiddle.transform.forward,out var hitInfo,hardpointRange,shootableMask))
        {
            var hitObj = hitInfo.transform.gameObject;
            if (hitInfo.collider.GetComponent<IDamageble>() != null)
            {
                ApplyDamage(hitObj.GetComponent<IDamageble>());
            }
            
            Instantiate(laserHitParticles, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            foreach (var laser in lasers)
            {
                Vector3 localHitPosition = laser.transform.InverseTransformPoint(hitInfo.point);
                laser.gameObject.SetActive(true);
                laser.SetPosition(1,localHitPosition);
            }
        }
        else
        {
            foreach (var laser in lasers)
            {
                laser.gameObject.SetActive(true);
                laser.SetPosition(1, new Vector3(0,0,hardpointRange));
            }
        }

        HeatLaser();
    }

    private void ApplyDamage(IDamageble IDamageble)
    {
        currentTimeBetweenDamageApplication += Time.deltaTime;

        if (currentTimeBetweenDamageApplication >= timeBetweenDamageApplication)
        {
            currentTimeBetweenDamageApplication = 0f;
            Debug.Log("Applying Damage to Asteroid: ");
            IDamageble.TakeDamage(laserDamage);
        }
    }

    private void HeatLaser()
    {
        if (firing && currentLaserHeat < laserHeatThreshold)
        {
            currentLaserHeat += laserHeatRate * Time.deltaTime;
            if (currentLaserHeat >= laserHeatThreshold)
            {
                overHeated = true;
                firing = false;
            }
        }
    }
    private void CoolLaser()
    {
        if (overHeated)
        {
            if (currentLaserHeat / laserHeatRate <= 0.5f)
            {
                overHeated = false;
            }
        }

        if (currentLaserHeat > 0f)
        {
            currentLaserHeat -= laserCoolRate * Time.deltaTime;
        }
    }


    public void OnFire(InputAction.CallbackContext contex)
    {
        firing = contex.performed;
    }
}