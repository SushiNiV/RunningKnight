using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip deathSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            PlaySound(pickupSound);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void Die()
    {
        PlaySound(deathSound);
        Invoke("DisablePlayer", 0.5f);
    }

    void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}