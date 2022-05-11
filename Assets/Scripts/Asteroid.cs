using System;
using System.CodeDom;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float initialForce = 10f;
    [SerializeField] private float initialTorque = 10f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.AddForce(Vector3.forward * initialForce);
        _rigidbody.AddTorque(Vector3.forward * initialTorque);

        float randomScale = Random.Range(1f, 5f);
        transform.localScale = Vector3.one * randomScale;
    }
}