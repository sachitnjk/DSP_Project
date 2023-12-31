using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskPointController : MonoBehaviour
{
	private PlayerInteraction playerInteraction;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
			if(playerInteraction != null ) 
			{
				playerInteraction.InteractionAvailable = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(playerInteraction != null) 
		{
			playerInteraction.InteractionAvailable = false;
		}
	}
}
