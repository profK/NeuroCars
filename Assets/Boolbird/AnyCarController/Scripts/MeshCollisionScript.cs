using System.Collections;
using UnityEngine;

public class MeshCollisionScript : MonoBehaviour
{
    #region VISUAL COLLISION

    [HideInInspector]
    public float maxCollisionStrength = 50f;
    [HideInInspector]
    public float demolutionRange = 100f;
    [HideInInspector]
    public MeshFilter meshFilter;

    private float maxMoveDelta = 1.5f;
    private float yForceDamp = 1f;
    private float impactDirManipulator = 0.5f;    

    private float sqrDemRange;

    [HideInInspector]
    public Collision hitPoint;

    [HideInInspector]
    public bool collisionHappened;

    [HideInInspector]
    public int loseAftCollisions;

    private bool fixedMesh = true;

    #region COLLISION PARTICLES

    [HideInInspector]
    public bool collisionParticlesON;
    private Transform collisionParticles;
    private Transform collisionParticlesPrefab;

    #endregion

    #endregion

    void Start()
    {
        sqrDemRange = demolutionRange * demolutionRange;

        if (loseAftCollisions == 0)
        {
            fixedMesh = true;
        }
        else
        {
            fixedMesh = false;
        }

        #region COLLISION PARTICLES

        collisionParticlesPrefab = Resources.Load<Transform>("CollisionParticles");

        #endregion
    }

    private void FixedUpdate()
    {
        if (collisionHappened)
        {
            CollisionCalculator();
        }
    }

    public void OnTriggerEnter(Collider objCollided)
    {
        if (objCollided)
        {
            loseAftCollisions--;

            if (!fixedMesh)
            {
                if (loseAftCollisions <= 0)
                {
                    this.transform.parent = null;
                    StartCoroutine(LoseObjectCoroutine());
                }
            }

            collisionHappened = true;
        }
    }

    private void CollisionCalculator()
    {
        if (hitPoint != null)
        {
            #region VISUAL

            #region COLLISION PARTICLES

            if (collisionParticlesON == true)
            {
                if (collisionParticles == null)
                {
                    collisionParticles = Instantiate(collisionParticlesPrefab);
                    collisionParticles.GetComponent<ParticleSystem>().Emit(10);
                    collisionParticles.transform.position = hitPoint.contacts[0].point;
                    Destroy(collisionParticles.gameObject, 5);
                }
            }

            #endregion

            Vector3 colRelVel = hitPoint.relativeVelocity;
            colRelVel *= yForceDamp;
            Vector3 colPointToMe = transform.position - hitPoint.contacts[0].point;
            float colStrength = colRelVel.magnitude * Vector3.Dot(hitPoint.contacts[0].normal, colPointToMe.normalized);
            OnMeshForce(hitPoint.contacts[0].point, Mathf.Clamp01(colStrength / maxCollisionStrength));

            #endregion
        }
    }

    public IEnumerator LoseObjectCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        this.transform.gameObject.GetComponent<MeshCollider>().isTrigger = false;
        if (this.transform.gameObject.GetComponent<Rigidbody>() == null)
        {
            this.transform.gameObject.AddComponent<Rigidbody>();
        }        
        this.transform.gameObject.GetComponent<Rigidbody>().mass = 50;
    }

    public void OnMeshForce(Vector3 originPos, float force)
    {
        force = Mathf.Clamp01(force);

        Vector3[] verts = meshFilter.mesh.vertices;

        for (int i = 0; i < verts.Length; ++i)
        {
            Vector3 scaledVert = Vector3.Scale(verts[i], transform.localScale);
            Vector3 vertWorldPos = meshFilter.transform.position + (meshFilter.transform.rotation * scaledVert);
            Vector3 originToMeDir = vertWorldPos - originPos;
            Vector3 flatVertToCenterDir = transform.position - vertWorldPos;
            flatVertToCenterDir.y = 0;

            if (originToMeDir.sqrMagnitude < sqrDemRange)
            {
                float dist = Mathf.Clamp01(originToMeDir.sqrMagnitude / sqrDemRange);
                float moveDelta = force * (1 - dist) * maxMoveDelta;
                Vector3 moveDir = Vector3.Slerp(originToMeDir, flatVertToCenterDir, impactDirManipulator).normalized * moveDelta;
                verts[i] += Quaternion.Inverse(transform.rotation) * moveDir;
            }
        }

        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.RecalculateBounds();

        hitPoint = null;
        collisionHappened = false;
    }
}
