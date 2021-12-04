using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orbit : MonoBehaviour
{
    private Scene parallelScene;
    private PhysicsScene parallelPhysicsScene;

    public Vector3 initalVelocity;
    public Planet mainObject;
    public GameObject plane;
    public GameObject plane2;
    private LineRenderer lineRenderer;
    private bool _rendered;

    public bool mainPhysics = true;

    void Start()
    {
        Physics.autoSimulation = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 500;

        CreateSceneParameters createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        parallelScene = SceneManager.CreateScene("ParallelScene", createSceneParameters);
        parallelPhysicsScene = parallelScene.GetPhysicsScene();

    }

    void FixedUpdate()
    {
        Debug.Log(_rendered);
        if (mainPhysics)
        {
            lineRenderer.enabled = false;
            _rendered = false;
            SceneManager.GetActiveScene().GetPhysicsScene().Simulate(0.01f);
        }
        else
        {
            if (FindObjectOfType<SelectionManager>().currentPlanet)
            {
                mainObject = FindObjectOfType<SelectionManager>().currentPlanet;
            }
            lineRenderer.enabled = true;
            if (!_rendered)
            {
                SimulatePhysics();
            }
        }
    }

    public void SimulatePhysics()
    {
        if (mainObject != null)
        {
            GameObject simulationObject = Instantiate(mainObject.gameObject);
            GameObject simulationPlane = Instantiate(plane);
            GameObject simulationPlane2 = Instantiate(plane2);

            SceneManager.MoveGameObjectToScene(simulationObject, parallelScene);
            SceneManager.MoveGameObjectToScene(simulationPlane, parallelScene);
            SceneManager.MoveGameObjectToScene(simulationPlane2, parallelScene);

            simulationObject.GetComponent<Planet>().velocity = mainObject.GetComponent<Planet>().velocity;
            simulationObject.GetComponent<Planet>().Mass = mainObject.GetComponent<Planet>().Mass;
            simulationPlane.GetComponent<Planet>().velocity = plane.GetComponent<Planet>().velocity;
            simulationPlane.GetComponent<Planet>().Mass = plane.GetComponent<Planet>().Mass;
            simulationPlane2.GetComponent<Planet>().velocity = plane2.GetComponent<Planet>().velocity;
            simulationPlane2.GetComponent<Planet>().Mass = plane2.GetComponent<Planet>().Mass;

            Planet[] simplanet = { simulationPlane.GetComponent<Planet>(), simulationObject.GetComponent<Planet>(), simulationPlane2.GetComponent<Planet>() };

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                parallelPhysicsScene.Simulate(0.01f);
                for (int j = 0; j < simplanet.Length; j++)
                {
                    simplanet[j].UpdateVelocity(simplanet, 0.01f);
                }

                for (int j = 0; j < simplanet.Length; j++)
                {
                    simplanet[j].UpdatePosition(0.01f);
                }
                //simulationObject.GetComponent<Planet>().UpdateVelocity(simplanet, 0.01f);
                //simulationObject.GetComponent<Planet>().UpdatePosition(0.01f);

                lineRenderer.SetPosition(i, simulationObject.GetComponent<Planet>().rb.position);
            }
            Destroy(simulationObject);
            Destroy(simulationPlane);
            Destroy(simulationPlane2);

            _rendered = true;
        }
       
    }
}
