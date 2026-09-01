using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeName399.EnvironmentManager;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

public sealed class EnvItem
{
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string Label { get; }
    public string Category { get; }
    public string? DefaultValue { get; }
    public bool Secret { get; }
    public bool Required { get; }

    public EnvItem(
        string name,
        string label,
        string category,
        string? defaultValue = null,
        bool secret = false,
        bool required = true)
    {
        Name = name;
        OriginalName = name;
        Label = label;
        Category = category;
        DefaultValue = defaultValue;
        Secret = secret;
        Required = required;
    }
}

public sealed class MainForm : Form
{
    // Keep this list synchronized with CodeName399.Shared.Constants.EnvironmentVariableConstants.
    // Do not add unrelated application variables here.
    readonly List<EnvItem> items = new()
    {
        // Database
        new(
            "CODENAME399_MongoDatabaseSetting__LocalConnectionString",
            "MongoDB connection string",
            "Database",
            secret: true),

        // Authentication
        new(
            "CODENAME399_PasswordBasedAuthenticationConfig__SecretKey",
            "JWT secret key",
            "Authentication",
            secret: true),

        // Broker connectivity
        new("CODENAME399_Broker__ApiKey", "API key", "Broker", secret: true),
        new("CODENAME399_Broker__ClientCode", "Client code", "Broker"),
        new("CODENAME399_Broker__Pin", "PIN", "Broker", secret: true),
        new("CODENAME399_Broker__TotpSecret", "TOTP secret", "Broker", secret: true),
        new("CODENAME399_Broker__PublicIp", "Public IP", "Broker"),
        new("CODENAME399_Broker__RegisteredStaticIp", "Registered static IP", "Broker"),
        new(
            "CODENAME399_Broker__RequireRegisteredStaticIpForLiveTrading",
            "Require registered static IP for live trading",
            "Broker",
            "true"),

        // SMTP
        new("CODENAME399_SMTP_HOST", "SMTP host", "SMTP", "smtp.zoho.in"),
        new("CODENAME399_SMTP_PORT", "SMTP port", "SMTP", "587"),
        new("CODENAME399_SMTP_USER", "SMTP username", "SMTP"),
        new("CODENAME399_SMTP_PASSWORD", "SMTP password", "SMTP", secret: true),
        new("CODENAME399_SMTP_TO", "Notification recipient", "SMTP"),

        // Logging
        new("CODENAME399_LOG_DIRECTORY", "Log directory", "Logging")
    };

    readonly Dictionary<EnvItem, TextBox> nameFields = new();
    readonly Dictionary<EnvItem, TextBox> valueFields = new();
    readonly Dictionary<EnvItem, Label> status = new();

    readonly RichTextBox log = new();
    readonly Label summary = new();
    readonly Button setMissing = new();
    readonly Button saveAll = new();
    readonly Button refresh = new();

    public MainForm()
    {
        Text = "CodeName399 Environment Manager";
        Width = 1250;
        Height = 900;
        MinimumSize = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F);

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 112,
            ColumnCount = 1,
            Padding = new Padding(12)
        };

        header.Controls.Add(new Label
        {
            Text = "CodeName399 Environment Manager",
            Font = new Font(Font.FontFamily, 16, FontStyle.Bold),
            AutoSize = true
        });

        header.Controls.Add(new Label
        {
            Text = "Machine-level environment variables • only shared constants are managed • variable names are editable",
            AutoSize = true
        });

        summary.Text = "Loading...";
        summary.AutoSize = true;
        header.Controls.Add(summary);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            IsSplitterFixed = false,
            Padding = new Padding(10)
        };

        var listHost = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(0, 0, 8, 0)
        };

        var list = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 7,
            Padding = new Padding(4),
            GrowStyle = TableLayoutPanelGrowStyle.AddRows
        };

        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));

        string? lastCategory = null;

        list.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        AddHeader(list, "Variable name", 0);
        AddHeader(list, "Value", 1);
        AddHeader(list, "Status", 2);
        AddHeader(list, "Save", 3);
        AddHeader(list, "Delete", 4);
        AddHeader(list, "Show", 5);
        AddHeader(list, "Reset", 6);
        list.RowCount++;

        foreach (var item in items)
        {
            if (item.Category != lastCategory)
            {
                list.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                var cat = new Label
                {
                    Text = item.Category,
                    Font = new Font(Font, FontStyle.Bold),
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(3, 7, 0, 0)
                };
                list.Controls.Add(cat, 0, list.RowCount);
                list.SetColumnSpan(cat, 7);
                list.RowCount++;
                lastCategory = item.Category;
            }

            var row = list.RowCount;

            var nameBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 4, 3, 4),
                Text = item.Name,
                Tag = item
            };

            var valueBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 4, 3, 4),
                UseSystemPasswordChar = item.Secret,
                Tag = item
            };

            var st = new Label
            {
                Text = "...",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 8, 4, 4)
            };

            nameFields[item] = nameBox;
            valueFields[item] = valueBox;
            status[item] = st;

            var save = new Button
            {
                Text = "Save",
                Dock = DockStyle.Fill,
                Height = 30,
                Tag = item,
                Margin = new Padding(3)
            };
            save.Click += async (_, _) => await SaveOneAsync(item);

            var del = new Button
            {
                Text = "Delete",
                Dock = DockStyle.Fill,
                Height = 30,
                Tag = item,
                Margin = new Padding(3)
            };
            del.Click += async (_, _) => await DeleteOneAsync(item);

            var reveal = new CheckBox
            {
                Text = "Show",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 6, 2, 3),
                Enabled = item.Secret
            };
            reveal.CheckedChanged += (_, _) =>
            {
                if (item.Secret)
                    valueBox.UseSystemPasswordChar = !reveal.Checked;
            };

            var reset = new Button
            {
                Text = "Reset",
                Dock = DockStyle.Fill,
                Height = 30,
                Margin = new Padding(3)
            };
            reset.Click += (_, _) =>
            {
                nameBox.Text = item.OriginalName;
                valueBox.Text = item.DefaultValue ?? "";
            };

            list.Controls.Add(nameBox, 0, row);
            list.Controls.Add(valueBox, 1, row);
            list.Controls.Add(st, 2, row);
            list.Controls.Add(save, 3, row);
            list.Controls.Add(del, 4, row);
            list.Controls.Add(reveal, 5, row);
            list.Controls.Add(reset, 6, row);
            list.RowCount++;
        }

        listHost.Controls.Add(list);
        split.Panel1.Controls.Add(listHost);

        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(8, 0, 0, 0)
        };

        right.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = true,
            AutoScroll = true,
            Padding = new Padding(0, 4, 0, 4)
        };

        ConfigureButton(setMissing, "Set Missing", async (_, _) => await SetMissingAsync());
        ConfigureButton(saveAll, "Save All", async (_, _) => await SaveAllAsync());
        ConfigureButton(refresh, "Refresh", async (_, _) => await RefreshAsync());

        actions.Controls.Add(setMissing);
        actions.Controls.Add(saveAll);
        actions.Controls.Add(refresh);

        right.Controls.Add(actions, 0, 0);

        log.Dock = DockStyle.Fill;
        log.ReadOnly = true;
        log.BackColor = Color.FromArgb(20, 22, 26);
        log.ForeColor = Color.Gainsboro;
        log.Font = new Font("Consolas", 9);
        right.Controls.Add(log, 0, 1);

        split.Panel2.Controls.Add(right);
        Controls.Add(split);
        Controls.Add(header);

        void SetSafeSplitterDistance()
        {
            if (split.ClientSize.Width <= 0) return;

            const int panel1Min = 700;
            const int panel2Min = 300;

            if (split.ClientSize.Width < panel1Min + panel2Min + split.SplitterWidth)
                return;

            split.Panel1MinSize = panel1Min;
            split.Panel2MinSize = panel2Min;

            var usable = split.ClientSize.Width;
            var max = usable - split.Panel2MinSize - split.SplitterWidth;
            var distance = Math.Clamp((int)(usable * 0.62), split.Panel1MinSize, max);

            if (distance >= split.Panel1MinSize && distance <= max)
                split.SplitterDistance = distance;
        }

        Shown += (_, _) => BeginInvoke(SetSafeSplitterDistance);

        SizeChanged += (_, _) =>
        {
            if (IsHandleCreated)
                BeginInvoke(SetSafeSplitterDistance);
        };

        Shown += async (_, _) => await RefreshAsync();
    }

    static void AddHeader(TableLayoutPanel list, string text, int column)
    {
        list.Controls.Add(
            new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Padding = new Padding(4, 8, 0, 0)
            },
            column,
            list.RowCount);
    }

    static void ConfigureButton(Button b, string text, EventHandler h)
    {
        b.Text = text;
        b.AutoSize = true;
        b.Height = 34;
        b.MinimumSize = new Size(105, 34);
        b.Click += h;
    }

    string? GetMachine(string name) =>
        Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

    void SetMachine(string name, string value) =>
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);

    void RemoveMachine(string name) =>
        Environment.SetEnvironmentVariable(name, null, EnvironmentVariableTarget.Machine);

    void WriteLog(string message, bool error = false)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => WriteLog(message, error));
            return;
        }

        log.AppendText($"{DateTime.Now:HH:mm:ss} {(error ? "ERROR " : "")}{message}{Environment.NewLine}");
        log.SelectionStart = log.TextLength;
        log.ScrollToCaret();
    }

    async Task SaveOneAsync(EnvItem item)
    {
        try
        {
            var newName = nameFields[item].Text.Trim();
            var value = valueFields[item].Text.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show(
                    "Enter an environment variable name.",
                    "Save Variable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (item.Required && string.IsNullOrWhiteSpace(value))
            {
                WriteLog($"SKIP    {newName} - empty required value", true);
                MessageBox.Show(
                    $"Enter a value for {item.Label}.",
                    "Save Variable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // If the name was changed, move the existing machine-level variable
            // rather than leaving the old variable behind.
            if (!string.Equals(item.OriginalName, newName, StringComparison.Ordinal))
            {
                var oldValue = GetMachine(item.OriginalName);

                if (string.IsNullOrWhiteSpace(value) && oldValue != null)
                    value = oldValue;

                SetMachine(newName, value);

                if (!string.Equals(item.OriginalName, newName, StringComparison.Ordinal))
                    RemoveMachine(item.OriginalName);

                WriteLog($"RENAMED {item.OriginalName} -> {newName}");
                item.Name = newName;
                item.OriginalName = newName;
            }
            else
            {
                SetMachine(newName, value);
                WriteLog($"SAVED   {newName}");
            }

            await RefreshAsync();
        }
        catch (Exception ex)
        {
            WriteLog($"ERROR   {item.Label}: {ex.Message}", true);
            MessageBox.Show(
                ex.Message,
                "Save Variable Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    async Task DeleteOneAsync(EnvItem item)
    {
        var name = nameFields[item].Text.Trim();

        if (MessageBox.Show(
                $"Delete the machine-level environment variable '{name}'?\n\nThis cannot be undone from the application.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            RemoveMachine(name);

            if (string.Equals(name, item.OriginalName, StringComparison.Ordinal))
                item.Name = item.OriginalName;

            WriteLog($"DELETED {name}");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            WriteLog($"ERROR   deleting {name}: {ex.Message}", true);
            MessageBox.Show(
                ex.Message,
                "Delete Variable Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    async Task RefreshAsync()
    {
        await Task.Run(() =>
        {
            foreach (var item in items)
            {
                var name = item.Name;
                var value = GetMachine(name);

                BeginInvoke(() =>
                {
                    nameFields[item].Text = name;
                    valueFields[item].Text = value ?? item.DefaultValue ?? "";
                    status[item].Text =
                        string.IsNullOrWhiteSpace(value)
                            ? (item.DefaultValue == null ? "MISSING" : "DEFAULT")
                            : "EXISTS";
                });
            }
        });

        var missing = items.Count(i =>
            string.IsNullOrWhiteSpace(GetMachine(i.Name)) &&
            i.DefaultValue == null);

        var defaults = items.Count(i =>
            string.IsNullOrWhiteSpace(GetMachine(i.Name)) &&
            i.DefaultValue != null);

        summary.Text =
            $"{items.Count} variables • {missing} missing required inputs • {defaults} missing with defaults";

        WriteLog("Environment status refreshed.");
    }

    async Task SetMissingAsync()
    {
        setMissing.Enabled = saveAll.Enabled = refresh.Enabled = false;

        try
        {
            foreach (var item in items)
            {
                var name = nameFields[item].Text.Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    WriteLog($"MISSING {item.Label} - enter a variable name", true);
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(GetMachine(name)))
                {
                    WriteLog($"EXISTS  {name}");
                    continue;
                }

                var value = valueFields[item].Text.Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (item.DefaultValue != null)
                        value = item.DefaultValue;
                    else
                    {
                        WriteLog($"MISSING {name} - enter a value", true);
                        continue;
                    }
                }

                SetMachine(name, value);
                item.Name = name;
                item.OriginalName = name;
                WriteLog($"SET     {name}");
            }

            MessageBox.Show(
                "Missing environment variables were set. Existing machine values were preserved.",
                "Environment Setup",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            WriteLog("ERROR: " + ex.Message, true);
            MessageBox.Show(
                ex.Message,
                "Environment Setup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            setMissing.Enabled = saveAll.Enabled = refresh.Enabled = true;
            await RefreshAsync();
        }
    }

    async Task SaveAllAsync()
    {
        if (MessageBox.Show(
                "Save all visible names and values to machine-level environment variables? Existing values will be overwritten and renamed variables will replace their previous names.",
                "Confirm Save All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        setMissing.Enabled = saveAll.Enabled = refresh.Enabled = false;

        try
        {
            foreach (var item in items)
            {
                var newName = nameFields[item].Text.Trim();
                var value = valueFields[item].Text.Trim();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    WriteLog($"SKIP    {item.Label} - empty variable name", true);
                    continue;
                }

                if (item.Required && string.IsNullOrWhiteSpace(value))
                {
                    WriteLog($"SKIP    {newName} - empty required value", true);
                    continue;
                }

                if (!string.Equals(item.OriginalName, newName, StringComparison.Ordinal))
                {
                    SetMachine(newName, value);
                    RemoveMachine(item.OriginalName);
                    WriteLog($"RENAMED {item.OriginalName} -> {newName}");

                    item.Name = newName;
                    item.OriginalName = newName;
                }
                else
                {
                    SetMachine(newName, value);
                    WriteLog($"SAVED   {newName}");
                }
            }

            MessageBox.Show(
                "Environment variables saved.",
                "Environment Setup",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            WriteLog("ERROR: " + ex.Message, true);
            MessageBox.Show(
                ex.Message,
                "Save Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            setMissing.Enabled = saveAll.Enabled = refresh.Enabled = true;
            await RefreshAsync();
        }
    }
}
