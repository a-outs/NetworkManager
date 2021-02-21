using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class fadein : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn() {
        GetComponent<Image>().DOFade(0f, 1f);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
