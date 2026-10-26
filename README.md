# Unity Movement Prototype

A university-era **C# movement prototype from 2022**, exploring custom acceleration, drag, directional calculations, camera movement, and wall-movement behavior around Unity's `CharacterController`.

This repository preserves the historical source. The portfolio update adds documentation; it does not rewrite the movement code or present it as a production-ready controller.

## What I worked on

- Custom input axes with acceleration and drag, including different control while airborne.
- Direction and magnitude calculations using trigonometry and quadrant adjustments.
- Rotation compensation intended to preserve movement behavior as the player turns in the air.
- Gravity, jumping, sprinting, and wall-running/jumping experiments.
- Mouse-controlled camera pitch and player yaw.

This was hands-on student programming. The movement logic uses Unity's controller, transforms, raycasts, and other engine facilities; it is not a wholly independent physics engine.

## Source guide

| File | What to inspect |
| --- | --- |
| [PlayerMovement.cs](Assets/Scripts/PlayerMovement.cs) | Input axes, acceleration/drag, gravity, direction limiting, and rotation compensation |
| [MouseLook.cs](Assets/Scripts/MouseLook.cs) | Camera pitch and player yaw |
| [WallRun.cs](Assets/Scripts/WallRun.cs) | Wall-state detection and communication with the movement controller |
| [WallChecks.cs](Assets/Scripts/WallChecks.cs) | Wall-contact bookkeeping |
| [SampleScene.unity](Assets/Scenes/SampleScene.unity) | The saved prototype scene |

## Historical environment and controls

The project records **Unity 2021.3.3f1** in [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt). Open the project with a compatible Unity editor and inspect the saved sample scene. Compatibility with recent Unity releases has not been verified.

The movement code reads WASD for its custom axes, Left Shift for sprinting, Unity's `Jump` action for jumping, and mouse input for camera control.

## Known limitations

This is an exploratory student prototype. The historical commits report movement glitches, including wall-jump behavior that only worked on alternating attempts. There is no automated test suite, and the project has not been rerun as part of this documentation update.

The repository retains its original course-project name, `mdr180004_p03`. Its commits and code are preserved so the development history remains available.

## More recent work

See my [data workflow portfolio](https://github.com/Zenoctra/Zenoctra/blob/main/PORTFOLIO.md) for later projects in ingestion, record identity, relational modeling, and validation, with explicit descriptions of my contributions and AI/contractor implementation.

The repository's existing [license](LICENSE.md) is unchanged.
