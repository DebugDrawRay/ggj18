using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityGrabber : ControlAction
{
    public Grabber grabber;
    public float grabOffset = 1f;
    private StatusController status;

    public float changeAmount;
    private bool hasStopped = true;
    public float storedVelocity;
    public float maxVelocity = 30;
    protected override void Awake()
    {
        base.Awake();
        status = GetComponent<StatusController>();
    }
    private void Start()
    {
        UIController.instance.UpdateVelocity(storedVelocity / maxVelocity);
    }
    public override void UpdateAction(ActionSet actions)
    {
        if (status.inKnockback)
        {
            actions.primaryAction = false;
        }
        else
        {
            if (grabber.currentObject != null)
            {
				Grabbable current = grabber.currentObject;
                if (actions.primaryAction)
                {
                    if (hasStopped)
                    {
                        current.StartChange(grabber.transform);
                        hasStopped = false;
                    }

                    float newMag = current.magnitude - changeAmount;
                    if (newMag > 0)
                    {
                        if (storedVelocity < maxVelocity)
                        {
                            storedVelocity += changeAmount;
                        }
						else
						{
							current.SetMagnitude(0);
						}
						current.ChangeVelocity(changeAmount);
                        status.arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    else
                    {
                        if (storedVelocity > 0 && Mathf.Abs(current.magnitude) < current.maxMag)
                        {
                            current.ChangeVelocity(changeAmount);
                            storedVelocity -= changeAmount;
                        }
                    }
                    status.arrow.transform.localRotation = Quaternion.Euler(newMag > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180));
                    storedVelocity = Mathf.Clamp(storedVelocity, 0, maxVelocity);
                    status.arrowCanvas.gameObject.SetActive(true);
                    status.arrow.fillAmount = Mathf.Abs(current.magnitude / current.maxMag);
                    UIController.instance.UpdateVelocity(storedVelocity / maxVelocity);
                }
                else
                {
                    if (!hasStopped)
                    {
                        current.StopChange();
                        hasStopped = true;
                    }
                    status.arrowCanvas.gameObject.SetActive(false);
                }
            }
        }
    }
    private void Update()
    {
        grabber.transform.position = transform.position + (status.CurrentFacing * grabOffset);
    }
}
