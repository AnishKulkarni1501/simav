using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace TT.Core.MonoBehaviourComponentSystem.V1
{
    public class ComponentRegistry<TComponent> where TComponent : class
    {
        private readonly HashSet<TComponent> components = new HashSet<TComponent>();

        public UnityAction<TComponent> OnComponentRegistered;

        public UnityAction<TComponent> OnBeforeComponentUnregistered;
        public UnityAction<TComponent> OnAfterComponentUnregistered;

        private readonly Dictionary<Type, UnityAction<TComponent, IComponentEvent>> eventMap = new();

        public IEnumerable<TComponent> Components => components;
        public int ComponentsCount => components.Count;


        public void Register(TComponent component)
        {
            if (components.Contains(component)) return;

            components.Add(component);
            OnComponentRegistered?.Invoke(component);
        }

        public void Unregister(TComponent component)
        {
            if (!components.Contains(component)) return;

            OnBeforeComponentUnregistered?.Invoke(component);
            components.Remove(component);
            OnAfterComponentUnregistered?.Invoke(component);
        }


        public void PublishEvent<TEvent>(TComponent component, TEvent _event) where TEvent : struct, IComponentEvent
        {
            if (eventMap.TryGetValue(typeof(TEvent), out var action))
            {
                action?.Invoke(component, _event);
            }
        }


        public void Subscribe<TEvent>(UnityAction<TComponent, TEvent> callback) where TEvent : struct, IComponentEvent
        {
            var type = typeof(TEvent);

            eventMap.TryGetValue(type, out var existing);

            eventMap[type] = existing + ((component, _event) => callback(component, (TEvent)_event));
        }

        public void Unsubscribe<TEvent>(UnityAction<TComponent, TEvent> callback) where TEvent : struct, IComponentEvent
        {
            var type = typeof(TEvent);

            if (!eventMap.TryGetValue(type, out var existing)) return;

            eventMap[type] = existing - ((component, _event) => callback(component, (TEvent)_event));
        }
    }
}