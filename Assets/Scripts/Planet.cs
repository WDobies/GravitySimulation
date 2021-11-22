using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    //TO FIX
    #region Range
    static public float minMass = 1f;
    static public float maxMass = 2000f;

    static public float minPos = 0f; // x,y,z
    static public float maxPos = 5f;

    static public float minVelocity = 0f;
    static public float maxVelocity = 10f;

    static public float minVelocityDirction = 0f; // x,y,z
    static public float maxVelocityDirction = 360f;

    #endregion

    private Vector3 _position;
    private float _currentRadius;

    public float Mass { get; set; } // 10^24 kg (1 - 2000) range 
    public float Velocity { get; set; } // km/s
    public Vector3 VelocityDirection { get; set; } //rotation
    public Vector3 Position
    {
        get => _position;

        set
        {
            transform.position = _position = value;
        }
    }

    private void Awake()
    {
        GenerateRandomProperties();
    }

    private void Update()
    {
        float radius = Mathf.InverseLerp(1, 2000, Mass) * 10;
        if (radius != _currentRadius)
        {
            _currentRadius = radius;
            transform.localScale = new Vector3(_currentRadius, _currentRadius, _currentRadius);
        }
    }

    private void GenerateRandomProperties()
    {
        Mass = Random.Range(minMass, maxMass);
        Position = new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), Random.Range(minPos, maxPos));
        Velocity = 1; //to fix
        VelocityDirection = Vector3.one; //to fix
    }

}