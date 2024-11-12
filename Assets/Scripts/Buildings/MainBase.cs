using TMPro;
using UnityEngine;

namespace Main.Buildings
{
    public class MainBase : Structure
    {
        public GameObject lostScreen;
        public MapManager mapManager;

        public TextMeshProUGUI healthText;
        public TextMeshProUGUI maxHealthText;


        protected override void Awake()
        {
            base.Awake();

            healthText.SetText(data.maxHealth.ToString());
            maxHealthText.SetText(data.maxHealth.ToString());
        }

        protected override void OnHit()
        {
            healthText.SetText(GetHealth().ToString());
            base.OnHit();
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            lostScreen.SetActive(true);
            //TODO: 0 LOSE SCREEN
        }

        public override void OpenGUI() { }
    }
}
