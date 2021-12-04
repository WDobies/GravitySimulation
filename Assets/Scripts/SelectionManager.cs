using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    // UI references
    [SerializeField] private TMP_InputField massInput;
    [SerializeField] private TMP_InputField xPositionInput;
    [SerializeField] private TMP_InputField yPositionInput;
    [SerializeField] private TMP_InputField zPositionInput;
    [SerializeField] private TMP_InputField velocityValueInput;
    [SerializeField] private TMP_InputField xVelocityInput;
    [SerializeField] private TMP_InputField yVelocityInput;
    [SerializeField] private TMP_InputField zVelocityInput;
        
    private Transform currentSelection;
    private Material originMaterial;
    public Planet currentPlanet;

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
                    xPositionInput.SetTextWithoutNotify(currentPlanet.Position.x.ToString());
                    yPositionInput.SetTextWithoutNotify(currentPlanet.Position.y.ToString());
                    zPositionInput.SetTextWithoutNotify(currentPlanet.Position.z.ToString());
                    velocityValueInput.SetTextWithoutNotify(currentPlanet.Velocity.ToString());
                    xVelocityInput.SetTextWithoutNotify(currentPlanet.VelocityDirection.x.ToString());
                    yVelocityInput.SetTextWithoutNotify(currentPlanet.VelocityDirection.y.ToString());
                    zVelocityInput.SetTextWithoutNotify(currentPlanet.VelocityDirection.z.ToString());
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
            currentPlanet.Velocity = float.Parse(velocityValueInput.text);
            currentPlanet.VelocityDirection = new Vector3(float.Parse(xVelocityInput.text), float.Parse(yVelocityInput.text), float.Parse(zVelocityInput.text));
        }
        catch (Exception e)
        {
            if (e.GetType() == Type.GetType("System.FormatException"))
                Debug.Log("Format exception, use comas instead of dots and enter a number");
            else
                Debug.Log(e);
        }
    }
}