using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class CameraController_Tests
    {
        [Test]
        public void Move_camera_left()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            Vector3 originalPosition = go.transform.position;
            controller.MapLimit = new Vector2(50, 25);
            controller.Move(new UnityEngine.Vector2(-1, 0), 1);
            Vector3 newPosition = go.transform.position;
            Vector3 expectedPosition = originalPosition + new Vector3(-1, 0, 0);
            Assert.AreEqual(expectedPosition, newPosition);
        }

        [Test]
        public void Move_camera_down()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            Vector3 originalPosition = go.transform.position;
            controller.MapLimit = new Vector2(50, 25);
            controller.Move(new UnityEngine.Vector2(0, -1), 1);
            Vector3 newPosition = go.transform.position;
            Vector3 expectedPosition = originalPosition + new Vector3(0, 0, -1);
            Assert.AreEqual(expectedPosition, newPosition);
        }

        [Test]
        public void Move_camera_right()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            Vector3 originalPosition = go.transform.position;
            controller.MapLimit = new Vector2(50, 25);
            controller.Move(new UnityEngine.Vector2(1, 0), 1);
            Vector3 newPosition = go.transform.position;
            Vector3 expectedPosition = originalPosition + new Vector3(1, 0, 0);
            Assert.AreEqual(expectedPosition, newPosition);
        }

        [Test]
        public void Move_camera_up()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            Vector3 originalPosition = go.transform.position;
            controller.MapLimit = new Vector2(50, 25);
            controller.Move(new UnityEngine.Vector2(0, 1), 1);
            Vector3 newPosition = go.transform.position;
            Vector3 expectedPosition = originalPosition + new Vector3(0, 0, 1);
            Assert.AreEqual(expectedPosition, newPosition);
        }

        [Test]
        public void Move_camera_outside_of_limit()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            Vector3 originalPosition = go.transform.position;
            controller.MapLimit = new Vector2(50, 25);
            controller.Move(new UnityEngine.Vector2(-100, 0), 1);
            Vector3 newPosition = go.transform.position;
            Vector3 expectedPosition = originalPosition + new Vector3(-50, 0, 0);
            Assert.AreEqual(expectedPosition, newPosition);
        }

        [Test]
        public void Zoom_in()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            controller.Camera = Camera.main;
            Vector3 originalPosition = controller.Camera.transform.position;
            controller.Zoom(1);
            Vector3 newPosition = controller.Camera.transform.position;
            
            Assert.Less(newPosition.y, originalPosition.y);
            
        }

        [Test]
        public void Zoom_in_outside_of_limit()
        {
            GameObject go = new GameObject();
            go.AddComponent<CameraController>();
            CameraController controller = go.GetComponent<CameraController>();
            controller.Camera = Camera.main;
            Vector3 originalPosition = controller.Camera.transform.position;
            controller.ZoomLimit = new Vector2(2, 100);
            controller.Zoom(1000);
            Vector3 newPosition = controller.Camera.transform.position;

            Assert.GreaterOrEqual(newPosition.y, controller.ZoomLimit.x);

        }


    }
}
