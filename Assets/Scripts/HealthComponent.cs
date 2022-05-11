using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEngine;
using UnityTemplateProjects;

public abstract class HealthComponent : MonoBehaviour
{
    [SerializeField] private float healt;
    public float Healt
    {
        get { return healt; }
        set { healt = value; }
    }
}