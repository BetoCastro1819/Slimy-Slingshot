using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour 
{
	[SerializeField] Transform target;
	[SerializeField] float speedToTarget = 10;
	[SerializeField] float speedToInitialPos = 10;
	[SerializeField] float delay = 0;
	[SerializeField] float timeOnStartPos = 0;
	[SerializeField] float timeOnTargetPos = 0;

	private Vector3 targetPosition;
	private Vector3 startPosition;
	private float timer;
	private bool canStartMoving;

	private enum MovingObjectState 
	{
		OnStartPosition,
		MovingTowardsTarget,
		OnTargetPosition,
		MovingTowardsStartPosition
	}
	private MovingObjectState state;

	private void Start()
	{
		targetPosition = target.position;
		startPosition = transform.position;

		timer = 0;
		state = MovingObjectState.OnStartPosition;

		canStartMoving = false;
		Invoke("EnableMovement", delay);
	}

	private void EnableMovement()
	{
		canStartMoving = true;
	}

	private void Update()
	{
		if (canStartMoving)
			UpdateState();
	}

	private void UpdateState()
	{
		switch(state)
		{
			case MovingObjectState.OnStartPosition:
				OnStartPosition();
				break;
			case MovingObjectState.MovingTowardsTarget:
				MovingTowardsTarget();
				break;
			case MovingObjectState.OnTargetPosition:
				OnTargetPosition();
				break;
			case MovingObjectState.MovingTowardsStartPosition:
				MovingTowardsStartPosition();
				break;
		}
	}

	private void OnStartPosition()
	{
		timer += Time.deltaTime;
		if (timer >= timeOnStartPos)
		{
			state = MovingObjectState.MovingTowardsTarget;
			timer = 0;
		}
	}

	private void MovingTowardsTarget()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedToTarget * Time.deltaTime);

		if (transform.position == targetPosition)
			state = MovingObjectState.OnTargetPosition;
	}

	private void OnTargetPosition()
	{
		timer += Time.deltaTime;
		if (timer >= timeOnTargetPos)
		{
			state = MovingObjectState.MovingTowardsStartPosition;
			timer = 0;
		}
	}

	private void MovingTowardsStartPosition()
	{
		transform.position = Vector3.MoveTowards(transform.position, startPosition, speedToInitialPos * Time.deltaTime);

		if (transform.position == startPosition)
			state = MovingObjectState.OnStartPosition;
	}
}
