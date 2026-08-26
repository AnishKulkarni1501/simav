READ ONLY ATTRIBUTE


----------------------------------------
Author
----------------------------------------

Transylvanian Tales


----------------------------------------
Overview
----------------------------------------

A lightweight Unity editor utility that adds a [ReadOnly] attribute for serialized fields.

It allows variables to be visible in the Inspector while preventing any manual modification, making it ideal for debugging and displaying runtime data safely.


----------------------------------------
Why Use This?
----------------------------------------

In Unity, fields are typically either:

- Editable (public / [SerializeField])
- Hidden ([HideInInspector])

There is no built-in way to make a field visible but non-editable in the standard Inspector.

This attribute fills that gap by providing a simple "look but don't touch" behavior on a per-field basis.


----------------------------------------
Features
----------------------------------------

- Makes Inspector fields non-editable (greyed out)
- Works with both public and [SerializeField] private fields
- Supports all standard Unity types
- Updates values in real time during Play Mode
- No runtime overhead (editor-only implementation)
- Minimal and dependency-free


----------------------------------------
Quick Usage
----------------------------------------

1. Add the namespace:

   using TT.Core.V1;

2. Apply the attribute:

   [ReadOnly]
   public int currentScore;

   [SerializeField, ReadOnly]
   private float velocity;

3. View the field in the Inspector (greyed out and non-editable)


----------------------------------------
When To Use
----------------------------------------

Use this attribute for:

- Debugging runtime values
- Displaying calculated data (speed, health, state, etc.)
- Exposing internal state safely
- Preventing accidental edits to important variables


----------------------------------------
Documentation
----------------------------------------

See the Documentation folder for more details.