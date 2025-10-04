using System.Collections.Generic;
using UnityEngine;

public enum SfxSoundName
{
    //goblin
    GoblinBossExertion, GoblinCannon, GoblinClubThrow, GoblinClubThrowImpact,
    GoblinGrowl1, GoblinGrowl2, GoblinRatty1, GoblinRatty2, GoblinRatty3,
    GoblinSpearThrow,

    //player
    GhostAttack, GhostMovement, PlayerHit, PlayerWarp,

    //other
    AmbienceJungle, DeathSound
}

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager instance { get; private set; }

    [SerializeField] private List<AudioClip> sfxSoundClips = new List<AudioClip>(); //stores the sounds, where the index align with the variables in the SoundName enum
    [SerializeField] private AudioSource audioSourceObject;

    [Header("Volume Settings")]
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple SoundManager instances found! Destroying the duplicate.");
            Destroy(gameObject);
        }

        if (!audioSourceObject) Debug.LogError("AudioSource audioSourceObject is null in SoundManager.cs");
    }

    #region Public player
    //plays a sound
    public AudioSource PlaySound(SfxSoundName soundName, Transform parent = null, bool loop = false)
    {
        return CreateSound(sfxSoundClips[(int)soundName], sfxVolume, loop, parent);
    }

    //plays a random sound
    public AudioSource PlayRandomSound(SfxSoundName[] soundNames, Transform parent = null, bool loop = false)
    {
        int randIndex = Random.Range(0, soundNames.Length);
        SfxSoundName chosenSound = soundNames[randIndex];
        return CreateSound(sfxSoundClips[(int)chosenSound], sfxVolume, loop, parent);
    }

    //stops a sound loop
    public void StopSoundLoop(AudioSource audioSource)
    {
        audioSourceObject.Stop();
        Destroy(audioSource.gameObject); //destorys the gameobject when it is no longer used
    }
    #endregion

    #region Sound player
    //plays the sound
    private void PlaySound(AudioClip clip, AudioSource audioSource, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
        Destroy(audioSource.gameObject, clip.length); //destorys the gameobject after the clip is finished

    }

    //plays a sound loop
    private void PlayLoop(AudioClip clip, AudioSource audioSource, float volume)
    {
        if (audioSource.isPlaying)
        {
            Debug.LogWarning(audioSource.name + " AudioSource is currently playing");
            return;
        }

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    //creates the sound in the scene where it can be stored under specific gameobjects
    private AudioSource CreateSound(AudioClip clip, float volume, bool loop, Transform parent = null)
    {
        //instantiate an audioSource as a child of a gameobject
        Transform actualParent = parent == null ? transform : parent; // ✅ safe null check
        AudioSource audioSource = Instantiate(audioSourceObject, Vector3.zero, Quaternion.identity, actualParent);

        if (loop) PlayLoop(clip, audioSource, volume);
        else PlaySound(clip, audioSource, volume);

        return audioSource;
    }
    #endregion
}