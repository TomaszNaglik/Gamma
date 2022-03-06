
using System;
using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController
{
    [SerializeField]
    private float ScreenBorder = 0.05f;
    [SerializeField]
    private int MoveSpeed = 1;
    private Vector2 mouse;
    private Vector2 movementDirection;
    private Vector2 screenDimensions;
    private Vector3 movementTranslation;
    private Vector3 newPosition;

    private Vector2 mapLimit;

    private float zoom;
    private Vector2 zoomLimit;

    private new Camera camera;
    private Transform cameraTransform;

    public Vector2 MapLimit { get => mapLimit; set => mapLimit = value; }
    public Camera Camera { get => camera; set => camera = Camera.main; }
    public Vector2 ZoomLimit { get => zoomLimit; set => zoomLimit = value; }

    private void Awake()
    {
        
        mouse = Input.mousePosition;
        movementDirection = new Vector2();
        screenDimensions = new Vector2();
        movementTranslation = new Vector3();
        newPosition = new Vector3();
        MapLimit = new Vector2(50, 25);
        ZoomLimit = new Vector2(0.5f, 10);
        Camera = Camera.main;

    }


    void Update()
    {
        GatherInput();
        Move(movementDirection, MoveSpeed);
        Zoom(zoom);

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
    public void Move(Vector2 moveDir, float _moveSpeed)
    {
        newPosition = transform.position;
        movementTranslation.x = moveDir.x * _moveSpeed;
        movementTranslation.z = moveDir.y * _moveSpeed;

        newPosition += movementTranslation;
        newPosition.x = Mathf.Clamp(newPosition.x, -MapLimit.x, MapLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -MapLimit.y, MapLimit.y);

        transform.position = newPosition;

        //print("Position: " + transform.position);

    }
    public void Zoom(float _zoom)
    {
        cameraTransform = Camera.transform;
        cameraTransform.Translate(0, 0, _zoom);
        while (cameraTransform.position.y < ZoomLimit.x)
        {
            cameraTransform.Translate(0, 0, -_zoom / 10);
        }
        Camera.transform.position = cameraTransform.position;

    }
}
