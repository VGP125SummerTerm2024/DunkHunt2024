using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 3;
    public int currentAmmo;

    public GameObject[] ammoImages; // Array to hold ammo UI images
    public Sprite fullAmmoSprite;
    public Sprite emptyAmmoSprite;

    public GameObject ammoUICanvas;

    AudioSource audioSource;
    public AudioClip gunSound;

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        ammoUICanvas.SetActive(true);
        audioSource = GetComponent<AudioSource>();
    }

    // Method to handle a missed shot
    public void UpdateAmmo()
    {
        if (currentAmmo > 0)
        {
            PlaySoundOnce(gunSound);
            --currentAmmo;
            UpdateAmmoUI();

            if (currentAmmo <= 0)
            {
                EndGame();
            }
        }

        //if (currentAmmo <= 0)
        //{
        //    //EndGame();
        //}
    }

    // Method to update the ammo UI **
    private void UpdateAmmoUI()
    {
        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (i < currentAmmo)
            {
                ammoImages[i].SetActive(true); // Display bullet
            }
            else
            {
                ammoImages[i].SetActive(false); // Hide bullet
            }
        }
    }
    // Method to handle the end of the game
    private void EndGame()
    {
        //ammoUICanvas.SetActive(false);
        // Additional end game logic can be added here
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void reload()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        ammoUICanvas.SetActive(true);
    }

    public int getAmmo()
    {
        return currentAmmo;
    }
}