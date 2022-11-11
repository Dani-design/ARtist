using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class HumanBodyTracker : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The Skeleton prefab to be controlled.")]
        GameObject m_SkeletonPrefab;

        [SerializeField]
        [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
        ARHumanBodyManager m_HumanBodyManager;

        /// <summary>
        /// Get/Set the <c>ARHumanBodyManager</c>.
        /// </summary>
        public ARHumanBodyManager humanBodyManager
        {
            get { return m_HumanBodyManager; }
            set { m_HumanBodyManager = value; }
        }

        /// <summary>
        /// Get/Set the skeleton prefab.
        /// </summary>
        public GameObject skeletonPrefab
        {
            get { return m_SkeletonPrefab; }
            set { m_SkeletonPrefab = value; }
        }

        Dictionary<TrackableId, BoneController> m_SkeletonTracker = new Dictionary<TrackableId, BoneController>();

        void OnEnable()
        {
            Debug.Assert(m_HumanBodyManager != null, "Human body manager is required.");
            m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
        }

        public void TestButtonClicked()
        {
            //go through some initialization and go through the item that's down there is destroyed 
            foreach (var id in m_SkeletonTracker.Keys)
            {
                var boneController = m_SkeletonTracker[id];

                TrailRenderer[] children = boneController.GetComponentsInChildren<TrailRenderer>(); ;
                if (children != null && children.Length > 0)
                {
                    foreach (TrailRenderer child in children)
                    {
                        Debug.Log(child.gameObject.name);
                        child.Clear();
                    }
                }
                else
                {
                    Debug.Log("no Children");
                }
            }
        }

        public void PauseButton()
        {
            foreach (var id in m_SkeletonTracker.Keys)
            {
                var boneController = m_SkeletonTracker[id];
                TrailRenderer[] children = boneController.GetComponentsInChildren<TrailRenderer>();
                if (children != null && children.Length > 0)
                {
                    if (children[0].emitting == true)
                    {
                        foreach (TrailRenderer child in children)
                        {
                            child.emitting = false;
                        }
                    }
                }
                else
                {
                    Debug.Log("no Children");
                }
            }
        }
        
        public void changeColor()
        {
            Material m_Material = gameObject.GetComponentInChildren<Renderer>().sharedMaterial;
            if (m_Material.color == Color.green)
            {
                m_Material.color = Color.red;
            }
            else
            {
                m_Material.color = Color.green;
            }
        }

        public void Head()
        {
            Emitting("Nose");
        }
        public void RightHand()
        {
            Emitting("RightHandIndex3");
        }
        public void LeftHand()
        {
            Emitting("LeftHandIndex3");
        }
        public void RightFoot()
        {
            Emitting("RightFoot");
        }
        public void Leftfoot()
        {
            Emitting("LeftFoot");
        }

        public void Emitting(string Name)
        {
            var something = transform;
            foreach (var id in m_SkeletonTracker.Keys)
            {
                var boneController = m_SkeletonTracker[id];
                Debug.Log("m_SkeletonTracker id is " + boneController);
                //TrailRenderer[]  = boneController.GetComponentsInChildren<TrailRenderer>();
                Transform[] ts = boneController.transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    Debug.Log("t is " + t);
                    if (t.gameObject.name == Name)
                    { something = t;
                      break; 
                    }
                }
                //var something = getChildGameObject(gameObject, Name);
                TrailRenderer trail = something.GetComponent<TrailRenderer>();

                if (trail != null)
                {
                    {
                        if (trail.emitting == true)
                        {
                            trail.emitting = false;
                        }
                        else
                        {
                            trail.emitting = true;
                        }
                    }
                }
                else
                {
                    Debug.Log("no Children");
                }
            }

        }

        static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
        {
            //Author: Isaac Dart, June-13.
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
            return null;
        }

        void OnDisable()
        {
            if (m_HumanBodyManager != null)
                m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
        }

        void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
        {
            BoneController boneController;

            foreach (var humanBody in eventArgs.added)
            {
                if (!m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    Debug.Log($"Adding a new skeleton [{humanBody.trackableId}].");
                    var newSkeletonGO = Instantiate(m_SkeletonPrefab, humanBody.transform);
                   // newSkeletonGO.transform.parent = gameObject.transform;
                    boneController = newSkeletonGO.GetComponent<BoneController>();
                    m_SkeletonTracker.Add(humanBody.trackableId, boneController);
                }

                boneController.InitializeSkeletonJoints();
                boneController.ApplyBodyPose(humanBody);
            }

            foreach (var humanBody in eventArgs.updated)
            {
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    boneController.ApplyBodyPose(humanBody);
                }
            }

            foreach (var humanBody in eventArgs.removed)
            {
                Debug.Log($"Removing a skeleton [{humanBody.trackableId}].");
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    Destroy(boneController.gameObject);
                    m_SkeletonTracker.Remove(humanBody.trackableId);
                }
            }
        }
    }
}