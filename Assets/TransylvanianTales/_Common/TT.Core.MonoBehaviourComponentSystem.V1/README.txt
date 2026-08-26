MONOBEHAVIOUR COMPONENT SYSTEM


----------------------------------------
Author
----------------------------------------

Transylvanian Tales


----------------------------------------
Overview
----------------------------------------

A lightweight framework for building modular gameplay architecture using MonoBehaviours as components and centralized systems.

Components focus on data and Unity callbacks, while systems handle all processing logic by operating on registered components.

This creates a clean separation of concerns while remaining fully compatible with standard Unity workflows.


----------------------------------------
Why Use This?
----------------------------------------

In traditional Unity development, MonoBehaviours often mix:

- Data
- Logic
- Unity callbacks

This can lead to tightly coupled and hard-to-maintain code.

This system separates responsibilities by:

- Keeping components simple and focused
- Moving logic into centralized systems
- Removing direct dependencies between objects

The result is a more scalable and maintainable architecture.


----------------------------------------
Features
----------------------------------------

- Component-based architecture using MonoBehaviours
- Automatic registration of components via static registries
- Centralized systems that process all active components
- Lightweight event system for decoupled communication
- System Bootstrapper for controlled initialization
- Optional persistence between scenes
- Minimal and flexible design


----------------------------------------
Quick Usage
----------------------------------------

1. Create a component:

   - Inherit from ComponentBase<T>

   Example:

   public class HealthComponent : ComponentBase<HealthComponent>
   {
       public int Value;
   }

2. Create a system:

   - Inherit from GenericSystemBase<T>

   Example:

   public class HealthSystem : GenericSystemBase<HealthSystem>
   {
       public override bool PersistBetweenScenes => true;

       private void Update()
       {
           foreach (var component in HealthComponent.Registry.Components)
           {
               // Process component
           }
       }
   }

3. Add systems to the scene:

   - Create system prefabs
   - Assign them to the SystemBootstrapper


----------------------------------------
When To Use
----------------------------------------

Use this system when you want:

- Clear separation between data and logic
- Centralized control over gameplay systems
- Decoupled communication between objects
- Scalable architecture for medium to large projects

It is especially useful for:

- Simulation systems
- Gameplay logic (health, movement, AI, etc.)
- Projects that are growing beyond simple MonoBehaviour setups


----------------------------------------
Documentation
----------------------------------------

See the Documentation folder for more details.