using BepInEx;
using BepInEx.Configuration;
using Logger;
using RoR2;
using RoR2.ContentManagement;
using RoR2BepInExPack.GameAssetPathsBetter;
using UnityEngine.AddressableAssets;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace EarlyMoon;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public sealed class EarlyMoon : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Onyx";
    public const string PluginName = "EarlyMoon";
    public const string PluginVersion = "1.0.0";

	public static EarlyMoon Instance;
	public static ConfigEntry<int> moonTpStage { get; set; }

	static InteractableSpawnCard defaultTp;
	static InteractableSpawnCard MoonTp;

	public void Awake()
	{
		Log.Init(Logger);
		Instance = SingletonHelper.Assign(Instance, this);
		Options.Init();
		
		AssetReferenceT<InteractableSpawnCard> loadObject = new(RoR2_Base_Teleporters.iscTeleporter_asset);
		AssetAsyncReferenceManager<InteractableSpawnCard>.LoadAsset(loadObject).Completed += (x) => defaultTp = x.Result;
		loadObject = new(RoR2_Base_Teleporters.iscLunarTeleporter_asset);
		AssetAsyncReferenceManager<InteractableSpawnCard>.LoadAsset(loadObject).Completed += (x) => MoonTp = x.Result;

		On.RoR2.SceneDirector.PlaceTeleporter += (orig, self) =>
		{
			if ((Run.instance.stageClearCount + 1) % moonTpStage.Value == 0)
			{
				self.teleporterSpawnCard = MoonTp;
			} else {
				self.teleporterSpawnCard = defaultTp;
			}
			orig(self);
		};
	}
}
