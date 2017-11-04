using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class Question {
	public int Chapter { get; private set; }
	public int Number { get; private set; }
	public bool[] Answer { get; private set; }
	private string answerString = "Raspunsul corect este: ";

	public Question(int chapter, int number, JSONNode jsonData) {
		this.Chapter = chapter;
		this.Number = number;
		this.Answer = getAnswer (jsonData);
	}

	private bool[] getAnswer(JSONNode jsonData) {
		bool[] answer = new bool[5];

		//format: data["capitole"][nr_capitol][0][nr_grila][index_litera]
		string[] rasp = {"A", "B", "C", "D", "E"};
		for(int i = 0; i<5; i++) {
			answer[i]= jsonData["capitole"][0][Chapter.ToString()][0][Number.ToString()][i].AsBool;
			if (answer[i]) {
				answerString += rasp[i] + ", ";
			}
		}
		char[] trimParams = {' ', ','};
		answerString = answerString.TrimEnd(trimParams);
		return answer;
	}

	public string toString() {
		return answerString;
	}
}