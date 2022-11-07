using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(UnitFriend))]
public class BattleGroundSwitcher : MonoBehaviour
{
    [SerializeField] private LevelSystem _levelSystem;
    [SerializeField] private CellsStage _cellStage;

    private UnitFriend _unit;

    [Serializable]
    private class CellsStage
    {
        public Cell cell1;
        public Cell cell2;
        public Cell cell3;
    }

    private void OnValidate()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
    }

    private void Awake()
    {
        _unit = GetComponent<UnitFriend>();
    }

    private void OnEnable()
    {
        _levelSystem.WaveFinished += MoveToNextCell;
    }

    private void OnDisable()
    {
        _levelSystem.WaveFinished -= MoveToNextCell;
    }

    private void MoveToNextCell()
    {
        switch (_levelSystem.CurrentWave)
        {
            case Wave.One:
                _unit.Mover.SetTransformOnCell(_cellStage.cell1);
                break;
            case Wave.Two:
                _unit.Mover.SetTransformOnCell(_cellStage.cell2);
                break;
            case Wave.Three:
                _unit.Mover.SetTransformOnCell(_cellStage.cell3);
                break;
        }
    }
}
