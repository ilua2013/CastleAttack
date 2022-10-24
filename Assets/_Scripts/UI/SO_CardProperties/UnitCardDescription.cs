using UnityEngine;

[CreateAssetMenu(fileName = "Unit Card", menuName = "Cards/Unit Cards/New Unit Card")]
public class UnitCardDescription : CardDescription
{
    [SerializeField] private GameObject _unitTemplate;

    public GameObject UnitTemplate => _unitTemplate;
}
