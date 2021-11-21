using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    [SerializeField] private Vector2 _sensivity;
    private Vector2 _rotation;
    private Vector2 _mainSensivity;
    [SerializeField] private float _scrollSensivity;

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        return input;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _mainSensivity = GetInput() * _sensivity;
            _rotation += _mainSensivity * Time.deltaTime;
            transform.localEulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            transform.position += new Vector3 (0, 0, _scrollSensivity * Time.deltaTime);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            transform.position += new Vector3(0, 0, -_scrollSensivity * Time.deltaTime);
        }
        
    }
}
