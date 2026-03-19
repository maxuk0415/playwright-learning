using NUnit.Framework;

namespace AutomationLearning.Tests.Tests.UI;

/// <summary>
/// Centralised test data for checkout tests.
/// Separating data from test logic makes it easy to add new scenarios
/// without touching the test methods themselves.
/// </summary>
public static class CheckoutTestData
{
    /// <summary>
    /// Valid customer profiles used for happy-path checkout tests.
    /// Each TestCaseData entry becomes one separate test run.
    /// </summary>
    public static IEnumerable<TestCaseData> ValidCustomers()
    {
        yield return new TestCaseData("Max", "Chen", "12345")
            .SetName("Checkout_HappyPath_LocalCustomer");

        yield return new TestCaseData("John", "Doe", "90210")
            .SetName("Checkout_HappyPath_USCustomer");

        yield return new TestCaseData("Jane", "Smith", "EC1A1BB")
            .SetName("Checkout_HappyPath_UKCustomer");
    }

    /// <summary>
    /// Invalid input scenarios for checkout form validation.
    /// Parameters: firstName, lastName, postalCode, expectedErrorFragment
    /// </summary>
    public static IEnumerable<TestCaseData> InvalidCustomerInputs()
    {
        yield return new TestCaseData("", "Chen", "12345", "First Name")
            .SetName("Validation_EmptyFirstName");

        yield return new TestCaseData("Max", "", "12345", "Last Name")
            .SetName("Validation_EmptyLastName");

        yield return new TestCaseData("Max", "Chen", "", "Postal Code")
            .SetName("Validation_EmptyPostalCode");
    }
}
