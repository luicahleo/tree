using System;

namespace TreeAPI.API.Mobile.General
{
    public class Locale
    {
        /// <summary>
        /// Converts a platform locale string to the Web side locale
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="localCode"></param>
        /// <returns></returns>
        public string MobileLocale(string platform, string localCode)
        {
            string retLocale = Comun.DefaultLocale;

            if (platform.Equals("android") || platform.Equals("Xamarin")) // android
            {
                // locale has the format xx_xx (with an underscore)
                switch (localCode)
                {
                    case "es_US": // Spanish americas es_US
                    case "es_ES": // Spanish es_ES
                        {
                            retLocale = "es-ES";
                        } break;
                    case "en_GB": // English en_GB
                    case "en_US": // English en_US
                        {
                            retLocale = "en-US";
                        } break;
                    case "fr_FR": // French fr_FR
                        {
                            retLocale = "fr-FR";
                        } break;
                    case "pt_BR":
                    case "pt_PT": // Portuguese pt_PT
                        {
                            retLocale = "pt-PT";
                        } break;
                    case "es_EC": // spanish equador es_EC
                        {
                            retLocale = "es-EC";
                        } break;
                    case "es_CL": // spanish chile es_CL
                        {
                            retLocale = "es-CL";
                        } break;
                    case "es_PA": // spanish panama es_PA
                        {
                            retLocale = "es-PA";
                        } break;
                    default:
                        {
                            // try and determine the langage from just two characters
                            string rootLanguageCode = localCode.Substring(0, Math.Min(localCode.Length, 2));

                            if (rootLanguageCode != null &&
                                rootLanguageCode.Length > 0)
                            {
                                retLocale = Comun.DefaultLocale;
                                
                                switch (rootLanguageCode)
                                {
                                    case "en":
                                        retLocale = "en-US";
                                        break;
                                    case "pt":
                                        retLocale = "pt-PT";
                                        break;
                                    case "es":
                                    default:
                                        retLocale = "es-ES";
                                        break;
                                }
                            }
                            else
                                retLocale = Comun.DefaultLocale;

                        } break;
                }

                return retLocale;
            }

            return retLocale;
        }
    }
}
