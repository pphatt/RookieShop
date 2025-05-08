# Test Report Generation Guide

This README provides instructions for generating a code coverage test report for a .NET project using `dotnet test` and `ReportGenerator`.

## Prerequisites
- **.NET SDK**: Version 6.0 or later recommended.
- **NuGet Packages**: Ensure `coverlet.collector` is installed in your test project (`dotnet add package coverlet.collector`).
- **ReportGenerator**: Install globally with `dotnet tool install -g dotnet-reportgenerator-globaltool`.

## Setup
1. **Clone the Repository** (if applicable):
   ```bash
   cd <repository-directory>.YourProject.UnitTests
   ```
2. Ensure your test project is in a directory like `YourProject.UnitTests`.

## Generating the Test Report

### Step 1: Navigate to the Unit Tests Directory
Move to the directory containing your unit tests:
```bash
cd YourProject.UnitTests
```

### Step 2: Run Tests with Code Coverage
Run the tests and collect code coverage data:
```bash
dotnet test --collect:"XPlat Code Coverage"
```
This creates a `TestResults` directory with a `coverage.cobertura.xml` file.

#### Using Custom Settings (Optional)
To customize coverage (e.g., exclude specific files), use a `coverlet.runsettings` file:
```bash
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```
Example `coverlet.runsettings`:
```xml
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="XPlat Code Coverage">
        <Configuration>
          <ExcludeByFile>**/Program.cs,**/Startup.cs</ExcludeByFile>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

### Step 3: Generate the HTML Report
Use `ReportGenerator` to create a coverage report:
```bash
reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:"codecoverageresults" -reporttypes:Html
```
- The wildcard `*` accounts for the unique GUID in the `TestResults` path (e.g., `TestResults/966544d8-f490-4e9a-afe6-7323d4a8bffb`).
- The report is saved in the `codecoverageresults` directory.

### Step 4: View the Report
Open `codecoverageresults/index.html` in a web browser to view the code coverage results.

## Additional Notes
- **Dynamic Paths**: The `TestResults` directory includes a GUID that changes per run. Use wildcards or check the exact path manually if needed.
- **Local ReportGenerator**: If not installed globally, run `dotnet tool run reportgenerator` instead.
- **CI/CD Integration**: Add these commands to your CI/CD pipeline (e.g., GitHub Actions, Azure DevOps) for automated reporting.

## Troubleshooting
- **Missing `coverage.cobertura.xml`**:
    - Confirm the `TestResults` directory exists.
    - Verify the GUID folder name in the path.
- **Low Coverage**:
    - Check `coverlet.runsettings` for excessive exclusions.
    - Ensure all relevant code paths are tested.
- **ReportGenerator Not Found**:
    - Install it globally or use the local tool command.

## Resources
- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator Documentation](https://github.com/danielpalme/ReportGenerator)
- [.NET Testing Guide](https://learn.microsoft.com/en-us/dotnet/core/testing/)
