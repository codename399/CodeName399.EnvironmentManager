using System.Diagnostics;

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

public sealed record EnvItem(
    string Name,
    string Label,
    string Category,
    string? DefaultValue = null,
    bool Secret = false,
    bool Required = true);

public sealed class MainForm : Form
{
    // The new architecture keeps runtime configuration in each application's
    // appsettings/configuration collection. The Environment Manager is therefore
    // intentionally limited to the one machine-level secret explicitly requested.
    readonly List<EnvItem> items = new()
    {
        new("OPENAI_API_KEY", "OpenAI API key", "OpenAI", Secret: true)
    };

    readonly Dictionary<string, TextBox> fields = new();
    readonly Dictionary<string, Label> status = new();
    readonly RichTextBox log = new();
    readonly Label summary = new();
    readonly Button setMissing = new(), saveAll = new(), refresh = new();

    public MainForm()
    {
        Text = "CodeName399 Environment Manager";
        Width = 1100;
        Height = 650;
        MinimumSize = new Size(850, 550);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F);

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 92,
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
            Text = "Machine-level environment variables • only the OpenAI API key is managed here",
            AutoSize = true
        });

        summary.Text = "Loading...";
        summary.AutoSize = true;
        header.Controls.Add(summary);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
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
            ColumnCount = 6,
            Padding = new Padding(4),
            GrowStyle = TableLayoutPanelGrowStyle.AddRows
        };

        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86));

        string? lastCategory = null;

        list.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        list.Controls.Add(new Label
        {
            Text = "Variable",
            Font = new Font(Font, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(4, 8, 0, 0)
        }, 0, list.RowCount);
        list.Controls.Add(new Label
        {
            Text = "Value",
            Font = new Font(Font, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(4, 8, 0, 0)
        }, 1, list.RowCount);
        list.Controls.Add(new Label
        {
            Text = "Status",
            Font = new Font(Font, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(4, 8, 0, 0)
        }, 2, list.RowCount);
        list.Controls.Add(new Label
        {
            Text = "Save",
            Font = new Font(Font, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(4, 8, 0, 0)
        }, 3, list.RowCount);
        list.Controls.Add(new Label
        {
            Text = "Delete",
            Font = new Font(Font, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(4, 8, 0, 0)
        }, 4, list.RowCount);
        list.Controls.Add(new Label
        {
            Text = "Show",
            Font = new Font(Font, FontStyle.Bold),
            Dock = DockStyle.Fill,
            Padding = new Padding(4, 8, 0, 0)
        }, 5, list.RowCount);
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
                list.SetColumnSpan(cat, 6);
                list.RowCount++;
                lastCategory = item.Category;
            }

            var row = list.RowCount;

            var label = new Label
            {
                Text = item.Label,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 8, 4, 4)
            };

            var tb = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 4, 3, 4),
                UseSystemPasswordChar = item.Secret,
                Tag = item
            };

            fields[item.Name] = tb;

            var st = new Label
            {
                Text = "...",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 8, 4, 4)
            };

            status[item.Name] = st;

            var save = new Button
            {
                Text = "Save",
                Dock = DockStyle.Fill,
                Height = 30,
                Tag = item,
                Margin = new Padding(3)
            };

            var del = new Button
            {
                Text = "Delete",
                Dock = DockStyle.Fill,
                Height = 30,
                Tag = item,
                Margin = new Padding(3)
            };

            save.Click += async (_, _) => await SaveOneAsync(item);
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
                    tb.UseSystemPasswordChar = !reveal.Checked;
            };

            list.Controls.Add(label, 0, row);
            list.Controls.Add(tb, 1, row);
            list.Controls.Add(st, 2, row);
            list.Controls.Add(save, 3, row);
            list.Controls.Add(del, 4, row);
            list.Controls.Add(reveal, 5, row);
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
            if (split.ClientSize.Width <= 0)
                return;

            const int panel1Min = 500;
            const int panel2Min = 300;

            if (split.ClientSize.Width < panel1Min + panel2Min + split.SplitterWidth)
                return;

            split.Panel1MinSize = panel1Min;
            split.Panel2MinSize = panel2Min;

            var usable = split.ClientSize.Width;
            var max = usable - split.Panel2MinSize - split.SplitterWidth;
            var distance = Math.Clamp(
                (int)(usable * 0.62),
                split.Panel1MinSize,
                max);

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

    void ConfigureButton(Button button, string text, EventHandler handler)
    {
        button.Text = text;
        button.AutoSize = true;
        button.Height = 34;
        button.MinimumSize = new Size(105, 34);
        button.Click += handler;
    }

    string? GetMachine(string name) =>
        Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

    void SetMachine(string name, string value) =>
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);

    void WriteLog(string message, bool error = false)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => WriteLog(message, error));
            return;
        }

        log.AppendText(
            $"{DateTime.Now:HH:mm:ss} {(error ? "[ERROR] " : "")}{message}{Environment.NewLine}");

        log.SelectionStart = log.TextLength;
        log.ScrollToCaret();
    }

    async Task SaveOneAsync(EnvItem item)
    {
        try
        {
            var value = fields[item.Name].Text.Trim();

            if (item.Required && string.IsNullOrWhiteSpace(value))
            {
                WriteLog($"SKIP    {item.Name} - empty required value", true);
                MessageBox.Show(
                    $"Enter a value for {item.Label}.",
                    "Save Variable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            SetMachine(item.Name, value);
            WriteLog($"SAVED   {item.Name}");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            WriteLog($"ERROR   {item.Name}: {ex.Message}", true);
            MessageBox.Show(
                ex.Message,
                "Save Variable Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    async Task DeleteOneAsync(EnvItem item)
    {
        if (MessageBox.Show(
                $"Delete the machine-level environment variable '{item.Name}'?\n\nThis cannot be undone from the application.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            Environment.SetEnvironmentVariable(
                item.Name,
                null,
                EnvironmentVariableTarget.Machine);

            WriteLog($"DELETED {item.Name}");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            WriteLog($"ERROR   deleting {item.Name}: {ex.Message}", true);
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
                var value = GetMachine(item.Name);

                BeginInvoke(() =>
                {
                    fields[item.Name].Text = value ?? item.DefaultValue ?? "";
                    status[item.Name].Text =
                        string.IsNullOrWhiteSpace(value)
                            ? (item.DefaultValue == null ? "MISSING" : "DEFAULT")
                            : "EXISTS";
                });
            }
        });

        var missing = items.Count(
            i => string.IsNullOrWhiteSpace(GetMachine(i.Name)) &&
                 i.DefaultValue == null);

        var defaults = items.Count(
            i => string.IsNullOrWhiteSpace(GetMachine(i.Name)) &&
                 i.DefaultValue != null);

        summary.Text =
            $"{items.Count} variable • {missing} missing required inputs • {defaults} missing with defaults";

        WriteLog("Environment status refreshed.");
    }

    async Task SetMissingAsync()
    {
        setMissing.Enabled = saveAll.Enabled = refresh.Enabled = false;

        try
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(GetMachine(item.Name)))
                {
                    WriteLog($"EXISTS  {item.Name}");
                    continue;
                }

                var value = fields[item.Name].Text.Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (item.DefaultValue != null)
                        value = item.DefaultValue;
                    else
                    {
                        WriteLog($"MISSING {item.Name} - enter a value", true);
                        continue;
                    }
                }

                SetMachine(item.Name, value);
                WriteLog($"SET     {item.Name}");
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
                "Save the OpenAI API key to the machine-level environment variable? The existing value will be overwritten.",
                "Confirm Save All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        setMissing.Enabled = saveAll.Enabled = refresh.Enabled = false;

        try
        {
            foreach (var item in items)
            {
                var value = fields[item.Name].Text.Trim();

                if (item.Required && string.IsNullOrWhiteSpace(value))
                {
                    WriteLog($"SKIP    {item.Name} - empty required value", true);
                    continue;
                }

                SetMachine(item.Name, value);
                WriteLog($"SAVED   {item.Name}");
            }

            MessageBox.Show(
                "Environment variable saved.",
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
