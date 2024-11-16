using Main.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI.Equipment
{
    public class IngredientContainer : MonoBehaviour
    {
        private Ingredient _ingredient;

        [SerializeField] private RecipeContainer _recipeContainer;
        [SerializeField, ReadOnly] private Inventory _inventory;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI amountText;

        private int amount;


        public void Initialize(Ingredient ingredient, Inventory inventory)
        {
            gameObject.SetActive(true);
            image.sprite = ingredient.itemData.icon;

            _ingredient = ingredient;
            _inventory = inventory;

            UpdateAmount();
        }

        public bool UpdateAmount()
        {
            amount = _inventory.GetAmount(_ingredient.itemData);
            UpdateText();
            return amount >= _ingredient.amount;
        }
        public void UpdateText()
        {
            amountText.SetText($"{amount}/{_ingredient.amount}");
        }

        public void Use()
        {
            _inventory.RemoveAmount(_ingredient.itemData, _ingredient.amount);
        }
    }
}
