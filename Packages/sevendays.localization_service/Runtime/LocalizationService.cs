namespace SevenDays.Localization
{
    public class LocalizationService
    {
        private LocalizationModel _localizationModel = new LocalizationModel(Language.Default);

        public void SetCurrentLanguage(Language language)
        {
            _localizationModel = new LocalizationModel(language);
        }
        
        public string GetLocalizedLine(string defaultLine)
        {
            return _localizationModel.Get(defaultLine);
        }
    }
}