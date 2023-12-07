using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	[SerializeField] private PlayerInteraction playerInteraction;

	[Header("TAB UI references")]
	[SerializeField] private GameObject TabPanel;
	[SerializeField] private GameObject WelcomeText;
	[SerializeField] private GameObject meetingsPanel;
	[SerializeField] private GameObject AIMeetingsEditSelectPanel;
	[SerializeField] private GameObject EditPanel;
	[SerializeField] private GameObject AISelectorPanel;
	[SerializeField] private GameObject PresenterPanel;
	[SerializeField] private GameObject AttendeePanel;

	[Header("Other UI references")]
	[SerializeField] GameObject canInteractPanel;

	public int MeetingsAttendedByAI{get; private set;}
	public List<GameObject> AIAttendedMeetingsText = new List<GameObject>();
	public List<GameObject> AIAttendedMeetingsButtons = new List<GameObject>();
	private bool hasStarted;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		TabPanel.SetActive(false);
		canInteractPanel.SetActive(false);
	}

	private void Update()
	{
		TabEnableCheck();
		OnInteractCheck();
	}

	private void TabEnableCheck()
	{
		if(playerInteraction != null) 
		{
			OnTabCheck();
		}
	}

	private void OnInteractCheck()
	{
		if(playerInteraction.InteractionAvailable) 
		{
			canInteractPanel.SetActive(true);
		}
		else
		{
			canInteractPanel.SetActive(false);
		}
	}

	private void OnTabCheck()
	{
		if (playerInteraction.TabActive)
		{
			EnableObject(TabPanel);
			WelcomeTextCycle();
		}
		else if (!playerInteraction.TabActive)
		{
			DisableObject(TabPanel);
			DisableObject(meetingsPanel);
			DisableObject(AIMeetingsEditSelectPanel);
			DisableObject(EditPanel);
			hasStarted = false;
		}
	}

	private void EnableObject(GameObject obj)
	{
		obj.SetActive(true);
	}
	private void DisableObject(GameObject obj)
	{
		obj.SetActive(false);
	}

	private void WelcomeTextCycle()
	{
		if(WelcomeText != null && !hasStarted) 
		{
			hasStarted = true;
			WelcomeText.SetActive(true);
			StartCoroutine(DisableObjectWDelay(WelcomeText, 2f));
		}
	}
	private IEnumerator DisableObjectWDelay(GameObject obj, float delay)
	{
		yield return new WaitForSeconds(delay);
		obj.SetActive(false);
		EnableObject(meetingsPanel);
	}

	public void AssignAI()
	{
		MeetingsAttendedByAI++;
	}
	public void EditAIMeetingsList()
	{
		DisableObject(meetingsPanel);
		EnableObject(AIMeetingsEditSelectPanel);

		// Disable all meetingTexts and meetingButtons first
		foreach (GameObject text in AIAttendedMeetingsText)
		{
			text.SetActive(false);
		}

		foreach (GameObject button in AIAttendedMeetingsButtons)
		{
			button.SetActive(false);
		}

		// Enable the necessary number of meetingTexts and meetingButtons based on meetingsAttendedByAI
		for (int i = 0; i < MeetingsAttendedByAI; i++)
		{
			AIAttendedMeetingsText[i].SetActive(true);
			AIAttendedMeetingsButtons[i].SetActive(true);
		}
	}
	public void EditMeeting()
	{
		DisableObject(AIMeetingsEditSelectPanel);
		EnableObject(EditPanel);
		EnableObject(AISelectorPanel);
	}
	public void PresenterPanelEnable()
	{
		DisableObject(AISelectorPanel);
		DisableObject(AttendeePanel);
		EnableObject(PresenterPanel);
	}
	public void AttendeePanelEnable() 
	{
		DisableObject(AISelectorPanel);
		DisableObject(PresenterPanel);
		EnableObject(AttendeePanel);
	}
	public void ReturnToAIMeetingsList()
	{
		DisableObject(EditPanel);
		DisableObject(AttendeePanel);
		DisableObject(PresenterPanel);
		EnableObject(AIMeetingsEditSelectPanel);
	}
	public void DisableOtherObject(GameObject other)
	{
		other.SetActive(false);
	}
	public void EnableOtherObject(GameObject other) 
	{
		other.SetActive(true);
	}
}
