using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orbit : MonoBehaviour
{
    static public bool mainPhysics = true;
    public Planet mainObject;
    public Planet[] planets;

    private Scene _parallelScene;
    private PhysicsScene _parallelPhysicsScene;
    private LineRenderer _lineRenderer;
    private bool _rendered;
    private SelectionManager _selectionManager;

    void Start()
    {
        Physics.autoSimulation = false;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 500;

        CreateSceneParameters createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        _parallelScene = SceneManager.CreateScene("ParallelScene", createSceneParameters);
        _parallelPhysicsScene = _parallelScene.GetPhysicsScene();

        _selectionManager = FindObjectOfType<SelectionManager>();
    }


    void FixedUpdate()
    {
        if (mainPhysics)
        {
            _lineRenderer.enabled = false;
            _rendered = false;
            SceneManager.GetActiveScene().GetPhysicsScene().Simulate(0.01f);
        }
        else
        {
            if(mainObject!= _selectionManager.currentPlanet)
            {
                _rendered = false;
            }
            
            mainObject = _selectionManager.currentPlanet;
            planets = Manager.planets;
            
            if (!_rendered)
            {                
                _lineRenderer.enabled = true;
                SimulatePhysics();
            }
        }
    }

    public void SimulatePhysics()
    {
        if (mainObject != null)
        {
            GameObject[] simulationGameObjects = new GameObject[planets.Length];
            Planet[] simulationPlanets = new Planet[planets.Length];

            for (int i = 0; i < planets.Length; i++)
            {
                simulationGameObjects[i] = Instantiate(planets[i].gameObject);
                SceneManager.MoveGameObjectToScene(simulationGameObjects[i], _parallelScene);
                simulationGameObjects[i].GetComponent<Planet>().velocity = planets[i].GetComponent<Planet>().velocity;
                simulationGameObjects[i].GetComponent<Planet>().Mass = planets[i].GetComponent<Planet>().Mass;
                simulationGameObjects[i].GetComponent<Planet>().Position = planets[i].GetComponent<Planet>().Position;
                simulationPlanets[i] = simulationGameObjects[i].GetComponent<Planet>();
            }

            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _parallelPhysicsScene.Simulate(0.01f);

                for (int j = 0; j < simulationPlanets.Length; j++)
                {
                    simulationPlanets[j].UpdateVelocity(simulationPlanets, 0.01f);
                }

                for (int j = 0; j < simulationPlanets.Length; j++)
                {
                    simulationPlanets[j].UpdatePosition(0.01f);
                }

                for (int k = 0; k < simulationPlanets.Length; k++)
                {
                    if (planets[k].gameObject == mainObject.gameObject)
                    {
                        _lineRenderer.SetPosition(i, simulationPlanets[k].Position);
                    }
                }               
            }

            foreach (var item in simulationGameObjects)
            {
                Destroy(item);
            }

            _rendered = true;
        }
       
    }
}
