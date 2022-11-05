using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] UnitFriend _unitFriend;
    


    private void Start()
    {
        StartCoroutine(HighLigthRadius(_unitFriend.Mover.CurrentCell));
    }

    public IEnumerator HighLigthRadius(Cell cell)
    {
        while (true)
        {
            List<Cell>cells = _unitFriend.Mover.CurrentCell.GetForwardsCell(_unitFriend.Fighter.DistanceAttack);

            foreach (var currentCell in cells)
            {
                MeshRenderer higtligthCell = currentCell.gameObject.GetComponent<MeshRenderer>()!=null ? currentCell.gameObject.GetComponent<MeshRenderer>():null;
                if (higtligthCell != null)
                {
                    higtligthCell.material.color = Color.gray;
                }
            }

            yield return null;
        }

       
    }



}
