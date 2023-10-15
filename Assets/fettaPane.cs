using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fettaPane : MonoBehaviour
{
    public float velocità, attrito, posFinale;
    public enum stato
    {
        sulNastro, inVolo, lanciato, ottenuto, distrutto
    }
    public stato statoCorrente;
    public Sprite[] fette, fetteSpiac;
    Transform posTempo;
    public UnityEngine.UI.Text testoTempo;
    int ruolo;
    public float[] masse;

    void Start()
    {
        statoCorrente = stato.sulNastro;
        ruolo = FindObjectOfType<manager>().livello;
        if (ruolo == 4)
        {
            ruolo = Random.Range(0, fette.Length);
        }
        GetComponentInChildren<SpriteRenderer>().sprite = fette[ruolo];
        if (ruolo == 1)
        {
            transform.localScale = Vector3.one;
        }
        GetComponent<Rigidbody2D>().gravityScale = masse[ruolo];
        StartCoroutine(Elimina());
    }

    void Update()
    {
        switch (statoCorrente)
        {
            case stato.sulNastro:
                transform.Translate(new Vector2(Time.deltaTime * velocità, 0));
                break;

            case stato.inVolo:
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, posFinale, Time.deltaTime * 5));
                if (transform.position.y > posFinale - .1f)
                {
                    statoCorrente = stato.lanciato;
                    GetComponentInChildren<SpriteRenderer>().sprite = fetteSpiac[ruolo];
                }
                break;

            case stato.lanciato:
                if (GetComponent<Rigidbody2D>().isKinematic)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 113);
                    GetComponent<Rigidbody2D>().isKinematic = false;
                }
                if (frames < 3)
                {
                    frames++;
                }
                break;

            case stato.ottenuto:
                transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(testoTempo.rectTransform.position), Time.deltaTime * 2);
                transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * 2);
                break;
        }

        if (transform.position.y < -5)
        {
            FindObjectOfType<manager>().secondi -= 10;
            Destroy(gameObject);
        }
    }
    int frames;
    public IEnumerator Elimina()
    {
        yield return new WaitForSeconds(8);
        if (statoCorrente == stato.sulNastro)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Scontrati(coll);
        }
        else
        {
            if (statoCorrente == stato.lanciato && frames < 2)
            {
                string color = coll.tag;
                manager mg = FindObjectOfType<manager>();
                switch (color)
                {
                    case "rosso":
                        if (mg.punti > 0)
                        {
                            mg.punti--;
                        }
                        break;
                    case "grigio":
                        mg.punti = 0;
                        break;
                    case "verde":
                        mg.punti++;
                        break;
                }/*
                else if (color == new Color(0, 0, 1, color.a))
                {
                    mg.punti++;
                }
                else if (color == new Color(0, 0, 0, color.a))
                {
                    mg.punti = 0;
                }
                else if (color == new Color(0, 1, 0, color.a))
                {
                    mg.punti *= 2;
                }*/
                mg.AggiornaPunti();
                statoCorrente = stato.distrutto;
                StartCoroutine(CambiaColore(coll.transform.GetChild(0).GetComponent<SpriteRenderer>()));
            }
        }
    }

    IEnumerator AggiungiSecondi(float s)
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<manager>().secondi += 5;
        Destroy(gameObject);
    }

    void Scontrati(Collider2D coll)
    {
        if (statoCorrente == stato.lanciato && coll.transform.parent.GetComponent<fettaPane>().statoCorrente == stato.lanciato)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            coll.transform.parent.GetComponent<Rigidbody2D>().gravityScale = 0;
            coll.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            coll.transform.parent.GetComponent<fettaPane>().statoCorrente = stato.ottenuto;
            statoCorrente = stato.ottenuto;
            Destroy(GetComponentInChildren<Collider2D>());
            Destroy(coll.GetComponent<Collider2D>());
            StartCoroutine(AggiungiSecondi(2));
        }
    }


    IEnumerator CambiaColore(SpriteRenderer sprite)
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        for (float i = 0; i <= 1; i += 0.1f)
        {
            yield return new WaitForSeconds(.1f);
            if (i < 0.5f)
            {
                sprite.color = new Color(1, 1, 1, sprite.color.a + 0.2f);
            }
            else
            {
                sprite.color = new Color(1, 1, 1, sprite.color.a - 0.2f);
            }
        }
        sprite.color = new Color(1, 1, 1, 0);
        Destroy(gameObject);
    }
}