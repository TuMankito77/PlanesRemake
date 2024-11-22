namespace PlanesRemake.Runtime.Localization
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using UnityEngine;

    using Newtonsoft.Json;

    using PlanesRemake.Runtime.Core;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;

    public class LocalizationManager : BaseSystem
    {
        private LocalizationDatabase localizationDatabase = null;
        private Dictionary<string, string> localizationKeyLocalizedTextPair = null;

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            localizationDatabase = await contentLoader.LoadAsset<LocalizationDatabase>(LocalizationDatabase.LOCALIZATION_DATABASE_SCRIPTABLE_OBJECT_PATH);
            
            if(localizationDatabase == null)
            {
                return false;
            }
            
            localizationDatabase.Initialize();
            
            if (localizationDatabase.Ids.Count > 0)
            {
                string operatingSystemLanguage = string.Empty;

                switch(Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        {
                            operatingSystemLanguage = LanguageIds.EN;
                            break;
                        }

                    case SystemLanguage.French:
                        {
                            operatingSystemLanguage = LanguageIds.FR;
                            break;
                        }

                    case SystemLanguage.Spanish:
                        {
                            operatingSystemLanguage = LanguageIds.ES;
                            break;
                        }

                    default:
                        {
                            operatingSystemLanguage = LanguageIds.EN;
                            break;
                        }
                }

                TextAsset textAsset = localizationDatabase.GetFile(operatingSystemLanguage);
                localizationKeyLocalizedTextPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
            }

            return true;
        }

        public void UpdatedSelectedLanguage(string langaugeKey)
        {
            if(localizationDatabase.DoesIdExist(langaugeKey))
            {
                TextAsset textAsset = localizationDatabase.GetFile(langaugeKey);
                localizationKeyLocalizedTextPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
                EventDispatcher.Instance.Dispatch(LocalizationEvents.OnLanguageUpdated);
            }
            else
            {
                LoggerUtil.Log($"{GetType()}: Language key {langaugeKey} not found, keeping the current language as the one selected.");
            }
        }

        public string GetLocalizedText(string localizationKey)
        {
            if(localizationKeyLocalizedTextPair != null && localizationKeyLocalizedTextPair.TryGetValue(localizationKey, out string localizedText))
            {
                return localizedText;
            }

            return localizationKey;
        }
    }
}
