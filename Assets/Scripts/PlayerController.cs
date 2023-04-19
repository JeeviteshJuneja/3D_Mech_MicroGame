using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float health = 10f;

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float pitchSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private Vector3 upperBounds;
    [SerializeField] private Vector3 lowerBounds;

    private bool jump = false;
    private bool crouch = false;
    private bool isOnGround = true;
    private bool boost = false;

    private int turn = 0;
    private Vector2 mouse;
    private float pitch = 0;
    private float pitchRange = 45;

    private Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotatePlayer();

        MovePlayer();

        PitchAim();

        if (jump)
        {
            PlayerJump();
        }

        if (crouch)
        {
            PlayerCrouch();
        }

        if (boost)
        {
            PlayerBoost();
        }

        BoundPlayer();

        //output player velocity for debuging
        //Debug.Log(playerRb.velocity);
    }

    private void Update()
    {
        if (health < 0)
        {
            Debug.Log("Game Over!");
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            jump = true;
        
        }
        if (Input.GetKeyDown(KeyCode.C) && !isOnGround)
        {
            crouch = true;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            boost = true;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            turn = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            turn = 1;
        }
        else
        {
            turn = 0;
        }

        mouse.x = Input.GetAxis("Mouse X");
        mouse.y = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void RotatePlayer()
    {
        Quaternion detaRot = Quaternion.Euler(new Vector3(0, turn+mouse.x, 0) * turnSpeed * Time.fixedDeltaTime);
        playerRb.MoveRotation(playerRb.rotation * detaRot);
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(transform.right * speed * horizontalInput, ForceMode.Force);

        playerRb.AddForce(transform.forward * speed * verticalInput, ForceMode.Force);
    }

    private void PitchAim()
    {
        if (isOnGround)
        {
            pitch = Mathf.Clamp(pitch - mouse.y * pitchSpeed, -pitchRange, 0);
        }
        else
        {
            pitch = Mathf.Clamp(pitch - mouse.y * pitchSpeed, -pitchRange, pitchRange);
        }
    }

    private void PlayerJump()
    {
        playerRb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        jump = false;
        isOnGround = false;
    }

    private void PlayerCrouch()
    {
        playerRb.AddForce(Vector3.down * jumpSpeed, ForceMode.Impulse);
        crouch = false;
    }

    private void PlayerBoost()
    {
        playerRb.AddForce(playerRb.transform.forward * boostSpeed, ForceMode.Impulse);
        boost = false;
    }

    private void BoundPlayer()
    {
        float x = Mathf.Clamp(playerRb.transform.position.x, lowerBounds.x, upperBounds.x);
        float y = Mathf.Clamp(playerRb.transform.position.y, lowerBounds.y, upperBounds.y);
        float z = Mathf.Clamp(playerRb.transform.position.z, lowerBounds.z, upperBounds.z);
        playerRb.transform.position = new Vector3(x, y, z);
    }

    private void Shoot()
    {
        Quaternion pitchRot = Quaternion.Euler(new Vector3(pitch, 0, 0));
        Instantiate(bulletPrefab, transform.position+transform.forward*2, transform.rotation*pitchRot*bulletPrefab.transform.rotation);
    }
}
