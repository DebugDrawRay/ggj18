using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public Rigidbody rigid
    {
        get;
        private set;
    }

    public float maxMag = 10;
    public bool changing
    {
        get;
        private set;
    }

    private Vector3 direction;

    public bool aggressive = true;

    public float heldDistance = 3;
    public GameObject changeVisual;

    public bool isHeld
    {
        get;
        private set;
    }
    public bool isEnemies = true;
    private Transform parent;
    public Transform visual;
    public float magnitude
    {
        get;
        private set;
    }

    public AudioClip thrown;
    public AudioClip grab;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void StartChange(Transform newParent)
    {
        direction = rigid.velocity.normalized;
        magnitude = rigid.velocity.magnitude;

        changing = true;
        changeVisual.SetActive(true);

        parent = newParent;
        isHeld = false;
    }
    public void ChangeVelocity(float amount)
    {
        magnitude -= amount * Time.deltaTime;
    }

    public void Update()
    {
        if (changing)
        {
            if (!isHeld)
            {
                Vector2 pos = Random.insideUnitSphere;
                Vector3 actual = new Vector3(pos.x, 0, pos.y);
                visual.transform.localPosition = actual * 0.05f;
            }
            if (magnitude > 0)
            {
                rigid.velocity = direction * magnitude;
            }
            else
            {
                if (!isHeld)
                {
                    if (Vector3.Distance(PlayerController.position, transform.position) <= heldDistance && PlayerController.instance.GetComponent<VelocityGrabber>().grabber.currentObject == null)
                    {
                        rigid.isKinematic = true;
                        transform.SetParent(parent);
                        transform.localPosition = Vector3.zero;
                        visual.transform.localPosition = Vector3.zero;
                        PlayerController.instance.GetComponent<VelocityGrabber>().grabber.currentObject = this;
                        isHeld = true;
                        AudioManager.instance.PlaySfx(grab);
                        changeVisual.SetActive(false);
                        gameObject.layer = LayerMask.NameToLayer("GrabbableFromEnemy");
                    }
                }
            }
        }


    }
    public void StopChange()
    {
        if (magnitude < 0)
        {
            gameObject.layer = LayerMask.NameToLayer("GrabbableFromPlayer");
            direction = -PlayerController.instance.GetComponent<StatusController>().CurrentFacing;
            aggressive = false;
        }

        transform.SetParent(null);
        rigid.isKinematic = false;

        parent = null;
        if (isHeld)
        {
            magnitude = Mathf.Clamp(magnitude, -maxMag, maxMag);
            rigid.velocity = direction * magnitude;
            isHeld = false;
            AudioManager.instance.PlaySfx(thrown);
        }

        magnitude = 0;
        direction = Vector3.zero;
        changing = false;
        changeVisual.SetActive(false);
    }

    public void SetMagnitude(float mag)
    {
        magnitude = magnitude;
        rigid.velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision other)
    {
        rigid.velocity = Vector3.zero;
        aggressive = false;
    }
}
