using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HandinAvatAR
{
    public class InputProvider : MonoBehaviour
    {
        [SerializeField] private double tapMaxLength = 300;
        [SerializeField] private double doubleClickInterval = 500;

        private IntReactiveProperty observableTouchCount = new IntReactiveProperty();

        private Subject<Unit> tapTrigger = new Subject<Unit>();

        public IObservable<Unit> OnSingleTouch => 
            observableTouchCount
            .Buffer(2, 1)
            .Where(list => list.Count() == 2)
            .Where(list => list[0] == 0)
            .Where(list => list[1] == 1)
            .Select(_ => new Unit())
            .Publish()
            .RefCount();

        public IObservable<Unit> OnDoubleTapped => OnSingleTouch.TimeInterval()
            .Select(t => t.Interval.TotalMilliseconds)
            .Buffer(2, 1)
            .Where(x => x.Count == 2)
            .Where(list => list[0] > doubleClickInterval)
            .Where(list => list[1] <= doubleClickInterval)
            .Select(_ => new Unit())
            .Publish()
            .RefCount();

        // Start is called before the first frame update
        void Start()
        {
            EnhancedTouchSupport.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            observableTouchCount.Value = Touch.activeFingers.Count;

            if (Mouse.current.leftButton.isPressed)
            {
                observableTouchCount.Value += 1;
            }
        }
    }
}