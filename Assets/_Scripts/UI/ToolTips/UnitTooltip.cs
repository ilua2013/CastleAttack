using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.ToolTips
{
    public class UnitTooltip : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private TMP_Text _damage;
        [SerializeField] private UnitFriend _unit;
        [SerializeField] private GameObject _separator;
        [SerializeField] private RectTransform _separatorsHolder;
        
        private void Awake()
        {
            _unit.Fighter.OnHPChanged += UpdateHPView;
            _unit.Fighter.Died += DestroyView;
            _unit.Fighter.OnInit += Init;
        }
        
        private void OnDestroy()
        {
            _unit.Fighter.OnHPChanged -= UpdateHPView;
            _unit.Fighter.Died -= DestroyView;
            _unit.Fighter.OnInit -= Init;
        }

        private void Init()
        {
            _damage.text = "x" + _unit.Fighter.Damage;
            
            for (var i = 0; i < _unit.Fighter.MaxHealth; i++)
                Instantiate(_separator, _separatorsHolder);
        }
        
        private void UpdateHPView(float value)
        {
            _fill.fillAmount = value;
        }
        
        private void DestroyView()
        {
            StartCoroutine(DestroingView());
        }
        
        private IEnumerator DestroingView()
        {
            Vector3 worldPos = transform.position;
            transform.SetParent(null);
            transform.position = worldPos;
        
            for (float i = 1; i > 0; i -= Time.deltaTime)
            {
                transform.localScale = Vector3.one * i;
                yield return null;
            }
            
            Destroy(gameObject);
        }
    }
}