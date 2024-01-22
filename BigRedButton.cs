using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using LethalLib;
using LethalLib.Modules;
using System.Diagnostics;

namespace BigRedButton
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class BigRedButton : BaseUnityPlugin
    {

        private const string modGUID = "bricct.bigredbutton";

        private const string modName = "Big Red Button";
        
        private const string modVersion = "1.0.0";

        private readonly Harmony _harmony = new Harmony(modGUID);

        private static BigRedButton _instance;


        internal ManualLogSource _logger;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            _logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "BigRedButton");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);

            GameObject bigRedButtonPrefab = bundle.LoadAsset<GameObject>("Assets/BigRedButton/BigRedButton.prefab");
            NetworkPrefabs.RegisterNetworkPrefab(bigRedButtonPrefab);

            UnlockableItem val2 = new UnlockableItem();
            val2.unlockableName = "Big Red Button";
            val2.prefabObject = bigRedButtonPrefab;
            val2.unlockableType = 1;
            val2.alwaysInStock = true;
            val2.maxNumber = 100;
            val2.IsPlaceable = true;
            val2.canBeStored = false;
            TerminalNode info = new TerminalNode();
            info.name = "Big Red Button";
            info.clearPreviousText = true;
            info.displayText = "Only press in emergency situations";
            Unlockables.RegisterUnlockable(val2, StoreType.ShipUpgrade, null, null, info, 100);
            _logger.LogInfo("Loading unlockable: Big Red Button");

            _harmony.PatchAll(typeof(BigRedButton));
            

            // Plugin startup logic
            _logger.LogInfo($"Plugin {modName} is loaded!");
        }
    }
}