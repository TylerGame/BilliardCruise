using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{

    public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour {

        private static T instance = null;
        public static T Instance {
            get {
                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance == null) {
                instance = this as T;
            }
            else if (instance != this) {
                Destroy (this.gameObject);
            }
        }

    }
}

