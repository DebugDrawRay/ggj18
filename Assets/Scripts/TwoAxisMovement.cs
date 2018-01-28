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
    public bool locked;
    public bool updateFacing = true;
    private StatusController status;
    private Animator anim;

    private AudioSource source;
    public AudioClip walk;
    protected override void Awake()
    {
        base.Awake();
        status = GetComponent<StatusController>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
       source = AudioManager.instance.AddSourcePersistent();
       source.Stop();
       source.playOnAwake = false;
       source.volume = .5f;
       source.clip = walk;
       source.loop = true;
    }
    public override void UpdateAction(ActionSet actions)
    {
        if (!status.inKnockback && !status.isDying)
        {
            Vector3 moveVector = actions.moveVector;

            if (moveVector != Vector3.zero)
            {
                currentDirection = moveVector;// Vector3.Lerp(currentDirection, moveVector, directionSmoothing);

                if (updateFacing && !locked)
                {
                    anim.SetFloat("x", moveVector.x);
                    anim.SetFloat("y", moveVector.z);
                    GetComponent<StatusController>().CurrentFacing = currentDirection.normalized;
                }
                currentAcceleration += acceleration;
                if(!source.isPlaying)
                {
                    source.Play();
                }
            }
            else
            {
                currentAcceleration -= acceleration;
                if(source.isPlaying)
                {
                    source.Stop();
                }
            }

            currentAcceleration = Mathf.Clamp(currentAcceleration, 0, 1);
            float currentSpeed = speed * accelerationCurve.Evaluate(currentAcceleration);
            if(locked)
            {
                currentSpeed *= .5f;
            }
            Vector3 totalDirection = currentDirection * Time.deltaTime * currentSpeed;

            transform.position = Vector3.Lerp(transform.position, transform.position + totalDirection, moveSmoothing);
        }
        else
        {
            source.Stop();
        }
    }
}
