using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [Header("Wave 1")]
    [SerializeField] private List<Mob> _mobs1;
    [SerializeField] private List<Tower> _tower1;
    [Header("Wave 2")]
    [SerializeField] private List<Mob> _mobs2;
    [SerializeField] private List<Tower> _tower2;
    [Header("Wave 3")]
    [SerializeField] private List<Mob> _mobs3;
    [SerializeField] private List<Tower> _tower3;

    private Wave _currentWave;

    public Wave CurrentWave => _currentWave;

    public event Action ClickedStartFight;
    public event Action Wave1Finished;
    public event Action Wave2Finished;
    public event Action Wave3Finished;

    private void OnValidate()
    {

    }

    private void Awake()
    {
        _currentWave = Wave.One;
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartFight);
        RegisterDie(_mobs1, _tower1);
        RegisterDie(_mobs2, _tower2);
        RegisterDie(_mobs3, _tower3);
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartFight);
    }

    private void StartFight()
    {
        ClickedStartFight?.Invoke();
    }

    private void RemoveDiedUnit(IMob mob)
    {
        switch (_currentWave)
        {
            case Wave.One:
                RemoveUnit(_mobs1, _tower1, mob);
                break;

            case Wave.Two:
                RemoveUnit(_mobs2, _tower2, mob);
                break;

            case Wave.Three:
                RemoveUnit(_mobs3, _tower3, mob);
                break;
        }
    }

    private void CheckWinWave()
    {
        switch (_currentWave)
        {
            case Wave.One:
                if(_mobs1.Count == 0 && _tower1.Count == 0)
                {
                    _currentWave++;
                    Wave1Finished?.Invoke();
                }
                break;

            case Wave.Two:
                if (_mobs2.Count == 0 && _tower2.Count == 0)
                {
                    _currentWave++;
                    Wave2Finished?.Invoke();
                }
                break;

            case Wave.Three:
                if (_mobs3.Count == 0 && _tower3.Count == 0)
                {
                    Wave3Finished?.Invoke();
                }
                break;
        }
    }

    private void RemoveUnit(List<Mob> mobs, List<Tower> towers, IMob mob)
    {
        for (int i = 0; i < mobs.Count; i++)
        {
            if (mobs[i] is IMob imob && imob == mob)
            {
                print("mob is mob");
                mobs[i].Deaded -= RemoveDiedUnit;
                mobs.RemoveAt(i);
            }
        }

        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] is IMob imob && imob == mob)
            {
                print("tower is mob");
                towers[i].Deaded -= RemoveDiedUnit;
                towers.RemoveAt(i);
            }
        }
    }

    private void RegisterDie(List<Mob> mobs, List<Tower> towers)
    {
        foreach (var mob in mobs)
            mob.Deaded += RemoveDiedUnit;

        foreach (var tower in towers)
            tower.Deaded += RemoveDiedUnit;
    }
}

public enum Wave
{
    One, Two, Three
}
