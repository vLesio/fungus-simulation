using UnityEngine;
using Utils;
using Utils.Singleton;

namespace Settings {
    [RequireComponent(typeof(DoNotDestroy))]
    public class DevSettings : Singleton<DevSettings> {
        public AppSettingsDefinition appSettings;
    }   
}
