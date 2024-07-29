using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 3;
    private int currentAmmo;

    public Image[] ammoImages; // Array to hold ammo UI images
    public Sprite fullAmmoSprite;
    public Sprite emptyAmmoSprite;

    public GameObject gameOverPanel; // Panel to show when the game is over

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        gameOverPanel.SetActive(false);
    }

    // Method to handle a missed shot
    public void ShotMissed()
    {
        currentAmmo--;
        UpdateAmmoUI();
        if (currentAmmo <= 0)
        {
            EndGame();
        }
    }

    // Method to update the ammo UI **
    private void UpdateAmmoUI()
    {
        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (i < currentAmmo)
            {
                ammoImages[i].enabled = true; // Display bullet
            }
            else
            {
                ammoImages[i].enabled = false; // Hide bullet
            }
        }
    }
    // Method to handle the end of the game
    private void EndGame()
    {
        gameOverPanel.SetActive(true);
        // Additional end game logic can be added here
    }
}