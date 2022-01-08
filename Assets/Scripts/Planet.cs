using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    //TO FIX
    #region Range
    static public float minMass = 1f;
    static public float maxMass = 2000f;
    static public float sunMass = 400000f;

    static public float minPos = -200f; // x,y,z
    static public float maxPos = 200f;
    static public Vector3 sunPos = new Vector3(0.0f, 0.0f, 0.0f);

    static public float minVelocity = -100f;
    static public float maxVelocity = 100f;
    static public Vector3 sunVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    #endregion

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = _mass;
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

    protected void GenerateRandomProperties()
    {
        Mass = Random.Range(minMass, maxMass);
        Position = new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), Random.Range(minPos, maxPos));
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
}