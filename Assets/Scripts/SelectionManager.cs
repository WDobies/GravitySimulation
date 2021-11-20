using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    private Transform currentSelection;
    private Material originMaterial;
    
    void Update()
    {
        Renderer selectionRenderer;
        RaycastHit hit;

        // Delete latest selection
        if (currentSelection != null)
        {
            selectionRenderer = currentSelection.GetComponent<Renderer>();
            selectionRenderer.material = originMaterial;
            currentSelection = null;
        }
        
        // Store mouse position in ray value
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Return if mouse doesnt intersect with any object, store hit
        if (!Physics.Raycast(ray, out hit)) return;
        
        // If mouse intersect with any object then change object material
        Transform selection = hit.transform;
        selectionRenderer = selection.GetComponent<Renderer>();
        if (selectionRenderer != null)
        {
            originMaterial = selectionRenderer.material;
            selectionRenderer.material = highlightMaterial;
            currentSelection = selection;
        }
    }
}