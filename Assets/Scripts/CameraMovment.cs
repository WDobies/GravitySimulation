using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    public float maxMouseSensivity;
    public float minMouseSensivity;
    public float mouseAcceleration;

    public float minScrollSensivity;
    public float scrollAcceleration;

    private Vector2 _mouseSensivity;
    private Vector2 _rotation;
    private Vector2 _mainSensivity;
    private float _scrollSensivity;
    

    private Vector2 GetInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        return input;
    }

    private void Update()
    {
        //mouseRotation
        if (Input.GetMouseButton(1))
        {
            _mainSensivity = GetInput() * _mouseSensivity;
            if (_mouseSensivity.x < maxMouseSensivity)
            {
                _mouseSensivity += new Vector2(mouseAcceleration, -mouseAcceleration);
            }
            
            _rotation += _mainSensivity * Time.deltaTime;
            transform.localEulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        }
        else
        {
            _mouseSensivity =  new Vector2(minMouseSensivity, -minMouseSensivity);
        }

        //Mouse Zoom
        if (Input.mouseScrollDelta.y > 0)
        {
            transform.position += transform.forward * _scrollSensivity * Time.deltaTime;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            transform.position -= transform.forward * _scrollSensivity * Time.deltaTime;
        }

        //Camera movment
        if (!KeyInput())
        {
            _scrollSensivity = minScrollSensivity;
        }
    }

    private bool KeyInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * _scrollSensivity * Time.deltaTime;
            _scrollSensivity += scrollAcceleration;
            return true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * _scrollSensivity * Time.deltaTime;
            _scrollSensivity += scrollAcceleration;
            return true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * _scrollSensivity * Time.deltaTime;
            _scrollSensivity += scrollAcceleration;
            return true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * _scrollSensivity * Time.deltaTime;
            _scrollSensivity += scrollAcceleration;
            return true;
        }

        return false;
    }
}
