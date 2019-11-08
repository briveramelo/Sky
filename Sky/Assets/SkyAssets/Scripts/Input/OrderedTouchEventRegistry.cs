using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderedTouchEventRegistry : Singleton<OrderedTouchEventRegistry>
{
    private OrderedEventCollection<Action<int, Vector2>> _onTouchBegin = new OrderedEventCollection<Action<int,Vector2>>();
    private OrderedEventCollection<Action<int, Vector2>> _onTouchHold = new OrderedEventCollection<Action<int,Vector2>>();
    private OrderedEventCollection<Action<int, Vector2>> _onTouchEnd = new OrderedEventCollection<Action<int,Vector2>>();

    public void OnTouchWorldBegin(Type registeringType, Action<int, Vector2> callback, bool subscribe)
    {
        _onTouchBegin.Subscribe(registeringType, callback, subscribe);
    }
    public void OnTouchWorldHeld(Type registeringType, Action<int, Vector2> callback, bool subscribe)
    {
        _onTouchHold.Subscribe(registeringType, callback, subscribe);
    }
    public void OnTouchWorldEnd(Type registeringType, Action<int, Vector2> callback, bool subscribe)
    {
        _onTouchEnd.Subscribe(registeringType, callback, subscribe);
    }

    private IEnumerator Start()
    {
        yield return null;
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        var beginCallbacks = _onTouchBegin.GetSortedCallbacks();
        for (int i = 0; i < beginCallbacks.Count; i++)
        {
            TouchInputManager.Instance.OnTouchWorldBegin += beginCallbacks[i];
        }
        var holdCallbacks = _onTouchHold.GetSortedCallbacks();
        for (int i = 0; i < holdCallbacks.Count; i++)
        {
            TouchInputManager.Instance.OnTouchWorldHeld += holdCallbacks[i];
        }
        var endCallbacks = _onTouchEnd.GetSortedCallbacks();
        for (int i = 0; i < endCallbacks.Count; i++)
        {
            TouchInputManager.Instance.OnTouchWorldEnd += endCallbacks[i];
        }
    }

    private class OrderedEventCollection<T> where T : class
    {
        private class TypeOrderCallback
        {
            public TypeOrderCallback(Type type)
            {
                Type = type;
            }

            public Type Type;
            public T Callback;
        }
        
        private List<TypeOrderCallback> _typeOrderCallbacks = new List<TypeOrderCallback>
        {
            new TypeOrderCallback(typeof(Joystick)),
            new TypeOrderCallback(typeof(Pauser)),
            new TypeOrderCallback(typeof(MaskCamera)),
            new TypeOrderCallback(typeof(Jai)),
        };

        public void Subscribe(Type registeringType, T callback, bool subscribe)
        {
            var typeOrder = _typeOrderCallbacks.Find(item => item.Type == registeringType);
            if (typeOrder == null)
            {
                Debug.LogError("No type order found for type: " + registeringType.FullName);
                return;
            }
            typeOrder.Callback = subscribe ? callback : null;
        }
        public List<T> GetSortedCallbacks()
        {
            return _typeOrderCallbacks.Select(item => item.Callback).ToList();
        }
    }
}

