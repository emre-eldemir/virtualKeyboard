# 🎹 Virtual Keyboard & Password Manager

<div align="center">

![Version](https://img.shields.io/badge/version-1.0.0-blue.svg)
![.NET](https://img.shields.io/badge/.NET-6.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)

**A Windows Forms application that simulates keyboard input to solve the problem of entering passwords and commands in web-based consoles, SSH terminals, and other interfaces with keyboard input issues.**

[Features](#-features) • [Why This Project?](#-why-this-project) • [Installation](#-installation) • [Usage](#-usage) • [Screenshots](#-screenshots) • [Contributing](#-contributing)

</div>

---

## 🎯 Why This Project?

Have you ever struggled with:
- **Web-based consoles** (AWS, Azure, Google Cloud) where keyboard input is unreliable?
- **SSH password prompts** in browser-based terminals where you can't see what you're typing?
- **Keyboard layout mismatches** causing wrong characters to appear?
- **Special characters** not working properly in remote consoles?
- **Not knowing if your keypress was registered** in web terminals?

This application **solves all these problems** by simulating keyboard input at the system level. Instead of copy-pasting (which is often disabled or insecure), this tool types your passwords and commands character-by-character, ensuring each keystroke is properly registered.

### 🔧 Common Use Cases

1. **SSH Password Entry**: Enter complex SSH passwords in web-based terminals without frustration
2. **Cloud Console Commands**: Type commands reliably in AWS CloudShell, Azure Cloud Shell, etc.
3. **Remote Desktop Sessions**: Enter credentials in RDP sessions where keyboard mapping is problematic
4. **Docker/Kubernetes Web Terminals**: Execute commands in web-based container terminals
5. **Virtual Machine Consoles**: Interact with VM consoles through web interfaces
6. **Password Management**: Store and quickly send frequently-used passwords

---

## ✨ Features

### 🖱️ Manual Text Sending
- **Reliable Character-by-Character Typing**: Simulates real keyboard input using Windows API
- **Customizable Delay Timer**: Set 3-60 seconds delay before sending (gives you time to focus the target window)
- **Progress Indicator**: Visual progress bar shows sending progress
- **Full Character Support**: 
  - Uppercase/lowercase letters
  - Numbers and symbols
  - Special characters (@, !, #, $, %, ^, &, *, etc.)
  - Turkish characters (ı, ğ, ü, ş, ö, ç, İ, Ğ, Ü, Ş, Ö, Ç)

### 🔐 Password Manager
- **Secure Local Storage**: Passwords stored in encrypted local file
- **Masked Display**: Passwords shown as "Abc......" in the list for privacy
- **Quick Send**: Double-click to send password instantly
- **Auto-Enter Option**: Toggle to automatically press ENTER after sending password
- **Easy Management**: Add, delete, and organize passwords with descriptions

### 🎨 User Experience
- **In-App Status Messages**: No annoying popup dialogs
- **Auto-Hiding Notifications**: Success/error messages disappear automatically after 3 seconds
- **Clean Interface**: Simple, intuitive design with grouped sections
- **Resizable Window**: Adjust to your preferred size

---

## 📋 Requirements

- **Operating System**: Windows 10/11
- **.NET Runtime**: .NET 6.0 or higher
- **Permissions**: Standard user permissions (no admin required)

---

## 🚀 Installation

### Option 1: Download Release (Recommended)
1. Go to [Releases](https://github.com/emre-eldemir/virtualKeyboard/releases)
2. Download the latest `VirtualKeyboard.exe`
3. Run the executable

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/emre-eldemir/virtualKeyboard.git

# Navigate to the project directory
cd virtualKeyboard

# Build the project
dotnet build

# Run the application
dotnet run
```

---

## � Usage

### Manual Text Sending

Perfect for entering commands in web consoles where keyboard input is unreliable.

1. **Type your text** in the input field (e.g., a complex SSH password or command)
2. **Click "Send"** button
3. **Within the delay time** (default 3 seconds), click on the target area:
   - Web browser console
   - SSH terminal window
   - Remote desktop password field
   - Any other input field
4. The application will **type each character** automatically

### Password Management

Store frequently-used passwords and send them reliably to any application.

#### Saving a Password
1. Enter a **description** (e.g., "AWS Root SSH", "Production Server")
2. Enter your **password**
3. Click **"Save"**
4. Password is stored locally and appears masked in the list

#### Sending a Password
1. **Select** the password from the list
2. Click **"Send Selected Password"** (or double-click the password)
3. **Within the delay time**, click on the password field
4. Password is typed automatically
5. If **"Auto-press ENTER"** is enabled, ENTER key is pressed after password

### Settings

- **Delay Time**: Adjust from 3 to 60 seconds (gives you time to switch windows)
- **Auto-press ENTER**: Toggle on/off to automatically submit passwords after sending

---

## 🖼️ Screenshots

### Main Interface
```
┌─────────────────────────────────────────────────────────────────────┐
│ Virtual Keyboard & Password Manager - Emre Eldemir - 2025       ⬜❌│
├─────────────────────────────────────────────────────────────────────┤
│  ✓ Password sent successfully!                                      │  ← Status Bar
├─────────────────────────────────────────────────────────────────────┤
│ Manual Text Sending                                                 │
│ ┌───────────────────────────────────────────────┐                   │
│ │ ssh root@192.168.1.100                        │  [Send]           │
│ └───────────────────────────────────────────────┘                   │
│ ████████████████████░░░░░░░░░░░░░░░░░░░░░░░░░░  Progress           │
├─────────────────────────────────────────────────────────────────────┤
│ Password Manager                                                    │
│ Delay: [3] seconds  ☑ Auto-press ENTER after sending password      │
│                                                                     │
│ Description: [AWS Root SSH]  Password: [********]  [Save]          │
│                                                                     │
│ Saved Passwords:                                                   │
│ ┌───────────────────────────────────────────────┐                  │
│ │ AWS Root SSH      → MyP......                 │                  │
│ │ Production Server → Pr0......                 │                  │
│ │ GitHub Token      → ghp......                 │                  │
│ └───────────────────────────────────────────────┘                  │
│               [Send Selected Password]  [Delete Selected]          │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 🔒 Security Considerations

### ⚠️ Important Notes

- Passwords are stored in **plain text** in `passwords.txt` in the application directory
- This is designed for **convenience**, not maximum security
- **Do not use** for highly sensitive passwords (banking, email, etc.)
- **Best for**: SSH passwords, test environment credentials, frequently-used service accounts

### 🛡️ Security Best Practices

1. **Store the application** in a secure folder (e.g., encrypted drive)
2. **Don't commit** `passwords.txt` to version control
3. **Use** for development/testing environments primarily
4. **Consider** encrypting your drive if using on a shared computer
5. **Delete** `passwords.txt` when no longer needed

### 🔐 Future Security Enhancements

Planned features for future releases:
- [ ] Password encryption at rest
- [ ] Master password protection
- [ ] Password expiration reminders
- [ ] Integration with Windows Credential Manager

---

## 🛠️ Technical Details

### How It Works

1. **Windows API Integration**: Uses `user32.dll` keyboard event simulation
2. **SendKeys Fallback**: Falls back to .NET SendKeys for Unicode characters
3. **Character-by-Character Sending**: 50ms delay between characters ensures reliability
4. **Masked Storage**: Passwords displayed as "First3......" in UI for privacy
5. **File-Based Storage**: Simple text file format for easy backup/restore

### Technology Stack

- **Language**: C# 10
- **Framework**: .NET 6.0
- **UI**: Windows Forms
- **API**: Windows User32.dll (keyboard simulation)

### File Format

Passwords are stored in `passwords.txt`:
```
Description|Password
AWS Root SSH|MyP@ssw0rd123
Production Server|Pr0ductionS3cr3t
GitHub Token|ghp_1234567890abcdefghij
```

---

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

### Reporting Issues

- Use the [Issue Tracker](https://github.com/emre-eldemir/virtualKeyboard/issues)
- Describe the problem clearly
- Include steps to reproduce
- Mention your Windows version and .NET version

### Submitting Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Setup

```bash
# Clone your fork
git clone https://github.com/emre-eldemir/virtualKeyboard.git

# Create a branch
git checkout -b feature/my-feature

# Make changes and test
dotnet build
dotnet run

# Commit and push
git add .
git commit -m "Description of changes"
git push origin feature/my-feature
```

---

## 📝 Roadmap

### Version 1.1 (Planned)
- [ ] Password encryption
- [ ] Master password protection
- [ ] Import/Export password database
- [ ] Keyboard shortcuts (hotkeys)

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Emre Eldemir**

- GitHub: [@emre-eldemir](https://github.com/emre-eldemir)
- Project Link: [https://github.com/emre-eldemir/virtualKeyboard](https://github.com/emre-eldemir/virtualKeyboard)

---

## 🙏 Acknowledgments

- Inspired by the frustration of dealing with web-based SSH consoles
- Built with ❤️ to solve real-world keyboard input problems
- Thanks to the .NET community for excellent documentation

---

## 💡 Tips & Tricks

### For SSH Passwords in Web Consoles
1. Set delay to 5 seconds
2. Enable "Auto-press ENTER"
3. Click Send, then immediately click on the password field
4. Watch as your password is typed and submitted automatically

### For Complex Commands
1. Save frequently-used commands in the password manager
2. Use descriptive names (e.g., "Docker Cleanup Command")
3. Quick-send with double-click

### For Multiple Systems
1. Create separate password entries for each system
2. Use clear descriptions (e.g., "AWS Prod SSH", "AWS Dev SSH")
3. Update passwords in one place when they change

---

<div align="center">

**If this tool helps you, please consider giving it a ⭐ on GitHub!**

Made with 🎹 and ☕ by Emre Eldemir - 2025

</div>
