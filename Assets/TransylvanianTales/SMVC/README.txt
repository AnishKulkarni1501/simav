SMVC (SIMPLE MODULAR VEHICLE CONTROLLER)


----------------------------------------
Overview
----------------------------------------

SMVC is a modular vehicle framework built using a component-system architecture.

This asset provides:

- A fully functional vehicle controller
- A pre-configured car prefab
- Demo scenes for multiple render pipelines
- A modular, package-based architecture
- Full documentation and learning resources

SMVC is designed both as a ready-to-use solution and as a reference implementation for building scalable and maintainable gameplay systems in Unity.


----------------------------------------
Project Structure
----------------------------------------

This asset is organized into two main parts:

1. Assets (Visuals & Demos)
   Location:
   /Assets/TransylvanianTales/SMVC

   Contains:
   - Demo scenes
   - Prefabs
   - Demo scripts
   - Visual elements (car, camera, UI)

2. Packages (Core Logic)
   Location:
   /Assets/TransylvanianTales/_Common/

   Contains:
   - Reusable core systems
   - Vehicle framework logic

This separation allows the core systems to be reused across multiple assets without duplicating code.


----------------------------------------
Internal Dependencies
----------------------------------------

SMVC relies on the following internal packages:

- TT.Core.ReadOnlyAttribute.V1
- TT.Core.MonoBehaviourComponentSystem.V1
- TT.SMVC.V1
- TT.Extensions.WheelCollider.V1

Location:
 /Assets/TransylvanianTales/_Common/

These packages are included with the asset.

Important:
- Do not remove these packages unless you fully understand their role
- They contain essential logic required for SMVC to function


----------------------------------------
Unity Package Requirements
----------------------------------------

The demo scenes require the following Unity packages:

- Input System (com.unity.inputsystem)
- TextMeshPro (com.unity.textmeshpro)

If these packages are not installed:
- Demo scenes may not function correctly
- Input will not work
- UI text may be missing

To install:
1. Open Window > Package Manager
2. Install:
   - Input System
   - TextMeshPro

Note:
- You may be prompted to enable the new Input System and restart Unity


----------------------------------------
Getting Started
----------------------------------------

1. Navigate to:
   /Assets/TransylvanianTales/SMVC/

2. If you are using URP or HDRP, locate the corresponding package:
   - URP.unitypackage
   - HDRP.unitypackage

   These files have a Unity icon (similar to a scene file).

3. Double-click the appropriate .unitypackage and import it.

   NOTE:
   - If you are using the Built-In Render Pipeline (BRP), no extra import is required.

4. After importing, navigate to:
   /Assets/TransylvanianTales/SMVC/RenderPipelines/

5. Open the folder matching your render pipeline:
   - BRP
   - URP
   - HDRP

6. Open the Demo scene inside the selected folder.

7. Press Play.

If you're unsure which render pipeline your project uses:
- Try importing and opening each demo scene
- The correct one will render properly
- Incorrect ones may appear pink/purple


----------------------------------------
Demo Scene Controls
----------------------------------------

- W / Up Arrow     -> Accelerate
- S / Down Arrow   -> Brake
- A / D or Arrows  -> Steer


----------------------------------------
Documentation
----------------------------------------

Detailed documentation is provided at the package level.

Each package includes:
- README (quick overview and usage)
- Full documentation (architecture and details)

Main packages:

- TT.SMVC.V1:
  Vehicle system and modules

- TT.Core.MonoBehaviourComponentSystem.V1:
  Component-system architecture

- TT.Core.ReadOnlyAttribute.V1:
  Inspector utility

- TT.Extensions.WheelCollider.V1:
  Utility that helps setup wheel colliders easy for new cars

Refer to the Packages folder for full details.


----------------------------------------
Video Tutorials
----------------------------------------

Step-by-step tutorials are available here:
https://www.youtube.com/playlist?list=PL2ESxGsWSBc4aL4EqsdrzWQ9cuPCrFuSN


----------------------------------------
Support
----------------------------------------

If you encounter issues or have questions:
transylvanian.tales.gamedev@gmail.com