using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager Instance { get; private set; }

    [Header("Player Sounds")]
    [SerializeField] private AudioClip ghostAttack;
    [SerializeField] private AudioClip ghostMovement;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerWarp;

    [Header("Goblin Sounds")]
    [SerializeField] private AudioClip goblinBossExertion;
    [SerializeField] private AudioClip goblinCannon;
    [SerializeField] private AudioClip goblinClubThrowImpact;
    [SerializeField] private AudioClip goblinClubThrow;
    [SerializeField] private AudioClip goblinGrowl1;
    [SerializeField] private AudioClip goblinGrowl2;
    [SerializeField] private AudioClip goblinRatty1;
    [SerializeField] private AudioClip goblinRatty2;
    [SerializeField] private AudioClip goblinRatty3;
    [SerializeField] private AudioClip goblinSpearThrow;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.6f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1.0f;

    [Header("Audio Source Pool")]
    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private int maxPoolSize = 15;

    // Audio sources
    private AudioSource musicSource;
    private AudioSource loopSource;
    
    // Pooling system
    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();
    private List<AudioSource> activeAudioSources = new List<AudioSource>();
    // Keyed loop channels so multiple different loops can run at once
    private readonly Dictionary<string, AudioSource> loopChannels = new Dictionary<string, AudioSource>();

    // State tracking
    private bool isWalking = false;
    private bool isSplattering = false;
    private GameObject player;
    
    
    private Coroutine bossMusicRoutine;


    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSoundSystem();
        }
        else
        {
            Debug.LogWarning("Multiple SoundManager instances found! Destroying the duplicate.");
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Get player reference
        player = GameObject.FindWithTag("Player");
        
        // Play background music
/*        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic, musicVolume);
        }
*/    }
    
    private void Update()
    {
        // Cleanup finished audio sources
        CleanupFinishedAudioSources();
    }
    
    private void InitializeSoundSystem()
    {
        // Create music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.ignoreListenerVolume = true;
        
        // Create loop source for walking etc.
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true;
        loopSource.playOnAwake = false;
        
        // Initialize audio source pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreatePooledAudioSource();
        }
    }
    
    private AudioSource CreatePooledAudioSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.loop = false;
        audioSourcePool.Enqueue(newSource);
        return newSource;
    }
    
    private AudioSource GetAudioSource()
    {
        if (audioSourcePool.Count == 0 && activeAudioSources.Count < maxPoolSize)
        {
            return CreatePooledAudioSource();
        }
        
        if (audioSourcePool.Count > 0)
        {
            AudioSource source = audioSourcePool.Dequeue();
            activeAudioSources.Add(source);
            return source;
        }
        
        // If pool is empty and at max capacity, reuse the oldest active source
        AudioSource oldestSource = activeAudioSources[0];
        activeAudioSources.RemoveAt(0);
        activeAudioSources.Add(oldestSource);
        oldestSource.Stop();
        return oldestSource;
    }
    
    private void CleanupFinishedAudioSources()
    {
        for (int i = activeAudioSources.Count - 1; i >= 0; i--)
        {
            if (!activeAudioSources[i].isPlaying)
            {
                audioSourcePool.Enqueue(activeAudioSources[i]);
                activeAudioSources.RemoveAt(i);
            }
        }
    }
    
    #region Public Sound Methods
    
    public void PlayCustomButtonSound(AudioClip clip)
    {
        if (clip != null)
        {
            PlaySound(clip, sfxVolume);
        }
    }

    public void PlayGoblinBossExertion()
    {
        PlaySound(goblinBossExertion, sfxVolume);
    }

    public void PlayGoblinCannon()
    {
        PlaySound(goblinCannon, sfxVolume);
    }

    public void PlayGoblinClubThrowImpact()
    {
        PlaySound(goblinClubThrowImpact, sfxVolume);
    }

    public void PlayGoblinClubThrow()
    {
        PlaySound(goblinClubThrow, sfxVolume);
    }

    public void PlayGoblinGrowl1()
    {
        PlaySound(goblinGrowl1, sfxVolume);
    }

    public void PlayGoblinGrowl2()
    {
        PlaySound(goblinGrowl2, sfxVolume);
    }

    public void PlayGoblinRatty1()
    {
        PlaySound(goblinRatty1, sfxVolume);
    }

    public void PlayGoblinRatty2()
    {
        PlaySound(goblinRatty2, sfxVolume);
    }

    public void PlayGoblinRatty3()
    {
        PlaySound(goblinRatty3, sfxVolume);
    }

    public void PlayGoblinSpearThrow()
    {
        PlaySound(goblinSpearThrow, sfxVolume);
    }

    public void PlayRandomGoblinNoise()
    {
        switch (UnityEngine.Random.Range(0, 5))
        {
            case 0:
                PlayGoblinGrowl1();
                break;
            case 1:
                PlayGoblinGrowl2();
                break;
            case 2:
                PlayGoblinRatty1();
                break;
            case 3:
                PlayGoblinRatty2();
                break;
            case 4:
                PlayGoblinRatty3();
                break;
            default:
                break;
        }
    }

    public void PlayLoopGhostAttack()
    {
        PlayLoop("ghostAttack", ghostAttack, sfxVolume);
    }

    public void PlayLoopGhostMovement()
    {
        PlayLoop("ghostMovement", ghostMovement, sfxVolume);
    }

    public void StopLoopGhostAttack()
    {
        StopLoop("ghostAttack");
    }

    public void StopLoopGhostMovement()
    {
        StopLoop("ghostMovement");
    }

    public void PlayPlayerHit()
    {
        PlaySound(playerHit, sfxVolume);
    }

    public void PlayPlayerWarp()
    {
        PlaySound(playerWarp, sfxVolume);
    }
    #endregion

    #region Private Helper Methods

    private void PlaySound(AudioClip clip, float volume)
    {
        Debug.Log("Playing Sound");
        return;
        if (clip == null)
            return;
            
        AudioSource source = GetAudioSource();
        source.clip = clip;
        source.volume = volume * masterVolume;
        source.spatialBlend = 0f; // 2D sound
        source.Play();
    }
    
    private void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume)
    {
        if (clip == null)
            return;
            
        AudioSource source = GetAudioSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume * masterVolume;
        source.spatialBlend = 0.5f; // Mix of 2D and 3D sound
        source.Play();
    }
    
    private Vector3 GetPlayerPosition()
    {
        return player != null ? player.transform.position : Vector3.zero;
    }
    
    private void PlayMusic(AudioClip music, float volume)
    {
        musicSource.clip = music;
        musicSource.volume = volume * masterVolume;
        musicSource.Play();
    }
    
    private IEnumerator FadeOutAndSwitchMusic(AudioClip newMusic, float fadeDuration, float targetVolume)
    {
        float startVolume = musicSource.volume;

        // Fade out current music
        while (musicSource.volume > 0f)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Change clip and reset volume
        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.volume = 0f;
        musicSource.Play();

        // Fade in new music
        while (musicSource.volume < targetVolume * masterVolume)
        {
            musicSource.volume += targetVolume * masterVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = targetVolume * masterVolume;
    }

    private void ReturnSourceToPool(AudioSource src)
    {
        if (src == null) return;
        src.Stop();
        src.loop = false;
        src.clip = null;

        // If it came from the pool, it should be in activeAudioSources
        int idx = activeAudioSources.IndexOf(src);
        if (idx >= 0)
        {
            activeAudioSources.RemoveAt(idx);
            audioSourcePool.Enqueue(src);
        }
    }

    // Plays a looping clip on a named channel (id). Use unique ids per loop type, e.g. "footsteps", "engine".
    private void PlayLoop(string id, AudioClip clip, float volume = -1f, float spatialBlend = 0f, bool restartIfSame = false)
    {
        Debug.Log("Play Loop Sound");
        return;
        if (string.IsNullOrEmpty(id) || clip == null) return;

        // If channel exists
        if (loopChannels.TryGetValue(id, out var src))
        {
            // Already playing the same clip and no restart requested
            if (src.isPlaying && src.clip == clip && !restartIfSame) return;

            // Reuse the same source
            src.clip = clip;
        }
        else
        {
            // Take from pool so we track/clean consistently
            src = GetAudioSource();
            loopChannels[id] = src;
        }

        src.loop = true;
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        float baseVol = (volume >= 0f ? Mathf.Clamp01(volume) : sfxVolume);
        src.volume = baseVol * masterVolume;
        src.Play();
    }

    // Stops the loop playing on the named channel (id).
    public void StopLoop(string id)
    {
        Debug.Log("Stop Loop Sound");
        return;
        if (string.IsNullOrEmpty(id)) return;
        if (!loopChannels.TryGetValue(id, out var src) || src == null || !src.isPlaying)
        {
            // Nothing to stop
            loopChannels.Remove(id);
            return;
        }

        ReturnSourceToPool(src);
        loopChannels.Remove(id);
    }
    #endregion

    #region Volume Control

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (loopSource != null && loopSource.isPlaying)
        {
            loopSource.volume = sfxVolume * masterVolume;
        }
    }
    
    private void UpdateAllVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
        
        if (loopSource != null && loopSource.isPlaying)
        {
            loopSource.volume = sfxVolume * masterVolume;
        }
        
        // Active sound effects will update on their next play
    }
    
/*    private IEnumerator WaitForBossDefeat(System.Func<bool> isBossDefeated)
    {
        PlayBossMusic();

        while (!isBossDefeated())
        {
            yield return null;
        }

        ResumeBackgroundMusic();
    }
*/
    
    #endregion
}