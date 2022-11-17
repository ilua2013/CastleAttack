using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject; 

    //private bool 

    public event Action UnitHighCell;

    private void Start()
    {
        _gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnitFriend triggered))
        {
            //UnitHighCell?.Invoke();

            
        }
    }

    private void ActivedWindowsView()
    {
        _gameObject.SetActive(true);
        Time.timeScale = 0;
    }


}
