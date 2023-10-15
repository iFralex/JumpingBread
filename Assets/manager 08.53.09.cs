using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;
using Firebase.Analytics;

public class manager : MonoBehaviour
{
    public enum stato { inMenu, inGame, GameOver };
    stato st = stato.inMenu;
    public Sprite[] audioSprite;
    public AudioSource musica;
    public SVGImage audioBot;
    public GameObject fetta, gameOverPannel, bloccaClick, tutorial;
    public Transform spawnPoint;
    public float speed, secondi, minuti, punti, velocitàFette, velocitàSpawn;
    Transform targer = null;
    float startPos, startTempo;
    public UnityEngine.UI.Text testoTempo, puntiTesto, testoLivelloGo, testoPuntiGO;
    public GameObject[] livelli;
    int puntiNec = 50, scena, sceleAtt;
    public GameObject[] colori;
    bool inizia;
    public Sprite successoSpriteGO, riprovaSpriteGO;
    int[] puntiPerLivello = new int[5];
    public RectTransform[] lucchetti;
    public int livello;
    public RectTransform posFinForm, posCent, formClas;
    string nome;
    InterstitialAdGameObject interstitailAd;
    public UnityEngine.UI.Text testoDebug;
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        MobileAds.Initialize(initStatus => { });
        interstitailAd = MobileAds.Instance.GetAd<InterstitialAdGameObject>("Interstitial Ad");
        interstitailAd.LoadAd();
        if (PlayerPrefs.HasKey("audio"))
        {
            if (PlayerPrefs.GetString("audio") == "no")
            {
                musica.Stop();
                audioBot.sprite = audioSprite[0];
            }
            Destroy(tutorial);
        }
        else
        {
            PlayerPrefs.SetString("audio", "si");
            tutorial.SetActive(true);
        }

        if (PlayerPrefs.HasKey("livello"))
        {
            sceleAtt = PlayerPrefs.GetInt("livello");
        }
        else
        {
            PlayerPrefs.SetInt("livello", 0);
            sceleAtt = 0;
        }

        if (!PlayerPrefs.HasKey("miglior punteggio"))
        {
            PlayerPrefs.SetInt("miglior punteggio", 0);
        }
        puntiPerLivello[0] = 20;
        puntiPerLivello[1] = 22;
        puntiPerLivello[2] = 23;
        puntiPerLivello[3] = 24;
        puntiPerLivello[4] = 25;
        puntiNec = puntiPerLivello[0];
        AggiornaLucchetti();
        AggiornaPunti();
    }

    void Update()
    {
        if (st == stato.inGame)
        {
            if (minuti >= 0)
            {
                secondi -= Time.deltaTime;
                if (secondi < 0)
                {
                    secondi += 60;
                    minuti--;
                    testoTempo.text = minuti.ToString() + ":" + ((int)secondi).ToString();
                }
                else
                {
                    if (secondi > 60)
                    {
                        secondi -= 60;
                        minuti++;
                        testoTempo.text = minuti.ToString() + ":" + ((int)secondi).ToString();
                    }
                    else
                    {
                        if (secondi < 10)
                        {
                            testoTempo.text = minuti.ToString() + ":0" + ((int)secondi).ToString();
                        }
                        else
                        {
                            testoTempo.text = minuti.ToString() + ":" + ((int)secondi).ToString();
                        }
                    }
                }

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        Vector3 posDito = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                        RaycastHit2D hit = Physics2D.Raycast(posDito, Vector2.zero);
                        if (hit.collider != null && hit.collider.tag == "Player" && hit.collider.transform.parent.GetComponent<fettaPane>().statoCorrente == fettaPane.stato.sulNastro)
                        {
                            startTempo = Time.time;
                            startPos = posDito.y;
                            hit.collider.isTrigger = true;
                            targer = hit.collider.transform.parent;
                        }
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        if (startPos != 0)
                        {
                            Vector3 posDito = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                            float tempo = Time.time - startTempo;
                            float dist = Mathf.Abs(posDito.y - startPos);
                            targer.gameObject.GetComponent<fettaPane>().StopCoroutine(targer.gameObject.GetComponent<fettaPane>().Elimina());
                            targer.gameObject.GetComponent<fettaPane>().statoCorrente = fettaPane.stato.inVolo;
                            targer.gameObject.GetComponent<fettaPane>().posFinale = startPos + (dist / tempo * Time.deltaTime * speed);
                            targer.gameObject.GetComponent<fettaPane>().testoTempo = testoTempo;
                            targer = null;
                            startTempo = 0;
                            startPos = 0;
                        }
                    }
                }
            }
            else
            {
                if (st != stato.GameOver && !inizia)
                {
                    inizia = true;
                    testoTempo.text = "0:00";
                    st = stato.GameOver;
                    gameOverPannel.SetActive(true);
                    gameOverPannel.GetComponent<Animator>().SetTrigger("entra");
                    testoLivelloGo.text = (livello + 1).ToString();
                    testoPuntiGO.text = punti.ToString();

                    if (punti >= puntiNec)
                    {
                        if (livello == 4)
                        {
                            if (punti > PlayerPrefs.GetInt("miglior punteggio"))
                            {
                                PlayerPrefs.DeleteKey("miglior punteggio");
                                PlayerPrefs.SetInt("miglior punteggio", (int)punti);
                                formClas.GetComponent<Animator>().SetTrigger("entra");
                                gameOverPannel.GetComponent<RectTransform>().position = new Vector2(posFinForm.position.x, gameOverPannel.GetComponent<RectTransform>().position.y);
                            }
                            gameOverPannel.transform.GetChild(0).gameObject.SetActive(true);
                            gameOverPannel.transform.GetChild(1).gameObject.SetActive(false);
                            gameOverPannel.transform.GetChild(2).gameObject.SetActive(false);
                        }
                        else
                        {
                            gameOverPannel.GetComponent<SVGImage>().sprite = successoSpriteGO;
                            gameOverPannel.transform.GetChild(1).gameObject.SetActive(true);
                            gameOverPannel.transform.GetChild(2).gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        gameOverPannel.GetComponent<SVGImage>().sprite = riprovaSpriteGO;
                        gameOverPannel.transform.GetChild(1).gameObject.SetActive(false);
                        gameOverPannel.transform.GetChild(2).gameObject.SetActive(true);
                    }
                    StopAllCoroutines();
                }
            }
        }
    }

    public void Musica()
    {
        if (musica.isPlaying)
        {
            musica.Stop();
            audioBot.sprite = audioSprite[0];
            ReimpostaAudio("no");
        }
        else
        {
            musica.Play();
            audioBot.sprite = audioSprite[1];
            ReimpostaAudio("si");
        }
    }

    void ReimpostaAudio(string valore)
    {
        PlayerPrefs.DeleteKey("audio");
        PlayerPrefs.SetString("audio", valore);
    }

    IEnumerator Spawn()
    {
        for (; st == stato.inGame;)
        {
            yield return new WaitForSeconds(2);
            Instantiate(fetta);
        }
    }

    public void IniziaPartita(int i)
    {
        scena = i;
        minuti = 1;
        secondi = 30;
        puntiNec = puntiPerLivello[livello];
        livelli[scena].SetActive(true);
        testoTempo.transform.parent.gameObject.SetActive(true);
        st = stato.inGame;
        inizia = false;
        punti = 0;
        gameOverPannel.GetComponent<RectTransform>().position = new Vector2(posCent.position.x, gameOverPannel.GetComponent<RectTransform>().position.y);
        formClas.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
        AggiornaPunti();
        StartCoroutine(Spawn());
    }

    public void FinePartita()
    {
        st = stato.inMenu;
        colori[(scena * 5) + livello].SetActive(false);
        colori[scena * 5].SetActive(true);
        livello = 0;
        testoTempo.transform.parent.gameObject.SetActive(false);
        scena++;
        if (scena > sceleAtt)
        {
            sceleAtt = scena;
            PlayerPrefs.DeleteKey("livello");
            PlayerPrefs.SetInt("livello", scena);
        }

        if (posFinForm.position.y == formClas.position.y)
        {
            formClas.GetComponent<Animator>().SetTrigger("esci");
        }
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, FirebaseAnalytics.ParameterLevelName, livelli[scena - 1].name);
        AggiornaLucchetti();
        StartCoroutine(disattivaLivello(scena - 1));
    }

    public void TornaAlMenù()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        st = stato.inMenu;
        colori[(scena * 5) + livello].SetActive(false);
        colori[scena * 5].SetActive(true);
        livello = 0;
        testoTempo.transform.parent.gameObject.SetActive(false);
        
        if (posFinForm.position.y == formClas.position.y)
        {
            formClas.GetComponent<Animator>().SetTrigger("esci");
        }
        StartCoroutine(disattivaLivello(scena));
    }

    IEnumerator disattivaLivello(int livello)
    {
        yield return new WaitForSeconds(1.5f);
        livelli[livello].SetActive(false);
        gameOverPannel.SetActive(false);
    }

    public void AggiornaPunti()
    {
        puntiTesto.text = punti.ToString() + "/" + puntiNec.ToString();
        if (punti < puntiNec)
        {
            puntiTesto.color = Color.red;
        }
        else
        {
            puntiTesto.color = Color.green;
        }
    }

    public void LivelloSuccessivo()
    {
        livello++;
        colori[(scena * 5) + livello].SetActive(true);
        colori[(scena * 5) + livello - 1].SetActive(false);
        fetta.gameObject.GetComponent<fettaPane>().velocità = 3 + livello;
        velocitàSpawn = 2 + (livello);
        IniziaPartita(scena);
    }

    public void Riprova()
    {
        IniziaPartita(scena);
    }

    void AggiornaLucchetti()
    {
        for (int i = 0; i < lucchetti.Length; i++)
        {
            if (sceleAtt < i)
            {
                lucchetti[i].gameObject.SetActive(true);
                lucchetti[i].transform.parent.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
            else if (sceleAtt > 0)
            {
                lucchetti[i].gameObject.SetActive(false);
                lucchetti[i].transform.parent.GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
        }
    }

    public void BloccaClick()
    {
        bloccaClick.SetActive(true);
        StartCoroutine(DisattivaBloccaClick());
    }

    IEnumerator DisattivaBloccaClick()
    {
        yield return new WaitForSeconds(1);
        bloccaClick.SetActive(false);
    }

    public void RegistraStart(string scena)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, FirebaseAnalytics.ParameterLevelName, scena);
    }

    public void RegistraNome(string name)
    {
        nome = name;
        if (!string.IsNullOrEmpty(nome))
        {
            formClas.GetComponentInChildren<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void RegistraScore()
    {
        GetComponent<dreamloLeaderBoard>().AddScore(nome, (int)punti);
        Debug.Log(nome + "  " + punti);
        nome = null;
        formClas.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
        formClas.GetComponentInChildren<UnityEngine.UI.InputField>().text = null;
        GetComponent<classifica>().LeggiScores();
    }

    public void Debuga(string t)
    {
        testoDebug.text = t;
    }
}