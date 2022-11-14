using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HandinAvatAR
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class Spawn : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] ARCameraManager cameraManager;

        private GameObject spawndAvatar;
        private ARRaycastManager raycastManager;
        private static List<ARRaycastHit> hits = new List<ARRaycastHit>();


        void Awake()
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }

        private void Start()
        {
            EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            if (Touch.activeTouches.Count > 0)
            {

                Vector2 touchPosition = new Vector2();
                foreach (var t in Touch.activeTouches)
                {
                    if (t.isTap)
                    {
                        OnTap(touchPosition);
                        return;
                    }
                }

            }
        }

        public void DebugSpawn()
        {
            spawndAvatar = Instantiate(prefab, new Vector3(1, -1, 0), Quaternion.identity);
        }

        void OnTap(Vector2 touchPosition)
        {

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
            {
                var hitPose = hits[0].pose;

                if (spawndAvatar)
                {
                }
                else
                {
                    var dir = (cameraManager.transform.position - hitPose.position);
                    dir.y = 0;

                    spawndAvatar = Instantiate(prefab, hitPose.position, Quaternion.LookRotation(dir, Vector3.up));

                    spawndAvatar.GetComponent<RigBuilder>().Build();
                }
            }
        }

        public void OnButtonTouch()
        {
            Destroy(spawndAvatar);
        }
    }
}