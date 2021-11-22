using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
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
    [SerializeField] private TMP_InputField xVelocityInput;
    [SerializeField] private TMP_InputField yVelocityInput;
    [SerializeField] private TMP_InputField zVelocityInput;
        
    private Transform currentSelection;
    private Material originMaterial;

    // TODO: Onclick action for the apply button to edit planet
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
                    Planet planet = selection.gameObject.GetComponent<Planet>();
                    // Populate edit panel by planet values
                    massInput.SetTextWithoutNotify(planet.Mass.ToString());
                    xPositionInput.SetTextWithoutNotify(planet.Position.x.ToString());
                    yPositionInput.SetTextWithoutNotify(planet.Position.y.ToString());
                    zPositionInput.SetTextWithoutNotify(planet.Position.z.ToString());
                    xVelocityInput.SetTextWithoutNotify(planet.VelocityDirection.x.ToString());
                    yVelocityInput.SetTextWithoutNotify(planet.VelocityDirection.y.ToString());
                    zVelocityInput.SetTextWithoutNotify(planet.VelocityDirection.z.ToString());
                }
            }
            else
            {
                // TODO: Ignore ui raycast
                
                // Delete latest selection
                if (currentSelection != null)
                {
                    // Get the current selection renderer and change its material back to origin one
                    selectionRenderer = currentSelection.GetComponent<Renderer>();
                    selectionRenderer.material = originMaterial;
                    currentSelection = null;
                }
            }
        }
    }
}