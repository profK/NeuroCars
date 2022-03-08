using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    #region VISUAL COLLISION

    private Collision hitPoint;

    #region COLLISION PARTICLES

    #endregion

    #endregion

    #region AUDIO COLLISION

    private AudioSource crashSound;
    private AudioClip collisionSound;
    private float collisionVolume;

    #endregion

    private AnyCarController ACC;

    void Start()
    {
        ACC = this.transform.GetComponent<AnyCarController>();

        #region AUDIO

        collisionSound = ACC.collisionSound;
        SetUpCollisionAudioSource(collisionSound);
        collisionVolume = ACC.collisionVolume;
        

        #endregion

        if (ACC.collisionSystem)
        {
            #region VISUAL      

            if (ACC.optionalMeshList.Length == 0 || !ACC.customMesh)
            {
                ACC.bodyMesh.AddComponent<MeshCollisionScript>();

                ACC.bodyMesh.GetComponent<MeshCollisionScript>().maxCollisionStrength /= ACC.demolutionStrenght;
                ACC.bodyMesh.GetComponent<MeshCollisionScript>().demolutionRange = ACC.demolutionRange;

                ACC.bodyMesh.GetComponent<MeshCollisionScript>().meshFilter = ACC.bodyMesh.GetComponent<MeshFilter>();

                ACC.bodyMesh.GetComponent<MeshCollisionScript>().collisionParticlesON = ACC.collisionParticles;
            }
            else
            {
                if (ACC.customMesh)
                {
                    foreach (var optional in ACC.optionalMeshList)
                    {
                        optional.modelMesh.gameObject.AddComponent<MeshCollisionScript>();

                        optional.modelMesh.gameObject.GetComponent<MeshCollisionScript>().maxCollisionStrength /= ACC.demolutionStrenght;
                        optional.modelMesh.gameObject.GetComponent<MeshCollisionScript>().demolutionRange = ACC.demolutionRange;

                        optional.modelMesh.gameObject.GetComponent<MeshCollisionScript>().meshFilter = optional.modelMesh.gameObject.GetComponent<MeshFilter>();

                        optional.modelMesh.gameObject.GetComponent<MeshCollisionScript>().collisionParticlesON = ACC.collisionParticles;

                        optional.modelMesh.gameObject.GetComponent<MeshCollisionScript>().loseAftCollisions = optional.loseAftCollisions;

                        if (optional.modelMesh.gameObject.GetComponent<MeshCollider>() == null)
                        {
                            optional.modelMesh.gameObject.AddComponent(typeof(MeshCollider));
                            optional.modelMesh.gameObject.GetComponent<MeshCollider>().convex = true;
                            optional.modelMesh.gameObject.GetComponent<MeshCollider>().isTrigger = true;
                        }
                    }
                }
            }

            #endregion
        }
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        hitPoint = collision;

        if (ACC.collisionSystem)
        {
            if (ACC.optionalMeshList.Length == 0 || !ACC.customMesh)
            {
                ACC.bodyMesh.GetComponent<MeshCollisionScript>().hitPoint = hitPoint;
                ACC.bodyMesh.GetComponent<MeshCollisionScript>().collisionHappened = true;
            }
            else
            {
                if (ACC.customMesh)
                {
                    foreach (var optional in ACC.optionalMeshList)
                    {
                        optional.modelMesh.gameObject.GetComponent<MeshCollisionScript>().hitPoint = hitPoint;
                    }
                }
            }
        }

        #region AUDIO

        crashSound.volume = hitPoint.relativeVelocity.magnitude / 100 * collisionVolume;
        crashSound.Play();

        #endregion
    }

    private AudioSource SetUpCollisionAudioSource(AudioClip clip)
    {
        crashSound = this.transform.gameObject.AddComponent<AudioSource>();

        crashSound.clip = clip;
        crashSound.loop = false;
        crashSound.pitch = 1f;

        crashSound.playOnAwake = false;
        crashSound.minDistance = 5;
        crashSound.reverbZoneMix = 1.5f;
        crashSound.maxDistance = 600;
        crashSound.dopplerLevel = 2;
        return crashSound;
    }
}
