using UnityEngine;

namespace _Scripts.UI.ToolTips
{
    public class RotatorToCamera : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            Vector3 direction = (_camera.transform.position - transform.position).normalized;
            direction.x = 0;
            transform.rotation = Quaternion.FromToRotation(Vector3.back, direction);
        }
    }
}