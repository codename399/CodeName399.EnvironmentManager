using Microsoft.Win32;
using System.Diagnostics;
using System.Text;

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

public sealed record EnvItem(string Name, string Label, string Category, string? DefaultValue = null, bool Secret = false, bool Required = true);

public sealed class MainForm : Form
{
    readonly List<EnvItem> items = new()
    {
        new("CODENAME399_LOG_DIRECTORY", "Log directory", "General"),
        new("OPENAI_API_KEY", "OpenAI API key", "OpenAI", Secret:true),
        new("CODENAME399_MongoDatabaseSetting__LocalConnectionString", "MongoDB connection string", "MongoDB", Secret:true),
        new("CODENAME399_PasswordBasedAuthenticationConfig__SecretKey", "JWT secret key", "JWT", Secret:true),
        new("CODENAME399_AngelOne__ApiKey", "Angel One API key", "Angel One", Secret:true),
        new("CODENAME399_AngelOne__ClientCode", "Angel One client code", "Angel One"),
        new("CODENAME399_AngelOne__Pin", "Angel One PIN", "Angel One", Secret:true),
        new("CODENAME399_AngelOne__TotpSecret", "Angel One TOTP secret", "Angel One", Secret:true),
        new("CODENAME399_AngelOne__PublicIp", "Registered public IP", "Angel One"),
        new("CODENAME399_AngelOne__RequireRegisteredStaticIpForLiveTrading", "Require registered static IP", "Angel One", "true"),
        new("CODENAME399_Smtp__Host", "SMTP host", "Email / SMTP", "smtp.zoho.in"),
        new("CODENAME399_Smtp__Port", "SMTP port", "Email / SMTP", "587"),
        new("CODENAME399_Smtp__EnableSsl", "SMTP SSL", "Email / SMTP", "true"),
        new("CODENAME399_Smtp__Username", "SMTP username/email", "Email / SMTP"),
        new("CODENAME399_Smtp__Password", "SMTP password", "Email / SMTP", Secret:true),
        new("CODENAME399_Smtp__From", "SMTP from email", "Email / SMTP"),
        new("CODENAME399_EMAIL_TO", "Notification recipient email", "Email / SMTP"),
        new("CODENAME399_EMAIL_SERVICE_URL", "Email service URL", "Service URLs", "http://127.0.0.1:5112"),
        new("CODENAME399_KuberXServiceUrl", "KuberX service URL", "Service URLs", "http://127.0.0.1:5004/"),
        new("CODENAME399_InstrumentServiceUrl", "Instrument service URL", "Service URLs", "http://127.0.0.1:5101/"),
        new("CODENAME399_HistoricalCandlesServiceUrl", "Historical Candles service URL", "Service URLs", "http://127.0.0.1:5102/"),
        new("CODENAME399_EvaluationServiceUrl", "Evaluation service URL", "Service URLs", "http://127.0.0.1:5103/"),
        new("CODENAME399_BuyingServiceUrl", "Buying service URL", "Service URLs", "http://127.0.0.1:5104/"),
        new("CODENAME399_VirtualTradingServiceUrl", "Virtual Trading service URL", "Service URLs", "http://127.0.0.1:5105/"),
        new("CODENAME399_StockPerformanceServiceUrl", "Stock Performance service URL", "Service URLs", "http://127.0.0.1:5106/"),
        new("CODENAME399_OptimizationServiceUrl", "Optimization service URL", "Service URLs", "http://127.0.0.1:5107/"),
        new("CODENAME399_SellingServiceUrl", "Selling service URL", "Service URLs", "http://127.0.0.1:5108/"),
        new("CODENAME399_EquityTradingServiceUrl", "Equity Trading service URL", "Service URLs", "http://127.0.0.1:5109/"),
        new("CODENAME399_FutureTradingServiceUrl", "Future Trading service URL", "Service URLs", "http://127.0.0.1:5110/"),
        new("CODENAME399_OptionsTradingServiceUrl", "Options Trading service URL", "Service URLs", "http://127.0.0.1:5111/"),
        new("CODENAME399_Otp__Length", "OTP length", "OTP Authentication", "6"),
        new("CODENAME399_Otp__ExpirationMinutes", "OTP expiration (minutes)", "OTP Authentication", "10"),
        new("CODENAME399_Otp__ResendCooldownSeconds", "OTP resend cooldown (seconds)", "OTP Authentication", "60"),
        new("CODENAME399_Otp__MaxVerificationAttempts", "OTP max verification attempts", "OTP Authentication", "5"),
        new("CODENAME399_Otp__EmailServiceUrl", "OTP email service URL", "OTP Authentication", "http://127.0.0.1:5112"),
        new("CODENAME399_Otp__Sms__Enabled", "Enable SMS OTP", "OTP / SMS"),
        new("CODENAME399_Otp__Sms__AccountSid", "Twilio account SID", "OTP / SMS"),
        new("CODENAME399_Otp__Sms__AuthToken", "Twilio auth token", "OTP / SMS", Secret:true),
        new("CODENAME399_Otp__Sms__FromNumber", "Twilio sender phone number", "OTP / SMS"),
        new("CODENAME399_OAuth__Google__Enabled", "Enable Google OAuth", "OAuth / Google"),
        new("CODENAME399_OAuth__Google__ClientId", "Google OAuth client ID", "OAuth / Google"),
        new("CODENAME399_OAuth__Google__ClientSecret", "Google OAuth client secret", "OAuth / Google", Secret:true),
        new("CODENAME399_OAuth__Microsoft__Enabled", "Enable Microsoft OAuth", "OAuth / Microsoft"),
        new("CODENAME399_OAuth__Microsoft__ClientId", "Microsoft OAuth client ID", "OAuth / Microsoft"),
        new("CODENAME399_OAuth__Microsoft__ClientSecret", "Microsoft OAuth client secret", "OAuth / Microsoft", Secret:true),
        new("CODENAME399_OAuth__CallbackBaseUrl", "OAuth callback base URL", "OAuth / Microsoft", "https://api.codename399.com")
    };

    readonly Dictionary<string, TextBox> fields = new();
    readonly Dictionary<string, Label> status = new();
    readonly RichTextBox log = new();
    readonly Label summary = new();
    readonly Button setMissing = new(), saveAll = new(), refresh = new();

    public MainForm()
    {
        Text = "CodeName399 Environment Manager";
        Width = 1250; Height = 900; MinimumSize = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F);

        var header = new TableLayoutPanel { Dock=DockStyle.Top, Height=92, ColumnCount=1, Padding=new Padding(12) };
        header.Controls.Add(new Label { Text="CodeName399 Environment Manager", Font=new Font(Font.FontFamily,16,FontStyle.Bold), AutoSize=true });
        header.Controls.Add(new Label { Text="Machine-level environment variables • existing values are preserved • secret fields are masked", AutoSize=true });
        summary.Text = "Loading..."; summary.AutoSize=true; header.Controls.Add(summary);

        var split = new SplitContainer {
            Dock=DockStyle.Fill,
            Orientation=Orientation.Vertical,
            SplitterDistance=980,
            IsSplitterFixed=false,
            Panel1MinSize=780,
            Panel2MinSize=320,
            Padding=new Padding(10)
        };

        var listHost = new Panel { Dock=DockStyle.Fill, AutoScroll=true, Padding=new Padding(0,0,8,0) };
        var list = new TableLayoutPanel {
            Dock=DockStyle.Top,
            AutoSize=true,
            AutoSizeMode=AutoSizeMode.GrowAndShrink,
            ColumnCount=6,
            Padding=new Padding(4),
            GrowStyle=TableLayoutPanelGrowStyle.AddRows
        };
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,190));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,90));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,88));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,96));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,86));

        string? lastCategory=null;

        list.RowStyles.Add(new RowStyle(SizeType.Absolute,34));
        list.Controls.Add(new Label { Text="Variable", Font=new Font(Font,FontStyle.Bold), Dock=DockStyle.Fill, Padding=new Padding(4,8,0,0) },0,list.RowCount);
        list.Controls.Add(new Label { Text="Value", Font=new Font(Font,FontStyle.Bold), Dock=DockStyle.Fill, Padding=new Padding(4,8,0,0) },1,list.RowCount);
        list.Controls.Add(new Label { Text="Status", Font=new Font(Font,FontStyle.Bold), Dock=DockStyle.Fill, Padding=new Padding(4,8,0,0) },2,list.RowCount);
        list.Controls.Add(new Label { Text="Save", Font=new Font(Font,FontStyle.Bold), Dock=DockStyle.Fill, Padding=new Padding(4,8,0,0) },3,list.RowCount);
        list.Controls.Add(new Label { Text="Delete", Font=new Font(Font,FontStyle.Bold), Dock=DockStyle.Fill, Padding=new Padding(4,8,0,0) },4,list.RowCount);
        list.Controls.Add(new Label { Text="Show", Font=new Font(Font,FontStyle.Bold), Dock=DockStyle.Fill, Padding=new Padding(4,8,0,0) },5,list.RowCount);
        list.RowCount++;

        foreach(var item in items)
        {
            if(item.Category != lastCategory)
            {
                list.RowStyles.Add(new RowStyle(SizeType.Absolute,30));
                var cat = new Label { Text=item.Category, Font=new Font(Font,FontStyle.Bold), AutoSize=true, Dock=DockStyle.Fill, Padding=new Padding(3,7,0,0) };
                list.Controls.Add(cat,0,list.RowCount);
                list.SetColumnSpan(cat,6);
                list.RowCount++;
                lastCategory=item.Category;
            }

            var row = list.RowCount;
            var label=new Label { Text=item.Label, AutoSize=true, Anchor=AnchorStyles.Left, Margin=new Padding(4,8,4,4) };
            var tb=new TextBox { Dock=DockStyle.Fill, Margin=new Padding(3,4,3,4), UseSystemPasswordChar=item.Secret, Tag=item };
            fields[item.Name]=tb;
            var st=new Label { Text="...", AutoSize=true, Anchor=AnchorStyles.Left, Margin=new Padding(4,8,4,4) };
            status[item.Name]=st;

            var save=new Button { Text="Save", Dock=DockStyle.Fill, Height=30, Tag=item, Margin=new Padding(3,3,3,3) };
            var del=new Button { Text="Delete", Dock=DockStyle.Fill, Height=30, Tag=item, Margin=new Padding(3,3,3,3) };
            save.Click += async (_,_)=>await SaveOneAsync(item);
            del.Click += async (_,_)=>await DeleteOneAsync(item);

            var reveal=new CheckBox {
                Text="Show",
                AutoSize=true,
                Anchor=AnchorStyles.Left,
                Margin=new Padding(4,6,2,3),
                Enabled=item.Secret
            };
            reveal.CheckedChanged += (_,_)=> {
                if(item.Secret) tb.UseSystemPasswordChar=!reveal.Checked;
            };

            list.Controls.Add(label,0,row);
            list.Controls.Add(tb,1,row);
            list.Controls.Add(st,2,row);
            list.Controls.Add(save,3,row);
            list.Controls.Add(del,4,row);
            list.Controls.Add(reveal,5,row);
            list.RowCount++;
        }

        listHost.Controls.Add(list);
        split.Panel1.Controls.Add(listHost);

        var right=new TableLayoutPanel {
            Dock=DockStyle.Fill,
            ColumnCount=1,
            RowCount=2,
            Padding=new Padding(8,0,0,0)
        };
        right.RowStyles.Add(new RowStyle(SizeType.Absolute,52));
        right.RowStyles.Add(new RowStyle(SizeType.Percent,100));

        var actions=new FlowLayoutPanel {
            Dock=DockStyle.Fill,
            WrapContents=true,
            AutoScroll=true,
            Padding=new Padding(0,4,0,4)
        };
        ConfigureButton(setMissing,"Set Missing",async (_,_)=>await SetMissingAsync());
        ConfigureButton(saveAll,"Save All",async (_,_)=>await SaveAllAsync());
        ConfigureButton(refresh,"Refresh",async (_,_)=>await RefreshAsync());
        actions.Controls.Add(setMissing);
        actions.Controls.Add(saveAll);
        actions.Controls.Add(refresh);
        right.Controls.Add(actions,0,0);

        log.Dock=DockStyle.Fill;
        log.ReadOnly=true;
        log.BackColor=Color.FromArgb(20,22,26);
        log.ForeColor=Color.Gainsboro;
        log.Font=new Font("Consolas",9);
        right.Controls.Add(log,0,1);

        split.Panel2.Controls.Add(right);
        Controls.Add(split); Controls.Add(header);
        Shown += async (_,_)=>await RefreshAsync();
    }

    void ConfigureButton(Button b,string text,EventHandler h){b.Text=text;b.AutoSize=true;b.Height=34;b.MinimumSize=new Size(105,34);b.Click+=h;}
    string? GetMachine(string n)=>Environment.GetEnvironmentVariable(n,EnvironmentVariableTarget.Machine);
    void SetMachine(string n,string v)=>Environment.SetEnvironmentVariable(n,v,EnvironmentVariableTarget.Machine);
    void WriteLog(string s,bool error=false){if(InvokeRequired){BeginInvoke(()=>WriteLog(s,error));return;} log.AppendText($"{DateTime.Now:HH:mm:ss} {s}{Environment.NewLine}");log.SelectionStart=log.TextLength;log.ScrollToCaret();}



    async Task SaveOneAsync(EnvItem item)
    {
        try
        {
            var value = fields[item.Name].Text.Trim();
            if (item.Required && string.IsNullOrWhiteSpace(value))
            {
                WriteLog($"SKIP    {item.Name} - empty required value", true);
                MessageBox.Show($"Enter a value for {item.Label}.", "Save Variable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetMachine(item.Name, value);
            WriteLog($"SAVED   {item.Name}");
            await RefreshAsync();
        }
        catch(Exception ex)
        {
            WriteLog($"ERROR   {item.Name}: {ex.Message}", true);
            MessageBox.Show(ex.Message, "Save Variable Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    async Task DeleteOneAsync(EnvItem item)
    {
        if(MessageBox.Show($"Delete the machine-level environment variable '{item.Name}'?\n\nThis cannot be undone from the application.",
            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        try
        {
            Environment.SetEnvironmentVariable(item.Name, null, EnvironmentVariableTarget.Machine);
            WriteLog($"DELETED {item.Name}");
            await RefreshAsync();
        }
        catch(Exception ex)
        {
            WriteLog($"ERROR   deleting {item.Name}: {ex.Message}", true);
            MessageBox.Show(ex.Message, "Delete Variable Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    async Task RefreshAsync()
    {
        await Task.Run(()=>{
            foreach(var i in items){var v=GetMachine(i.Name); BeginInvoke(()=>{fields[i.Name].Text=v??i.DefaultValue??""; status[i.Name].Text=string.IsNullOrWhiteSpace(v)?(i.DefaultValue==null?"MISSING":"DEFAULT"):"EXISTS";});}
        });
        var missing=items.Count(i=>string.IsNullOrWhiteSpace(GetMachine(i.Name)) && i.DefaultValue==null);
        var defaults=items.Count(i=>string.IsNullOrWhiteSpace(GetMachine(i.Name)) && i.DefaultValue!=null);
        summary.Text=$"{items.Count} variables • {missing} missing required inputs • {defaults} missing with defaults";
        WriteLog("Environment status refreshed.");
    }

    async Task SetMissingAsync()
    {
        setMissing.Enabled=saveAll.Enabled=refresh.Enabled=false;
        try
        {
            foreach(var i in items)
            {
                if(!string.IsNullOrWhiteSpace(GetMachine(i.Name))){WriteLog($"EXISTS  {i.Name}");continue;}
                var value=fields[i.Name].Text.Trim();
                if(string.IsNullOrWhiteSpace(value)){ if(i.DefaultValue!=null)value=i.DefaultValue; else { WriteLog($"MISSING {i.Name} - enter a value",true); continue; } }
                SetMachine(i.Name,value); WriteLog($"SET     {i.Name}" );
            }
            MessageBox.Show("Missing environment variables were set. Existing machine values were preserved.","Environment Setup",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        catch(Exception ex){WriteLog("ERROR: "+ex.Message,true);MessageBox.Show(ex.Message,"Environment Setup Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);}
        finally{setMissing.Enabled=saveAll.Enabled=refresh.Enabled=true;await RefreshAsync();}
    }

    async Task SaveAllAsync()
    {
        if(MessageBox.Show("Save all visible values to machine-level environment variables? Existing values will be overwritten.","Confirm Save All",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)!=DialogResult.Yes)return;
        setMissing.Enabled=saveAll.Enabled=refresh.Enabled=false;
        try{foreach(var i in items){var v=fields[i.Name].Text.Trim();if(i.Required&&string.IsNullOrWhiteSpace(v)){WriteLog($"SKIP    {i.Name} - empty required value",true);continue;}SetMachine(i.Name,v);WriteLog($"SAVED   {i.Name}");}MessageBox.Show("Environment variables saved.","Environment Setup",MessageBoxButtons.OK,MessageBoxIcon.Information);}catch(Exception ex){WriteLog("ERROR: "+ex.Message,true);MessageBox.Show(ex.Message,"Save Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);}finally{setMissing.Enabled=saveAll.Enabled=refresh.Enabled=true;await RefreshAsync();}
    }
}
