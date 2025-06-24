using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CRUD_Operations.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;

        public BaseController ( IStringLocalizer localizer )
        {
            _localizer = localizer;
        }

        protected string Localize ( string key , params object[] args )
        {
            return string.Format ( _localizer[key] , args );
        }
    }
}