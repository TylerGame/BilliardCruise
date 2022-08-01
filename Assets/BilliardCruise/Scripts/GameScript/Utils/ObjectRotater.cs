using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    public class ObjectRotater : MonoBehaviour
    {

        [SerializeField]
        float speed;
        public enum Direction { X, Y, Z };
        public Direction dir = Direction.Y;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch (dir)
            {
                case Direction.X:
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x + speed * Time.deltaTime, transform.eulerAngles.y, transform.eulerAngles.z);
                    break;
                case Direction.Y:
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + speed * Time.deltaTime, transform.eulerAngles.z);
                    break;

                case Direction.Z:
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + speed * Time.deltaTime);
                    break;
            }
        }
    }
}

