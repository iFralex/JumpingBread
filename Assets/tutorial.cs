using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    int passaggio;
    public Text testo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PassaggioSuccessivo()
    {
        passaggio++;
        switch (passaggio)
        {
            case 1:
                testo.text = "instead this you can use it to view the leaderboard.  Find out if you are first!";
                break;
            case 2:
                testo.text = "To start a game, press this button. Choose the level and PLAY!";
                break;
            case 3:
                Destroy(gameObject);
                break;
        }
        transform.GetChild(passaggio - 1).gameObject.SetActive(false);
        transform.GetChild(passaggio).gameObject.SetActive(true);
    }
}
