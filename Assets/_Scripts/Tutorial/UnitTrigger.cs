using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private GameObject _cardSelection;
    [SerializeField] private LevelSystem _levelSystem;

    //private bool 

    public event Action UnitHighCell;

    private void Start()
    {
        _gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.TryGetComponent(out UnitFriend triggered))
        {
            _levelSystem.enabled = false;
            _cardSelection.SetActive(false);
            _gameObject.SetActive(true);
        }
    }

    private void ActivedWindowsView()
    {
        _gameObject.SetActive(true);
        Time.timeScale = 0;
    }


}
