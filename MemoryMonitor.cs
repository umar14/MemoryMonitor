// MemoryMonitor.cs
// Compile with build.bat — requires .NET Framework 4, C# 5

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;

class MemoryMonitorApp : Form
{
    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;
    private System.Timers.Timer updateTimer;
    private PerformanceCounter ramCounter;
    private float totalRAM;


    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetPhysicallyInstalledSystemMemory(out long totalMemoryInKilobytes);

    public MemoryMonitorApp()
    {
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;
        this.Visible = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Size = new Size(1, 1);

        long totalKB;
        GetPhysicallyInstalledSystemMemory(out totalKB);
        totalRAM = totalKB / 1024f / 1024f;

        ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("Memory Monitor", null, null).Enabled = false;
        trayMenu.Items.Add(new ToolStripSeparator());

        var detailsItem = new ToolStripMenuItem("Show Details");
        detailsItem.Click += ShowDetails;
        trayMenu.Items.Add(detailsItem);
        trayMenu.Items.Add(new ToolStripSeparator());

        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += OnExit;
        trayMenu.Items.Add(exitItem);

        trayIcon = new NotifyIcon();
        trayIcon.Text = "Memory Monitor";
        trayIcon.ContextMenuStrip = trayMenu;
        trayIcon.Visible = true;
        trayIcon.DoubleClick += ShowDetails;

        UpdateStats();

        // Alternate RAM / CPU display every 2 seconds
        updateTimer = new System.Timers.Timer(2000);
        updateTimer.Elapsed += (s, e) => {
            try { this.Invoke((Action)UpdateStats); }
            catch { }
        };
        updateTimer.Start();
    }

    private void UpdateStats()
    {
        try
        {
            float availableMB = ramCounter.NextValue();
            float usedGB  = (totalRAM * 1024f - availableMB) / 1024f;
            float usedPct = (usedGB / totalRAM) * 100f;

            trayIcon.Text = string.Format("RAM: {0:F1} / {1:F1} GB ({2:F0}%)", usedGB, totalRAM, usedPct);

            Color c = usedPct < 60 ? Color.Lime : usedPct < 85 ? Color.Yellow : Color.Red;
            trayIcon.Icon = DrawIcon((int)Math.Round(usedPct), c);

            trayMenu.Items[0].Text = string.Format("RAM {0:F0}%", usedPct);
        }
        catch { }
    }

    private Icon DrawIcon(int value, Color textColor)
    {
        var bmp = new Bitmap(128, 128);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(Color.Black);

            string numStr = string.Format("{0}", value);
            var numFont   = new Font("Arial", 80f, FontStyle.Bold, GraphicsUnit.Pixel);
            var numBrush  = new SolidBrush(textColor);
            var numSize   = g.MeasureString(numStr, numFont);
            g.DrawString(numStr, numFont, numBrush,
                         (128f - numSize.Width)  / 2f,
                         (128f - numSize.Height) / 2f);
            numFont.Dispose();
            numBrush.Dispose();
        }

        return Icon.FromHandle(bmp.GetHicon());
    }

    private void ShowDetails(object sender, EventArgs e)
    {
        float availableMB = ramCounter.NextValue();
        float usedGB  = (totalRAM * 1024f - availableMB) / 1024f;
        float usedPct = (usedGB / totalRAM) * 100f;

        MessageBox.Show(
            string.Format(
                "  RAM Usage\n" +
                "  -----------------\n" +
                "  Total:     {0:F2} GB\n" +
                "  Used:      {1:F2} GB  ({2:F1}%)\n" +
                "  Available: {3:F2} GB\n\n" +
                "  Green < 60%   Yellow < 85%   Red > 85%",
                totalRAM, usedGB, usedPct, availableMB / 1024f),
            "Memory Monitor",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    private void OnExit(object sender, EventArgs e)
    {
        trayIcon.Visible = false;
        Application.Exit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (updateTimer != null) { updateTimer.Stop(); updateTimer.Dispose(); }
            if (trayIcon   != null) { trayIcon.Dispose(); }
            if (ramCounter != null) { ramCounter.Dispose(); }
        }
        base.Dispose(disposing);
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MemoryMonitorApp());
    }
}
