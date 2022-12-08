using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CellSpawner))]
public class CellWinAnimations : MonoBehaviour
{
    [SerializeField] private float _delay = 0.5f;

    private CellSpawner _cellSpawner;
    private List<Cell> _cells = new List<Cell>();

    private void Awake()
    {
        _cellSpawner = GetComponent<CellSpawner>();
        _cells = FindObjectsOfType<Cell>().ToList();
    }

    public void Play(Action onEnd = null)
    {
        StartCoroutine(Recolor(_delay, onEnd));
    }

    private IEnumerator Recolor(float time, Action onEnd = null)
    {
        yield return new WaitForSeconds(_delay * 6);

        for (int i = 0; i < _cellSpawner.Rows; i++)
        {
            yield return new WaitForSeconds(time);

            foreach (CellView cell in GetRow(i))
                cell.RecolorToWin();
        }

        yield return new WaitForSeconds(time);
        onEnd?.Invoke();
    }

    private List<CellView> GetRow(int row)
    {
        List<CellView> _views = new List<CellView>();

        foreach (Cell cell in _cells)
            if (cell.Number == row)
                _views.Add(cell.CellView);

        return _views;
    }
}