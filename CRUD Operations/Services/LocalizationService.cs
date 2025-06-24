using Microsoft.Extensions.Localization;

namespace CRUD_Operations.Services
{
    public class LocalizationService
    {
        private readonly IStringLocalizer _localizer;

        public LocalizationService ( IStringLocalizerFactory factory )
        {
            _localizer = factory.Create ( "ApiMessages" , "CRUD_Operations" );
        }

        public string GetLocalizedString ( string key , params object[] args )
        {
            return string.Format ( _localizer[key] , args );
        }

        public Dictionary<string , string> GetLocalizedStrings ( )
        {
            return _localizer.GetAllStrings ()
                .ToDictionary ( x => x.Name , x => x.Value );
        }
    }
}