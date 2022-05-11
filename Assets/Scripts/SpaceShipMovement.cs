using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipMovement : MonoBehaviour
{
    [Header("---> Ship Movement Settings")] [SerializeField]
    private float yawTorque = 500f; // spin left and right Torque

    [SerializeField] private float pitchTorque = 1000f; // Up and Down Torque
    [SerializeField] private float rollTorque = 1000f; // rolling
    [SerializeField] private float thrust = 100f;
    [SerializeField] private float upThrust = 50f;
    [SerializeField] private float strafeThrust = 50f;

    [Header("---> Boost Settings")] 
    [SerializeField] private bool boosting = false;
    [SerializeField] private float currentBoostAmount;
    [SerializeField] private float maxBoostAmount = 2f;
    [SerializeField] private float boostDeprecationRate = 0.25f;
    [SerializeField] private float boostRechargeRate = 0.5f;
    [SerializeField] private float boostMultiplier = 5f;


    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 8.999f)]
    private float leftRightGlideReduction = 0.111f;

    [SerializeField] private CinemachineVirtualCamera shipThirdPersonCam;
    [SerializeField] private CinemachineVirtualCamera shipFirstPersonCam;

    private float _glide = 0f;
    private float _verticalGlide = 0f;
    private float _horizontalGlide = 0f;

    private float _thrust1D;
    private float _upDown1D;
    private float _strafe1D;
    private float _roll1D;
    private Vector2 _pitchYaw;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        currentBoostAmount = maxBoostAmount;
        CinemachineCameraSwitcher.IsActiveCamera(shipFirstPersonCam);
        CinemachineCameraSwitcher.SwitchCamera(shipFirstPersonCam);
    }

    private void OnEnable()
    {
        CinemachineCameraSwitcher.RegisterCamera(shipFirstPersonCam);
        CinemachineCameraSwitcher.RegisterCamera(shipThirdPersonCam);
    }

    private void OnDisable()
    {
        CinemachineCameraSwitcher.UnRegisterCamera(shipFirstPersonCam);
        CinemachineCameraSwitcher.UnRegisterCamera(shipThirdPersonCam);
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleBoosting();
    }

    private void HandleBoosting()
    {
        if (boosting && currentBoostAmount > 0f)
        {
            currentBoostAmount -= boostDeprecationRate;
            if (currentBoostAmount <= 0f)
            {
                boosting = false;
            }
        }
        else
        {
            if (currentBoostAmount < maxBoostAmount)
            {
                currentBoostAmount += boostRechargeRate;
            }
        }
    }

    private void HandleMovement()
    {
        //Roll
        _rigidbody.AddRelativeTorque(Vector3.back * _roll1D * rollTorque * Time.deltaTime);
        //Pitch
        _rigidbody.AddRelativeTorque(Vector3.right * Mathf.Clamp(-_pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        //Yaw
        _rigidbody.AddRelativeTorque(Vector3.up * Mathf.Clamp(_pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);

        //Thrust
        if (_thrust1D > 0.1f || _thrust1D < -0.1f)
        {
            float currentThrust = thrust;
            if (boosting)
            {
                currentThrust = thrust * boostMultiplier;
            }
            else
            {
                currentThrust = thrust;
            }

            _rigidbody.AddRelativeForce(Vector3.forward * _thrust1D * currentThrust * Time.deltaTime);
            _glide = thrust;
        }
        else
        {
            _rigidbody.AddRelativeForce(Vector3.forward * _glide * Time.deltaTime);
            _glide *= thrustGlideReduction;
        }

        // UP/DOWN
        if (_upDown1D > 0.1f || _upDown1D < -0.1f)
        {
            _rigidbody.AddRelativeForce(Vector3.up * _upDown1D * upThrust * Time.fixedDeltaTime);
            _verticalGlide = _upDown1D * upThrust;
        }
        else
        {
            _rigidbody.AddRelativeForce(Vector3.up * _verticalGlide * Time.fixedDeltaTime);
            _verticalGlide *= upDownGlideReduction;
        }

        // STRAFING
        if (_strafe1D > 0.1f || _strafe1D < -0.1f)
        {
            _rigidbody.AddRelativeForce(Vector3.right * _strafe1D * upThrust * Time.fixedDeltaTime);
            _horizontalGlide = _strafe1D * strafeThrust;
        }
        else
            _rigidbody.AddRelativeForce(Vector3.right * _horizontalGlide * Time.fixedDeltaTime);

        _horizontalGlide *= leftRightGlideReduction;
    }

    public void OnThrust(InputAction.CallbackContext context) => _thrust1D = context.ReadValue<float>();

    public void OnStrafe(InputAction.CallbackContext context) => _strafe1D = context.ReadValue<float>();

    public void OnUpDown(InputAction.CallbackContext context) => _upDown1D = context.ReadValue<float>();

    public void OnRoll(InputAction.CallbackContext context) => _roll1D = context.ReadValue<float>();

    public void OnPitchYaw(InputAction.CallbackContext context) => _pitchYaw = context.ReadValue<Vector2>();

    public void OnBoost(InputAction.CallbackContext context) => boosting = context.performed;

    public void OnSwitchCamera(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (CinemachineCameraSwitcher.IsActiveCamera(shipFirstPersonCam))
                CinemachineCameraSwitcher.SwitchCamera(shipThirdPersonCam);
            
            else if (CinemachineCameraSwitcher.IsActiveCamera(shipThirdPersonCam))
                CinemachineCameraSwitcher.SwitchCamera(shipFirstPersonCam);
        }
    }
}