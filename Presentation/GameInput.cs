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

        public static bool IsMouseOnBlock(float x, float y)
        {
            if (_mousePosition.x < x - RealSize || _mousePosition.x > x + RealSize) return false;
            if (_mousePosition.y < y - RealSize || _mousePosition.y > y + RealSize) return false;
            return true;
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
