using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using BUAnalytics;

public enum Gender{ Male, Female }

public class MenuController : MonoBehaviour {

	Text startText;
	InputField nameField;
	InputField ageField;
	Dropdown genderSelect;

	void Awake(){
		
		//Create menu fields
		startText = GameObject.Find("StartButton").GetComponentInChildren<Text>();
		nameField = GameObject.Find("NameField").GetComponentInChildren<InputField>();
		ageField = GameObject.Find("AgeField").GetComponentInChildren<InputField>();
		genderSelect = GameObject.Find("GenderSelect").GetComponentInChildren<Dropdown>();

		//Set backend api url and authentication details
		BUAPI.Instance.Auth = new BUAccessKey("58ac40cd126553000c426f91", "06239e3a1401ba6d7250260d0f8fd680e52ff1e754ebe10a250297ebda2bac41");
		//BGAPI.Instance.URL = "https://192.168.0.69"; //Defaults to https://bu-games.bmth.ac.uk

		//Start loading a cache of 200 unique identifiers from backend
		BUID.Instance.Start(200);

		//Create collections with names
		BUCollectionManager.Instance.Create(new string[]{
			"Users",
			"Sessions",
			"Clicks"
		});

		//Subscribe to collection errors
		BUCollectionManager.Instance.Error = (collection, error) => {
			Debug.Log("[BUAnalytics][" + collection.Name + "] Failed to upload with error code " + error.ToString());
		};

		//Subscribe to collection uploads
		BUCollectionManager.Instance.Success = (collection, count) => {
			Debug.Log("[BUAnalytics][" + collection.Name + "] Successfully uploaded " + count.ToString() + " documents");
		};

		//Configure upload and id refresh checks to be performed every 4 seconds
		BUCollectionManager.Instance.Interval = 4000;
		BUID.Instance.Interval = 4000;
	}

	public void StartClicked(){
		if (startText.text == "Play!"){

			//Check name field has been filled
			if (nameField.text == ""){
				Debug.LogError("Please enter a valid name");
				return;
			}

			//Check age field has been filled and is numeric
			int age;
			if (!Int32.TryParse (ageField.text.Trim (), out age)) {
				Debug.LogError("Please enter a valid age");
				return;
			}

			//Convert gender to Enum
			Gender gender;
			if (genderSelect.value > 0){
				gender = (Gender)Enum.ToObject(typeof(Gender), genderSelect.value-1);
			}else{
				Debug.LogError("Please select a gender");
				return;
			}

			//Generate unique user identifier
			if (Utility.userId == null){
				Utility.userId = BUID.Instance.Generate();
			}

			//Create new user in collection
			var userDoc = new BUDocument(new Dictionary<string, object>(){
				{ "userId", Utility.userId },
				{ "name", nameField.text },
				{ "age", age },
				{ "gender", gender },
				{ "device", new Dictionary<string, string>{
					{ "type", SystemInfo.deviceType.ToString() },
					{ "name", SystemInfo.deviceName },
					{ "model", SystemInfo.deviceModel },
				} }
			});
				
			//Add documents to collections
			BUCollectionManager.Instance.Collections["Users"].Add(userDoc);

			StartCoroutine(StartCountdown());
		}
	}

	IEnumerator StartCountdown(){
		startText.text = "3";
		yield return new WaitForSeconds(1);
		startText.text = "2";
		yield return new WaitForSeconds(1);
		startText.text = "1";
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("Game");
		yield return new WaitForSeconds(0.5f);
		startText.text = "Play!";
	}
}
