using UnityEngine;

namespace Presentation
{
    public class GameInput : MonoBehaviour
    {
        public GameObject topRightGameObject;
        public static bool isPressed;
        private static Vector2 _mousePosition;
        public static float RealSize;
        public static float RealWidth;
        private static Ray _ray;
        private static RaycastHit _hit;

        public static string MouseOnBlockName()
        {
            if (Camera.main != null) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(_ray, out _hit) ? _hit.collider.name : null;
        }

        public static Vector3 MouseWorldPosition()
        {
            var y = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            y.z = 0;
            return y;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) isPressed = true;
            if (Input.GetMouseButtonUp(0)) isPressed = false;

            _mousePosition = Input.mousePosition;
            var p1 = topRightGameObject.GetComponent<Transform>().position;
            RealWidth = p1.x;
            RealSize = RealWidth * 50f / 720f;
        }
    }
}
