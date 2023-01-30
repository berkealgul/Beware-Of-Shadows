using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarknessManager : MonoBehaviour
{
    [Header("Time Wawes")]
    // seconds
    [Min(1)]
    public float timeBetweenPlayerAttacks = 60;
    [Min(1)]
    public float timeBetweenGenAttacks = 5;
    [Min(10)]
    public float timeBetweenRoomSwitch = 50;

    float timeToPlayerAttack;
    float timeToGenAttack;
    float timeToRoomSwitch;

    [Header("Room Switch")]
    [Min(0.1f)]
    public float particleRotDuration = 5;
    [Min(1)]
    public float particleMinRotSpeed = 30; // deg / sec
    [Min(10)]
    public float particleMaxRotSpeed = 50; // deg / sec
    [Min(0.1f)]
    public float particleRotationRadius = 5;
    [Min(4)]
    public int rotatingParticleCount = 4;
    [Range(0.02f, 1)]
    public float particleAddDelay = 0.2f;
    [Range(0, 1)]
    public float zoomPercentage = 0.5f;
    [Min(0.02f)]
    public float zoomDuration = 2f;
    [Min(0.02f)]
    public float blackoutDuration = 2f;

    public AnimationCurve cameraZoomCurve;
    public AnimationCurve blackoutCurve;
    public Camera camera;
    public Image blackout;
    public GameObject rotatingParticlePrefab;

    [Header("Darkness Spawn")]
    [Min(1)]
    public float spawnRadius = 5;
    [Min(0)]
    public int maxSpawn = 3;
    [Range(1, 360)]
    public float spawnArcDegrees = 60;

    public Vector3 mapCenter;
    public GameObject darknessPrefab;

    // private field
    List<GameObject> rotatingParticles;
    List<GameObject> spawnedDarkness;
    GameObject[] generators;
    AudioSource[] sfxs;

    public Player p;

    float difficulty = 1;
    float maxEnergy;

    bool active = false;
    bool RSAnimating = false;
    bool PAAnimating = false;

    bool specialAttack = false;
 
    void Start() 
    {
        sfxs = GetComponents<AudioSource>();
        Init();
    }

    void Update()
    {
        if(!active) { return; }

        timeToGenAttack -= Time.deltaTime;

        if(!PAAnimating)
        {
            timeToPlayerAttack -= Time.deltaTime;
        }

        if(!RSAnimating)
        {
            timeToRoomSwitch -= Time.deltaTime;
        }    

        // switch room
        if(timeToRoomSwitch <= 0)
        {
            StartCoroutine(RoomSwitch());
            timeToRoomSwitch = timeBetweenRoomSwitch;

        }

        // attack to generators
        if(timeToGenAttack <= 0)
        {
            int i = Random.Range(0, generators.Length);
            AttackToGenerator(generators[i]);
            timeToGenAttack = timeBetweenGenAttacks;
        }

        // attack to player
        if (timeToPlayerAttack <= 0)
        {
            //StartCoroutine(AttackPlayer());
            timeToPlayerAttack = timeBetweenPlayerAttacks;
        }
    }

    void Init()
    {
        generators = GameObject.FindGameObjectsWithTag("Generator");
        rotatingParticles = new List<GameObject>();
        spawnedDarkness = new List<GameObject>();
        timeToGenAttack = timeBetweenGenAttacks;
        timeToPlayerAttack = timeBetweenPlayerAttacks;
        timeToRoomSwitch = timeBetweenRoomSwitch;
        maxEnergy = CalculateTotalEnergy();
        Activate();
    }

    void AttackToGenerator(GameObject gen)
    {
        int spawns = Random.Range(1, maxSpawn + 1);
        Vector2 spawnDir = (gen.transform.position - mapCenter).normalized;  

        for(int i = 0; i < spawns * difficulty; i++)
        {
            float angle = Random.Range(-spawnArcDegrees / 2, spawnArcDegrees / 2);

            Vector2 spawnPos = RotateVector(spawnDir, angle) * spawnRadius;
            Quaternion q = new Quaternion();

            GameObject instance = Instantiate(darknessPrefab, spawnPos, q);
            instance.GetComponent<Darkness>().SetTarget(gen);

            spawnedDarkness.Add(instance);
        }
    }

    IEnumerator RoomSwitch()
    {
        RSAnimating = true;
        sfxs[2].Play();
        yield return StartCoroutine(AninamateParticleRotation());
        yield return StartCoroutine(AnimatePreSwitch());

        // cleanup 
        foreach (GameObject d in spawnedDarkness)
        {
            Destroy(d);
        }
        foreach (GameObject rotP in rotatingParticles)
        {
            Destroy(rotP);
        }

        if(!p.Hidden()) 
        {
            Debug.Log("Difficulty Increased");
            difficulty *= 4;     
        }

        // switch room
        GetComponent<SwitchManager>().SwitchObjects();

        yield return new WaitForSeconds(0.01f);

        yield return StartCoroutine(AnimateAfterSwitch()); // back to normal

        RSAnimating = false;
        sfxs[2].Stop();
    }

    IEnumerator AnimatePreSwitch()
    {
        float initalCameraFOV = camera.fieldOfView;
        float time = 0;
        while(time < zoomDuration)
        {
            camera.fieldOfView = Mathf.Lerp(initalCameraFOV, initalCameraFOV * (1 + zoomPercentage), cameraZoomCurve.Evaluate(time / zoomDuration));
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        time = 0;
        while (time < blackoutDuration)
        {
            float a = Mathf.Lerp(0, 255, blackoutCurve.Evaluate(time / blackoutDuration));
            blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, a);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerator AnimateAfterSwitch()
    {
        float initalCameraFOV = camera.fieldOfView;
        float time = 0;

        while (time < blackoutDuration / 2) // shorter revert black out
        {
            float a = Mathf.Lerp(150, 0, blackoutCurve.Evaluate(time / blackoutDuration));
            blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, a);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, 0);

        yield return new WaitForSeconds(0.3f);

        time = 0; 

        while (time < zoomDuration)
        {
            camera.fieldOfView = Mathf.Lerp(initalCameraFOV, initalCameraFOV / (1 + zoomPercentage), cameraZoomCurve.Evaluate(time / zoomDuration));
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerator AninamateParticleRotation()
    {
        for(int i = 0; i < rotatingParticleCount; i++)
        {
            rotatingParticles.Add(InstanciateRotatingParticle());
            yield return new WaitForSeconds(particleAddDelay);
        }

        yield return new WaitForSeconds(particleRotDuration);
    }

    GameObject InstanciateRotatingParticle()
    {
        var particle = Instantiate(rotatingParticlePrefab);
        particle.GetComponent<RotatingDarkness>().SetCenter(mapCenter);
        particle.GetComponent<RotatingDarkness>().SetPolar(Random.Range(0, 2*Mathf.PI), particleRotationRadius);
        particle.GetComponent<RotatingDarkness>().SetSpeed(Random.Range(particleMinRotSpeed, particleMaxRotSpeed));
        return particle;
    }

    Vector2 RotateVector(Vector2 v, float angle)
    {
        float _x = v.x;
        float _y = v.y;

        float _angle = angle * Mathf.Deg2Rad;
        float _cos = Mathf.Cos(_angle);
        float _sin = Mathf.Sin(_angle);

        float _x2 = _x * _cos - _y * _sin;
        float _y2 = _x * _sin + _y * _cos;

        return new Vector2(_x2, _y2); 
    }

    float CalculateTotalEnergy()
    {
        float totalEnergy = 0;

        foreach(var gen in generators)
        {
            totalEnergy += gen.GetComponent<Generator>().GetEnergy();
        }

        return totalEnergy;
    }

    public void IncreaseDifficulty()
    {
        difficulty *= 2;
    }
    public float CalculateDoomPercentage()
    {
        return CalculateTotalEnergy() / maxEnergy;
    }

    public bool isAttackingPlayer() { return specialAttack; }
    public void Activate() { active = true; }
    public void Deactivate() { active = false; }
}
