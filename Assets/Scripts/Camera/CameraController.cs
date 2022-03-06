
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float ScreenBorder;
    [SerializeField]
    private int MoveSpeed;
    private Vector2 mouse;
    private Vector2 movementDirection;
    private Vector2 screenDimensions;
    private Vector3 movementTranslation;
    private Vector3 newPosition;
    
    private Vector2 MapLimit;

    private float zoom;
    private Vector2 zoomClamp;

    private Camera camera;
    private Transform cameraTransform;

    private void Awake()
    {
        mouse = Input.mousePosition;
        movementDirection = new Vector2();
        screenDimensions = new Vector2();
        movementTranslation = new Vector3();
        newPosition = new Vector3();
        MapLimit = new Vector2(50, 25);
        zoomClamp = new Vector2(0.5f, 10);
        camera = Camera.main;

    }

    void Start()
    {
        
    }

    
    void Update()
    {
        GatherInput();
        Move();
        Zoom();

    }


    private void GatherInput()
    {
        screenDimensions.x = (float)Screen.width;
        screenDimensions.y = (float)Screen.height;
        mouse = Input.mousePosition / screenDimensions;
        movementDirection.x = 0;
        movementDirection.y = 0;

        //detect if movement needed
        if (mouse.x < ScreenBorder)
        {
            movementDirection.x = -1;

        }

        else if (mouse.x > 1 - ScreenBorder)
        {
            movementDirection.x = 1;

        }
        if (mouse.y < ScreenBorder)
        {
            movementDirection.y = -1;

        }

        else if (mouse.y > 1 - ScreenBorder)
        {
            movementDirection.y = 1;

        }

        zoom = Input.mouseScrollDelta.y;

        //print("Movement direction: " + movementDirection + "  Zoom: " + zoom);
    }
    private void Move()
    {
        newPosition = transform.position;
        movementTranslation.x = movementDirection.x * MoveSpeed;
        movementTranslation.z = movementDirection.y * MoveSpeed;
        
        newPosition += movementTranslation;
        newPosition.x = Mathf.Clamp(newPosition.x, -MapLimit.x, MapLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -MapLimit.y, MapLimit.y);

        transform.position = newPosition;

        //print("Position: " + transform.position);
       
    }

    private void Zoom()
    {
        cameraTransform = camera.transform;
        cameraTransform.Translate(0, 0, zoom);
        while(cameraTransform.position.y < zoomClamp.x)
        {
            cameraTransform.Translate(0, 0, -zoom/10);
        }
        /*while (cameraTransform.position.y > zoomClamp.y)
        {
            cameraTransform.Translate(0, 0, zoom / 10);
        }*/
        camera.transform.position = cameraTransform.position;
        
    }
}
