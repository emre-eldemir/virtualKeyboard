# Contributing to Virtual Keyboard & Password Manager

First off, thank you for considering contributing to Virtual Keyboard & Password Manager! 🎉

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Pull Request Process](#pull-request-process)
- [Coding Guidelines](#coding-guidelines)
- [Reporting Bugs](#reporting-bugs)
- [Suggesting Enhancements](#suggesting-enhancements)

## Code of Conduct

This project and everyone participating in it is governed by respect and professionalism. Please be kind and constructive in your interactions.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce** the issue
- **Expected behavior** vs actual behavior
- **Screenshots** if applicable
- **Environment details** (Windows version, .NET version)

Example:
```markdown
**Bug**: Password not sent when special characters are present

**Steps to Reproduce**:
1. Save a password with `@` symbol
2. Try to send it to Notepad
3. Observe that `@` is not typed

**Expected**: Password should include `@` character
**Actual**: `@` character is skipped

**Environment**: Windows 11, .NET 6.0
```

### Suggesting Enhancements

Enhancement suggestions are welcome! Please include:

- **Clear use case**: Why is this enhancement needed?
- **Proposed solution**: How would it work?
- **Alternatives considered**: What other approaches did you think about?

### Your First Code Contribution

Unsure where to begin? Look for issues labeled:
- `good first issue` - Good for newcomers
- `help wanted` - Extra attention needed

## Development Setup

1. **Prerequisites**
   - Visual Studio 2022 or VS Code
   - .NET 6.0 SDK or higher
   - Git

2. **Clone and Build**
   ```bash
   git clone https://github.com/yourusername/virtualKeyboard.git
   cd virtualKeyboard
   dotnet build
   ```

3. **Run Tests**
   ```bash
   dotnet run
   ```

4. **Make Changes**
   - Create a new branch: `git checkout -b feature/my-feature`
   - Make your changes
   - Test thoroughly

## Pull Request Process

1. **Update Documentation**
   - Update README.md if needed
   - Add comments to complex code
   - Update CHANGELOG.md

2. **Test Your Changes**
   - Build successfully: `dotnet build`
   - Run the application: `dotnet run`
   - Test all affected features
   - Test on different Windows versions if possible

3. **Commit Guidelines**
   - Use clear, descriptive commit messages
   - Reference issues: `Fixes #123`
   - Use present tense: "Add feature" not "Added feature"

   Examples:
   ```
   Add password encryption feature
   Fix Turkish character input issue
   Update README with installation instructions
   ```

4. **Submit PR**
   - Fill out the PR template
   - Link related issues
   - Request review

## Coding Guidelines

### C# Style

- Follow standard C# naming conventions
- Use meaningful variable names
- Add XML documentation for public methods
- Keep methods focused and small

Example:
```csharp
/// <summary>
/// Sends a single character to the active window using keyboard simulation.
/// </summary>
/// <param name="character">The character to send</param>
private void SendKey(char character)
{
    // Implementation
}
```

### File Organization

- Keep related code together
- One class per file (generally)
- Organize using regions for clarity

### Error Handling

- Handle exceptions gracefully
- Provide meaningful error messages
- Use status messages, not MessageBox

### Performance

- Minimize UI thread blocking
- Use async/await for long operations
- Keep character delays reasonable (50ms default)

## Reporting Bugs

### Security Issues

If you discover a security vulnerability, please email directly instead of opening a public issue.

### Bug Report Template

```markdown
**Description**
A clear description of the bug

**To Reproduce**
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '...'
3. See error

**Expected Behavior**
What you expected to happen

**Screenshots**
If applicable, add screenshots

**Environment**
- OS: [e.g. Windows 11]
- .NET Version: [e.g. .NET 6.0]
- Application Version: [e.g. 1.0.0]

**Additional Context**
Add any other context about the problem
```

## Suggesting Enhancements

### Enhancement Template

```markdown
**Is your feature request related to a problem?**
A clear description of the problem

**Describe the solution you'd like**
A clear description of what you want to happen

**Describe alternatives you've considered**
Other solutions you've thought about

**Additional context**
Screenshots, mockups, or examples
```

## Questions?

Feel free to open an issue with the `question` label.

---

Thank you for contributing! 🎉
