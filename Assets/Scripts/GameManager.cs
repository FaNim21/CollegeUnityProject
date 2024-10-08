using UnityEngine;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        //public List<d>

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
