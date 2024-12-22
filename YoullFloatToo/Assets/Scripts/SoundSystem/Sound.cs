using UnityEngine;

[System.Serializable]
public class Sound
{
    public string ClipName;
    public int bpm;
    public bool isBGSound, isTune, isDefaultBG;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
    public bool loop = false;
    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;
    public bool isStereo = true;
    bool isFull = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.panStereo = isStereo ? 0.0f : -1.0f; // Set spatial blend based on stereo toggle
    }

    public AudioSource GetAudioSource()
    {
        return source;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }

    public bool isPlaying()
    {
        return source.isPlaying;
    }

    public void SetVolume(float amount)
    {
        source.volume = amount * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
    }

    public void SetPitch(float amount)
    {
        source.pitch = amount * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
    }

    public void SetStereo(bool stereo)
    {
        isStereo = stereo;
        if (source != null)
        {
            source.panStereo = isStereo ? 0.0f : -1.0f; // Update spatial blend when toggling between mono and stereo
        }
    }

    public void ToggleLoudness(bool full = true)
    {
        isFull = full;
        if (source != null)
        {
            // Adjust volume when toggling loudness
            source.volume = (isFull ? volume : volume * 0.5f) * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        }
    }
}
