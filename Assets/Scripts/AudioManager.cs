using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] MusicFiles;
    public AudioSource[] SoundEffects;


    public static AudioManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public enum MusicId
    {
        MainMusic
    }

    public enum AudioId
    {
        StepSlow,
        StepFast,
        Beep,
    }

    public void PlaySFX(AudioId audioId, bool pitch = true)
    {
        if (!GamePreferences.Sound) return;
        PlaySFX((int)audioId, pitch);
    }
    public void StopSFX(AudioId audioId)
    {
        if (!GamePreferences.Sound) return;
        StopSFX((int)audioId);
    }
    public void PlaySFX(int soundToPlay, bool pitch = true)
    {
        // if (!GamePreferences.Sound) return;
        // SoundEffects[soundToPlay].Stop();

        // if (pitch)
        // {
        //     SoundEffects[soundToPlay].pitch = Random.Range(0.95f, 1.05f);
        // }

        // SoundEffects[soundToPlay].Play();

        if (GamePreferences.Sound)
        {

            if (!SoundEffects[soundToPlay].isPlaying)
            {
                SoundEffects[soundToPlay].Play();
            }
        }
    }

    public void StopSFX(int soundToPlay)
    {
        if (!GamePreferences.Sound) return;
        SoundEffects[soundToPlay].Stop();
    }

    public void PlayMainMusic()
    {
        if (!GamePreferences.Music) return;
        MusicFiles[((int)MusicId.MainMusic)].Play();
    }

    public bool IsPlaying(AudioId audioId)
    {
        return SoundEffects[(int)audioId].isPlaying;
    }
}
