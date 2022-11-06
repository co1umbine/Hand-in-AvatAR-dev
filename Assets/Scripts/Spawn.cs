using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UniRx;
using VRM;
using UnityEngine.Animations.Rigging;

namespace HandinAvatAR
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class Spawn : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] ARCameraManager cameraManager;
        [SerializeField] InputProvider input;

        private GameObject spawndAvatar;
        private ARRaycastManager raycastManager;
        private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // Start is called before the first frame update
        void Awake()
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }

        private void Start()
        {
            input.OnSingleTouch.Subscribe(_ => OnTap()).AddTo(this.gameObject);
        }

        void OnTap()
        {
            if (Input.touchCount > 0)
            {

                Vector2 touchPosition = Input.GetTouch(0).position;
                if (raycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
                {
                    var hitPose = hits[0].pose;

                    if (spawndAvatar)
                    {
                        //var dir = (cameraManager.transform.position - hitPose.position);
                        //dir.y = 0;
                        //spawndAvatar.transform.position = hitPose.position;
                        //spawndAvatar.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                    }
                    else
                    {
                        var dir = (cameraManager.transform.position - hitPose.position);
                        dir.y = 0;

                        spawndAvatar = Instantiate(prefab, hitPose.position, Quaternion.LookRotation(dir, Vector3.up));

                        //foreach (Transform lookat in spawndAvatar.GetComponent<RigBuilder>().layers[2].rig.transform)
                        //{
                        //    var constraintData = lookat.GetComponent<MultiAimConstraint>().data.sourceObjects;
                        //    constraintData.SetTransform(0, cameraManager.transform);
                        //    lookat.GetComponent<MultiAimConstraint>().data.sourceObjects = constraintData;
                        //}
                        //spawndAvatar.GetComponent<RigBuilder>().Build();
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnButtonTouch();
            }
        }

        public void OnButtonTouch()
        {
            Destroy(spawndAvatar);
        }
    }
}