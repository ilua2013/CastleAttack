using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDoor : MonoBehaviour
{
    [SerializeField] private LevelSystem _levelSystem;
    [SerializeField] private float _speed;

    private void OnValidate()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
    }

    private void OnEnable()
    {
        _levelSystem.Wave2Finished += OpenDoor;
    }

    private void OnDisable()
    {
        _levelSystem.Wave2Finished -= OpenDoor;
    }

    private void OpenDoor()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float time = 0;
        Vector3 pos = transform.position + new Vector3(0, -15, 0);

        while (time < 7)
        {
            yield return null;
            time += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pos, _speed * Time.deltaTime);
        }
    }
}
