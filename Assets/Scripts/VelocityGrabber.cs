using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityGrabber : ControlAction
{
    public Grabber grabber;
    public float grabOffset = 1f;
    private StatusController status;

    public float changeAmount;
    public float storedVelocity;
    public float maxVelocity = 30;

    public AudioClip full;
    private AudioSource shakeSource;
    public AudioClip shake;
    public enum Mode
    {
        Pull,
        Push
    }
    public Mode currentMode;
    protected override void Awake()
    {
        base.Awake();
        status = GetComponent<StatusController>();
    }

    private bool hasPressed;
    private bool hasStopped = false;

    public GameObject throwVis;
    private void Start()
    {
        GetComponent<Animator>().SetFloat("charge", storedVelocity / maxVelocity);
        shakeSource = AudioManager.instance.AddSourcePersistent();
        shakeSource.Stop();
        shakeSource.playOnAwake = false;
        shakeSource.volume = .5f;
        shakeSource.clip = shake;
        shakeSource.loop = true;
    }
    public override void UpdateAction(ActionSet actions)
    {
        if (!status.isDying)
        {
            switch (currentMode)
            {
                case Mode.Pull:
                    Pull(actions.primaryAction);
                    break;
                case Mode.Push:
                    Push(actions.primaryAction);
                    break;
            }
        }
        else
        {
            foreach (Grabbable current in grabber.currentObjects)
            {
                current.StopChange();
            }
        }
    }
    public void Pull(bool trigger)
    {
        grabber.visual.SetActive(trigger);
        GetComponent<TwoAxisMovement>().locked = trigger;
        if (trigger)
        {
            if(grabber.currentObjects.Count > 0)
            {
                if(!shakeSource.isPlaying)
                {
                    shakeSource.Play();
                }
            }
            else
            {
                if(shakeSource.isPlaying)
                {
                    shakeSource.Stop();
                }
            }
            hasPressed = true;
            if (grabber.currentObject == null)
            {
                foreach (Grabbable current in grabber.currentObjects)
                {
                    if (!current.isEnemies)
                    {
                        if (!current.changing)
                        {
                            current.StartChange(grabber.holder.transform);
                        }
                        if (current.magnitude > 0)
                        {
                            if (storedVelocity < maxVelocity)
                            {
                                storedVelocity += changeAmount * Time.deltaTime;
                            }
                            current.ChangeVelocity(changeAmount);
                        }
                        else
                        {
                            current.SetMagnitude(0);
                        }
                    }
                }
                storedVelocity = Mathf.Clamp(storedVelocity, 0, maxVelocity); ;
                GetComponent<Animator>().SetFloat("charge", storedVelocity / maxVelocity);
            }
            else
            {
                if (!hasStopped)
                {
                    foreach (Grabbable current in grabber.currentObjects)
                    {
                        if (!current.Equals(grabber.currentObject))
                        {
                            current.StopChange();
                        }
                    }
                    hasStopped = true;
                }
                if(shakeSource.isPlaying)
                {
                    shakeSource.Stop();
                }
            }
        }
        else
        {
            if (hasPressed)
            {
                foreach (Grabbable current in grabber.currentObjects)
                {
                    if (!current.Equals(grabber.currentObject))
                    {
                        current.StopChange();
                    }
                }
                hasPressed = false;
                hasStopped = false;
                currentMode = Mode.Push;
            }
        }
    }

    public void Push(bool trigger)
    {
        Grabbable current = grabber.currentObject;
        GetComponent<TwoAxisMovement>().locked = trigger;
        if (current)
        {
            throwVis.SetActive(trigger);
            if(trigger)
            {
                if(!shakeSource.isPlaying)
                {
                    shakeSource.Play();
                }
            }
            else
            {
                if(shakeSource.isPlaying)
                {
                    shakeSource.Stop();
                }
            }
            if (trigger)
            {
                hasPressed = true;
                if (Mathf.Abs(current.magnitude) < current.maxMag)
                {
                    if (storedVelocity > 0)
                    {
                        current.ChangeVelocity(changeAmount);
                        storedVelocity -= changeAmount * Time.deltaTime;
                    }
                }
                storedVelocity = Mathf.Clamp(storedVelocity, 0, maxVelocity);
                GetComponent<Animator>().SetFloat("charge", storedVelocity / maxVelocity);
            }
            else
            {
                if (hasPressed)
                {
                    current.StopChange();
                    grabber.currentObject = null;
                    hasPressed = false;
                    hasStopped = false;
                    currentMode = Mode.Pull;
                }
            }
        }
        else
        {
            hasPressed = false;
            hasStopped = false;
            currentMode = Mode.Pull;
        }
    }
    private bool fullUp;
    private void Update()
    {
        grabber.transform.position = transform.position + (status.CurrentFacing * grabOffset);
        grabber.transform.rotation = Quaternion.LookRotation(status.CurrentFacing, Vector3.up);
        if (fullUp)
        {
            if (storedVelocity < maxVelocity)
            {
                fullUp = false;
            }
        }
        else
        {
            if (storedVelocity >= maxVelocity)
            {
                AudioManager.instance.PlaySfx(full);
                fullUp = true;
            }
        }
    }
}
