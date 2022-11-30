using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    public bool start;
    public bool finish;
    public Coroutine coroutine;

    private void Update()
    {
        print(coroutine);
        if (start)
        {
             coroutine = StartCoroutine(test());
            start = false;
        }

        if (finish)
        {
            StopCoroutine(coroutine);
            finish = false;
        }
    }

    private IEnumerator test()
    {
        int i = 0;
        while(i < 5)
        {
            i++;
            print(i + " - i ");
            yield return new WaitForSeconds(0.25f);

        }

        //print("wait 0.5f");
        //yield return new WaitForSeconds(0.5f);
        //i = 0;
        //while (i < 5)
        //{
        //    i++;
        //    print(i + " - y ");
        //    yield return new WaitForSeconds(0.25f);

        //}
    }
}
