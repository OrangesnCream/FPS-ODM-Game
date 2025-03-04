using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{   
    public float xSensitivity;
    public float ySensitivity;
    public Transform orientation;
    float xRotation;
    float yRotation;
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update(){
        float mouseX=Input.GetAxisRaw("Mouse X")*Time.deltaTime*xSensitivity;
        float mouseY=Input.GetAxisRaw("Mouse Y")*Time.deltaTime*ySensitivity;

        yRotation+= mouseX;
        xRotation-= mouseY;

        xRotation=Mathf.Clamp(xRotation, -90,90f);
        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation= Quaternion.Euler(0,yRotation,0);
    }
}

