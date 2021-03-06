using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    // UI references
    [SerializeField] private TMP_InputField massInput;
    [SerializeField] private Slider massSlider;
    [SerializeField] private TMP_InputField xPositionInput;
    [SerializeField] private Slider xPositionSlider;
    [SerializeField] private TMP_InputField yPositionInput;
    [SerializeField] private Slider yPositionSlider;
    [SerializeField] private TMP_InputField zPositionInput;
    [SerializeField] private Slider zPositionSlider;
    [SerializeField] private TMP_InputField xVelocityInput;
    [SerializeField] private Slider xVelocitySlider;
    [SerializeField] private TMP_InputField yVelocityInput;
    [SerializeField] private Slider yVelocitySlider;
    [SerializeField] private TMP_InputField zVelocityInput;
    [SerializeField] private Slider zVelocitySlider;

    public Transform currentSelection;
    private Material originMaterial;
    public Planet currentPlanet;

    private void Start()
    {
        // set input fields listeners
        massInput.onValueChanged.AddListener(delegate {OnMassInputChanged();});
        
        xPositionInput.onValueChanged.AddListener(delegate {OnXPosInputChanged();});
        yPositionInput.onValueChanged.AddListener(delegate {OnYPosInputChanged();});
        zPositionInput.onValueChanged.AddListener(delegate {OnZPosInputChanged();});
        
        xVelocityInput.onValueChanged.AddListener(delegate {OnXVelocityInputChanged();});
        yVelocityInput.onValueChanged.AddListener(delegate {OnYVelocityInputChanged();});
        zVelocityInput.onValueChanged.AddListener(delegate {OnZVelocityInputChanged();});
        
        // set sliders min and max values here in code so there is no need to manually set it every time we change ranges
        // register listeners for sliders
        // mass range
        massSlider.minValue = Planet.minMass;
        massSlider.maxValue = Planet.maxMass;
        massSlider.onValueChanged.AddListener(delegate{OnMassSliderChanged();});
        
        // position range
        xPositionSlider.minValue = Planet.minPos;
        xPositionSlider.maxValue = Planet.maxPos;
        xPositionSlider.onValueChanged.AddListener(delegate{OnXPosSliderChanged();});

        yPositionSlider.minValue = Planet.minPos;
        yPositionSlider.maxValue = Planet.maxPos;
        yPositionSlider.onValueChanged.AddListener(delegate{OnYPosSliderChanged();});

        zPositionSlider.minValue = Planet.minPos;
        zPositionSlider.maxValue = Planet.maxPos;
        zPositionSlider.onValueChanged.AddListener(delegate{OnZPosSliderChanged();});

        // velocity range
        xVelocitySlider.minValue = Planet.minVelocity;
        xVelocitySlider.maxValue = Planet.maxVelocity;
        xVelocitySlider.onValueChanged.AddListener(delegate{OnXVelocitySliderChanged();});

        yVelocitySlider.minValue = Planet.minVelocity;
        yVelocitySlider.maxValue = Planet.maxVelocity;
        yVelocitySlider.onValueChanged.AddListener(delegate{OnYVelocitySliderChanged();});

        zVelocitySlider.minValue = Planet.minVelocity;
        zVelocitySlider.maxValue = Planet.maxVelocity;
        zVelocitySlider.onValueChanged.AddListener(delegate{OnZVelocitySliderChanged();});
    }

    void Update()
    {
        Renderer selectionRenderer;
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))    // Check if mouse clicked
        {
            // Store mouse position in ray value
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))    // Check if raycast from mouse hit any object
            {
                // Get the selected object's renderer
                Transform selection = hit.transform;
                selectionRenderer = selection.GetComponent<Renderer>();
                
                if (selectionRenderer != null)
                {
                    // If current selection is not null then change its material back to origin one
                    if (currentSelection != null) 
                        currentSelection.GetComponent<Renderer>().material = originMaterial;
                    
                    // Save new origin material and transform as its easy way for changing it back later
                    originMaterial = selectionRenderer.material;
                    currentSelection = selection;
                    
                    // Change object's material for highlight material
                    selectionRenderer.material = highlightMaterial;
                    
                    // Get the planet from selection
                    currentPlanet = selection.gameObject.GetComponent<Planet>();
                    // Populate edit panel by planet values
                    massInput.SetTextWithoutNotify(currentPlanet.Mass.ToString());
                    massSlider.SetValueWithoutNotify(currentPlanet.Mass);
                    
                    xPositionInput.SetTextWithoutNotify(currentPlanet.Position.x.ToString());
                    xPositionSlider.SetValueWithoutNotify(currentPlanet.Position.x);
                    
                    yPositionInput.SetTextWithoutNotify(currentPlanet.Position.y.ToString());
                    yPositionSlider.SetValueWithoutNotify(currentPlanet.Position.y);

                    zPositionInput.SetTextWithoutNotify(currentPlanet.Position.z.ToString());
                    zPositionSlider.SetValueWithoutNotify(currentPlanet.Position.z);

                    xVelocityInput.SetTextWithoutNotify(currentPlanet.Velocity.x.ToString());
                    xVelocitySlider.SetValueWithoutNotify(currentPlanet.Velocity.x);

                    yVelocityInput.SetTextWithoutNotify(currentPlanet.Velocity.y.ToString());
                    yVelocitySlider.SetValueWithoutNotify(currentPlanet.Velocity.y);

                    zVelocityInput.SetTextWithoutNotify(currentPlanet.Velocity.z.ToString());
                    zVelocitySlider.SetValueWithoutNotify(currentPlanet.Velocity.z);
                }
            }
            else
            {
                // Delete latest selection if it isn't already null and the cursor is not covering ui
                if (currentSelection != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    // Get the current selection renderer and change its material back to origin one
                    selectionRenderer = currentSelection.GetComponent<Renderer>();
                    selectionRenderer.material = originMaterial;
                    currentSelection = null;
                }
            }
        }
    }

    // Onclick action for apply planet button in edit panel
    public void ApplyPlanet()
    {
        try
        {
            // If no planet selected then return
            if (currentPlanet == null) return;
        
            // Apply data to the planet here
            currentPlanet.Mass = float.Parse(massInput.text);
            currentPlanet.Position = new Vector3(float.Parse(xPositionInput.text), float.Parse(yPositionInput.text), float.Parse(zPositionInput.text));
            currentPlanet.Velocity = new Vector3(float.Parse(xVelocityInput.text), float.Parse(yVelocityInput.text), float.Parse(zVelocityInput.text));
            Orbit.applied = true;
        }
        catch (Exception e)
        {
            if (e.GetType() == Type.GetType("System.FormatException"))
                Debug.Log("Format exception, use comas instead of dots and enter a number");
            else
                Debug.Log(e);
        }
    }

    // Custom listeners for sliders which will change input text fields
    public void OnMassSliderChanged()
    {
        massInput.SetTextWithoutNotify(massSlider.value.ToString());
        currentPlanet.Mass = massSlider.value;
        Orbit.applied = true;
    }

    public void OnMassInputChanged()
    {
        currentPlanet.Mass = float.Parse(massInput.text);
        massSlider.value = currentPlanet.Mass;
        Orbit.applied = true;
    }
    public void OnXPosSliderChanged()
    {
        xPositionInput.SetTextWithoutNotify(xPositionSlider.value.ToString());
        Vector3 pos = currentPlanet.Position;
        pos[0] = xPositionSlider.value;
        currentPlanet.Position = pos;
        Orbit.applied = true;
    }

    public void OnXPosInputChanged()
    {
        Vector3 pos = currentPlanet.Position;
        pos[0] = float.Parse(xPositionInput.text);
        currentPlanet.Position = pos;
        xPositionSlider.value = pos[0];
        Orbit.applied = true;
    }
    public void OnYPosSliderChanged()
    {
        yPositionInput.SetTextWithoutNotify(yPositionSlider.value.ToString());
        Vector3 pos = currentPlanet.Position;
        pos[1] = yPositionSlider.value;
        currentPlanet.Position = pos;
        Orbit.applied = true;
    }
    public void OnYPosInputChanged()
    {
        Vector3 pos = currentPlanet.Position;
        pos[1] = float.Parse(yPositionInput.text);
        currentPlanet.Position = pos;
        yPositionSlider.value = pos[1];
        Orbit.applied = true;
    }
    public void OnZPosSliderChanged()
    {
        zPositionInput.SetTextWithoutNotify(zPositionSlider.value.ToString());
        Vector3 pos = currentPlanet.Position;
        pos[2] = zPositionSlider.value;
        currentPlanet.Position = pos;
        Orbit.applied = true;
    }
    public void OnZPosInputChanged()
    {
        Vector3 pos = currentPlanet.Position;
        pos[2] = float.Parse(zPositionInput.text);
        currentPlanet.Position = pos;
        zPositionSlider.value = pos[2];
        Orbit.applied = true;
    }
    public void OnXVelocitySliderChanged()
    {
        xVelocityInput.SetTextWithoutNotify(xVelocitySlider.value.ToString());
        Vector3 vel = currentPlanet.Velocity;
        vel[0] = xVelocitySlider.value;
        currentPlanet.Velocity = vel;
        Orbit.applied = true;
    }
    public void OnXVelocityInputChanged()
    {
        Vector3 vel = currentPlanet.Velocity;
        vel[0] = float.Parse(xVelocityInput.text);
        currentPlanet.Velocity = vel;
        xVelocitySlider.value = vel[0];
        Orbit.applied = true;
    }
    public void OnYVelocitySliderChanged()
    {
        yVelocityInput.SetTextWithoutNotify(yVelocitySlider.value.ToString());
        Vector3 vel = currentPlanet.Velocity;
        vel[1] = yVelocitySlider.value;
        currentPlanet.Velocity = vel;
        Orbit.applied = true;
    }
    public void OnYVelocityInputChanged()
    {
        Vector3 vel = currentPlanet.Velocity;
        vel[1] = float.Parse(yVelocityInput.text);
        currentPlanet.Velocity = vel;
        yVelocitySlider.value = vel[1];
        Orbit.applied = true;
    }
    public void OnZVelocitySliderChanged()
    {
        zVelocityInput.SetTextWithoutNotify(zVelocitySlider.value.ToString());
        Vector3 vel = currentPlanet.Velocity;
        vel[2] = zVelocitySlider.value;
        currentPlanet.Velocity = vel;
        Orbit.applied = true;
    }
    public void OnZVelocityInputChanged()
    {
        Vector3 vel = currentPlanet.Velocity;
        vel[2] = float.Parse(zVelocityInput.text);
        currentPlanet.Velocity = vel;
        zVelocitySlider.value = vel[2];
        Orbit.applied = true;
    }
}