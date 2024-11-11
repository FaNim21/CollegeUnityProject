using System.Collections.Generic;
using UnityEngine;

namespace Main.Combat
{
    public class WavesController : MonoBehaviour
    {
        private GameManager gameManager;

        public List<Enemy> enemies = new();


        private void Start()
        {
            gameManager = GetComponent<GameManager>();
        }

        private void Update()
        {
            
        }

        private void SpawnEnemy()
        {

        }
    }
}
