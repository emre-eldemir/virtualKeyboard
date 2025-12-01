using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace VirtualKeyboard
{
    public partial class MainForm : Form
    {
        // Windows API declarations
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const string PasswordFile = "passwords.txt";

        // Controls - Manual Sending Section
        private TextBox inputTextBox = null!;
        private Button sendButton = null!;
        private Label instructionLabel = null!;
        private ProgressBar progressBar = null!;

        // Controls - Password Management Section
        private TextBox passwordTextBox = null!;
        private TextBox descriptionTextBox = null!;
        private TextBox usernameTextBox = null!;
        private ComboBox separatorComboBox = null!;
        private NumericUpDown speedNumericUpDown = null!;
        private Button savePasswordButton = null!;
        private DataGridView passwordDataGridView = null!;
        private Button sendPasswordButton = null!;
        private Button deletePasswordButton = null!;
        private GroupBox manualGroupBox = null!;
        private GroupBox passwordGroupBox = null!;
        private NumericUpDown delayNumericUpDown = null!;
        private Label delayLabel = null!;
        private CheckBox autoEnterCheckBox = null!;
        
        // Status messages
        private Label statusLabel = null!;
        private System.Windows.Forms.Timer statusTimer = null!;

        public MainForm()
        {
            InitializeComponent();
            LoadPasswords();
        }

        private void InitializeComponent()
        {
            this.Text = "Virtual Keyboard & Password Manager / Emre Eldemir - 2025";
            this.Size = new Size(900, 770);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(900, 770);

            // Status Label (at the top)
            statusLabel = new Label
            {
                Location = new Point(20, 5),
                Size = new Size(840, 30),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.Green,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(240, 255, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            this.Controls.Add(statusLabel);

            // Status Timer
            statusTimer = new System.Windows.Forms.Timer();
            statusTimer.Interval = 3000; // 3 seconds
            statusTimer.Tick += (s, e) =>
            {
                statusLabel.Visible = false;
                statusTimer.Stop();
            };

            // ====== MANUAL SENDING SECTION ======
            manualGroupBox = new GroupBox
            {
                Text = "Manual Text Sending",
                Location = new Point(20, 45),
                Size = new Size(840, 180),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(manualGroupBox);

            // Label
            instructionLabel = new Label
            {
                Text = "Enter the text you want to send:",
                Location = new Point(15, 30),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9F)
            };
            manualGroupBox.Controls.Add(instructionLabel);

            // TextBox
            inputTextBox = new TextBox
            {
                Location = new Point(15, 55),
                Size = new Size(650, 30),
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Type your text here..."
            };
            manualGroupBox.Controls.Add(inputTextBox);

            // Button
            sendButton = new Button
            {
                Text = "Send",
                Location = new Point(680, 53),
                Size = new Size(140, 35),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Click += SendButton_Click;
            manualGroupBox.Controls.Add(sendButton);

            // ProgressBar
            progressBar = new ProgressBar
            {
                Location = new Point(15, 100),
                Size = new Size(805, 20),
                Visible = false
            };
            manualGroupBox.Controls.Add(progressBar);

            // Info Label
            Label infoLabel = new Label
            {
                Text = "NOTE: After pressing the button, click on the target area within the delay time!",
                Location = new Point(15, 130),
                Size = new Size(805, 35),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.DarkSlateGray
            };
            manualGroupBox.Controls.Add(infoLabel);

            // ====== PASSWORD MANAGEMENT SECTION ======
            passwordGroupBox = new GroupBox
            {
                Text = "Password Manager",
                Location = new Point(20, 235),
                Size = new Size(840, 480),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(passwordGroupBox);

            // Delay Time Label
            delayLabel = new Label
            {
                Text = "Delay Time (seconds):",
                Location = new Point(15, 30),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9F)
            };
            passwordGroupBox.Controls.Add(delayLabel);

            // Delay Time NumericUpDown
            delayNumericUpDown = new NumericUpDown
            {
                Location = new Point(200, 28),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 10F),
                Minimum = 3,
                Maximum = 60,
                Value = 3,
                DecimalPlaces = 0
            };
            passwordGroupBox.Controls.Add(delayNumericUpDown);

            // Auto-Enter CheckBox
            autoEnterCheckBox = new CheckBox
            {
                Text = "Auto-press ENTER after sending password",
                Location = new Point(300, 28),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 9F),
                Checked = false
            };
            passwordGroupBox.Controls.Add(autoEnterCheckBox);

            // Description Label
            Label descLabel = new Label
            {
                Text = "Description:",
                Location = new Point(15, 65),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9F)
            };
            passwordGroupBox.Controls.Add(descLabel);

            // Description TextBox
            descriptionTextBox = new TextBox
            {
                Location = new Point(15, 90),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "e.g.: Server 1, etc..."
            };
            passwordGroupBox.Controls.Add(descriptionTextBox);

            // Username Label (NEW)
            Label userLabel = new Label
            {
                Text = "Username (Optional):",
                Location = new Point(15, 125),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9F)
            };
            passwordGroupBox.Controls.Add(userLabel);

            // Username TextBox (NEW)
            usernameTextBox = new TextBox
            {
                Location = new Point(15, 150),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Enter username..."
            };
            passwordGroupBox.Controls.Add(usernameTextBox);

            // Separator Label (NEW)
            Label sepLabel = new Label
            {
                Text = "Separator:",
                Location = new Point(380, 125),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9F)
            };
            passwordGroupBox.Controls.Add(sepLabel);

            // Separator ComboBox (NEW)
            separatorComboBox = new ComboBox
            {
                Location = new Point(380, 150),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            separatorComboBox.Items.AddRange(new object[] { "TAB", "ENTER" });
            separatorComboBox.SelectedIndex = 0;
            passwordGroupBox.Controls.Add(separatorComboBox);

            // Speed Label (NEW)
            Label speedLabel = new Label
            {
                Text = "Speed (ms):",
                Location = new Point(500, 125),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9F)
            };
            passwordGroupBox.Controls.Add(speedLabel);

            // Speed NumericUpDown (NEW)
            speedNumericUpDown = new NumericUpDown
            {
                Location = new Point(500, 150),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 10F),
                Minimum = 1,
                Maximum = 1000,
                Value = 50,
                Increment = 10
            };
            passwordGroupBox.Controls.Add(speedNumericUpDown);

            // Password Label
            Label passLabel = new Label
            {
                Text = "Password:",
                Location = new Point(15, 185),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9F)
            };
            passwordGroupBox.Controls.Add(passLabel);

            // Password TextBox
            passwordTextBox = new TextBox
            {
                Location = new Point(15, 210),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Enter your password here...",
                UseSystemPasswordChar = true
            };
            passwordGroupBox.Controls.Add(passwordTextBox);

            // Save Button
            savePasswordButton = new Button
            {
                Text = "Save",
                Location = new Point(380, 208),
                Size = new Size(140, 35),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(16, 124, 16),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            savePasswordButton.FlatAppearance.BorderSize = 0;
            savePasswordButton.Click += SavePasswordButton_Click;
            passwordGroupBox.Controls.Add(savePasswordButton);

            // Saved Passwords Label
            Label savedLabel = new Label
            {
                Text = "Saved Passwords:",
                Location = new Point(15, 255),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            passwordGroupBox.Controls.Add(savedLabel);

            // Password DataGridView
            passwordDataGridView = new DataGridView
            {
                Location = new Point(15, 280),
                Size = new Size(650, 180),
                Font = new Font("Segoe UI", 9F),
                BackgroundColor = Color.FromArgb(245, 245, 245),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Add Columns
            passwordDataGridView.Columns.Add("Description", "Description");
            passwordDataGridView.Columns.Add("Username", "Username");
            passwordDataGridView.Columns.Add("Separator", "Separator");
            passwordDataGridView.Columns.Add("Password", "Password");
            passwordDataGridView.Columns.Add("Speed", "Speed (ms)");

            // Adjust column widths
            passwordDataGridView.Columns["Description"].FillWeight = 25;
            passwordDataGridView.Columns["Username"].FillWeight = 25;
            passwordDataGridView.Columns["Separator"].FillWeight = 15;
            passwordDataGridView.Columns["Password"].FillWeight = 20;
            passwordDataGridView.Columns["Speed"].FillWeight = 15;

            passwordDataGridView.CellDoubleClick += PasswordDataGridView_CellDoubleClick;
            passwordGroupBox.Controls.Add(passwordDataGridView);

            // Send Selected Button
            sendPasswordButton = new Button
            {
                Text = "Send Selected\nPassword",
                Location = new Point(680, 280),
                Size = new Size(140, 60),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            sendPasswordButton.FlatAppearance.BorderSize = 0;
            sendPasswordButton.Click += SendPasswordButton_Click;
            passwordGroupBox.Controls.Add(sendPasswordButton);

            // Delete Button
            deletePasswordButton = new Button
            {
                Text = "Delete Selected",
                Location = new Point(680, 350),
                Size = new Size(140, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(196, 43, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            deletePasswordButton.FlatAppearance.BorderSize = 0;
            deletePasswordButton.Click += DeletePasswordButton_Click;
            passwordGroupBox.Controls.Add(deletePasswordButton);

            // Info Label
            Label infoLabel2 = new Label
            {
                Text = "TIP: Double-click on a password in the list to send it quickly!",
                Location = new Point(15, 410),
                Size = new Size(650, 60),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.DarkSlateGray
            };
            passwordGroupBox.Controls.Add(infoLabel2);
        }

        private void ShowStatus(string message, bool isError = false)
        {
            statusLabel.Text = message;
            statusLabel.ForeColor = isError ? Color.Red : Color.Green;
            statusLabel.BackColor = isError ? Color.FromArgb(255, 240, 240) : Color.FromArgb(240, 255, 240);
            statusLabel.Visible = true;
            statusTimer.Stop();
            statusTimer.Start();
        }

        private async void SendButton_Click(object? sender, EventArgs e)
        {
            string text = inputTextBox.Text;

            if (string.IsNullOrEmpty(text))
            {
                ShowStatus("⚠ Please enter text to send!", true);
                return;
            }

            await SendTextToKeyboard(text, sendButton, progressBar);
        }

        private async void SendPasswordButton_Click(object? sender, EventArgs e)
        {
            if (passwordDataGridView.SelectedRows.Count == 0)
            {
                ShowStatus("⚠ Please select a password to send!", true);
                return;
            }

            var row = passwordDataGridView.SelectedRows[0];
            if (row.Tag is PasswordEntry entry)
            {
                // Send Username if present
                if (!string.IsNullOrEmpty(entry.Username))
                {
                    await SendTextToKeyboard(entry.Username, sendPasswordButton, null, entry.TypingSpeed);
                    await Task.Delay(100);
                    
                    // Send Separator
                    if (entry.Separator == "ENTER")
                        SendKey('\n');
                    else
                        SendKey('\t');
                        
                    await Task.Delay(100);
                }

                // Send Password
                await SendTextToKeyboard(entry.Password, sendPasswordButton, null, entry.TypingSpeed);
                
                // If auto-enter is enabled, send ENTER key
                if (autoEnterCheckBox.Checked)
                {
                    await Task.Delay(100); // Small delay before sending ENTER
                    SendKey('\n');
                    ShowStatus("✓ Password sent with ENTER!");
                }
            }
        }

        private void PasswordDataGridView_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SendPasswordButton_Click(sender, e);
            }
        }

        private async Task SendTextToKeyboard(string text, Button button, ProgressBar? progress, int typingDelay = 50)
        {
            button.Enabled = false;
            string originalText = button.Text;
            
            if (progress != null)
            {
                progress.Visible = true;
                progress.Maximum = text.Length;
                progress.Value = 0;
            }

            // Get delay time (minimum 3, maximum 60)
            int delaySeconds = Math.Max(3, Math.Min(60, (int)delayNumericUpDown.Value));

            // Wait for the specified delay time
            for (int i = delaySeconds; i > 0; i--)
            {
                button.Text = $"Waiting... {i}";
                await Task.Delay(1000);
            }

            button.Text = "Sending...";

            // Send each character in sequence
            await Task.Run(() =>
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    SendKey(c);
                    
                    // Update progress bar
                    if (progress != null)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            progress.Value = i + 1;
                        });
                    }

                    // Small delay between characters
                    Thread.Sleep(typingDelay);
                }
            });

            if (progress != null)
            {
                progress.Visible = false;
            }
            
            button.Text = originalText;
            button.Enabled = true;
        }

        private void SendKey(char character)
        {
            try
            {
                // More reliable character sending with SendKeys
                string charToSend = character.ToString();

                // Special characters need escaping
                if ("{}()[]+-^%~".Contains(character))
                {
                    charToSend = "{" + character + "}";
                }
                else if (character == '+')
                {
                    charToSend = "{+}";
                }
                else if (character == '^')
                {
                    charToSend = "{^}";
                }
                else if (character == '%')
                {
                    charToSend = "{%}";
                }
                else if (character == '~')
                {
                    charToSend = "{~}";
                }
                else if (character == '(')
                {
                    charToSend = "{(}";
                }
                else if (character == ')')
                {
                    charToSend = "{)}";
                }
                else if (character == '{')
                {
                    charToSend = "{{}";
                }
                else if (character == '}')
                {
                    charToSend = "{}}";
                }
                else if (character == '[')
                {
                    charToSend = "{[}";
                }
                else if (character == ']')
                {
                    charToSend = "{]}";
                }
                else if (character == '\n')
                {
                    charToSend = "{ENTER}";
                }
                else if (character == '\t')
                {
                    charToSend = "{TAB}";
                }

                SendKeys.SendWait(charToSend);
            }
            catch
            {
                // If SendKeys fails, use Windows API
                SendKeyWithAPI(character);
            }
        }

        private void SendKeyWithAPI(char character)
        {
            // Shift control for special characters
            bool needShift = char.IsUpper(character) || "!@#$%^&*()_+{}|:\"<>?".Contains(character);

            if (needShift)
            {
                keybd_event(0x10, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero); // Shift down
            }

            // Convert character to virtual key code
            byte vkCode = GetVirtualKeyCode(character);
            
            if (vkCode != 0)
            {
                keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
                Thread.Sleep(10);
                keybd_event(vkCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            }

            if (needShift)
            {
                keybd_event(0x10, 0, KEYEVENTF_KEYUP, UIntPtr.Zero); // Shift up
            }
        }

        private byte GetVirtualKeyCode(char character)
        {
            char upperChar = char.ToUpper(character);

            // Letters (English)
            if (upperChar >= 'A' && upperChar <= 'Z')
            {
                return (byte)upperChar;
            }

            // Numbers
            if (character >= '0' && character <= '9')
            {
                return (byte)character;
            }

            // Turkish characters and special characters
            return character switch
            {
                'ı' or 'I' => 0x49,
                'İ' or 'i' when character == 'İ' => 0x49,
                'ğ' or 'Ğ' => 0xDB,
                'ü' or 'Ü' => 0xDC,
                'ş' or 'Ş' => 0xBA,
                'ö' or 'Ö' => 0xDE,
                'ç' or 'Ç' => 0xC0,
                
                ' ' => 0x20,
                '\n' => 0x0D,
                '\t' => 0x09,
                '.' => 0xBE,
                ',' => 0xBC,
                ';' => 0xBA,
                '\'' => 0xDE,
                '[' => 0xDB,
                ']' => 0xDD,
                '\\' => 0xDC,
                '/' => 0xBF,
                '-' => 0xBD,
                '=' => 0xBB,
                '`' => 0xC0,
                
                '!' => 0x31,
                '@' => 0x32,
                '#' => 0x33,
                '$' => 0x34,
                '%' => 0x35,
                '^' => 0x36,
                '&' => 0x37,
                '*' => 0x38,
                '(' => 0x39,
                ')' => 0x30,
                
                _ => 0
            };
        }

        // ====== PASSWORD MANAGEMENT FUNCTIONS ======

        private void SavePasswordButton_Click(object? sender, EventArgs e)
        {
            string description = descriptionTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string separator = separatorComboBox.SelectedItem?.ToString() ?? "TAB";
            int speed = (int)speedNumericUpDown.Value;

            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(password))
            {
                ShowStatus("⚠ Please fill in description and password fields!", true);
                return;
            }

            // Save password to file
            // Format: Description|Username|Separator|Password|Speed
            string line = $"{description}|{username}|{separator}|{password}|{speed}";
            string encryptedLine = Encrypt(line);
            File.AppendAllText(PasswordFile, encryptedLine + Environment.NewLine);

            // Update DataGridView
            var entry = new PasswordEntry 
            { 
                Description = description, 
                Username = username, 
                Separator = separator, 
                Password = password,
                TypingSpeed = speed
            };
            
            int rowIndex = passwordDataGridView.Rows.Add(
                entry.Description, 
                entry.Username, 
                entry.Separator, 
                "******",
                entry.TypingSpeed
            );
            passwordDataGridView.Rows[rowIndex].Tag = entry;

            // Clear fields
            descriptionTextBox.Clear();
            passwordTextBox.Clear();
            usernameTextBox.Clear();
            separatorComboBox.SelectedIndex = 0;
            speedNumericUpDown.Value = 50;
            descriptionTextBox.Focus();

            ShowStatus("✓ Password saved successfully!");
        }

        private void DeletePasswordButton_Click(object? sender, EventArgs e)
        {
            if (passwordDataGridView.SelectedRows.Count == 0)
            {
                ShowStatus("⚠ Please select a password to delete!", true);
                return;
            }

            foreach (DataGridViewRow row in passwordDataGridView.SelectedRows)
            {
                passwordDataGridView.Rows.Remove(row);
            }

            // Rewrite the file
            SaveAllPasswords();

            ShowStatus("✓ Password deleted!");
        }

        private void LoadPasswords()
        {
            if (!File.Exists(PasswordFile))
            {
                return;
            }

            string[] lines = File.ReadAllLines(PasswordFile);
            bool migrationNeeded = false;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Try to decrypt the line
                string decryptedLine = Decrypt(line);
                
                // If the line was not encrypted (same as input) but should have been (doesn't look like base64), mark for migration
                if (line == decryptedLine && line.Contains("|"))
                {
                    migrationNeeded = true;
                }

                string[] parts = decryptedLine.Split('|');
                PasswordEntry? entry = null;

                if (parts.Length == 2)
                {
                    entry = new PasswordEntry 
                    { 
                        Description = parts[0], 
                        Password = parts[1] 
                    };
                }
                else if (parts.Length == 4)
                {
                    entry = new PasswordEntry 
                    { 
                        Description = parts[0], 
                        Username = parts[1],
                        Separator = parts[2],
                        Password = parts[3],
                        TypingSpeed = 50 // Default
                    };
                }
                else if (parts.Length == 5)
                {
                    int.TryParse(parts[4], out int speed);
                    entry = new PasswordEntry 
                    { 
                        Description = parts[0], 
                        Username = parts[1],
                        Separator = parts[2],
                        Password = parts[3],
                        TypingSpeed = speed > 0 ? speed : 50
                    };
                }

                if (entry != null)
                {
                    int rowIndex = passwordDataGridView.Rows.Add(
                        entry.Description, 
                        entry.Username, 
                        entry.Separator, 
                        "******",
                        entry.TypingSpeed
                    );
                    passwordDataGridView.Rows[rowIndex].Tag = entry;
                }
            }

            // If we found plain text passwords, rewrite the file encrypted
            if (migrationNeeded)
            {
                SaveAllPasswords();
            }
        }

        private void SaveAllPasswords()
        {
            List<string> lines = new List<string>();
            
            foreach (DataGridViewRow row in passwordDataGridView.Rows)
            {
                if (row.Tag is PasswordEntry entry)
                {
                    string line = $"{entry.Description}|{entry.Username}|{entry.Separator}|{entry.Password}|{entry.TypingSpeed}";
                    lines.Add(Encrypt(line));
                }
            }

            File.WriteAllLines(PasswordFile, lines);
        }

        // ====== ENCRYPTION HELPERS ======
        private static readonly byte[] EncryptionKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("VirtualKeyboard_Secret_Key_2025"));
        private static readonly byte[] EncryptionIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("VirtualKeyboard_IV"));

        private static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = EncryptionKey;
                    aes.IV = EncryptionIV;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            return Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return plainText;
            }
        }

        private static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return "";

            try
            {
                // Check if it's likely encrypted (Base64)
                // Simple check: if it contains '|' it might be plain text (legacy format)
                if (cipherText.Contains("|") && !IsBase64(cipherText))
                {
                    return cipherText;
                }

                byte[] cipherBytes;
                try
                {
                    cipherBytes = Convert.FromBase64String(cipherText);
                }
                catch
                {
                    // Not base64, assume plain text
                    return cipherText;
                }

                using (Aes aes = Aes.Create())
                {
                    aes.Key = EncryptionKey;
                    aes.IV = EncryptionIV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return cipherText;
            }
        }

        private static bool IsBase64(string s)
        {
            try
            {
                Convert.FromBase64String(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private class PasswordEntry
        {
            public string Description { get; set; } = "";
            public string Username { get; set; } = "";
            public string Separator { get; set; } = "TAB";
            public string Password { get; set; } = "";
            public int TypingSpeed { get; set; } = 50;
        }
    }
}
