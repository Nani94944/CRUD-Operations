using CRUD_Operations.Services;
using Microsoft.AspNetCore.Mvc;

public class BaseController : ControllerBase
{
    private readonly LocalizationService _localization;

    public BaseController ( LocalizationService localization )
    {
        _localization = localization;
    }

    protected IActionResult LocalizedOk ( string messageKey , object data = null )
    {
        return Ok ( new
        {
            Success = true ,
            Message = _localization.GetLocalizedString ( messageKey ) ,
            Data = data
        } );
    }

    protected IActionResult LocalizedBadRequest ( string messageKey , object errors = null )
    {
        return BadRequest ( new
        {
            Success = false ,
            Message = _localization.GetLocalizedString ( messageKey ) ,
            Errors = errors
        } );
    }
}