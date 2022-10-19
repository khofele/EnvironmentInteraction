using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    // General fields
    private CharacterController characterController = null;
    private Animator animator = null;
    private IKController iKController = null;
    private AnimationManager animationManager = null;
    private Cinemachine.CinemachineFreeLook freeLookCamera = null;
    private Cinemachine.CinemachineVirtualCamera virtualCamera = null;

    // Movement fields
    private float speed = 0f;
    private float gravity = -9.81f * 2;
    private Vector3 velocity = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private bool isCrouching = false;
    private bool isLeaning = false;
    private float xAxis = 0f;
    private float zAxis = 0f;

    public Animator Animator { get => animator; }
    public bool IsCrouching { get => isCrouching; }
    public bool IsLeaning { get => isLeaning; set => isLeaning = value; }
    public float XAxis { get => xAxis; }
    public float ZAxis { get => zAxis; }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        iKController = GetComponent<IKController>();
        animationManager = GetComponent<AnimationManager>();
        freeLookCamera = GetComponentInChildren<Cinemachine.CinemachineFreeLook>();
        virtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Limbs"), LayerMask.NameToLayer("Default"));

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        moveDirection = new Vector3(xAxis, 0f, zAxis).normalized;
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);

        // TODO Cameramovement in eigenes Skript
        if (isLeaning == false)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized, Camera.main.transform.up);
            virtualCamera.enabled = false;
            freeLookCamera.enabled = true;
        }
        else
        {
            freeLookCamera.enabled = false;
            virtualCamera.enabled = true;
        }

        // check if button is pressed
        if (moveDirection.magnitude >= 0.01f)
        {
            if (Input.GetKey(KeyCode.LeftShift) && isCrouching == false)
            {
                // Running
                speed = 10f;
                animationManager.ExecuteRunAnimation(zAxis, xAxis);
            }
            else if (isCrouching == true)
            {
                // Crouching
                speed = 2.5f;
            }
            else
            {
                // Walking
                speed = 3f;
                animationManager.ExecuteWalkAnimation(zAxis, xAxis);
            }

            // move character
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(moveDirection * speed * Time.deltaTime);
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            // Idle
            speed = 0;
        }

        // Crouch Toggle
        if (Input.GetKeyDown(KeyCode.C) && isCrouching == false)
        {
            isCrouching = true;
            animationManager.ExecuteCrouchAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.C) && isCrouching == true)
        {
            isCrouching = false;
            animationManager.StopCrouchAnimation();
        }

        animationManager.SetSpeed(speed);
    }
}
