using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    public CharacterController playerControl;
    public GameObject deadPanel;
    public GameObject victoryPanel;

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

    private Vector3 touchStartPosition, touchEndPosition;
    private float originalSpeed;
    private Quaternion originalRotation;
    private Vector3 lastCheckpoint;

    private void Start()
    {
        originalRotation = transform.rotation;
        originalSpeed = actPlayerSpeed;
        timeForNextIdle = Time.timeSinceLevelLoad + Random.Range(30, 60) / 10f;
        playerAnimator = GetComponent<Animator>();
    }

    public void Respawn()
    {
        transform.rotation = originalRotation;
        transform.position = lastCheckpoint;
        gameObject.SetActive(true);
        //Hold player input for 3 seconds, if possible.
    }

    public void ChangeCheckpoint(Transform transformCheckpoint)
    {
        lastCheckpoint = transformCheckpoint.position;
    }

    private void GetInput()
    {
        if (playerControl.isGrounded)
        {
            if (jumping)
            {
                jumping = false;
                playerAnimator.SetBool("Jump", jumping);
            }
        }

#if UNITY_EDITOR
        playerMotionVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        if (Input.GetAxis("Horizontal") > 0)
        {
            if (isFacingLeft)
            {
                isFacingLeft = false;
                StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(0, 90, 0)), .25f));
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (!isFacingLeft)
            {
                isFacingLeft = true;
                StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(0, -90, 0)), .25f));
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            if (playerControl.isGrounded)
            {
                Jump();
            }
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) //jump if up
            {
                touchEndPosition = touch.position;


                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) != 0 && Mathf.Abs(y) != 0) //Tap
                {
                    if (!(Mathf.Abs(x) > Mathf.Abs(y))) //Left Right
                    {
                        //else
                        if (y > 0)
                        {
                            Jump();
                        }
                    }
                }
            }
            else if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                touchStartPosition = touch.position;

                playerMotionVector = new Vector3(touchStartPosition.x < Screen.width / 2 ? -1 : 1, 0, 0);

                if (playerMotionVector.x > 0)
                {
                    if (isFacingLeft)
                    {
                        isFacingLeft = false;
                        StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(0, 90, 0)), .25f));
                    }
                } else
                {
                    if (!isFacingLeft)
                    {
                        isFacingLeft = true;
                        StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(0, -90, 0)), .25f));
                    }
                }
            }
        } else
        {
            playerMotionVector = Vector3.zero;
        }
#endif


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

    public void OnBecameInvisible()
    {
        GameFlags.Instance.DeadCounter();
        deadPanel.SetActive(true);
        IAPManager.Instance.DeadPlayer();
        gameObject.SetActive(false);
    }

    private void Jump()
    {
        jumping = true;
        playerAnimator.SetBool("Jump", jumping);

        AudioManager.Instance.PlayAudio(AudioTypeEnum.SoundEffects_01, 0f, true, .1f);
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
            }
            else
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

                    timeForNextIdle = Time.timeSinceLevelLoad + Random.Range(30, 60) / 10f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            OnBecameInvisible();
        } else if (other.tag == "Water")
        {
            actPlayerSpeed *= 0.8f;
        } else if (other.tag == "Victory")
        {
            victoryPanel.SetActive(true);
            PauseController.Instance.OnPause();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            actPlayerSpeed = originalSpeed;
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        Movement();

        if (transform.position.y < -1)
        {
            OnBecameInvisible();
        }
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