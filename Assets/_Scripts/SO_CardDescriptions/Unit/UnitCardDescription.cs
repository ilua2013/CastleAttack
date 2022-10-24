using UnityEngine;

public class UnitCardDescription : CardDescription
{
    [SerializeField] private GameObject _unitTemplate;

    public GameObject UnitTemplate => _unitTemplate;
}
