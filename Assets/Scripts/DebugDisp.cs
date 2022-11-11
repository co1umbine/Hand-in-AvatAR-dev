using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HandinAvatAR
{
    public class DebugDisp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txt;
        [SerializeField] private Transform origin;
        [SerializeField] private Transform spot;

        private float lastTapTime;
        private Transform cam;

        private void Start()
        {
            cam = Camera.main.transform;
        }

        void Update()
        {
            bool isTap = false;
            int count = 0;
            if (Touch.activeTouches.Count > 0)
            {

                foreach (var t in Touch.activeTouches)
                {
                    isTap = t.isTap || isTap;
                    count = Mathf.Max(count, t.tapCount);
                }
                if (isTap)
                    lastTapTime = Time.time;

            }
            txt.text = $"last tap: {lastTapTime} \n tapCount: {count} \n origin pos: {origin.position} \n camera: {cam.position} \n spot: {spot.position}";
        }
    }
}