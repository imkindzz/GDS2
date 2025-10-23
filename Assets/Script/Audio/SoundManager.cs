using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;

public enum SfxSoundName
{
    //goblin
    GoblinBossExertion, GoblinCannon, GoblinClubThrow, GoblinClubThrowImpact,
    GoblinGrowl1, GoblinGrowl2, GoblinRatty1, GoblinRatty2, GoblinRatty3,
    GoblinSpearThrow,

    //villager
    VillagerBossRaking, VillagerBossStomp, VillagerDynamiteLessDramatic, 
    VillagerDynamiteMoreDramatic, VillagerGroundStomp, VillagerHaybaleImpact,

    //castle
    KnightCling, LanceCling, LanceChargeAttack, SwordSwing,

    //player
    GhostAttack, GhostMovement, PlayerHit, PlayerWarp, SoulRetraction,
    DashCharge, GainHP, LowHP,

    //other
    AmbienceJungle, AmbienceVillage, DeathSound
}

public enum MusicName
{
    Menu, Death,

    //goblin
    GoblinNormal, GoblinBoss,

    //village
    VillageNormal, VillageBoss,

    //castle
    CastleNormal, CastleBoss,
}

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager instance { get; private set; }

    [SerializeField] private MusicName startMusicName;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> sfxSoundClips = new List<AudioClip>(); //stores the sounds, where the index align with the variables in the SfxSoundName enum
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>(); //stores the music, where the index align with the variables in the MusicName enum
    [SerializeField] private AudioSource audioSourceObject;

    [Header("Volume Settings")]
    [SerializeField, Range(0f, 1f)] private float baseSfxVolume = 1.0f;
    [SerializeField, Range(0f, 1f)] private float increaseSfxVolumePercent = 0f;
    [SerializeField, Range(0f, 1f)] private float decreaseSfxVolumePercent = 0f;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 1.0f;

    private AudioSource musicPlayer;
    private bool isPlayingDeathMusic = false;

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
        else
        {
            musicPlayer = Instantiate(audioSourceObject, transform);
            musicPlayer.gameObject.name = "Music Player AudioSource";
        }
    }

    private void Start()
    {
        //plays the music of the scene it is at
        PlayMusic(startMusicName);
    }

    #region Public player
    //plays a sound
    public AudioSource PlaySound(SfxSoundName soundName, Transform parent = null, bool loop = false)
    {
        float actualVolume = AdjustVolumeBasedOnSound(soundName);

        return CreateSound(sfxSoundClips[(int)soundName], actualVolume, loop, parent);
    }

    //plays a random sound
    public AudioSource PlayRandomSound(SfxSoundName[] soundNames, Transform parent = null, bool loop = false)
    {
        int randIndex = Random.Range(0, soundNames.Length);
        SfxSoundName chosenSound = soundNames[randIndex];
        return PlaySound(chosenSound, parent, loop);
    }

    //plays a music loop, where there can only be one music playing in the scene
    public void PlayMusic(MusicName musicName, float volume = -1)
    {
        AudioClip clip = musicClips[(int)musicName];
        if (clip.Equals(musicPlayer.clip))
        {
            Debug.LogWarning("The music clip selected is already playing");
            return;
        }
        else musicPlayer.Stop();

        float actualVolume = volume >= 0 && volume <= 1 ? volume : musicVolume;

        PlayLoop(clip, musicPlayer, actualVolume);

        if (musicName.Equals(MusicName.Death)) isPlayingDeathMusic = true;
        else isPlayingDeathMusic = false;
    }

    //stops a sound loop
    public void StopSoundLoop(AudioSource audioSource)
    {
        audioSource.Stop();
        Destroy(audioSource.gameObject); //destroys the gameobject when it is no longer used
    }

    public void PreloadMusic(MusicName musicName)
    {
        var clip = musicClips[(int)musicName];
        if (clip && clip.loadState != AudioDataLoadState.Loaded)
            clip.LoadAudioData(); // async when possible
    }

    private IEnumerator WaitUntilLoaded(AudioClip clip)
    {
        while (clip && clip.loadState == AudioDataLoadState.Loading)
            yield return null;
    }

    public IEnumerator PlayMusicPreloaded(MusicName musicName, float volume = -1f)
    {
        var clip = musicClips[(int)musicName];
        if (!clip) yield break;

        // Warm up first (prevents the hitch)
        PreloadMusic(musicName);
        yield return StartCoroutine(WaitUntilLoaded(clip));

        float actualVol = (volume >= 0f && volume <= 1f) ? volume : musicVolume;

        if (musicPlayer.isPlaying) musicPlayer.Stop();
        musicPlayer.clip = clip;
        musicPlayer.volume = actualVol;
        musicPlayer.Play();
    }
    #endregion

    #region Sound player
    //plays the sound
    private void PlaySound(AudioClip clip, AudioSource audioSource, float volume)
    {
        if (!clip || !audioSource)
        {
            Debug.LogWarning("The AudioClip or AudioSource in the PlaySound function from the SoundManager script is null");
            return;
        }

        audioSource.PlayOneShot(clip, volume);
        Destroy(audioSource.gameObject, clip.length); //destroys the gameobject after the clip is finished

    }

    //plays a sound loop
    private void PlayLoop(AudioClip clip, AudioSource audioSource, float volume)
    {
        if (!clip || !audioSource)
        {
            Debug.LogWarning("The AudioClip or AudioSource in the PlayLoop function from the SoundManager script is null");
            return;
        }

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

    //adjusts the volume of the clips that are extra loud
    private float AdjustVolumeBasedOnSound(SfxSoundName sfxSoundName)
    {
        float volume = baseSfxVolume;

        //decrease volume
        if (sfxSoundName.Equals(SfxSoundName.GoblinBossExertion) ||
            sfxSoundName.Equals(SfxSoundName.GoblinCannon) ||
            sfxSoundName.Equals(SfxSoundName.VillagerDynamiteLessDramatic) ||
            sfxSoundName.Equals(SfxSoundName.VillagerDynamiteMoreDramatic) ||
            //sfxSoundName.Equals(SfxSoundName.VillagerGroundStomp) ||
            //sfxSoundName.Equals(SfxSoundName.VillagerHaybaleImpact) ||
            sfxSoundName.Equals(SfxSoundName.LanceChargeAttack) ||
            sfxSoundName.Equals(SfxSoundName.SwordSwing) ||
            sfxSoundName.Equals(SfxSoundName.GainHP)
            )
                return volume - (volume * decreaseSfxVolumePercent);

        //increase volume
        if (sfxSoundName.Equals(SfxSoundName.GoblinClubThrow) ||
            sfxSoundName.Equals(SfxSoundName.GoblinRatty3) ||
            //sfxSoundName.Equals(SfxSoundName.VillagerBossStomp) ||
            //sfxSoundName.Equals(SfxSoundName.VillagerGroundStomp) ||
            sfxSoundName.Equals(SfxSoundName.GhostAttack) ||
            //sfxSoundName.Equals(SfxSoundName.GhostMovement) ||
            sfxSoundName.Equals(SfxSoundName.PlayerHit) ||
            //sfxSoundName.Equals(SfxSoundName.PlayerWarp) ||
            //sfxSoundName.Equals(SfxSoundName.SoulRetraction) ||
            sfxSoundName.Equals(SfxSoundName.LowHP)
            )
                return volume + (volume * increaseSfxVolumePercent);

        return volume;
    }

    //creates the sound in the scene where it can be stored under specific gameobjects
    private AudioSource CreateSound(AudioClip clip, float volume, bool loop, Transform parent = null)
    {
        if (isPlayingDeathMusic) return null;

        //instantiate an audioSource as a child of a gameobject
        Transform actualParent = parent == null ? transform : parent; 
        AudioSource audioSource = Instantiate(audioSourceObject, Vector3.zero, Quaternion.identity, actualParent);

        //names the audioSource
        audioSource.name = clip.name + " AudioSource";

        //plays the sound
        if (loop) PlayLoop(clip, audioSource, volume);
        else PlaySound(clip, audioSource, volume);

        return audioSource;
    }
    #endregion
}