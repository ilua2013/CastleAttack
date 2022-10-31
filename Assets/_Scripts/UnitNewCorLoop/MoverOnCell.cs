using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverOnCell : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Cell _currentCell;

    public Cell CurrentCell => _currentCell;

    public void SetCurrentCell(Cell cell)
    {
        _currentCell = cell;
    }

    public void Move(TeamUnit teamUnit)
    {
        Cell target;

        if (teamUnit == TeamUnit.Friend)
            target = _currentCell.Top;
        else
            target = _currentCell.Bot;

        StartCoroutine(MoveTo(target.transform));

        _currentCell.SetFree();
        _currentCell = target;
    }

    private IEnumerator MoveTo(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
