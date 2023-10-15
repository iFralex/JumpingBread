using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class classifica : MonoBehaviour
{
	dreamloLeaderBoard dl;
	public Transform nomiEPunti;
	public List<UnityEngine.UI.Text> nome, punti;

	public void Start()
    {
		this.dl = GetComponent<dreamloLeaderBoard>();
		for (int i = 0; i < nomiEPunti.childCount; i++)
        {
			nome.Add(nomiEPunti.GetChild(i).GetChild(1).GetComponent<UnityEngine.UI.Text>());
			punti.Add(nomiEPunti.GetChild(i).GetChild(2).GetComponent<UnityEngine.UI.Text>());
		}
		dl.AddScore("Anonimus", 0);
		StartCoroutine(avvia());
	}

    public void LeggiScores()
	{
		List<dreamloLeaderBoard.Score> listaPunti = dl.ToListHighToLow();
		if (listaPunti.Count > 10)
		{
			for (int i = 0; i < nomiEPunti.childCount; i++)
			{
				nome[i].text = listaPunti[i].playerName;
				punti[i].text = listaPunti[i].score.ToString();
			}
		}
		else
        {
			dl.AddScore("Anonimus", 0);
			Debug.LogError("non va " + listaPunti.Count);
			StartCoroutine(avvia());
		}
	}

	IEnumerator avvia()
    {
		yield return new WaitForSeconds(1);
		LeggiScores();
	}
}