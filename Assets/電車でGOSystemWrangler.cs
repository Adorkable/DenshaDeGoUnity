using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.LowLevel;
using System.Linq;

// Based on Minis - https://github.com/keijiro/Minis/blob/master/Packages/jp.keijiro.minis/Runtime/Internal/MidiSystemWrangler.cs
namespace 電車でGO
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    sealed class 電車でGOSystemWrangler
    {
        static 電車でGODriver _driver;

        static void RegisterLayout()
        {
            // InputSystem.RegisterLayout<電車でGO新幹線Button>("電車でGO新幹線 Button");
            // InputSystem.RegisterLayout<電車でGOValueControl>("電車でGOValue");

            InputSystem.RegisterLayout<電車でGO新幹線Device>(
                name: "電車でGO新幹線 Controller",
                matches: new InputDeviceMatcher().WithInterface("電車でGO新幹線").WithManufacturer("Taito Corp.")
            );
        }

        static void InsertPlayerLoopSystem()
        {
            var customSystem = new PlayerLoopSystem()
            {
                type = typeof(電車でGOSystemWrangler),
                updateDelegate = () => _driver?.Update()
            };

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            for (var i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                ref var phase = ref playerLoop.subSystemList[i];
                if (phase.type == typeof(UnityEngine.PlayerLoop.EarlyUpdate))
                {
                    phase.subSystemList =
                        phase.subSystemList.Concat(new[] { customSystem }).ToArray();
                    break;
                }
            }

            PlayerLoop.SetPlayerLoop(playerLoop);
        }


#if UNITY_EDITOR

        //
        // On Editor, we use InitializeOnLoad to install the subsystem. At the
        // same time, we use AssemblyReloadEvents to temporarily disable the
        // system to avoid issue #1192379.
        // #FIXME This workaround should be removed when the issue is solved.
        //

        static 電車でGOSystemWrangler()
        {
            RegisterLayout();
            InsertPlayerLoopSystem();

            // We use not only PlayerLoopSystem but also the
            // EditorApplication.update callback because the PlayerLoop events
            // are not invoked in the edit mode.
            UnityEditor.EditorApplication.update += () => _driver?.Update();

            // Uninstall the driver on domain reload.
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                _driver?.Dispose();
                _driver = null;
            };

            // Reinstall the driver after domain reload.
            UnityEditor.AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                _driver = _driver ?? new 電車でGODriver();
            };
        }

#else

        //
        // On Player, we use RuntimeInitializeOnLoadMethod to install the
        // subsystems. We don't do anything about finalization.
        //

        [UnityEngine.RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            RegisterLayout();
            InsertPlayerLoopSystem();
            _driver = new 電車でGODriver();
        }

#endif
    }
}