using System.Collections;
using UnityEngine;

public class piastrella : MonoBehaviour
{
    Vector2 scala;
    bool cresci;
    float n;
    public bool sposta, scalare;
    public Vector2 spostamento;

    void Start()
    {
        n = Random.Range(0f, 3f);
        scala = transform.localScale;
    }

    void Update()
    {
        if (scalare)
        {
            if (cresci)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, scala, Time.deltaTime * n);
                if (transform.localScale.y > scala.y - (scala.y / 10))
                {
                    cresci = false;
                }
            }
            else
            {
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * n);
                if (transform.localScale.y < scala.y / 5)
                {
                    cresci = true;
                }
            }
        }
    }

    IEnumerator Sposta()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(1);
            transform.Translate(new Vector2(spostamento.x * 1.66f, spostamento.y * 1.507f));
            yield return new WaitForSeconds(1);
            transform.Translate(new Vector2(spostamento.x * -1.66f, spostamento.y * -1.507f));
        }
    }

    void OnEnable()
    {
        if (sposta)
        {
            StartCoroutine(Sposta());
        }
    }
}