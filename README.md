# Looga FMOD

Looga FMOD is a small gameplay-facing wrapper around FMOD Unity. It is intentionally separate from Looga Sound System so AudioSource-based projects can keep using that package, while FMOD projects can use FMOD events directly.

## Requirements

- FMOD for Unity installed in the consuming project.
- FMOD banks configured and loading correctly.

Steam Audio is not a hard dependency. Add Steam Audio Spatializer to your FMOD Studio events and configure Steam Audio in the Unity project when needed.

## Basic Usage

```csharp
LoogaFmod.PlayOneShot(fireEvent, muzzleTransform);

LoogaFmodHandle loop = LoogaFmod.Start(slideLoopEvent, playerTransform);
loop.SetParameter("Speed", normalizedSpeed);
loop.Stop();
```

## Recommended Flow

- Use `LoogaFmodEvent` assets for gameplay data and reusable default parameters.
- Use FMOD Studio for randomization, layering, mixer routing, attenuation, and Steam Audio spatialization.
- Use Looga FMOD from gameplay scripts to express intent: play, start loop, stop, and set parameters.
