﻿using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.HalibutLegend
{
    internal class TextContent : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "Halibut";
        protected override void Register() { }
        #region 字段内容

        #endregion
        #region Utils
        public static string GetTextKey(string key) => $"Mods.CalamityOverhaul.Halibut.TextContent.{key}";
        public static string GetTextValue(string key) => Language.GetTextValue($"Mods.CalamityOverhaul.Halibut.TextContent.{key}");
        public static LocalizedText GetText(string key) => Language.GetText($"Mods.CalamityOverhaul.Halibut.TextContent.{key}");
        #endregion
        public override void Load() {
            //使用反射进行属性的自动加载
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties) {
                if (property.PropertyType == typeof(LocalizedText)) {
                    property.SetValue(this, this.GetLocalization(property.Name));
                }
            }
        }
    }
}
