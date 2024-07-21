using UnityEngine;

public class FireflyParticleSystem : MonoBehaviour
{
    public ParticleSystem fireflyParticleSystem;

    void Start()
    {

        fireflyParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = fireflyParticleSystem.main;

        main.duration = 10f;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(3f, 5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 0.5f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.5f, 1f);
        main.startColor = Color.yellow;

        var emission = fireflyParticleSystem.emission;
        emission.rateOverTime = 500f;

        // var shape = fireflyParticleSystem.shape;
        // shape.shapeType = ParticleSystemShapeType.Sphere;
        // shape.radius = 5f;

        var sizeOverLifetime = fireflyParticleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0.0f, 0.0f);
        sizeCurve.AddKey(0.5f, 1.0f);
        sizeCurve.AddKey(1.0f, 0.0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, sizeCurve);

        var colorOverLifetime = fireflyParticleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.white, 0.5f), new GradientColorKey(Color.yellow, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        var noise = fireflyParticleSystem.noise;
        noise.enabled = true;
        noise.strength = 0.4f;

        // var renderer = fireflyParticleSystem.GetComponent<ParticleSystemRenderer>();
        // renderer.renderMode = ParticleSystemRenderMode.Billboard;
        // renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        // renderer.material.mainTexture = Resources.Load<Texture2D>("Textures/Firefly");

        fireflyParticleSystem.Play();

    }
}
