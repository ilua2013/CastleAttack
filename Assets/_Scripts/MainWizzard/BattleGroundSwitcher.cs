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

        var stages = FindObjectsOfType<Stage>();

        for (int i = 0; i < stages.Length; i++)
        {
            switch (stages[i].StageNumber)
            {
                case StageNumber.One:
                    foreach (var item in stages[i].CellSpawner.GetComponentsInChildren<Cell>())
                        if (item.CellIs == CellIs.Wizzard)
                            _cellStage.cell1 = item;
                    break;
                case StageNumber.Two:
                    foreach (var item in stages[i].CellSpawner.GetComponentsInChildren<Cell>())
                        if (item.CellIs == CellIs.Wizzard)
                            _cellStage.cell2 = item;
                    break;
                case StageNumber.Three:
                    foreach (var item in stages[i].CellSpawner.GetComponentsInChildren<Cell>())
                        if (item.CellIs == CellIs.Wizzard)
                            _cellStage.cell3 = item;
                    break;
            }
        }
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
            case StageNumber.One:
                _unit.Mover.SetTransformOnCell(_cellStage.cell1);
                break;
            case StageNumber.Two:
                _unit.Mover.SetTransformOnCell(_cellStage.cell2);
                break;
            case StageNumber.Three:
                _unit.Mover.SetTransformOnCell(_cellStage.cell3);
                break;
        }
    }
}
