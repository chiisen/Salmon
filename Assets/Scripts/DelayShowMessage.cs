using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayShowMessage : MonoBehaviour
{
    // 延遲顯示幾秒
    [Header("延遲顯示幾秒")]
    public float DelayTime = 5f;

    protected bool switchMessage = false;

    void Start ()
    {
        StartCoroutine(StartCoroutineDelayDestroy(gameObject));
    }
	
    protected virtual IEnumerator StartCoroutineDelayDestroy(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(DelayTime);

        Destroy(obj);
    }
}
