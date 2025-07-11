using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
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
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else { transform.localRotation = Quaternion.Euler(0, -90, 0); }
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
                    if (open) { open = false; Debug.Log("Closed"); }
                    else { open = true; Debug.Log("Open"); }
                }
            }
        }
    }
}
