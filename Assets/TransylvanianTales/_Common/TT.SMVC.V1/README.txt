SMVC (SIMPLE MODULAR VEHICLE CONTROLLER)


----------------------------------------
Author
----------------------------------------

Transylvanian Tales


----------------------------------------
Overview
----------------------------------------

SMVC is a lightweight, modular vehicle framework built around a component-system architecture.

Instead of a single monolithic controller, vehicles are composed of independent modules (Engine, Gearbox, Wheels, Steering, etc.), each with its own data, component, and system.

This approach provides a clean separation between data, state, and logic, making the system easy to extend, modify, and understand.


----------------------------------------
Features
----------------------------------------

- Fully modular vehicle architecture (Engine, Gearbox, Wheels, etc.)
- Component-System design using MonoBehaviours
- Automatic component registration via registries
- Data-driven configuration using ScriptableObjects
- Global systems that process all vehicle components
- Built-in modules (Engine, Gearbox, Brakes, Steering, Speedometer, Wheels)
- Gearbox auto-setup tool for generating ratios and shift points
- Vehicle-based component lookup system
- Simple chase camera and dashboard UI support
- Designed for extensibility and learning


----------------------------------------
Core Concept
----------------------------------------

SMVC is built on three main layers:

- Data (ScriptableObjects) -> static configuration (torque, RPM, gear ratios)
- Components (MonoBehaviours) -> runtime state per vehicle
- Systems -> global logic that processes all components

Each vehicle is a collection of modules, not a single script.

Flow:

Vehicle -> Components -> Registries -> Systems

Systems never directly reference specific vehicles. Instead, they process all active components, ensuring a decoupled and scalable design.


----------------------------------------
Usage
----------------------------------------

1. Create a Vehicle:

   - Create a root GameObject
   - Add:
     - Rigidbody
     - Vehicle component

2. Add Modules:

   - Add components such as:
     - Engine
     - Gearbox
     - Wheels
     - Steering Mechanism
     - Brake
     - Speedometer

   - Assign required references (WheelColliders, configurations, etc.)

3. Create Configurations:

   - Create ScriptableObjects for:
     - Engine
     - Gearbox
     - Speedometer
     - Steering
     - Brake

4. Setup Systems:

   - Add system prefabs (EngineSystem, GearboxSystem, etc.)
   - Use the SystemBootstrapper (recommended)

5. Provide Input:

   - Set values like:
     - Engine.throttleInput
     - Steering.input
     - Brake.input

   (Input is not included, allowing full flexibility)


----------------------------------------
Documentation
----------------------------------------

See the Documentation folder for more details.