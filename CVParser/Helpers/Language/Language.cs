using System;
using System.Collections.Generic;
using System.Text;
using CVParser.Helpers.Language.Detector;

namespace CVParser.Helpers.Language
{
    public static class Language
    {
        public static string Detect(string[] lines)
        {
            LanguageDetector languageDetector = new LanguageDetector();
            return languageDetector.Detect(string.Join(string.Empty, lines));
        }
    }
}
