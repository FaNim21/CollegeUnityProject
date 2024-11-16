using Main.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI.Equipment
{
    public class RecipeContainer : MonoBehaviour
    {
        private CraftingWindow _crafting;
        private Recipe _recipe;

        [SerializeField] private IngredientContainer[] _containers;

        [SerializeField] private TextMeshProUGUI _desciptionText;
        [SerializeField] private Image _mainImage;


        public void Initialize(CraftingWindow craftingWindow, Recipe recipe)
        {
            _crafting = craftingWindow;
            _recipe = recipe;

            _desciptionText.SetText(_recipe.item.description);
            _mainImage.sprite = _recipe.item.icon;

            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                var current = _containers[i];
                var ingredient = recipe.ingredients[i];
                current.Initialize(ingredient, _crafting.inventory);
            }
        }

        public void UpdateIngredientsInfo()
        {
            for (int i = 0; i < _recipe.ingredients.Length; i++)
            {
                var current = _containers[i];
                current.UpdateAmount();
            }
        }

        //all here could be done at least x5 better but there is no need and time for that
        public void OnCraft()
        {
            for (int i = 0; i < _recipe.ingredients.Length; i++)
            {
                var current = _containers[i];
                if (!current.UpdateAmount())
                {
                    Utils.LogWarning($"You cant craft: {_recipe.item.name}");
                    return;
                }
            }

            for (int i = 0; i < _recipe.ingredients.Length; i++)
            {
                var current = _containers[i];
                current.Use();
            }

            _crafting.UpdateIngredients();
            _crafting.inventory.AddToInventory(_recipe.item, 1);
            Utils.Log($"Crafted: {_recipe.item.name}");
        }
    }
}
