using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void OnEnable()
    {
        mainCamera = GetComponent<Camera>();
        startPosition = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;
    }
    public void ResetCamera()
    {
        mainCamera.transform.position = startPosition;
        mainCamera.transform.rotation = startRotation;
    }

}
