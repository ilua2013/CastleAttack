using System.Collections;
using UnityEngine;

public class DiedUnitEnemy : MonoBehaviour
{
    [SerializeField] private UnitEnemy _unitEnemy;
    [SerializeField] private float _timeDelay;

    private void OnEnable()
    {
        _unitEnemy.Fighter.EffectDied += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitEnemy.Fighter.EffectDied -= RagDollEnable;
    }

    public void RagDollEnable()
    {
        StartCoroutine(DelayDied());
    }

    private IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(_timeDelay);
        GamesStatistics.RegisterEnemyKill();
        Destroy(_unitEnemy.gameObject);
    }
}
