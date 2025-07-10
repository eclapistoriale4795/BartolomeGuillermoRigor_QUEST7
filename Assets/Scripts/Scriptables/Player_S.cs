using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Players/Player")]
public class Player_S : ScriptableObject
{
    [Header("Controller Properties")]
    public CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;
    public float rotationX = 0f;

    [Header("Base Values")]
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpForce;
    public float gravity;

    [Header("Player Values")]
    public int playerLVL;
    public float baseHP;
    public float tempHP;
    public float baseStamina;
    public float tempStamina;
    public float atk;
    public bool gameOK;
    //More Coming Soon

    public void InitializeAll()
    {
        gameOK = true;
        baseHP = 100;
        baseStamina = 100;
        tempHP = 100;
        tempStamina = 100;
        atk = 1;
    }
}
