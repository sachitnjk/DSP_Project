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
	[SerializeField] GameObject virtualPlayerPrefab;

	private PlayerMovementFP playerMove;
	private GameManager gameManager;


	public bool InteractionAvailable { get; set; }
	public bool TabActive { get; private set; }
	private bool interactionActive;

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
		if(virtualPlayerPrefab != null && UIManager.Instance.MeetingsAttendedByAI != 0 && debugAction.WasPerformedThisFrame()) 
		{
			for(int i = 0; i < UIManager.Instance.MeetingsAttendedByAI;  i++) 
			{
				Instantiate(virtualPlayerPrefab, Vector3.zero, Quaternion.identity);
			}
		}
	}
}
