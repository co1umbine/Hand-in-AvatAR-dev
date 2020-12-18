using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using VRM;

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

        // Start is called before the first frame update
        void Awake()
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.touchCount > 0)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                if(raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
                {
                    var hitPose = hits[0].pose;

                    if (spawndAvatar)
                    {
                        var dir = (cameraManager.transform.position - hitPose.position);
                        dir.y = 0;
                        spawndAvatar.transform.position = hitPose.position;
                        spawndAvatar.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                    }
                    else
                    {
                        var dir = (cameraManager.transform.position - hitPose.position);
                        dir.y = 0;
                        spawndAvatar = Instantiate(prefab, hitPose.position, Quaternion.LookRotation(dir, Vector3.up));
                        spawndAvatar.GetComponent<VRMLookAtHead>().Target = cameraManager.transform;
                    }
                }
            }
        }

        public void OnButtonTouch()
        {
            Destroy(spawndAvatar.gameObject);
        }
    }
}