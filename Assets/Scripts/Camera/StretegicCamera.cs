using System;
using UnityEngine;

public class StretegicCamera : MonoBehaviour
{

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MovementTime;
    [SerializeField] private float RotationAmount;
    [SerializeField] private Vector3 ZoomAmount;
    [SerializeField] private float MaxZoom;
    [SerializeField] private float MinZoom;
    
    private Vector3 NewZoom;    
    private Vector3 NewPosition;

    
   
    private Quaternion NewRotation;
    private Transform CameraTransform;

    private Vector3 DragStartPosition;
    private Vector3 DragCurrentPosition;
    private Plane MouseReferencePlane;

    private Vector3 RotateStartPosition;
    private Vector3 RotateCurrentPosition;


    void Awake()
    {
        NewPosition = transform.position;
        NewRotation = transform.rotation;

        CameraTransform = Camera.main.transform;
        NewZoom = CameraTransform.localPosition;

        MouseReferencePlane = new Plane(Vector3.up, Vector3.zero);

        
    }
    
	void Start()
    {
        
    }

    void Update()
    {
        GetMouseInput();
        GetKeyboardInput();
        ValidateInput();
        ApplyCameraTransformation();
        
        
    }

    private void ValidateInput()
    {
        if (NewZoom.y < -MaxZoom)
        {
            NewZoom.y = -MaxZoom;
            NewZoom.z = MaxZoom;
        }

        if (NewZoom.y > -MinZoom)
        {
            NewZoom.y = -MinZoom;
            NewZoom.z = MinZoom;
        }

        
    }

    private void ApplyCameraTransformation()
    {

        transform.position = Vector3.Lerp(transform.position, NewPosition, Time.deltaTime * MovementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, Time.deltaTime * MovementTime);
        CameraTransform.localPosition = Vector3.Lerp(CameraTransform.localPosition, NewZoom, Time.deltaTime * MovementTime);
    }

    private void GetKeyboardInput()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            NewPosition += (transform.forward * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            NewPosition += (transform.forward * -MovementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            NewPosition += (transform.right * -MovementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            NewPosition += (transform.right * MovementSpeed);
        }

        


        if (Input.GetKey(KeyCode.Q))
        {
            NewRotation *= Quaternion.Euler(Vector3.up * RotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            NewRotation *= Quaternion.Euler(Vector3.up * -RotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            NewZoom += ZoomAmount;
            
        }
        if (Input.GetKey(KeyCode.T))
        {
            NewZoom -= ZoomAmount;
        }
       


        
    }

    private void GetMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            NewZoom += Input.mouseScrollDelta.y * ZoomAmount;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if(MouseReferencePlane.Raycast(ray,out entry))
            {
                DragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (MouseReferencePlane.Raycast(ray, out entry))
            {
                DragCurrentPosition = ray.GetPoint(entry);
                NewPosition = transform.position + DragStartPosition - DragCurrentPosition;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            RotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            RotateCurrentPosition = Input.mousePosition;

            Vector3 difference = RotateStartPosition - RotateCurrentPosition;
            RotateStartPosition = RotateCurrentPosition;

            NewRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }
}
