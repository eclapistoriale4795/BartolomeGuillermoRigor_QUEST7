using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float damage_cooldown = 1f;
    public float dmg_cooldownTimer = 0f;
    [Header("Camera Reference")]
    public Camera playerCam;

    [Header("Camera Rotation")]
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    [Header("Movement Condition")]
    public bool canMove = true;

    [Header("Scriptable Object")]
    public Player_S player;
    [SerializeField] bool alive;
    [SerializeField] float hp;
    [SerializeField] float st;
    public bool isAttacking=false;
    public GameObject attackOBJ;

    // Start is called before the first frame update
    void Start()
    {
        player.characterController=GetComponent<CharacterController>();
        player.InitializeAll();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Stats()
    {
        alive = player.gameOK;
        hp = player.tempHP;
        st = player.tempStamina;
        if (dmg_cooldownTimer > 0)
        {
            dmg_cooldownTimer -= Time.deltaTime;
        }
        else
        {
            dmg_cooldownTimer = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        if (dmg_cooldownTimer == 0)
        {
            player.tempHP -= damage;
            dmg_cooldownTimer = damage_cooldown;
            if (player.tempHP <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Stats();
        if (player.gameOK)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
            if (isAttacking)
            {
                attackOBJ.SetActive(true);
            }
            else
            {
                attackOBJ.SetActive(false);
            }
            //this is for showing the cursor------------------------------------
            if (Input.GetKey(KeyCode.Z))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            //end cursor conditions---------------------------------------------

            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);


            //press left shift to run
            //this will return true if the specific button is pressed (lShift)
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            if (isRunning && Input.GetKey(KeyCode.W) && player.tempStamina >= 0)
            {
                //sliderStamina.gameObject.SetActive(true);
                player.tempStamina -= 10 * Time.deltaTime; //decreases the stamina overtime;
                if (player.tempStamina <= 0)
                {
                    player.tempStamina = 0;
                }
            }
            if (player.tempStamina <= player.baseStamina && !isRunning)
            {
                player.tempStamina += 10 * Time.deltaTime;
                if (player.tempStamina >= player.baseStamina)
                {
                    player.tempStamina = player.baseStamina; //this will prevent the stamina from geting a higher value
                }
            }

            //conditions for movement
            // if ? then : else
            float curSpeedX = canMove ? (isRunning && player.tempStamina >= 1 ? player.runningSpeed : player.walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning && player.tempStamina >= 1 ? player.runningSpeed : player.walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = player.moveDirection.y;
            player.moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            //for the jumping condition
            if (Input.GetButton("Jump") && canMove && player.characterController.isGrounded)
            {
                player.moveDirection.y = player.jumpForce;
            }
            else
            {
                player.moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!player.characterController.isGrounded)
            {
                //pull the object down
                player.moveDirection.y -= player.gravity * Time.deltaTime;
            }

            // Move the controller
            player.characterController.Move(player.moveDirection * Time.deltaTime);

            //Player and Camera rotation
            if (canMove)
            {
                player.rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                player.rotationX = Mathf.Clamp(player.rotationX, -lookXLimit, lookXLimit); //this limits the angle of the x rotation
                playerCam.transform.localRotation = Quaternion.Euler(player.rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
