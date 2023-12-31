﻿using System;

namespace SevenDays.Localization
{
    public class LocalizationService
    {
        private LocalizationModel _localizationModel;

        public LocalizationService()
        {
            _localizationModel = new LocalizationModel(Language.Default);
        }

        public LocalizationService(Language language)
        {
            _localizationModel = new LocalizationModel(language);
        }

        [Obsolete("Use ctor instead")]
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