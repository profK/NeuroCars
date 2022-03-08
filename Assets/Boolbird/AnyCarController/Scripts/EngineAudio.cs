using UnityEngine;

public class EngineAudio : MonoBehaviour
{
    #region REFERENCES

    private AudioClip lowAccelClip;
    private AudioClip lowDecelClip;
    private AudioClip highAccelClip;
    private AudioClip highDecelClip;
    private AudioClip nosAudioClip;
    private AudioClip turboAudioClip;

    private AudioSource lowAccelSource;
    private AudioSource lowDecelSource;
    private AudioSource highAccelSource;
    private AudioSource highDecelSource;
    private AudioSource nosAudioSource;
    private AudioSource turboAudioSource;

    private AnyCarController ACC;

    #endregion

    #region UTILITY

    private float engineVolume;
    private float nosVolume;
    private float turboVolume;

    private float lowPitchMin = 1f;
    private float lowPitchMax = 6f;
    private float pitchMultiplier = 1f;
    private float highPitchMultiplier = 0.25f;

    #endregion

    void Start()
    {
        ACC = this.transform.GetComponent<AnyCarController>();

        #region MAIN ENGINE

        #region VOLUME

        engineVolume = ACC.engineVolume;

        #endregion

        #region CLIPS

        lowAccelClip = ACC.lowAcceleration;
        lowDecelClip = ACC.lowDeceleration;
        highAccelClip = ACC.highAcceleration;
        highDecelClip = ACC.highDeceleration;

        #endregion

        #region SOURCES

        highAccelSource = SetUpEngineAudioSource(highAccelClip);
        lowAccelSource = SetUpEngineAudioSource(lowAccelClip);
        lowDecelSource = SetUpEngineAudioSource(lowDecelClip);
        highDecelSource = SetUpEngineAudioSource(highDecelClip);

        #endregion

        #endregion

        #region NOS

        if (ACC.nosON)
        {
            nosVolume = ACC.nosVolume;
            nosAudioClip = ACC.nosAudioClip;
            nosAudioSource = SetUpEngineAudioSource(nosAudioClip);
            nosAudioSource.loop = false;
            nosAudioSource.volume = nosVolume;
            nosAudioSource.playOnAwake = false;
        }

        #endregion

        #region TURBO

        if (ACC.turboON)
        {
            turboVolume = ACC.turboVolume;
            turboAudioClip = ACC.turboAudioClip;
            turboAudioSource = SetUpEngineAudioSource(turboAudioClip);
            turboAudioSource.volume = turboVolume;
        }

        #endregion
    }


    void Update()
    {
        PlayEngineSound();

        NOSSoundEffect();
    }

    private void PlayEngineSound()
    {
        float pitch = SoundLerp(lowPitchMin, lowPitchMax, ACC.RPM);

        pitch = Mathf.Min(lowPitchMax, pitch);


        lowAccelSource.pitch = pitch * pitchMultiplier;
        lowDecelSource.pitch = pitch * pitchMultiplier;
        highAccelSource.pitch = pitch * highPitchMultiplier * pitchMultiplier;
        highDecelSource.pitch = pitch * highPitchMultiplier * pitchMultiplier;

        float accFade = Mathf.Abs(ACC.AccelInput);
        float decFade = 1 - accFade;


        float highFade = Mathf.InverseLerp(0.2f, 0.8f, ACC.RPM);
        float lowFade = 1 - highFade;


        highFade = 1 - ((1 - highFade) * (1 - highFade));
        lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
        accFade = 1 - ((1 - accFade) * (1 - accFade));
        decFade = 1 - ((1 - decFade) * (1 - decFade));


        lowAccelSource.volume = lowFade * accFade * engineVolume;
        lowDecelSource.volume = lowFade * decFade * engineVolume;
        highAccelSource.volume = highFade * accFade * engineVolume;
        highDecelSource.volume = highFade * decFade * engineVolume;

        if (ACC.turboON)
        {
            turboAudioSource.pitch = pitch * highPitchMultiplier * pitchMultiplier;
            turboAudioSource.volume = highFade * accFade * turboVolume + 0.1f;
        }
    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.dopplerLevel = 0;
        return source;
    }

    private static float SoundLerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    #region NOS

    private void NOSSoundEffect()
    {
        if (nosAudioSource != null)
        {
            if (ACC.nosActive)
            {
                nosAudioSource.PlayOneShot(nosAudioClip, 0.7F);
            }
            else
            {
                nosAudioSource.Stop();
            }
        }
    }

    #endregion
}
