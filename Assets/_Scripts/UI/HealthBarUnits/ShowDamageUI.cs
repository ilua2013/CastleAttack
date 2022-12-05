using UnityEngine;
using TMPro;

public class ShowDamageUI : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _iUnit;
    [SerializeField] private TMP_Text _text;

    private IUnit _unit => (IUnit)_iUnit;

    private void OnValidate()
    {
        _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();
        if (_iUnit is IUnit == false)
        {
            _iUnit = null;
            return;
        }

        if (_unit.Fighter.FighterType == FighterType.Build)
            gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    private void Start()
    {
        if (_iUnit == null)
            _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();

        _text.text = _unit.Fighter.Damage.ToString();
    }
}
