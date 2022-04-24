using System;
using System.Collections.Generic;

namespace DialogSystem.Localization
{
    public class LocalizationModel
    {
        private const string _engLocalizationPath = "Localization/TestLocalizationEng";
    
        private Dictionary<int, string> _linesById = new Dictionary<int, string>();
        private Language _currentLanguage;

        public LocalizationModel(Language currentLanguage)
        {
            if(currentLanguage is Language.Default)
                return;

            _currentLanguage = currentLanguage;
        
            LoadLocalizationData();
        }

        public string Get(string defaultText)
        {
            if (_currentLanguage is Language.Default)
                return defaultText;

            defaultText = defaultText.Trim('\n');

            if (_linesById.TryGetValue(defaultText.GetHashCode(), out var result))
            {
                return result + "\n";
            }

            throw new ArgumentException($"There is no localisation for -{defaultText}- line");
        }

        private void LoadLocalizationData()
        {
            var locName = string.Empty;
        
            switch (_currentLanguage)
            {
                case Language.Default:
                    return;
            
                case Language.English:
                    locName = _engLocalizationPath;
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            var locData = CSVReader.Read(locName);

            for (int i = 0; i < locData.Count; i++)
            {
                _linesById.Add(locData[i]["defaultText"].GetHashCode(), (string)locData[i]["locData"]);
            }
        }
    }
}
