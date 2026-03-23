using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationLearning.Core.Validators;

namespace AutomationLearning.Tests.Tests.Unit;

[TestFixture]
[AllureNUnit]
[AllureSuite("Unit Tests")]
[AllureFeature("Checkout Validation")]
[Category("Unit")]
public class CheckoutValidatorTests
{
    // ── IsValidFirstName ────────────────────────────────────────────────────

    [Test]
    [Description("Empty first name should be invalid")]
    public void IsValidFirstName_Empty_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidFirstName(""), Is.False,
            "Empty string should not be a valid first name");
    }

    [Test]
    [Description("Whitespace-only first name should be invalid")]
    public void IsValidFirstName_Whitespace_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidFirstName("   "), Is.False,
            "Whitespace-only string should not be a valid first name");
    }

    [Test]
    [Description("Single character first name should be invalid (min length is 2)")]
    public void IsValidFirstName_SingleChar_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidFirstName("A"), Is.False,
            "Single character should not meet the minimum length of 2");
    }

    [Test]
    [Description("Two-character first name should be valid")]
    public void IsValidFirstName_TwoChars_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsValidFirstName("Jo"), Is.True,
            "Two characters should meet the minimum length of 2");
    }

    [Test]
    [Description("Normal first name should be valid")]
    public void IsValidFirstName_NormalName_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsValidFirstName("Max"), Is.True,
            "A normal first name should be valid");
    }

    // ── IsValidLastName ─────────────────────────────────────────────────────

    [Test]
    [Description("Empty last name should be invalid")]
    public void IsValidLastName_Empty_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidLastName(""), Is.False,
            "Empty string should not be a valid last name");
    }

    [Test]
    [Description("Single character last name should be invalid")]
    public void IsValidLastName_SingleChar_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidLastName("C"), Is.False,
            "Single character should not meet the minimum length of 2");
    }

    [Test]
    [Description("Two-character last name should be valid (boundary: min length is 2)")]
    public void IsValidLastName_TwoChars_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsValidLastName("Li"), Is.True,
            "Two characters should meet the minimum length of 2");
    }

    [Test]
    [Description("Normal last name should be valid")]
    public void IsValidLastName_NormalName_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsValidLastName("Chen"), Is.True,
            "A normal last name should be valid");
    }

    // ── IsValidPostalCode ───────────────────────────────────────────────────

    [Test]
    [Description("Empty postal code should be invalid")]
    public void IsValidPostalCode_Empty_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidPostalCode(""), Is.False,
            "Empty string should not be a valid postal code");
    }

    [Test]
    [Description("Three-character postal code should be invalid (min length is 4)")]
    public void IsValidPostalCode_ThreeChars_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsValidPostalCode("123"), Is.False,
            "Three characters should not meet the minimum length of 4");
    }

    [Test]
    [Description("Four-character postal code should be valid")]
    public void IsValidPostalCode_FourChars_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsValidPostalCode("1234"), Is.True,
            "Four characters should meet the minimum length of 4");
    }

    [Test]
    [Description("UK-style postal code should be valid")]
    public void IsValidPostalCode_UKFormat_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsValidPostalCode("EC1A1BB"), Is.True,
            "UK-style postal code should be valid");
    }

    // ── IsFormValid ─────────────────────────────────────────────────────────

    [Test]
    [Description("All valid fields should make the form valid")]
    public void IsFormValid_AllValidFields_ReturnsTrue()
    {
        Assert.That(CheckoutValidator.IsFormValid("Max", "Chen", "12345"), Is.True,
            "All valid inputs should result in a valid form");
    }

    [Test]
    [Description("Invalid first name makes entire form invalid")]
    public void IsFormValid_InvalidFirstName_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsFormValid("", "Chen", "12345"), Is.False,
            "Empty first name should invalidate the form");
    }

    [Test]
    [Description("Invalid last name makes entire form invalid")]
    public void IsFormValid_InvalidLastName_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsFormValid("Max", "", "12345"), Is.False,
            "Empty last name should invalidate the form");
    }

    [Test]
    [Description("Invalid postal code makes entire form invalid")]
    public void IsFormValid_InvalidPostalCode_ReturnsFalse()
    {
        Assert.That(CheckoutValidator.IsFormValid("Max", "Chen", "12"), Is.False,
            "Short postal code should invalidate the form");
    }

    // ── GetValidationError ──────────────────────────────────────────────────

    [Test]
    [Description("All valid inputs should return empty error string")]
    public void GetValidationError_AllValid_ReturnsEmpty()
    {
        var error = CheckoutValidator.GetValidationError("Max", "Chen", "12345");
        Assert.That(error, Is.Empty,
            "No error should be returned when all fields are valid");
    }

    [Test]
    [Description("Invalid first name should return first-name-specific error message")]
    public void GetValidationError_EmptyFirstName_ReturnsFirstNameError()
    {
        var error = CheckoutValidator.GetValidationError("", "Chen", "12345");
        Assert.That(error, Does.Contain("First Name"),
            "Error should mention First Name when first name is missing");
    }

    [Test]
    [Description("Invalid last name should return last-name-specific error message")]
    public void GetValidationError_EmptyLastName_ReturnsLastNameError()
    {
        var error = CheckoutValidator.GetValidationError("Max", "", "12345");
        Assert.That(error, Does.Contain("Last Name"),
            "Error should mention Last Name when last name is missing");
    }

    [Test]
    [Description("Invalid postal code should return postal-code-specific error message")]
    public void GetValidationError_EmptyPostalCode_ReturnsPostalCodeError()
    {
        var error = CheckoutValidator.GetValidationError("Max", "Chen", "12");
        Assert.That(error, Does.Contain("Postal Code"),
            "Error should mention Postal Code when postal code is too short");
    }

    [Test]
    [Description("First name error takes priority over last name error")]
    public void GetValidationError_BothNamesInvalid_ReturnsFirstNameErrorFirst()
    {
        var error = CheckoutValidator.GetValidationError("", "", "12345");
        Assert.That(error, Does.Contain("First Name"),
            "First Name validation should be reported first");
        Assert.That(error, Does.Not.Contain("Last Name"),
            "Only one error should be reported at a time");
    }
}
