using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskPointManager : MonoBehaviour
{
	public bool isOccupied {  get; set; }

	private PlayerInteraction playerInteraction;

	private void Start()
	{
		isOccupied = false;
	}

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
