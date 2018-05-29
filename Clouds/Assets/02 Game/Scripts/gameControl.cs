using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameControl : MonoBehaviour {

	public int powerClouds;
	public int gamePoints = 0;
	public float xOffset;
	public float yOffset;
	public GameObject cloud;
	public PereyeControl pereye;
	public BonusControl bonusControl; 

	private float gameHistory;
	private int gameEasy;
	private int gameMedium;
	private int gameHard;
	private int gameStat;
	private float gameTime;
	private int difficulty;

	private Vector3[][] cloudPositions;
	private GameObject[][] cloudMatrix;

	private float mDeltaY;
	private float mEndY;

	void Start () {
		// Recuperar Variables
		gamePoints = PlayerPrefs.GetInt("gamepoints");
		gameHistory = PlayerPrefs.GetFloat ("history");
		gameEasy = PlayerPrefs.GetInt ("easy");
		gameMedium = PlayerPrefs.GetInt ("medium");
		gameHard = PlayerPrefs.GetInt ("hard");
		gameStat = PlayerPrefs.GetInt ("stat");
		gameTime = 30 + ((1 - gameHistory) * 20);
		powerClouds = gameStat + 4;
		// Inicializacion de Pantalla
		float initx = (1 - xOffset) * Screen.width / 2;
		float inity = ((1 - yOffset)/2 + yOffset) * Screen.height;
		float deltx = (xOffset * Screen.width) / (powerClouds - 1);
		float delty = (yOffset * Screen.height) / (powerClouds - 1);
		// Construcción de Matriz
		cloudPositions = new Vector3[powerClouds][];
		cloudMatrix = new GameObject[powerClouds][];
		for (int i = 0; i < powerClouds; i++) {
			cloudPositions[i] = new Vector3[powerClouds];
			cloudMatrix[i] = new GameObject[powerClouds];
			for (int j = 0; j < powerClouds; j++) {
				cloudPositions[i][j] = Camera.main.ScreenToWorldPoint(new Vector3 (initx + (deltx * i), inity - (delty * j), 10));
				cloudMatrix[i][j] = Instantiate(cloud, cloudPositions[i][j], Quaternion.identity) as GameObject;
			}
		}
		mEndY = cloudPositions[powerClouds - 1][powerClouds - 1].y;
		mDeltaY = Mathf.Abs(mEndY - cloudPositions[powerClouds - 2][powerClouds - 2].y);
	
		Invoke ("CloudOrder", 0.1f);
		Invoke ("CloudLost", gameTime);
	}

	private void CloudOrder(){
		// configuración dificultad
		if (gameHistory < 0.30f)
			difficulty = 0;
		else if (gameHistory >= 0.30f && gameHistory < 0.45f)
			difficulty = 1;
		else if (gameHistory >= 0.45f && gameHistory < 0.60f)
			difficulty = 2;
		else if (gameHistory >= 0.60f && gameHistory < 0.81f)
			difficulty = 3;
		else if (gameHistory >= 0.81f)
			difficulty = 4;
		if (powerClouds <= 4 && difficulty <= 1)
			difficulty = 2;
		if (powerClouds > 4 && difficulty >= 3 && gameHard >= 3) {
			difficulty = 0;
			gameHard = 0;
			PlayerPrefs.SetInt ("hard", gameHard);
			PlayerPrefs.Save ();
		}
		// Inicialización de variables
		int tmp = 0;
		int maxBlack = 0;
		switch (difficulty) {
		case 0:					// Facil
			maxBlack = (int)Random.Range (1.0f, (Mathf.Pow (powerClouds, 2.0f)) * 0.10f);
			break;
		case 1:					// medio-facil
			maxBlack = (int)Random.Range (Mathf.Pow (powerClouds, 2.0f)* 0.10f, Mathf.Pow (powerClouds, 2.0f) * 0.15f);
			break;
		case 2:					// medio
			maxBlack = (int)Random.Range (Mathf.Pow (powerClouds, 2.0f)* 0.15f, Mathf.Pow (powerClouds, 2.0f) * 0.20f);
			break;
		case 3:					// dificil
			maxBlack = (int)Random.Range (Mathf.Pow (powerClouds, 2.0f)* 0.20f, Mathf.Pow (powerClouds, 2.0f) * 0.27f);
			break;
		case 4:					// hardcore
			maxBlack = (int)Random.Range (Mathf.Pow (powerClouds, 2.0f)* 0.27f, Mathf.Pow (powerClouds, 2.0f) / 3);
			break;
		}
		int nonBlack = (int)Random.Range (0.0f, maxBlack);
		int pairBlack = maxBlack - nonBlack;
		int nonWhite = nonBlack;
		int pairWhite = pairBlack;
		if (powerClouds % 2 == 0) {
			nonWhite += powerClouds / 2;
			pairWhite += powerClouds / 2;
		} else {
			tmp = (int)Random.Range (0.0f, 1.1f);
			if (tmp == 0) {
				nonWhite += (powerClouds + 1) / 2;
				pairWhite += (powerClouds - 1) / 2;
			} else {
				nonWhite += (powerClouds - 1) / 2;
				pairWhite += (powerClouds + 1) / 2;
			}
		}
		// Valores iniciales de la Matriz
		int xPos, yPos, cycle = 0;
		int[] colWeight = new int[powerClouds]; 
		for (int i = 0; i < powerClouds; i++) {
			for (int j = 0; j < powerClouds; j++) {
				cloudMatrix [i] [j].GetComponent<CloudBehavior> ().turnGray ();
				colWeight [i] += 1; 
			}
		}
		// Organizacion pseudo-aleatoria
		while ((pairBlack + nonBlack + pairWhite + nonWhite) > 0 && cycle < maxBlack * 20) {
			cycle += 1;
			xPos = (int)Random.Range (0.0f, powerClouds);
			if (xPos >= powerClouds)
				xPos = powerClouds - 1;
			yPos = (int)Random.Range (0.0f, powerClouds);
			if (yPos >= powerClouds)
				yPos = powerClouds - 1;
			if (cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().state == 2) {
				if ((xPos + yPos) % 2 == 0) {
					if (pairBlack > pairWhite) {
						if (pairBlack > 0) {
							cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().turnBlack ();
							pairBlack -= 1;
							colWeight [xPos] += 1; 						
						}
					} else {
						if (pairWhite > 0){
							cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().turnWhite ();
							pairWhite -= 1;
							colWeight [xPos] -= 1; 						
						}
					}
				} else {
					if (nonBlack > nonWhite) {
						if (nonBlack > 0){
							cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().turnBlack ();
							nonBlack -= 1;
							colWeight [xPos] += 1; 						
						}
					} else {
						if (nonWhite > 0) {
							cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().turnWhite ();
							nonWhite -= 1;
							colWeight [xPos] -= 1; 						
						}
					}
				}
			}
		}
		// Verificación de la Matriz
		for (xPos = 0; xPos < powerClouds; xPos++) {
			if (colWeight [xPos] <= (powerClouds / 2)) {
				pairBlack = (int)(powerClouds / 2) - colWeight [xPos];
				do {
					yPos = (int)Random.Range (0.0f, powerClouds);
					if (yPos >= powerClouds)
						yPos = powerClouds - 1;
					if (Random.Range (-1.0f, 1.0f) >= 0)
						cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().turnBlack ();
					else
						cloudMatrix [xPos] [yPos].GetComponent<CloudBehavior> ().turnGray ();
					pairBlack -= 1;
				} while (pairBlack > 0);
			}
		}		
	}

	public void CloudVerify(){
		Vector2[] vect;
		vect = new Vector2 [3];
		int vectCount = 0;
		bool stat = true;
		for (int i = 0; i < powerClouds; i++) {
			for (int j = 0; j < powerClouds; j++) {
				if (cloudMatrix [i] [j].GetComponent<CloudBehavior> ().cloudID > -1) {
					if (vectCount == 0) {
						vect [vectCount] = new Vector2 (i, j);
						vectCount = 1;
						stat = true;
					} else if (stat) {
						if ((Mathf.Abs ((int)vect [vectCount - 1].x - i) == 1 && Mathf.Abs ((int)vect [vectCount - 1].y - j) == 1)) {
							if (vectCount < 3) {
								vect [vectCount] = new Vector2 (i, j);
								vectCount += 1;
								stat = true;
							} else
								stat = false;
						} else
							stat = false;
					}
					cloudMatrix [i] [j].GetComponent<CloudBehavior> ().cloudID = -1;
					cloudMatrix [i] [j].GetComponent<CloudBehavior> ().turnCloud ();
				}
			}
		}
		if (vectCount >= 2 && vectCount <= 3 && stat) {
			switch (vectCount) {
			case 2: 
				if (cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().state == 1) {
					if (cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().state == 2) {
						cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().turnGray ();
						cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().turnWhite ();
					} else
						stat = false;						
				} else if (cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().state == 2) {
					if (cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().state == 1) {
						cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().turnWhite ();
						cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().turnGray ();
					} else
						stat = false;
				}
				break;
			case 3:
				if (cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().state == 3) {
					if (cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().state == 1) {
						if (cloudMatrix [(int)vect [2].x] [(int)vect [2].y].GetComponent<CloudBehavior> ().state == 1) {
							cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().turnWhite ();
							cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().turnGray ();
							cloudMatrix [(int)vect [2].x] [(int)vect [2].y].GetComponent<CloudBehavior> ().turnGray ();
						} else
							stat = false;
					} else
						stat = false;
				} else if (cloudMatrix [(int)vect [2].x] [(int)vect [2].y].GetComponent<CloudBehavior> ().state == 3) {
					if (cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().state == 1) {
						if (cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().state == 1) {
							cloudMatrix [(int)vect [0].x] [(int)vect [0].y].GetComponent<CloudBehavior> ().turnGray ();
							cloudMatrix [(int)vect [1].x] [(int)vect [1].y].GetComponent<CloudBehavior> ().turnGray ();
							cloudMatrix [(int)vect [2].x] [(int)vect [2].y].GetComponent<CloudBehavior> ().turnWhite ();
						} else
							stat = false;
					} else
						stat = false;
				}
				break;
			}
		}
		if (stat) {
			ClearWayCloud ();
			gamePoints += vectCount - 1;
			if ((gamePoints % 20) == 0) {
				if (!bonusControl.BonusStat ())
					bonusControl.EnableBonus ();
			}
		}
	}

	public void ClearWayCloud(){
		bool clearway = false;
		float initX = 0.0f; 
		// Verificación de Estados
		int[] colWeight = new int[powerClouds]; 
		float[] posWin = new float[powerClouds];
		for (int i = 0; i < powerClouds; i++) {
			for (int j = 0; j < powerClouds; j++) {
				if (j == 0) {
					colWeight [i] = 0;
					posWin [i] = cloudMatrix [i] [j].transform.position.x;
				} 
				colWeight [i] += cloudMatrix [i] [j].GetComponent<CloudBehavior> ().state - 1; 
				if (i == powerClouds - 1) {
					if (colWeight [j] == 0) {
						clearway = true;
						initX = posWin [j];
					}
				}
			}
		}
		if (clearway) {
			float gameFactor = 0.0f;
			switch (difficulty) {
			case 0:					// Facil
				gameFactor = 0.05f;
				gameEasy += 1;
				break;
			case 1:					// medio-facil
				gameFactor = 0.10f;
				gameMedium += 1;
				break;
			case 2:					// medio
				gameFactor = 0.10f;
				gameMedium += 1;
				break;
			case 3:					// dificil
				gameFactor = 0.15f;
				gameHard += 1;
				break;
			case 4:					// hardcore
				gameFactor = 0.15f;
				gameHard += 1;
				break;
			}
			gameHistory += gameFactor;
			if (gameHistory > 1.0f)
				gameHistory = 1.0f;
			if (gameStat == 2)
				gameStat = 0;
			else
				gameStat += 1;
			PlayerPrefs.SetInt("gamepoints", gamePoints);
			PlayerPrefs.SetFloat ("history", gameHistory);
			PlayerPrefs.SetInt ("easy", gameEasy);
			PlayerPrefs.SetInt ("medium", gameMedium);
			PlayerPrefs.SetInt ("hard", gameHard);
			PlayerPrefs.SetInt ("stat", gameStat);
			PlayerPrefs.Save ();
			GetComponent<TouchControl> ().enabled = false;
			CancelInvoke ();
			pereye.PereyeFinalStage (powerClouds, initX, mEndY, mDeltaY);
		}
	}

	private void CloudLost(){
		float gameFactor = 0.0f;
		switch (difficulty) {
		case 0:					// Facil
			gameFactor = 0.025f;
			break;
		case 1:					// medio-facil
			gameFactor = 0.050f;
			break;
		case 2:					// medio
			gameFactor = 0.050f;
			break;
		case 3:					// dificil
			gameFactor = 0.075f;
			break;
		case 4:					// hardcore
			gameFactor = 0.075f;
			break;
		}
		gameHistory -= gameFactor;
		if (gameHistory < 0.0f)
			gameHistory = 0.0f;
		gameEasy = 0;
		gameMedium = 0;
		gameHard = 0;
		if (gameStat == 0)
			gameStat = 0;
		else
			gameStat -= 1;
		PlayerPrefs.SetInt("gamepoints", gamePoints);
		PlayerPrefs.SetFloat ("history", gameHistory);
		PlayerPrefs.SetInt ("easy", gameEasy);
		PlayerPrefs.SetInt ("medium", gameMedium);
		PlayerPrefs.SetInt ("hard", gameHard);
		PlayerPrefs.SetInt ("stat", gameStat);
		PlayerPrefs.Save ();
		SceneManager.LoadScene ("prototype");
	}
}
