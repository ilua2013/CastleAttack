using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FightSystem : MonoBehaviour
{
    [SerializeField] private Button _button;

    public event UnityAction Moved;


    private void OnEnable()
    {
        _button.onClick.AddListener(MovedEvent);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(MovedEvent);
    }

    private void MovedEvent()
    {
        Moved?.Invoke();
    }


}
