using BepInEx;
using BepInEx.Bootstrap;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EarlyMoon;

[BepInDependency(RiskOfOptions.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
static class Options
{
	public static bool IsEnabled => Chainloader.PluginInfos.ContainsKey(RiskOfOptions.PluginInfo.PLUGIN_GUID);

	public static void Init()
	{
		EarlyMoon.moonTpStage = EarlyMoon.Instance.Config.Bind(
		"General", 
		"Stages until Lunar Tp", 
		5, 
		"The number of stages you have to beat between each occurence of a lunar teleporters, vanilla is 5");

		if (Options.IsEnabled)
		{
			RiskOfOptionsConfig();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	public static void RiskOfOptionsConfig() {
		const string MOD_GUID = EarlyMoon.PluginGUID;
		const string MOD_NAME = EarlyMoon.PluginName;

		ModSettingsManager.AddOption(new IntSliderOption(EarlyMoon.moonTpStage, new IntSliderConfig() { min = 0, max = 10 }), MOD_GUID, MOD_NAME);

		ModSettingsManager.SetModDescription($"Options for {MOD_NAME}", MOD_GUID, MOD_NAME);

		FileInfo iconFile = null;
		DirectoryInfo dir = new DirectoryInfo(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
		do
		{
			FileInfo[] files = dir.GetFiles("icon.png", SearchOption.TopDirectoryOnly);
			if (files != null && files.Length > 0)
			{
				iconFile = files[0];
				break;
			}

			dir = dir.Parent;
		} while (dir != null && dir.Exists && !string.Equals(dir.Name, "plugins", StringComparison.OrdinalIgnoreCase));

		if (iconFile != null)
		{
			Texture2D iconTexture = new Texture2D(256, 256);
			if (iconTexture.LoadImage(File.ReadAllBytes(iconFile.FullName)))
			{
				Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
				iconSprite.name = $"{MOD_NAME}Icon";

				ModSettingsManager.SetModIcon(iconSprite, MOD_GUID, MOD_NAME);
			}
		}
	}
}