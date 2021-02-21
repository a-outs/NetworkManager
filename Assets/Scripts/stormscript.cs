using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.U2D;

public class stormscript : MonoBehaviour
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private Vector3 maxDistance;

    [SerializeField]
    private float lightningInterval;

    [SerializeField]
    private float lightningWindow;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPosition = transform.position;
        startPosition.x = -maxDistance.x;
        startPosition.y = Random.Range(-maxDistance.y, maxDistance.y);
        transform.position = startPosition;

        Vector3 endPosition = transform.position;
        endPosition.x = maxDistance.x;
        endPosition.y = Random.Range(-maxDistance.y, maxDistance.y);

        transform.DOMove(endPosition, duration).SetEase(Ease.Linear);

        StartCoroutine("Lightning");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Lightning() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(lightningInterval-lightningWindow, lightningInterval+lightningWindow));
            int lightningStrikes = Random.Range(1, 3);
            for(int i = 0; i < lightningStrikes; i++) {
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.7f);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.7f);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));
            }
        }
    }
}
