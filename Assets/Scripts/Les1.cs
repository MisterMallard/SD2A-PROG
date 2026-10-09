using System.Collections.Generic;
using UnityEngine;

public class Opdrachten : MonoBehaviour
{
	// Class for 1.8, 1.9 en 1.10
	public class Speler
	{
		public string Naam;
		public int HP;
		public int Score;

		public void Vertel()
		{
			Debug.Log("Ik ben " + Naam +
					  ", mijn HP is " + HP +
					  " en mijn score is " + Score + ".");
		}
	}

	void Start()
	{
		// OPDRACHT 1.1
		Scheiding("OPDRACHT 1.1 - Variabelen");

		string naam = "Erwin";
		int score = 1000;
		bool alive = true;

		Debug.Log("Naam : " + naam);
		Debug.Log("Score : " + score);
		Debug.Log("Alive : " + alive);


		// OPDRACHT 1.2
		Scheiding("OPDRACHT 1.2 - HP berekenen");

		int hp = 100;
		hp -= 35;

		Debug.Log("HP : " + hp);

		hp -= 80;

		if (hp > 0)
		{
			Debug.Log("Speler Leeft nog!");
		}
		else
		{
			Debug.Log("Speler is dood!");
		}


		// OPDRACHT 1.3
		Scheiding("OPDRACHT 1.3 - Begroeting");

		Begroet("Calvin");


		// OPDRACHT 1.4
		Scheiding("OPDRACHT 1.4 - Max van twee getallen");

		Debug.Log("Result : " + Max(30, 50));
		Debug.Log("Result : " + Max(70, 40));


		// OPDRACHT 1.5
		Scheiding("OPDRACHT 1.5 - Schade berekenen");

		Debug.Log("schade : " + BerekenSchade(1200, 200));


		// OPDRACHT 1.6
		Scheiding("OPDRACHT 1.6 - Vijanden array");

		string[] vijanden =
		{
			"Alien",
			"Cyclops",
			"Kraken",
			"Lochness",
			"Dragon"
		};

		for (int i = 0; i < vijanden.Length; i++)
		{
			Debug.Log(vijanden[i]);
		}


		// OPDRACHT 1.7
		Scheiding("OPDRACHT 1.7 - Hoogste score");

		int[] scores = { 500, 2500, 10000, 750, 4000 };
		int hoogste = scores[0];

		for (int i = 1; i < scores.Length; i++)
		{
			if (scores[i] > hoogste)
			{
				hoogste = scores[i];
			}
		}

		Debug.Log("highest : " + hoogste);


		// OPDRACHT 1.8
		Scheiding("OPDRACHT 1.8 - Speler classes");

		Speler mario = new Speler();
		mario.Naam = "Wario";
		mario.HP = 10;
		mario.Score = 1000;

		Speler luigi = new Speler();
		luigi.Naam = "Waluigi";
		luigi.HP = 4;
		luigi.Score = 500;

		Debug.Log("Speler.name : " + mario.Naam);
		Debug.Log("Speler.HP : " + mario.HP);
		Debug.Log("Speler.Score : " + mario.Score);

		Debug.Log("");

		Debug.Log("Speler.name : " + luigi.Naam);
		Debug.Log("Speler.HP : " + luigi.HP);
		Debug.Log("Speler.Score : " + luigi.Score);


		// OPDRACHT 1.9
		Scheiding("OPDRACHT 1.9 - Methode Vertel");

		mario.Vertel();
		luigi.Vertel();


		// OPDRACHT 1.10
		Scheiding("OPDRACHT 1.10 - Array van spelers");

		Speler[] spelers = new Speler[3];

		spelers[0] = mario;
		spelers[1] = luigi;
		spelers[2] = new Speler
		{
			Naam = "Daisy",
			HP = 8,
			Score = 800
		};

		DrukSpelersAf(spelers);


		// OPDRACHT 1.11
		Scheiding("OPDRACHT 1.11 - List van vijanden");

		List<string> vijandenLijst = new List<string>();

		vijandenLijst.Add("Alien");
		vijandenLijst.Add("Cyclops");
		vijandenLijst.Add("Kraken");
		vijandenLijst.Add("Lochness");
		vijandenLijst.Add("Dragon");

		vijandenLijst.Remove("Kraken");

		foreach (string vijand in vijandenLijst)
		{
			Debug.Log(vijand);
		}

		Debug.Log("Aantal vijanden : " + vijandenLijst.Count);

		Scheiding("ALLE OPDRACHTEN KLAAR");
	}

	// Opdracht 1.3
	void Begroet(string naam)
	{
		Debug.Log("Welkom, " + naam + "!");
	}

	// Opdracht 1.4
	int Max(int a, int b)
	{
		if (a > b)
		{
			return a;
		}

		return b;
	}

	// Opdracht 1.5
	int BerekenSchade(int aanval, int verdediging)
	{
		return Mathf.Max(0, aanval - verdediging);
	}

	// Opdracht 1.10
	void DrukSpelersAf(Speler[] spelers)
	{
		foreach (Speler speler in spelers)
		{
			speler.Vertel();
		}
	}

	// Scheidt de opdrachten in de Console
	void Scheiding(string titel)
	{
		Debug.Log("========================================");
		Debug.Log(titel);
		Debug.Log("========================================");
	}
}