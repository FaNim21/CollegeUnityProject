using TMPro;
using UnityEngine;

namespace Main.Buildings
{
    public class MainBase : Structure
    {
        public MapManager mapManager;

        public TextMeshProUGUI healthText;
        public TextMeshProUGUI maxHealthText;


        protected override void Awake()
        {
            base.Awake();

            healthText.SetText(data.maxHealth.ToString());
            maxHealthText.SetText(data.maxHealth.ToString());

            mapManager.AddStructure(this);
        }

        protected override void OnHit()
        {
            healthText.SetText(GetHealth().ToString());
            base.OnHit();
        }

        public override void OpenGUI() { }
    }
}
