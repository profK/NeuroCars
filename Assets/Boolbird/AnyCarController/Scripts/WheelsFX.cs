using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


public class WheelsFX : MonoBehaviour
{
    #region AUDIO REFERENCES

    private AudioClip skidAudio;
    private AudioSource skidAudioSource;
    private float skidAudioVolume;

    #endregion

    #region VISUAL SKID REFERENCES

    private ParticleSystem skidParticles;
    private static Transform skidTrailsDetachedParent;
    private Transform skidTrail;
    private Transform skidTrailPrefab;
    private WheelCollider wheelCollider;
    private AnyCarController ACC;

    #endregion

    #region BOOLS

    public bool skidding { get; private set; }
    public bool playingAudio { get; private set; }

    #endregion

    private void Start()
    {
        #region VISUAL

        skidParticles = transform.parent.parent.GetComponentInChildren<ParticleSystem>();
        skidTrailPrefab = Resources.Load<Transform>("SkidTrail");
        ACC = transform.parent.parent.GetComponent<AnyCarController>();

        if (skidParticles == null)
        {
            Debug.LogWarning(" no particle system found on car to generate smoke particles", gameObject);
        }
        else
        {
            if (!ACC.smokeOn)
            {
                skidParticles.Stop();
            }            
        }

        wheelCollider = this.transform.GetComponent<WheelCollider>();

        if (skidTrailsDetachedParent == null)
        {
            skidTrailsDetachedParent = new GameObject("TemporarySkids").transform;                
        }

        #endregion

        #region AUDIO

        Transform parent = this.transform.parent;
        skidAudio = parent.transform.parent.GetComponent<AnyCarController>().skidSound;
        skidAudioVolume = parent.transform.parent.GetComponent<AnyCarController>().skidVolume;
        SetUpSkidAudioSource(skidAudio);        
        playingAudio = false;


        #endregion
    }

    #region VISUAL FUNCTIONS
    public void EmitTyreSmoke()
    {
        skidParticles.transform.position = transform.position - transform.up * wheelCollider.radius;
        skidParticles.Emit(1);
        if (!skidding)
        {
            StartCoroutine(StartSkidTrail());
        }
    }

    public IEnumerator StartSkidTrail()
    {

        skidTrail = Instantiate(skidTrailPrefab);
        skidding = true;

        while (skidTrail == null)
        {
            yield return null;
        }
        
        skidTrail.parent = this.gameObject.transform;
        skidTrail.localPosition = -Vector3.up * wheelCollider.radius;
    }

    public void EndSkidTrail()
    {
        if (!skidding)
        {
            return;
        }

        skidding = false;
        skidTrail.parent = skidTrailsDetachedParent;
        Destroy(skidTrail.gameObject, 15);
    }

    #endregion

    #region AUDIO FUNCTIONS

    public void PlayAudio()
    {
        skidAudioSource.Play();
        playingAudio = true;
    }

    public void StopAudio()
    {
        skidAudioSource.Stop();
        playingAudio = false;
    }

    private AudioSource SetUpSkidAudioSource(AudioClip clip)
    {
        if (this.transform.parent.GetComponent<AudioSource>() == null)
        {
            skidAudioSource = this.transform.parent.gameObject.AddComponent<AudioSource>();
        }
        else
        {
            skidAudioSource = this.transform.parent.GetComponent<AudioSource>();
        }


        skidAudioSource.clip = clip;
        skidAudioSource.volume = skidAudioVolume;
        skidAudioSource.loop = false;
        skidAudioSource.pitch = 1f;

        skidAudioSource.playOnAwake = false;
        skidAudioSource.minDistance = 5;
        skidAudioSource.reverbZoneMix = 1.5f;
        skidAudioSource.maxDistance = 600;
        skidAudioSource.dopplerLevel = 2;
        return skidAudioSource;
    }


    #endregion
}
