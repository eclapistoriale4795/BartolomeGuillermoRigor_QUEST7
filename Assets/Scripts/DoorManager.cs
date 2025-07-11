using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Animator animator;
    public bool open;
    public float toggleCooldown;
    public float cooldownTimer;

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            cooldownTimer = 0;
        }
        if (open)
        {
            animator.Play("DoorOpenIdle");
        }
        else { animator.Play("DoorCloseIdle"); }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (cooldownTimer == 0f)
                {
                    cooldownTimer = toggleCooldown;
                    if (open) { open = false; animator.Play("DoorClosed"); Debug.Log("Closed"); }
                    else { open = true; animator.Play("DoorOpened"); Debug.Log("Open"); }
                }
            }
        }
    }
}
