using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public UIDocument uiDocument;

    private Rigidbody rb;
    private Animator anim;
    private Animator animE;
    private int count;
    private float movementX;
    private float movementY;
    private Button homeButton;
    private Button restartButton;
    private Label CountText;
    private Label WinText;
    public float speed = 0;

    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip winSound;
    public AudioClip deathSound;

    void Start()
    {
        rb= GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        count = 0;
        SetTextCount();

        var root = uiDocument.rootVisualElement;
        restartButton = root.Q<Button>("Restart");
        CountText = root.Q<Label>("CountText");
        WinText = root.Q<Label>("WinText");

        if (restartButton != null)
        {
            restartButton.style.display = DisplayStyle.None;
            restartButton.clicked += ReloadScene;
        }

        if (WinText != null)
        {
            WinText.style.display = DisplayStyle.None;
        }
    }
    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        homeButton = root.Q<Button>("Home");
        if (homeButton != null)
        {
            homeButton.clicked += () => SceneManager.LoadScene("MainScreen");
        }
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        float inputMagnitude = new Vector2(movementX, movementY).magnitude;
        anim.SetFloat("Speed", inputMagnitude);

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            anim.SetFloat("Speed", 0.0f);
        }
        Vector3 currentEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentEuler.y, 0);

        rb.angularVelocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            PlaySound(pickupSound);
            other.gameObject.SetActive(false);
            count = count + 1;
            SetTextCount();
        }
    }
    void SetTextCount()
    {
        if (CountText != null) CountText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            PlaySound(winSound);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            if (WinText != null)
            {
                WinText.style.display = DisplayStyle.Flex;
            }
            if (restartButton != null)
            {
                restartButton.style.display = DisplayStyle.Flex;
                restartButton.clicked += ReloadScene;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlaySound(deathSound);
            Animator enemyAnim = collision.gameObject.GetComponentInChildren<Animator>();

            if (enemyAnim != null)
            {
                enemyAnim.SetFloat("Speed", 0.0f);
            }
            gameObject.SetActive(false);
            if (WinText != null)
            {
                WinText.style.display = DisplayStyle.Flex;
                WinText.Q<Label>().text = "You Lose!";
            }
            if (restartButton != null)
            {
                restartButton.style.display = DisplayStyle.Flex;
                restartButton.clicked += ReloadScene;
            }
        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}