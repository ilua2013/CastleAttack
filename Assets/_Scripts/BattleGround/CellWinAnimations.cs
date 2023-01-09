using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CellSpawner))]
public class CellWinAnimations : MonoBehaviour
{
    public bool test;
    [SerializeField] private float _delayToStart = 0.5f;
    [SerializeField] private float _delayStepChanger = 0;

    private CellSpawner _cellSpawner;
    private List<Cell> _cells = new List<Cell>();

    private void Start()
    {
        _cellSpawner = GetComponent<CellSpawner>();
        _cells = GetComponentsInChildren<Cell>().ToList();
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            Play();
        }
    }

    public void Play(Action onEnd = null)
    {
        StartCoroutine(Recolor(onEnd));
    }

    private IEnumerator Recolor(Action onEnd = null)
    {
        yield return new WaitForSeconds(_delayToStart);

        float delayChangerSum = 0;

        for (int i = 0; i < _cellSpawner.Rows; i++)
        {
            yield return new WaitForSeconds(delayChangerSum);
            delayChangerSum += _delayStepChanger;

            foreach (CellView cell in GetRow(i))
            {
                cell.RecolorToWin();

                if (i == _cellSpawner.Rows - 2)
                    onEnd?.Invoke();
            }
        }
    }

    private List<CellView> GetRow(int row)
    {
        List<CellView> views = new List<CellView>();

        foreach (Cell cell in _cells)
            if (cell.Number == row && cell.CellIs != CellIs.Wizzard)
                views.Add(cell.CellView);

        return views;
    }
}
