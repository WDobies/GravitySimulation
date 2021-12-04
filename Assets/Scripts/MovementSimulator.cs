using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementSimulator : MonoBehaviour
{
    private Scene parallelScene;
    private PhysicsScene parallelPhysicsScene;

    public Vector3 initalVelocity;
    public GameObject mainObject;
    public GameObject plane;
    private LineRenderer lineRenderer;

    private bool mainPhysics = true;

    void Start()
    {
        Physics.autoSimulation = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 500;

        CreateSceneParameters createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        parallelScene = SceneManager.CreateScene("ParallelScene", createSceneParameters);
        parallelPhysicsScene = parallelScene.GetPhysicsScene();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            mainPhysics = false;
            Manager.TimeStep = 0;
            SimulatePhysics();
        }

        if (Input.GetMouseButtonUp(0))
        {
            mainPhysics = true;
            Manager.TimeStep = 0.01f;
            Shoot();
        }
    }

    void FixedUpdate()
    {

        if (mainPhysics)
        {
            SceneManager.GetActiveScene().GetPhysicsScene().Simulate(0.01f);
        }
        else
        {
                    
        }
    }

    void SimulatePhysics()
    {
        GameObject simulationObject = Instantiate(mainObject);
        GameObject simulationPlane = Instantiate(plane);

        SceneManager.MoveGameObjectToScene(simulationObject, parallelScene);
        SceneManager.MoveGameObjectToScene(simulationPlane, parallelScene);

        simulationObject.GetComponent<Planet>().velocity = mainObject.GetComponent<Planet>().velocity;
        simulationObject.GetComponent<Planet>().Mass = mainObject.GetComponent<Planet>().Mass;
        simulationPlane.GetComponent<Planet>().velocity = plane.GetComponent<Planet>().velocity;
        simulationPlane.GetComponent<Planet>().Mass = plane.GetComponent<Planet>().Mass;

        Planet[] simplanet = {simulationPlane.GetComponent<Planet>() };



        //parallelPhysicsScene.Simulate(0.01f);
        //simulationObject.GetComponent<Planet>().UpdateVelocity(simplanet, 0.01f);
        //simulationObject.GetComponent<Planet>().UpdatePosition(0.01f);



        Debug.Log(simulationObject.GetComponent<Planet>().Mass);
        // simulationObject.GetComponent<Rigidbody>().angularVelocity = mainObject.GetComponent<Rigidbody>().angularVelocity;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            parallelPhysicsScene.Simulate(0.01f);
            simulationObject.GetComponent<Planet>().UpdateVelocity(simplanet,0.01f);
            simulationObject.GetComponent<Planet>().UpdatePosition(0.01f);
            
            lineRenderer.SetPosition(i, simulationObject.GetComponent<Planet>().rb.position);
        }
        Destroy(simulationObject);
        Destroy(simulationPlane);
    }

    void Shoot()
    {
        //mainObject.GetComponent<Rigidbody>().velocity += initalVelocity;
    }

}