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
    private void Start()
    {
        UIController.instance.UpdateVelocity(storedVelocity / maxVelocity);
    }
    public override void UpdateAction(ActionSet actions)
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
    public void Pull(bool trigger)
    {
        grabber.visual.SetActive(trigger);
        GetComponent<TwoAxisMovement>().locked = trigger;
        if (trigger)
        {
            hasPressed = true;
            if (grabber.currentObject == null)
            {
                foreach (Grabbable current in grabber.currentObjects)
                {
                    float newMag = current.magnitude - changeAmount;
                    if (newMag >= 0)
                    {
                        if (storedVelocity < maxVelocity)
                        {
                            storedVelocity += changeAmount;
                        }
                        current.ChangeVelocity(changeAmount);
                    }
                }
                storedVelocity = Mathf.Clamp(storedVelocity, 0, maxVelocity); ;
                UIController.instance.UpdateVelocity(storedVelocity / maxVelocity);
            }
            else
            {
                if (!hasStopped)
                {
                    foreach (Grabbable current in grabber.currentObjects)
                    {
                        current.StopChange();
                    }
                    hasStopped = true;
                }
            }
        }
        else
        {
            if (hasPressed)
            {
                hasPressed = false;
                hasStopped = false;
                currentMode = Mode.Push;
            }
        }
    }

    public void Push(bool trigger)
    {
        Grabbable current = grabber.currentObject;
        if (current)
        {
            if (trigger)
            {
                hasPressed = true;
                if (Mathf.Abs(current.magnitude) < current.maxMag)
                {
                    current.ChangeVelocity(changeAmount);
                    if (current.isHeld && storedVelocity > 0)
                    {
                        storedVelocity -= changeAmount;
                    }
                }
                //status.arrow.transform.localRotation = Quaternion.Euler(newMag > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180));
                storedVelocity = Mathf.Clamp(storedVelocity, 0, maxVelocity);
                status.arrowCanvas.gameObject.SetActive(true);
                status.arrow.fillAmount = Mathf.Abs(current.magnitude / current.maxMag);
                UIController.instance.UpdateVelocity(storedVelocity / maxVelocity);
            }
            else
            {
                if (hasPressed)
                {
                    current.StopChange();
					current = null;
                    hasPressed = false;
                    hasStopped = false;
                    currentMode = Mode.Pull;
                }
                status.arrowCanvas.gameObject.SetActive(false);
            }
        }
        else
        {
            hasPressed = false;
            hasStopped = false;
            currentMode = Mode.Pull;
        }
    }
    private void Update()
    {
        grabber.transform.position = transform.position + (status.CurrentFacing * grabOffset);
        grabber.transform.rotation = Quaternion.LookRotation(status.CurrentFacing, Vector3.up);
    }
}
