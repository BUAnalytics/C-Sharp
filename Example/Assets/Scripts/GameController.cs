using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using BUAnalytics;

public class GameController : MonoBehaviour {

	int clicksRemaining = 5;
	float startTime;
	float delayTime;
	Vector2 target;
	List<Vector2> attempts = new List<Vector2>();
	DateTime sessionStart;

	void Start(){
		StartCoroutine(DelayButton());

		//Set session start
		sessionStart = DateTime.UtcNow;

		//Generate unique session identifier
		Utility.sessionId = BUID.Instance.Generate();
	}

	IEnumerator DelayButton(){

		//No more clicks remaining
		if (clicksRemaining <= 0) {

			//Create new session in collection
			var sessionDoc = new BUDocument(new Dictionary<string, object>(){
				{ "sessionId", Utility.sessionId },
				{ "userId", Utility.userId },
				{ "start", sessionStart },
				{ "end", DateTime.UtcNow }
			});

			//Add documents to collections
			BUCollectionManager.Instance.Collections["Sessions"].Add(sessionDoc);

			//Make sure collections are uploaded incase of closure
			BUCollectionManager.Instance.UploadAll();

			//Return to menu
			SceneManager.LoadScene("Menu");
			yield return false;
		}

		//Reset all previous attempts
		attempts = new List<Vector2>();

		//Hide button from screen
		transform.position = new Vector3(-100f, -100f);

		//Calculate random delay and wait
		delayTime = UnityEngine.Random.Range(0.5f, 4f);
		yield return new WaitForSeconds(delayTime);

		ShowButton();
	}

	void ShowButton(){
		startTime = Time.time;

		//Show button in random position on screen
		target.x = UnityEngine.Random.Range(0f, 840f);
		target.y = UnityEngine.Random.Range(0f, 300f);
		transform.position = new Vector3(target.x, target.y, 0);
	}

	public void MissedButton(){
		attempts.Add(Input.mousePosition);
	}

	public void ClickButton(){
		clicksRemaining--;

		//Calculate speed and accuracy data
		Vector3 mousePosition = Input.mousePosition;
		Vector2 accuracy = new Vector2(Math.Abs(target.x - mousePosition.x), Math.Abs(target.y - mousePosition.y));
		float clickTime = Time.time - startTime;

		//Create new click and add it to list
		var clickDoc = new BUDocument(new Dictionary<string, object>(){
			{ "sessionId", Utility.sessionId },
			{ "userId", Utility.userId },
			{ "clickTime", clickTime },
			{ "delayTime", delayTime },
			{ "accuracy", accuracy },
			{ "target", target },
			{ "attempts", attempts }
		});

		//Add click to collection
		BUCollectionManager.Instance.Collections["Clicks"].Add(clickDoc);

		StartCoroutine(DelayButton());
	}
}
