# AutomationLearning Project — Claude Instructions

## Project Overview
This is a learning project for practicing C# test automation with Playwright and API testing.

- **UI Tests target**: https://www.saucedemo.com (demo e-commerce site)
- **API Tests target**: https://reqres.in (public REST API mock)
- **Framework**: NUnit + Microsoft.Playwright
- **Language**: C# (.NET 10)

## Project Structure
```
AutomationLearning.Core/          ← Source library (Pages, Models, Config)
├── Config/
│   └── TestSettings.cs           # Loaded from appsettings.json in Tests
├── Models/
│   └── UserModels.cs             # Typed response models (records + JsonPropertyName)
├── Pages/                        # Page Object Model — one class per page
│   ├── BasePage.cs
│   ├── LoginPage.cs
│   ├── InventoryPage.cs
│   ├── CartPage.cs
│   ├── CheckoutInfoPage.cs
│   └── CheckoutCompletePage.cs
└── stryker-config.json           # Mutation testing config

AutomationLearning.Tests/         ← Test project (references Core)
├── Helpers/
│   ├── TestBase.cs               # UI test base (inherits PageTest)
│   └── ApiTestBase.cs            # API test base (HttpClient setup)
├── Tests/
│   ├── UI/                       # Playwright UI tests
│   └── API/                      # HttpClient API tests
├── appsettings.json              # Config: BaseUrl, ApiKey, credentials
└── allureConfig.json             # Allure report settings
```

## Why Two Projects?
`Core` has no NUnit/test dependencies — it only contains Pages, Models, and Config.
This lets mutation testing (Stryker) treat Core as the "source under test".
`Tests` has all NUnit, Playwright.NUnit, and test infrastructure.

## Coding Conventions
- Page Objects go in `Core/Pages/` and must inherit from `BasePage`
- UI tests must inherit from `TestBase` (in `Tests/Helpers/`, inherits `PageTest`)
- API tests must inherit from `ApiTestBase` (in `Tests/Helpers/`)
- Use `data-test` attributes as the preferred selector strategy for UI tests
- All test methods must be `async Task`
- Test descriptions should be written in plain English using `[Description]`
- Group related tests using `[Category]` attributes

## Running Tests
```bash
# Build and install Playwright browsers (first time only)
dotnet build
pwsh AutomationLearning.Tests/bin/Debug/net10.0/playwright.ps1 install

# Run all tests
dotnet test

# Run only UI tests
dotnet test --filter "Category=UI"

# Run only API tests
dotnet test --filter "Category=API"

# Run specific test class
dotnet test --filter "FullyQualifiedName~LoginTests"
```

## Mutation Testing (Stryker.NET)
Run from the `AutomationLearning.Core` directory:
```bash
cd AutomationLearning.Core
dotnet stryker --test-project ../AutomationLearning.Tests/AutomationLearning.Tests.csproj
```
The HTML report is saved to `AutomationLearning.Core/StrykerOutput/`.

## Test Site Credentials (SauceDemo)
- Valid: `standard_user` / `secret_sauce`
- Locked out: `locked_out_user` / `secret_sauce`
- Problem user: `problem_user` / `secret_sauce`

## Common Tasks for Claude
- When asked to add a new page, create it in `Core/Pages/` inheriting `BasePage`
- When asked to add tests, follow the existing pattern in `Tests/Tests/UI/` or `Tests/Tests/API/`
- When debugging a flaky test, first check for missing `await`, hardcoded waits, or fragile selectors
- Prefer `data-test` attributes over CSS classes or XPath
