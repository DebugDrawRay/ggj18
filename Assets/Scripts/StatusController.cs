using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusController : MonoBehaviour
{
    private Vector3 currentFacing = Vector3.forward;
    public Vector3 CurrentFacing
    {
        get
        {
            return currentFacing;
        }
        set
        {
            if (cardinalizeDirection)
            {
				currentFacing = GetFacingCardinal(value);
            }
			else
			{
            	currentFacing = value;
			}
        }
    }

    public Vector3 GetFacingCardinal(Vector3 vector)
    {
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
        {
            vector.z = 0;
            vector.x = Mathf.Round(vector.x);
        }
        else
        {
            vector.x = 0;
            vector.z = Mathf.Round(vector.z);
        }
		return vector;
    }

    public bool cardinalizeDirection = true;
	public GameObject arrowCanvas;
	public Image arrow;
	public int currentHealth = 3;
	public bool inKnockback;
	public float knockbackTime = .5f;
	private void Start()
	{
		UIController.instance.UpdateHealth(currentHealth);
	}
	private void Update()
	{
		if(arrowCanvas)
		{
			arrowCanvas.transform.rotation = Quaternion.LookRotation(GetFacingCardinal(currentFacing), Vector3.up);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<Grabbable>())
		{
			
		}
	}
}
