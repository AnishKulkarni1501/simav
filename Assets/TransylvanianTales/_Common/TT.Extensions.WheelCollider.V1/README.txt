WHEEL COLLIDER EXTENSIONS


----------------------------------------
Author
----------------------------------------

Transylvanian Tales


----------------------------------------
Overview
----------------------------------------

A specialized utility package and editor tool designed to automate the alignment and scaling of Unity WheelColliders. It ensures that physical wheel dimensions and positions perfectly match their visual mesh counterparts with a single click or a single line of code.


----------------------------------------
Features
----------------------------------------

- Automatic radius calculation based on mesh bounds
- Precise spatial alignment between collider and renderer
- Built-in Editor Window for fast workflow
- Support for both local and world space positioning
- Compensation for lossy scale to ensure physical accuracy
- Full Undo/Redo support in the Unity Editor
- Runtime-ready static API


----------------------------------------
Core Concept
----------------------------------------

Setting up WheelColliders manually is often a tedious process of trial and error. This system treats the visual Renderer as the "source of truth." By analyzing the vertical extents of a wheel's bounds, the system mathematically derives the correct radius and center point, accounting for the object's scale. This eliminates visual "sinking" or "floating" of vehicles caused by mismatched physical and visual dimensions.


----------------------------------------
Usage
----------------------------------------

1. Editor Tool (Recommended):
   - Navigate to Tools > Transylvanian Tales > Wheel Collider Setup Tool.
   - Assign the WheelCollider you wish to configure.
   - Assign the Renderer (mesh) of the wheel.
   - Click "Setup Wheel Collider."

2. Scripting API:
   - Call WheelColliderExtensions.Setup(myCollider, myRenderer) from any script.
   - This is useful for procedural vehicle generation or runtime wheel swapping.

To ensure the best results:
- Ensure your wheel mesh is oriented correctly (Y-axis typically representing height).
- The tool will automatically handle parent-child transform relationships to place the collider at the mesh's center.


----------------------------------------
Documentation
----------------------------------------

See the Documentation folder for full details.