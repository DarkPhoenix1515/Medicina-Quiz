using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public GameObject canvas;
	private bool[] chapters = { false, false, false, false, false, false, false, false, false, false, false };
	private bool isRandom = false;

	public void startButton() {
		bool chapterSelected = false;
		for (int i = 0; i < 11; i++) {
			if (chapters[i]) {
				chapterSelected = true;
			}
		}

		if (chapterSelected) {
			string chaptersLocation = Application.persistentDataPath + "/chapters.txt";
			TextAsset chaptersFile = Resources.Load ("chapters") as TextAsset;
			string chaptersContent = chaptersFile.ToString ();
			JSONNode jsonData = JSON.Parse (chaptersContent);

			//format: jsonData["chapters"][index_capitol]
			for (int i = 0; i < 11; i++) {
				jsonData["chapters"][i].AsBool = chapters[i];
			}
			jsonData["random"].AsBool = isRandom;
			System.IO.File.WriteAllText(chaptersLocation, jsonData.ToString());
			Application.LoadLevel ("Quiz");
		} else {
			// Do nothing
		}
	}

	public void toggleRandomness() {
		isRandom = !isRandom;
	}

	public void toggleChapter(int chapter) {
		if (chapters [chapter - 1]) {
			chapters [chapter - 1] = false;
		} else {
			chapters [chapter - 1] = true;
		}
	}
}

