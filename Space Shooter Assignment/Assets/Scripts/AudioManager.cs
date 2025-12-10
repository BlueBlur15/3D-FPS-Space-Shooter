using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    // SFX Sounds
    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip landSound; // For landing from a high place or jump
    public AudioClip walkSound; // Make different sounds for different floors if we do anything other than metal
    public AudioClip sprintSound; // It's safer just to stick with metal, though
    public AudioClip crouchSound;
    public AudioClip deathSound;
    public AudioClip hurtSoundOne;
    public AudioClip hurtSoundTwo;

    [Header("Enemy Sounds")]
    public AudioClip alienOneIdleSound;
    public AudioClip alienOneDeathSound;
    public AudioClip alienOneAttackSound;
    public AudioClip alienOneHurtSound;

    public AudioClip alienTwoIdleSound;
    public AudioClip alienTwoDeathSound;
    public AudioClip alienTwoAttackSound;
    public AudioClip alienTwoHurtSound;

    public AudioClip alienThreeIdleSound;
    public AudioClip alienThreeDeathSound;
    public AudioClip alienThreeAttackSound;
    public AudioClip alienThreeHurtSound;

    [Header("Weapon Sounds")]
    public AudioClip pistolFireSound;
    public AudioClip pistolReloadSound;
    public AudioClip semiRifleFireSound;
    public AudioClip semiRifleReloadSound;
    public AudioClip autoRifleFireSound;
    public AudioClip autoRifleReloadSound;
    public AudioClip ricochetGunFireSound;
    public AudioClip ricochetGunReloadSound;
    public AudioClip knifeSwingSound;
    public AudioClip knifeFleshSound; // Making contact with an enemy
    public AudioClip knifeObjectSound; // Stabbing a wall or some other inanimate object
    public AudioClip grenadeThrowSound; // Include whatever pin or button sound needed and a throwing noise
    public AudioClip grenadeExplodeSound;
    public AudioClip weaponPickupSound;
    public AudioClip weaponObjectSound; // For shooting inanimate objects and walls

    [Header("Level Sounds")]
    public AudioClip horrorOneSound; // Think Minecraft cave noises
    public AudioClip horrorTwoSound;
    public AudioClip horrorThreeSound;
    public AudioClip explosionSound; // Different from grenade explosion
    public AudioClip generatorSound;
    public AudioClip beepingSounds; // Like for high tech areas, think Sci-Fi movies whenever terminals and buttons are nearby
    public AudioClip doorOpenSound; // Futuristic sliding door
    public AudioClip doorCloseSound;
    public AudioClip terminalEnterSound;
    public AudioClip terminalExitSound;


    // Music Sounds
    public AudioClip mainMenuMusic; // Plays for the Instructions Scene too
    public AudioClip pauseMenuMusic;
    public AudioClip gameOverMusic;
    public AudioClip gameWinMusic;
    public AudioClip escapeMusic; // If we ever want to pull off a Metroid escape scene in one of our levels
    public AudioClip combatMusic; // Something metal/rock guitar, there are some good free musics out there
    public AudioClip levelOneMusic;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip) { sfxSource.PlayOneShot(clip); }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic() { musicSource.Stop(); }
}
