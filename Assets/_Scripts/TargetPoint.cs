using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private Transform _nextPoint;
    [SerializeField] private bool _isLastPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ProbaMonstr probaMonstr))
        {
            if (_isLastPoint == false)
                probaMonstr.SetTargetPoint(_nextPoint);
            else
                probaMonstr.SetTargetPoint(probaMonstr.transform);
        }
    }
}
