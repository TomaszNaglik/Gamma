using UnityEngine;

public interface ICameraController
{
    Vector2 MapLimit { get; set; }
    Vector2 ZoomLimit { get; set; }
    Camera Camera { get; set; }
    void Move(Vector2 moveDir, float moveSpeed);
    void Zoom(float zoom);
}