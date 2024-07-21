using UnityEngine;

public class CollectibleInstrument : MonoBehaviour
{
    public float bobbingSpeed = 1f;
    public float bobbingHeight = 0.5f;
    public float rotationSpeed = 50f;
    public ParticleSystem musicNoteParticleSystem;
    public Texture2D[] noteTextures; // Array to hold the music note textures
    public Material particleMaterial; // Custom material for transparency

    private Vector3 originalPosition;
    private ParticleSystemRenderer particleSystemRenderer;
    private bool emitParticles = false;
    private float bobbingTime = 0f;

    void Start()
    {
        originalPosition = transform.position;

        // Setup the particle system if not already attached
        //if (musicNoteParticleSystem == null)
        //{
        //    GameObject particleObject = new GameObject("MusicNotes");
        //    particleObject.transform.SetParent(transform);
        //    particleObject.transform.localPosition = Vector3.zero;

        //    musicNoteParticleSystem = particleObject.AddComponent<ParticleSystem>();
        //    var main = musicNoteParticleSystem.main;
        //    main.startLifetime = .7f;
        //    main.startSpeed = 0.8f;
        //    main.startSize = 0.08f; // Start bigger
        //    main.simulationSpace = ParticleSystemSimulationSpace.World;

        //    var emission = musicNoteParticleSystem.emission;
        //    emission.rateOverTime = 0f; // Turn off continuous emission
        //    emission.rateOverDistance = 0f;

        //    var shape = musicNoteParticleSystem.shape;
        //    shape.shapeType = ParticleSystemShapeType.Donut;
        //    shape.radius = 0.01f;
        //    shape.donutRadius = 0.02f;
        //    shape.radiusThickness = 1f;
        //    shape.arc = 360f;

        //    var sizeOverLifetime = musicNoteParticleSystem.sizeOverLifetime;
        //    sizeOverLifetime.enabled = true;
        //    var size = new AnimationCurve();
        //    size.AddKey(0.0f, 1.0f);
        //    size.AddKey(1.0f, 0.1f);
        //    sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, size);

        //    particleSystemRenderer = musicNoteParticleSystem.GetComponent<ParticleSystemRenderer>();
        //    particleSystemRenderer.material = particleMaterial;
        //}
    }

    void Update()
    {
        BobbingEffect();
        RotationEffect();
        //UpdateParticleTexture();
    }

    void BobbingEffect()
    {
        bobbingTime += Time.deltaTime * bobbingSpeed;
        float newY = Mathf.Sin(bobbingTime) * bobbingHeight + originalPosition.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (Mathf.Sin(bobbingTime) < -0.99f && !emitParticles) // At lowest point
        {
            //EmitParticles();
            emitParticles = true;
        }
        else if (Mathf.Sin(bobbingTime) >= -0.99f)
        {
            emitParticles = false;
        }
    }

    void RotationEffect()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    void UpdateParticleTexture()
    {
        if (noteTextures.Length > 0)
        {
            // Randomly select a texture from the array
            Texture2D selectedTexture = noteTextures[Random.Range(0, noteTextures.Length)];
            particleSystemRenderer.material.mainTexture = selectedTexture;
        }
    }

    void EmitParticles()
    {
        var burst = new ParticleSystem.Burst(0.0f, 10); // Adjust burst count as needed
        var emission = musicNoteParticleSystem.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { burst });
        musicNoteParticleSystem.Emit(10); // Emit a burst of particles
        ApplySineWaveMovement();
    }

    void ApplySineWaveMovement()
    {
        var velocityOverLifetime = musicNoteParticleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(2f, new AnimationCurve(
            new Keyframe(0.0f, 0.0f),
            new Keyframe(0.5f, 1.0f),
            new Keyframe(1.0f, 0.0f)
        ));
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0.5f);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(2f, new AnimationCurve(
            new Keyframe(0.0f, 0.0f),
            new Keyframe(0.5f, 1.0f),
            new Keyframe(1.0f, 0.0f)
        ));
    }
}
