using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction interactionAction;
	private InputAction tabAction;

	[SerializeField] private PlayerCameraFP playerCamera;
	private PlayerMovementFP playerMove;
	private GameManager gameManager;

	private Transform interactionPoint;
	private Transform otherDeskPoint;

	private GameObject virtualPlayerInstance;
	[SerializeField] GameObject virtualPlayerPrefab;

	public bool interactionAvailable { get; private set; }
	private bool interactionActive;
	public bool TabActive{get; private set;}

	private void Start()
	{
		playerMove = GetComponent<PlayerMovementFP>();
		gameManager = GameManager.Instance;
		playerInput = InputProvider.GetPlayerInput();
		if(playerInput != null )
		{
			interactionAction = playerInput.actions["Interact"];
			tabAction = playerInput.actions["Tab"];
		}

		interactionAvailable = false;
		interactionActive = false;
		TabActive = false;
	}

	private void Update()
	{
		InteractionCheck();
		TabCheck();
		if(interactionActive || TabActive) 
		{
			playerMove.CanMove = false;
		}
		else
		{
			playerMove.CanMove = true;
		}
		//Debug.Log(TabActive);
	}

	private void TabCheck()
	{
		if(tabAction.WasPerformedThisFrame())
		{
			TabActive = !TabActive;
		}
		if(TabActive)
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
		if(interactionAvailable && interactionAction.triggered && !interactionActive && !TabActive)
		{
			interactionActive = true;
			transform.position = Vector3.Lerp(transform.position, interactionPoint.position, 2f);

			otherDeskPoint = FindOtherDeskPoint();
			if(otherDeskPoint != null )
			{
				virtualPlayerInstance = Instantiate(virtualPlayerPrefab, otherDeskPoint.position, Quaternion.identity);
			}
		}
		else if(interactionActive && interactionAction.triggered)
		{
			interactionActive = false;

			if(virtualPlayerInstance != null) 
			{
				Destroy(virtualPlayerInstance);
			}
			otherDeskPoint = null;
		}
	}

	private Transform FindOtherDeskPoint()
	{
		if(gameManager != null && gameManager.deskPoints.Count > 0)
		{
			foreach(Transform deskPoint in gameManager.deskPoints)
			{
				if(deskPoint != interactionPoint)
				{
					return deskPoint;
				}
			}
		}
		return null;
	}

	public void SetInteractionActive(bool isActive, Transform deskPoint)
	{
		interactionAvailable = isActive;
		if(deskPoint != null ) 
		{
			this.interactionPoint = deskPoint;
		}
		else
		{
			this.interactionPoint = null;
		}
	}
}
