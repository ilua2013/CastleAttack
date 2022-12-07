using System.Collections;
using UnityEngine;

public class DiedUnitFriend : MonoBehaviour
{

    [SerializeField] private UnitFriend _unitFriend;
    [SerializeField] private float _timeDelay;

    private void OnEnable()
    {
        _unitFriend.Fighter.EffectDied += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitFriend.Fighter.EffectDied -= RagDollEnable;
    }

    private void RagDollEnable()
    {
        StartCoroutine(DelayDied());
    }

    private IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(_timeDelay);
        GamesStatistics.RegisterFriendKill();
        Destroy(_unitFriend.gameObject);
    }
}
