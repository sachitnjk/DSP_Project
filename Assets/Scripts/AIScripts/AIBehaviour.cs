using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
	[SerializeField] private GameObject targetDeskPoint;
	private bool atTargetPoint;

	private void Start()
	{
		atTargetPoint = false;
	}

	private void Update()
	{
		if(!atTargetPoint)
		{
			GoToDeskPoint(targetDeskPoint);
		}
		if(transform.position == targetDeskPoint.transform.position) 
		{
			atTargetPoint=true;
		}
	}

	private void GoToDeskPoint(GameObject target)
	{
		transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);
	}
}
