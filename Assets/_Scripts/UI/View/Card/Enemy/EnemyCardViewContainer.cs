using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardViewContainer : MonoBehaviour
{
    [SerializeField] private List<EnemyCardView> _viewPrefabs;

    private List<EnemyCardView> _views = new List<EnemyCardView>();

    private void Awake()
    {
        CreateViews();
    }

    public EnemyCardView GetEnemyView(UnitEnemyType typeId)
    {
        return _views.Find((card) => card.TypeId == typeId);
    }

    private void CreateViews()
    {
        foreach (EnemyCardView enemy in _viewPrefabs)
        {
            EnemyCardView view = Instantiate(enemy, transform);

            view.gameObject.SetActive(false);
            _views.Add(view);
        }
    }
}
