using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    //TO FIX
    #region Range

    static public float maxInScreenX = 40;
    static public float maxInScreenY = 24;
    
    static public float minMass = 0.0001f;
    static public float maxMass = 30000f;
    static public float rocketMass = 0.01f;
    static public float sunMass = 2500000f;

    static public float minPos = -350f; // x,y,z
    static public float maxPos = 350f;
    static public Vector3 sunPos = new Vector3(0.0f, 0.0f, 0.0f);

    static public float minVelocity = -50f;
    static public float maxVelocity = 50f;
    static public Vector3 sunVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    #endregion
    
    [SerializeField] private bool isExample = false;
    [SerializeField] private float e_Mass;
    [SerializeField] private Vector3 e_pos;
    [SerializeField] private Vector3 e_velocity;

    private Vector3 _position;
    float _currentRadius;
    protected float _mass;
    public Rigidbody rb;

    public float Mass 
    { get => _mass; 

      set 
        {
            rb.mass = _mass = value;
        } 
    } // 10^24 kg (1 - 2000) range 

    public Vector3 Velocity { get; set; } // km/s
    public Vector3 Position
    {
        get => _position;

        set
        {
            rb.position = _position = value;
        }
    }

    private void OnEnable()
    {
        if (!isExample)
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = _mass;
            GenerateRandomProperties();
        }
        else
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = _mass;
            Mass = e_Mass;
            Position = e_pos;
            Velocity = e_velocity;
        }
    }

    private void Update()
    {
        float radius;
        
        if(gameObject.name == "sun")
            radius = 11;
        else
        {
            radius = Mathf.InverseLerp(minMass, maxMass, Mass) * 10;
            if (radius < 1.5) radius = 1.5f;
            if (radius > 7) radius = 7;
        }

        if (radius != _currentRadius)
        {
            _currentRadius = radius;
            transform.localScale = new Vector3(_currentRadius, _currentRadius, _currentRadius);
        }
    }

    protected void GenerateRandomProperties()
    {
        if (gameObject.name == "rocket")
            Mass = rocketMass;
        else 
            Mass = Random.Range(minMass, maxMass);
        
        Position = new Vector3(Random.Range(-maxInScreenX, maxInScreenX), Random.Range(-maxInScreenY, maxInScreenY), Random.Range(-maxInScreenX, maxInScreenX));
        Velocity = Vector3.zero; //to fix
    }

    public void UpdateVelocity(Planet[] allPlanets, float timeStep)
    {
        foreach (var planet in allPlanets)
        {
            if(planet != this && planet.gameObject.activeInHierarchy)
            {
                float sqrDst = (planet.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (planet.rb.position - rb.position).normalized; 
                Vector3 acceleration = forceDir * Manager.GravityConstant * planet._mass / sqrDst;  //gravity Const
                Velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        Position += Velocity * timeStep;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (this.gameObject.name != "sun" && Manager.TimeStep != 0)
        {
            if (other.gameObject.name == "sun")
            {
                // If planet collides with sun, deactivate it
                this.gameObject.SetActive(false);
            }
        }
    }
}