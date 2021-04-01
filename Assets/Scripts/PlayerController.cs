using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController playerControl;

    public float actPlayerSpeed;
    //public float playerSprint;
    //public float playerDuck;
    public float playerJumpForce;
    //public float playerRotateSpeed;

    //private Vector3 playerMotionInput;
    private Vector3 playerMotionVector;
    private Animator playerAnimator;
    private Coroutine rotateAround;
    private bool isFacingLeft;
    private bool jumping = false;
    private float timeForNextIdle;

    private void Start()
    {
        timeForNextIdle = Time.timeSinceLevelLoad + Random.Range(30, 60) / 10f;
        playerAnimator = GetComponent<Animator>();
    }

    private void GetInput()
    {
        playerMotionVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        if (Input.GetAxis("Horizontal") > 0)
        {
            if (isFacingLeft)
            {
                isFacingLeft = false;
                StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(0, 90, 0)), .25f));
            }
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            if (!isFacingLeft)
            {
                isFacingLeft = true;
                StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(0, -90, 0)), .25f));
            }
        }


        //Get input from WASD keys.
        //playerMotionInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //playerMotionVector = transform.TransformDirection(playerMotionInput);


        //LeftCtrl to sprint. LeftShift to crouch.
        /*if (Input.GetAxis("Sprint") != 0 && playerControl.isGrounded)
        {
            actPlayerSpeed = playerSprint;
        }
        else
        {
            actPlayerSpeed = playerSpeed;
        }*/
        //if (Input.GetAxis("Duck") != 0)
        //{
        //    actPlayerSpeed = playerDuck;
        //}
        //if (Input.GetAxis("Sprint") == 0 && Input.GetAxis("Duck") == 0)
        //{
        //    actPlayerSpeed = playerSpeed;
        //}
    }

    private void Movement()
    {
        playerAnimator.SetBool("Walk", false);
        if (jumping)
        {
            playerControl.Move(new Vector3(playerMotionVector.x * (actPlayerSpeed / 2.5f) * Time.deltaTime, playerJumpForce, playerMotionVector.z * (actPlayerSpeed / 2) * Time.deltaTime) * Time.deltaTime);
            playerControl.SimpleMove(Vector3.zero * actPlayerSpeed * Time.deltaTime);
        }
        else
        {
            playerControl.SimpleMove(playerMotionVector * actPlayerSpeed * Time.deltaTime);
            if (playerMotionVector.magnitude > 0)
            {
                playerAnimator.SetBool("Walk", true);
                timeForNextIdle += Time.fixedDeltaTime;
            } else
            {
                if (Time.timeSinceLevelLoad > timeForNextIdle)
                {
                    int zeroToNine = Random.Range(0, 10000000) / 1000000;

                    if (zeroToNine > 6) //0 to 9
                    {
                        playerAnimator.SetBool("Turn Head", true);
                        this.Invoke(() => DisableAnimation("Turn Head"), 1f);
                    }
                    else if (zeroToNine > 3)
                    {
                        playerAnimator.SetBool("Eat", true);
                        this.Invoke(() => DisableAnimation("Eat"), 1f);
                    }

                    timeForNextIdle = Time.timeSinceLevelLoad + Random.Range(30,60)/10f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Jump") != 0 && playerControl.isGrounded)
        {
            jumping = true;
            playerAnimator.SetBool("Jump", jumping);
        }
        else if (playerControl.isGrounded)
        {
            if (jumping)
            {
                jumping = false;
                playerAnimator.SetBool("Jump", jumping);
            }
        }
        GetInput();
        Movement();
    }

    void DisableAnimation(string boolName)
    {
        playerAnimator.SetBool(boolName, false);
    }

    IEnumerator LerpFunction(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
    }
}