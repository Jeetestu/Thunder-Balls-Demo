using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _i;

    public static SoundManager i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("SoundManager")) as GameObject).GetComponent<SoundManager>();
            return _i;
        }
    }

    public enum SOUND_ID
    {
        BALL_CONNECTED,
        BALL_DESTROYED_NEGATIVE,
        BALL_DESTROYED_POSITIVE,
        BAR_MOVE_DOWN,
        BAR_MOVE_UP,
        GAME_OVER_LIGHTNING,
        BALL_LAUNCHED
    }

    [System.Serializable]
    public struct SoundAsset
    {
        public SOUND_ID id;
        public AudioClip clip;
        public float defaultVolume;
        public float defaultPitch ;
    }

    public List<SoundAsset> sounds;
    //converts to dictionary for efficient lookup
    private Dictionary<SOUND_ID, SoundAsset> soundsLookup;

    private static GameObject soundGameObject;

    private void Awake()
    {
        setupDictionary();
    }

    private void setupDictionary()
    {
        soundsLookup = new Dictionary<SOUND_ID, SoundAsset>();
        foreach (SoundAsset s in sounds)
        {
            soundsLookup.Add(s.id, s);
        }
    }

    public void modifyAudioSource(GameObject ob, float time = -1, float volume = -1, float pitch = -100)
    {
        modifyAudioSource(ob.GetComponent<AudioSource>(), time, volume, pitch);
    }

    //modifies part of an audiosource over time
    public void modifyAudioSource (AudioSource source, float time = -1, float volume = -1, float pitch = -100)
    {
        if (volume == -1)
            volume = source.volume;
        if (pitch == -100)
            pitch = source.pitch;

        //if time == -1 then do it instantly;
        if (time == -1)
        {
            source.volume = volume;
            source.pitch = pitch;
        }
        else
        {
            StartCoroutine(modifyAudioSourceRoutine(source, time, volume, pitch));
        }
    }

    IEnumerator modifyAudioSourceRoutine(AudioSource source, float time, float targetVolume, float targetPitch)
    {
        float timer = 0f;
        float progress;
        float startingVolume = source.volume;
        float startingPitch = source.pitch;
        while (timer < time)
        {
            progress = timer / time;
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(startingVolume, targetVolume, progress);
            source.pitch = Mathf.Lerp(startingPitch, targetPitch, progress);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlayAudioSource(GameObject ob)
    {
        PlayAudioSource(ob.GetComponent<AudioSource>());
    }

    //plays an existing audiosource on the given object
    public void PlayAudioSource(AudioSource source)
    {
        if (source.enabled == false)
            source.enabled = true;
        source.Play();
    }

    //looks up the sound asset and plays from a one-shot object
    public void PlaySound(SOUND_ID id, float volume = -1f, float pitch = -1f)
    {
        SoundAsset sound;
        if (!i.soundsLookup.TryGetValue(id, out sound))
            Debug.LogError("Could not find soundasset of id: " + id);
        else
        {
            if (volume == -1f)
                volume = sound.defaultVolume;
            if (pitch == -1f)
                pitch = sound.defaultPitch;
            PlaySound_Direct(sound.clip, volume, pitch);
        }
    }

    //looks up the sound asset and plays from a one-shot object at a specific position
    public void PlaySound(SOUND_ID id, Vector3 pos, float volume = -1f, float pitch = -1f)
    {
        SoundAsset sound;
        if (!i.soundsLookup.TryGetValue(id, out sound))
            Debug.LogError("Could not find soundasset of id: " + id);
        else
        {
            if (volume == -1f)
                volume = sound.defaultVolume;
            if (pitch == -1f)
                pitch = sound.defaultPitch;
            PlaySound_Direct(sound.clip, pos, volume, pitch);
        }
    }

    //NOTE: This leave a component artifact after the sound has been played. Need to fix that to prevent memory leak
    public void PlaySound_Direct(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Sound Manager called with a null value clip");
            return;
        }
        if (soundGameObject == null)
            soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlaySound_Direct(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Sound Manager called with a null value clip");
            return;
        }
        GameObject oneShotSoundObject = new GameObject("Sound");
        oneShotSoundObject.transform.position = position;
        AudioSource audioSource = oneShotSoundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        AudioSource.PlayClipAtPoint(clip, position);
        Destroy(oneShotSoundObject, audioSource.clip.length);
    }
}
