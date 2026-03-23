namespace AutomationLearning.Core.Validators;

/// <summary>
/// Pure business logic for validating checkout form fields.
/// No browser dependency — easy to unit test and mutation test.
/// </summary>
public static class CheckoutValidator
{
    public static bool IsValidFirstName(string firstName) =>
        !string.IsNullOrWhiteSpace(firstName) && firstName.Trim().Length >= 2;

    public static bool IsValidLastName(string lastName) =>
        !string.IsNullOrWhiteSpace(lastName) && lastName.Trim().Length >= 2;

    public static bool IsValidPostalCode(string postalCode) =>
        !string.IsNullOrWhiteSpace(postalCode) && postalCode.Trim().Length >= 4;

    public static bool IsFormValid(string firstName, string lastName, string postalCode) =>
        IsValidFirstName(firstName) && IsValidLastName(lastName) && IsValidPostalCode(postalCode);

    public static string GetValidationError(string firstName, string lastName, string postalCode)
    {
        if (!IsValidFirstName(firstName)) return "First Name is required";
        if (!IsValidLastName(lastName))   return "Last Name is required";
        if (!IsValidPostalCode(postalCode)) return "Postal Code is required";
        return string.Empty;
    }
}
