using CRUD_Operations.Services;
using System.ComponentModel.DataAnnotations;

public class LocalizedRequiredAttribute : RequiredAttribute
{
    protected override ValidationResult IsValid ( object value , ValidationContext validationContext )
    {
        var localizationService = (LocalizationService)validationContext
            .GetService ( typeof ( LocalizationService ) );

        if (!IsValid ( value ))
        {
            var displayName = validationContext.DisplayName ?? validationContext.MemberName;
            return new ValidationResult (
                localizationService.GetLocalizedString ( "RequiredField" , displayName ) );
        }

        return ValidationResult.Success;
    }
}