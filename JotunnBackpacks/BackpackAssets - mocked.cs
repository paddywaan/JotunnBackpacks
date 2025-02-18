﻿/* BackpackAssets.cs
 * 
 * From JotunnModExample, including eviesbackpacks made by CinnaBunn (Evie).
 * https://github.com/Valheim-Modding/JotunnModExample/
 * 
 */

using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System.Collections.Generic;
using UnityEngine;

public class BackpackAssets
{
    // Asset and prefab loading
    private AssetBundle EmbeddedResourceBundle;
    private GameObject BackpackIronPrefab;
    private GameObject BackpackSilverPrefab;

    public void LoadAssets()
    {
        // Load asset bundle from embedded resources
        Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(JotunnBackpacks.JotunnBackpacks).Assembly.GetManifestResourceNames())}");
        EmbeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("eviesbackpacks", typeof(JotunnBackpacks.JotunnBackpacks).Assembly);
        BackpackIronPrefab = EmbeddedResourceBundle.LoadAsset<GameObject>("Assets/Evie/CapeIronBackpack.prefab");
        BackpackSilverPrefab = EmbeddedResourceBundle.LoadAsset<GameObject>("Assets/Evie/CapeSilverBackpack.prefab");
    }

    public void AddTranslations()
    {
        LocalizationManager.Instance.AddLocalization(new LocalizationConfig("English")
        {
            Translations = {
                {"item_cape_silverbackpack", "Fine Backpack"},
                {"item_cape_silverbackpack_description", "A Fine backpack, complete with buckles and fine leather straps. Increases carry capacity by 200."},
                {"se_silverbackpackstrength_start", "Your carry capacity has been increased."},
                {"item_ironbackpackstrength", "Backpack equipped" },
                {"item_cape_ironbackpack", "Rugged Backpack" },
                {"item_cape_ironbackpack_description", "A Rugged backpack, complete with buckles and fine leather straps. Increases carry capacity by 150."},
                {"se_ironbackpackstrength_start", "Your carry capacity has been increased."},
                { "item_silverbackpackstrength", "Backpack equipped"}
            }
        });
    }

    // Implementation of assets using mocks, adding recipes manually without the config abstraction
    public void AddMockedItems()
    {
        // Iron Backpack
        if (!BackpackIronPrefab) Jotunn.Logger.LogWarning($"Failed to load asset from bundle: {EmbeddedResourceBundle}");
        else
        {
            // Create and add a custom item
            CustomItem CI = new CustomItem(BackpackIronPrefab, true);
            ItemManager.Instance.AddItem(CI);

            //Create and add a custom recipe
            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.name = "Recipe_CapeIronBackpack";
            recipe.m_item = BackpackIronPrefab.GetComponent<ItemDrop>();
            recipe.m_craftingStation = Mock<CraftingStation>.Create("piece_workbench");
            var ingredients = new List<Piece.Requirement>
            {
                MockRequirement.Create("LeatherScraps", 8),
                MockRequirement.Create("DeerHide", 2),
                MockRequirement.Create("Iron", 10),
            };
            recipe.m_resources = ingredients.ToArray();
            CustomRecipe CR = new CustomRecipe(recipe, true, true);
            ItemManager.Instance.AddRecipe(CR);

            //Enable BoneReorder
            BoneReorder.ApplyOnEquipmentChanged();
        }

        // Silver Backpack
        if (!BackpackSilverPrefab) Jotunn.Logger.LogWarning($"Failed to load asset from bundle: {EmbeddedResourceBundle}");
        else
        {
            // Create and add a custom item
            CustomItem CI = new CustomItem(BackpackSilverPrefab, true);
            ItemManager.Instance.AddItem(CI);

            //Create and add a custom recipe
            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.name = "Recipe_CapeSilverBackpack";
            recipe.m_item = BackpackSilverPrefab.GetComponent<ItemDrop>();
            recipe.m_craftingStation = Mock<CraftingStation>.Create("piece_workbench");
            var ingredients = new List<Piece.Requirement>
            {
                MockRequirement.Create("LeatherScraps", 8),
                MockRequirement.Create("DeerHide", 2),
                MockRequirement.Create("Silver", 10),
            };
            recipe.m_resources = ingredients.ToArray();
            CustomRecipe CR = new CustomRecipe(recipe, true, true);
            ItemManager.Instance.AddRecipe(CR);

            //Enable BoneReorder
            BoneReorder.ApplyOnEquipmentChanged();
        }
        EmbeddedResourceBundle.Unload(false);
    }
}