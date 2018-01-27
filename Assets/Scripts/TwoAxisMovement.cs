using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoAxisMovement : ControlAction 
{
	public float speed = 100;
	public float acceleration = 10;
	public float moveSmoothing = 0.1f;
	public AnimationCurve accelerationCurve;
	private Vector3 currentDirection;
	private float currentAcceleration;
    public bool canMove;
	public override void UpdateAction(ActionSet actions)
	{
        if(canMove)
        {
            Vector3 moveVector = actions.moveVector;
            
            if(moveVector != Vector3.zero)
            {
                currentDirection = moveVector;// Vector3.Lerp(currentDirection, moveVector, directionSmoothing);
                GetComponent<StatusController>().CurrentFacing = currentDirection.normalized;
                currentAcceleration += acceleration;
            }
            else
            {
                currentAcceleration -= acceleration;
            }

            currentAcceleration = Mathf.Clamp(currentAcceleration, 0, 1);
            float currentSpeed = speed * accelerationCurve.Evaluate(currentAcceleration);
            Vector3 totalDirection = currentDirection * Time.deltaTime * currentSpeed;

            transform.position = Vector3.Lerp(transform.position, transform.position + totalDirection, moveSmoothing);
        }
	}
}
