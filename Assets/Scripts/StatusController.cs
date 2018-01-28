using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public int maxHealth = 3;
    private int currentHealth;
    public bool inKnockback
    {
        get;
        private set;
    }
    public float knockbackTime = .5f;
    private float currentKnock = 0;
    public float knockbackForce = 5;
    private Vector3 knockbackTarget;
    public bool isPlayer;
    public LayerMask damageLayer;
    public Vector3 damageTriggerExtends;
    public Vector3 damageTriggerCenter;
    public float damageThresh = 5f;
    public AudioClip hurt;
    public AudioClip death;
    public AudioClip low;
    public bool isDying;
    private void Start()
    {
        currentHealth = maxHealth;
        if (isPlayer)
        {
            UIController.instance.UpdateHealth(currentHealth);
        }
    }
    private void Update()
    {
        if (currentKnock > 0)
        {
            transform.position = Vector3.Lerp(transform.position, knockbackTarget, .1f);
            currentKnock -= Time.deltaTime;
        }
        else
        {
            inKnockback = false;
        }
    }

    private void FixedUpdate()
    {
        if (!inKnockback)
        {
            Collider[] hits = Physics.OverlapBox(transform.position, damageTriggerExtends, Quaternion.identity, damageLayer);
            if (hits.Length > 0)
            {
                Grabbable obj = hits[0].gameObject.GetComponent<Grabbable>();
                if (obj && obj.rigid.velocity.magnitude > damageThresh)
                {
                    obj.rigid.velocity = Vector3.zero;
                    currentHealth -= 1;
                    if (isPlayer)
                    {
                        UIController.instance.UpdateHealth(currentHealth);
                    }
                    if (currentHealth <= 0)
                    {
                        Death();
                    }
					else
					{
                        if (isPlayer && currentHealth == 1)
                        {
                            AudioManager.instance.PlaySfx(low);
                        }
                        AudioManager.instance.PlaySfx(hurt);
                        Knockback(obj.transform.position);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Grabbable obj = other.gameObject.GetComponent<Grabbable>();
        // if (obj && obj.rigid.velocity.magnitude > damageThresh && !inKnockback)
        // {
        //     obj.rigid.velocity = Vector3.zero;
        //     Knockback(obj.transform.position);
        //     currentHealth -= 1;
        //     if (currentHealth <= 0)
        //     {
        //         Death();
        //     }
        // }
    }
    private void Knockback(Vector3 from)
    {
        currentKnock = knockbackTime;
        Vector3 dir = (transform.position - from).normalized;
        dir.y = 0;
        knockbackTarget = dir * knockbackForce;
        inKnockback = true;
    }
    private void Death()
    {
		StartCoroutine(Death_Coroutine());
    }

    private IEnumerator Death_Coroutine()
    {
        AudioManager.instance.PlaySfx(death);
        isDying = true;
        GetComponent<Animator>().SetTrigger("death");
        yield return new WaitForSeconds(1f);
		if(isPlayer)
		{
			SceneManager.LoadScene("You Lose");
		}
		else
		{
        	Destroy(gameObject);
		}
    }
}
