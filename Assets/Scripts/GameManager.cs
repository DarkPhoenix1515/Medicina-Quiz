
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using MaterialUI;


public class GameManager : MonoBehaviour {

	private bool[] answer = {false, false, false, false, false};
	private bool[] chapters;
	private bool[] correctAnswer;
	private bool endGame = false;
	private bool answerSelected;
	private bool isCorrect;
	private bool isRandom = false;
	private int[] chapterSize = {189, 385, 285, 199, 232, 170, 292, 88, 100, 130, 82};
	private Dictionary<int, string> variante;
	private List<Question> questions;
	private string actualAnswer = "";
	private string scoreTotal = "Grile rezolvate: 0, Corecte: 0, Gresite: 0";

	public int totalAnswers;
	public int correctAnswers;
	public int falseAnswers;

	public Text intrebareTXT;
	public Text score;
	public Text dialogTitle;
	public Text dialogMessage;

	public GameObject checkA;
	public GameObject checkB;
	public GameObject checkC;
	public GameObject checkD;
	public GameObject checkE;

	string chaptersLocation;
	string jsonLocation = Application.persistentDataPath + "/answers.txt";
	string jsonContent;
	JSONNode jsonData;

	GameObject dialogBox;

	void Start () {
		dialogBox = GameObject.Find ("DialogBox");
		dialogBox.SetActive(false);
		chaptersLocation = Application.persistentDataPath + "/chapters.txt";
		totalAnswers = 0;
		correctAnswers = 0;
		falseAnswers = 0;
		variante = new Dictionary<int, string> ();
		variante.Add (0, "A");
		variante.Add (1, "B");
		variante.Add (2, "C");
		variante.Add (3, "D");
		variante.Add (4, "E");

		chapters = getChapters ();
		resetChapters ();
		generateJsonData ();
		initializeQuestions ();
		newQuestion ();
	}
	private void generateJsonData() {
		TextAsset answersFile = Resources.Load ("answers") as TextAsset;
		jsonContent = answersFile.ToString ();
		this.jsonData = JSON.Parse (jsonContent);
	}

	void initializeQuestions () {
		questions = new List<Question> ();

		for (int i = 0; i < 11; i++) {
			if (chapters[i]) {
				for (int j = 1; j <= chapterSize[i]; j++) {
					questions.Add(new Question(i+1, j, jsonData));
				}
			}
		}
	}

	void newQuestion() {
		int questionsCount = questions.Count;

		if (questionsCount > 0) {
			int i = 0;
			if (isRandom) {
				i = Random.Range (0, questionsCount - 1);
			}
			Question q = questions [i];
			questions.Remove (q);
			correctAnswer = q.Answer;
			answerSelected = false;
			intrebareTXT.text = "Grila " + q.Number + ", Capitolul " + q.Chapter;
			actualAnswer = q.toString();
		} else {
			dialogTitle.text = "Gata intrebarile!";
			dialogMessage.text = "Felicitari! \n" + scoreTotal + " :*";
			endGame = true;
			dialogBox.SetActive(true);
		}
	}
	
	public void verifyButton () {
		if (answerSelected) {
			isCorrect = true;
			string answerString = "Raspunsul corect este: ";

			for (int i = 0; i < 5; i++) {
				if (correctAnswer[i]) {
					answerString += variante[i] + ", ";
				}

				if (answer[i] == correctAnswer[i]) {

				} else {
					isCorrect = false;
				}
			}
			char[] trimParams = {' ', ','};
			answerString = answerString.TrimEnd(trimParams);
			totalAnswers++;

			if (isCorrect) {
				correctAnswers++;
				updateUI("Raspuns Corect!", "Bravo! " + actualAnswer);
			} else {
				falseAnswers++;
				updateUI("Raspuns Gresit!", actualAnswer);
			}
		} else {
			// No answer selected
		}
	}

	void updateUI(string title, string message) {
		dialogTitle.text = title;
		dialogMessage.text = message;
		dialogBox.SetActive(true);
		scoreTotal = "Grile rezolvate: " + totalAnswers + ", Corecte: " + correctAnswers + ", Gresite: " + falseAnswers;
		score.text = scoreTotal;
		resetCheckboxes();
	}

	void resetCheckboxes () {
		CheckboxConfig config = checkA.GetComponent<CheckboxConfig>();
		config.resetCheckbox ();
		config = checkB.GetComponent<CheckboxConfig>();
		config.resetCheckbox ();
		config = checkC.GetComponent<CheckboxConfig>();
		config.resetCheckbox ();
		config = checkD.GetComponent<CheckboxConfig>();
		config.resetCheckbox ();
		config = checkE.GetComponent<CheckboxConfig>();
		config.resetCheckbox ();
	}

	public void toggleAnswer (int letter) {
		if (!answerSelected) {
			answerSelected = true;
		}
		if (answer [letter]) {
			answer [letter] = false;
		} else {
			answer [letter] = true;
		}
	}

	private bool[] getChapters() {
		string text = "";
		text = System.IO.File.ReadAllText (chaptersLocation);
		JSONNode chaptersData = JSON.Parse (text);

		bool[] selectedChapters = new bool[11];
		//format: chaptersData["chapters"][index_capitol]
		for(int i = 0; i < 11; i++) {
			selectedChapters[i] = chaptersData["chapters"][i].AsBool;
		}

		this.isRandom = chaptersData ["random"].AsBool;
		return selectedChapters;
	}
	
	private void resetChapters() {
		string text = "";
		text = System.IO.File.ReadAllText (chaptersLocation);
		JSONNode chaptersData = JSON.Parse (text);

		//format: chaptersData["chapters"][index_capitol]
		for(int i = 0; i < 11; i++) {
			chaptersData["chapters"][i].AsBool = false;
		}
		System.IO.File.WriteAllText(chaptersLocation, chaptersData.ToString());
	}

	public void dialogOK() {
		if (endGame) {
			Application.LoadLevel ("Menu");
		}
		dialogBox.SetActive(false);
		newQuestion();
	}
	
	public void menuButton() {
		Application.LoadLevel ("Menu");
	}
}
