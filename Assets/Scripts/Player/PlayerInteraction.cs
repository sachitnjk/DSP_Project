using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction interactionAction;
	private InputAction tabAction;
	private InputAction debugAction;

	[SerializeField] private PlayerCameraFP playerCamera;

	private PlayerMovementFP playerMove;
	private GameManager gameManager;

	public bool InteractionAvailable { get; set; }
	public bool TabActive { get; private set; }
	private bool interactionActive;

	private int numberOfAI;

	private void Start()
	{
		playerMove = GetComponent<PlayerMovementFP>();
		gameManager = GameManager.Instance;
		playerInput = InputProvider.GetPlayerInput();
		if(playerInput != null )
		{
			interactionAction = playerInput.actions["Interact"];
			tabAction = playerInput.actions["Tab"];
			debugAction = playerInput.actions["DebugX"];
		}

		interactionActive = false;
		TabActive = false;
		InteractionAvailable = false;
	}

	private void Update()
	{
		TabCheck();
		InteractionCheck();
		SpawnAI();

		if (interactionActive || TabActive)
		{
			playerMove.CanMove = false;
		}
		else
		{
			playerMove.CanMove = true;
		}
	}
	private void TabCheck()
	{
		if (tabAction.WasPerformedThisFrame())
		{
			TabActive = !TabActive;
		}
		if (TabActive)
		{
			Cursor.lockState = CursorLockMode.Confined;
			playerCamera.enabled = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			playerCamera.enabled = true;
		}
	}
	private void InteractionCheck()
	{
		if(InteractionAvailable && interactionAction.WasPerformedThisFrame() && !interactionActive)
		{
			interactionActive = true;
			playerMove.CanMove = false;
		}
		else if(interactionActive && interactionAction.WasPerformedThisFrame()) 
		{
			interactionActive = false;
			playerMove.CanMove = true;
		}
	}

	private void SpawnAI()
	{
		if(UIManager.Instance.MeetingsAttendedByAI != 0 && debugAction.WasPerformedThisFrame()) 
		{
			int numberOfAI = UIManager.Instance.MeetingsAttendedByAI;

			// Ensure that the number of AI doesn't exceed the available virtual players
			numberOfAI = Mathf.Min(numberOfAI, gameManager.virtualPlayers.Count);

			StartCoroutine(SpawnAICoroutine(numberOfAI));
		}
	}
	private IEnumerator SpawnAICoroutine(int numberOfAI)
	{
		for (int i = 0; i < gameManager.virtualPlayers.Count; i++)
		{
			bool activateAI = i < numberOfAI;

			yield return new WaitForSeconds(1f);

			gameManager.virtualPlayers[i].SetActive(activateAI);
		}
	}
}
