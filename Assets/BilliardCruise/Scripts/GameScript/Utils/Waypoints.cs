using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    public class Waypoints : MonoBehaviour
    {
        public List<Vector3> waypoints;
        // Start is called before the first frame update
        void Start()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms)
            {
                if (t != transform)
                {
                    waypoints.Add(t.position);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

