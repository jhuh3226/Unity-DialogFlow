using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Syrus.Plugins.DFV2Client;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class DF2ClientAudioTester : MonoBehaviour {
	public InputField session, content;

	public Text chatbotText;

	private DialogFlowV2Client client;

	public AudioClip testClip;

	public AudioSource audioPlayer;

	public Text LangueButtonText;

	// en-US-Wavenet-F
	private string languageCode = "en-US";

	private bool isEnglish = true;

	public GameObject WaitingPanel;

	public GameObject WaitingRecord;

	public GameObject eventSystem;
	// Start is called before the first frame update

	public string petitionText = "";
	public bool dolphinSpeaking = false;

	void Start () {
		client = GetComponent<DialogFlowV2Client> ();

		audioPlayer = GetComponent<AudioSource> ();

		// Adjustes session name if it is blank.
		// string sessionName = GetSessionName ();
		string sessionName = null;

		client.ChatbotResponded += LogResponseText;
		client.DetectIntentError += LogError;
		client.ReactToContext ("DefaultWelcomeIntent-followup",
			context => Debug.Log ("Reacting to welcome followup"));
		client.SessionCleared += sess => Debug.Log ("Cleared session " + session);

		// may deactivate following code as I am not making dolphin to talk in the biggining
		client.AddInputContext (new DF2Context ("userdata", 1, ("name", "George")), sessionName);

		Dictionary<string, object> parameters = new Dictionary<string, object> () { { "name", "George" }
		};
		// may deactivate following code as I am not making dolphin to talk in the biggining
		client.DetectIntentFromEvent ("test-inputcontexts", parameters, sessionName);

		WaitingPanel.SetActive (false);
		WaitingRecord.SetActive (false);

		// added, asking micriphone permit to user
#if UNITY_ANDROID
		CheckPermission ();
#endif
	}

	// added, asking micriphone permit to user
	void CheckPermission () {
#if UNITY_ANDROID
		if (!Permission.HasUserAuthorizedPermission (Permission.Microphone)) {
			Permission.RequestUserPermission (Permission.Microphone);
		}
#endif
	}

	public void OnChangeLanguageButton () {
		isEnglish = !isEnglish;

		if (isEnglish) {
			languageCode = "en-US";
			LangueButtonText.text = "English";
			// en-US-Wavenet-H	
		} else {
			languageCode = "ja";
			LangueButtonText.text = "Japanese";
		}
	}
	private void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			byte[] audioBytes = WavUtility.FromAudioClip (testClip);
			string audioString = Convert.ToBase64String (audioBytes);
			SendAudio (audioString);
		}
	}

	private void LogResponseText (DF2Response response) {

		WaitingPanel.SetActive (false);

		Debug.Log (JsonConvert.SerializeObject (response, Formatting.Indented));
		// Debug.Log(GetSessionName() + " said: \"" + response.queryResult.fulfillmentText + "\"");
		Debug.Log (response.queryResult.fulfillmentText);

		// pass the fulfillmentText to other script
		eventSystem.GetComponent<DialogFlowSystemOutput> ().systemOutput = response.queryResult.fulfillmentText;

		Debug.Log ("Audio " + response.OutputAudio);

		// chatbotText.text = response.queryResult.queryText + "\n";	// User's speech

		// chatbot's response is showing <speak> and </speak>, get rid of them
		string chatbotResponse = response.queryResult.fulfillmentText;
		string speak = "<speak>";
		string speakEnd = "</speak>";

		chatbotResponse = chatbotResponse.Replace (speak, "");
		chatbotResponse = chatbotResponse.Replace (speakEnd, "");
		chatbotText.text = chatbotResponse; // Chatbot's reply

		// chatbotText.text += response.queryResult.fulfillmentText; // Chatbot's reply

		LogVoiceInput (response.queryResult.queryText); // Output the voice input

		byte[] audioBytes = Convert.FromBase64String (response.OutputAudio);
		AudioClip clip = WavUtility.ToAudioClip (audioBytes);

		audioPlayer.clip = clip;

		Debug.Log ("Audio clip length : " + audioPlayer.clip.length);

		// changed to invoke
		Invoke ("playAudio", 0.5f);
		Invoke ("ChangeToDefaultAnimation", 0.5f + audioPlayer.clip.length);
		// audioPlayer.Play();

	}

	// added to use invoke
	public void playAudio () {
		dolphinSpeaking = true;
		audioPlayer.Play ();
	}

	void ChangeToDefaultAnimation () {
		Debug.Log ("Change to default animation");
		dolphinSpeaking = false;
	}

	private void LogError (DF2ErrorResponse errorResponse) {
		WaitingPanel.SetActive (false);
		Debug.LogError (string.Format ("Error {0}: {1}", errorResponse.error.code.ToString (),
			errorResponse.error.message));
	}

	//@hoatong
	public void SendAudio (string audioString) {
		WaitingPanel.SetActive (true);
		// isWaitingPanelOn = true;	// change ui 
		string sessionName = GetSessionName ();
		client.DetectIntentFromAudio (audioString, sessionName, languageCode);
	}

	public void SendText () {
		// DF2Entity name0 = new DF2Entity("George", "George");
		// DF2Entity name1 = new DF2Entity("Greg", "Greg");
		// DF2Entity potion = new DF2Entity("Potion", "Potion", "Cure", "Healing potion");
		// DF2Entity antidote = new DF2Entity("Antidote", "Antidote", "Poison cure");
		// DF2EntityType names = new DF2EntityType("names", DF2EntityType.DF2EntityOverrideMode.ENTITY_OVERRIDE_MODE_SUPPLEMENT,
		// 	new DF2Entity[] { name0, name1 });
		// DF2EntityType items = new DF2EntityType("items", DF2EntityType.DF2EntityOverrideMode.ENTITY_OVERRIDE_MODE_SUPPLEMENT,
		// 	new DF2Entity[] { potion, antidote });

		WaitingPanel.SetActive (true);
		string sessionName = GetSessionName ();
		//client.AddEntityType(names, sessionName);
		//client.AddEntityType(items, sessionName);

		// Debug.Log ("content.text: " + content.text + " sessionName: " + sessionName);	// context.text is user's input

		client.DetectIntentFromText (content.text, sessionName, languageCode);

	}

	// Output the voice input
	public void LogVoiceInput (string data) {
		petitionText = data; // pass the text and save to petitionText to send to OutputPetition
		// Debug.Log ("petitionText: " + petitionText);
	}

	// send string response to dialogflow
	public float SendResponse (float data) {
		client.DetectIntentFromText (data.ToString (), "DefaultSession", "en-US");
		return data;
	}

	public void SendEvent () {
		client.DetectIntentFromEvent (content.text,
			new Dictionary<string, object> (), GetSessionName ());
	}

	public void Clear () {
		client.ClearSession (GetSessionName ());
	}

	private string GetSessionName (string defaultFallback = "DefaultSession") {
		string sessionName = session.text;
		if (sessionName.Trim ().Length == 0)
			sessionName = defaultFallback;
		return sessionName;
	}

	#region AUDIO RECORD

	AudioClip recordedAudioClip;

	//Keep this one as a global variable (outside the functions) too and use GetComponent during start to save resources
	AudioSource audioSource;

	private float startRecordingTime;

	private bool isRecording = false;
	bool isWaitingPanelOn = false;

	public Text recordButtonText;

	public Sprite squeakImg, sendImg;
	public GameObject btnRecord;

	public void OnButtonRecord () {
		if (!isRecording) {
			StartRecord ();
			isRecording = true;
			recordButtonText.text = "Send to dolphin";
			btnRecord.GetComponent<Image> ().sprite = sendImg; // change the btn image

		} else {
			isRecording = false;
			recordButtonText.text = "Squeak!";
			AudioClip recorded = StopRecord ();
			btnRecord.GetComponent<Image> ().sprite = squeakImg; // change the btn image

			byte[] audioBytes = WavUtility.FromAudioClip (recorded);
			string audioString = Convert.ToBase64String (audioBytes);
			SendAudio (audioString);
		}
	}

	public AudioClip StopRecord () {
		WaitingRecord.SetActive (false);
		//End the recording when the mouse comes back up, then play it
		Microphone.End ("");

		//Trim the audioclip by the length of the recording
		AudioClip recordingNew = AudioClip.Create (recordedAudioClip.name,
			(int) ((Time.time - startRecordingTime) * recordedAudioClip.frequency), recordedAudioClip.channels,
			recordedAudioClip.frequency, false);
		float[] data = new float[(int) ((Time.time - startRecordingTime) * recordedAudioClip.frequency)];
		recordedAudioClip.GetData (data, 0);
		recordingNew.SetData (data, 0);
		this.recordedAudioClip = recordingNew;

		return recordedAudioClip;
		//Play recording
		//audioSource.clip = recordedAudioClip;
		//audioSource.Play();
	}

	public void StartRecord () {
		WaitingRecord.SetActive (true);
		//Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
		int minFreq;
		int maxFreq;
		int freq = 44100;
		Microphone.GetDeviceCaps ("", out minFreq, out maxFreq);
		if (maxFreq < 44100)
			freq = maxFreq;

		//Start the recording, the length of 300 gives it a cap of 5 minutes
		recordedAudioClip = Microphone.Start ("", false, 300, 44100);
		startRecordingTime = Time.time;
	}

	#endregion
}