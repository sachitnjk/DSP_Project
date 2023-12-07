using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public List<GameObject> deskPoints;
	public List<GameObject> virtualPlayers;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
}
