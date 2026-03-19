# AutomationLearning Project — Claude Instructions

## Project Overview
This is a learning project for practicing C# test automation with Playwright and API testing.

- **UI Tests target**: https://www.saucedemo.com (demo e-commerce site)
- **API Tests target**: https://reqres.in (public REST API mock)
- **Framework**: NUnit + Microsoft.Playwright
- **Language**: C# (.NET 10)

## Project Structure
```
AutomationLearning.Tests/
├── Config/         # TestSettings loaded from appsettings.json
├── Helpers/        # TestBase (UI) and ApiTestBase (API) — all tests inherit from these
├── Pages/          # Page Object Model — one class per page
│   ├── BasePage.cs
│   ├── LoginPage.cs
│   └── InventoryPage.cs
├── Tests/
│   ├── UI/         # Playwright UI tests
│   └── API/        # HttpClient API tests
└── appsettings.json
```

## Coding Conventions
- Page Objects go in `Pages/` and must inherit from `BasePage`
- UI tests must inherit from `TestBase` (which inherits `PageTest`)
- API tests must inherit from `ApiTestBase`
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

## Test Site Credentials (SauceDemo)
- Valid: `standard_user` / `secret_sauce`
- Locked out: `locked_out_user` / `secret_sauce`
- Problem user: `problem_user` / `secret_sauce`

## Common Tasks for Claude
- When asked to add a new page, create it in `Pages/` inheriting `BasePage`
- When asked to add tests, follow the existing pattern in `Tests/UI/` or `Tests/API/`
- When debugging a flaky test, first check for missing `await`, hardcoded waits, or fragile selectors
- Prefer `data-test` attributes over CSS classes or XPath
