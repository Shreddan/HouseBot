using Discord;
using Discord.WebSocket;
using System.Net;
using System.Linq;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HouseBot
{
    public class DisBot
    {
        private readonly IServiceProvider _serviceProvider;
        public static ComHandler? Com;
        public ulong General = 870416615190167565;
        public ulong Announcement = 876551668827848755;
        public ulong Admin = 995702942327918733;
        public string kidstag = "<@&870417301869052015>";

        //public List<TimeOnly> times;

        public static NotifyIcon? ni;
        public Form form;
        public TextBox tb1;


        public DisBot()
        {
            ni = new NotifyIcon();
            ni.Visible = true;
            ni.Icon = new System.Drawing.Icon(@"Icon1.ico");
            ni.Text = "HouseBot";

            _serviceProvider = CreateProvider();


            Task.Run(() => Formloop());
        }

        static IServiceProvider CreateProvider()
        {

            var dc = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true

            };

            var collection = new ServiceCollection()
            .AddSingleton(dc)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandService>();

            return collection.BuildServiceProvider();
        }

        public async Task DiscordRun()
        {
            var dsc = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            var cs = _serviceProvider.GetRequiredService<CommandService>();

            await dsc.LoginAsync(TokenType.Bot, "OTcxMDY1MTg1Njc2NzE0MDQ0.YnFE7Q.2e4SjmCydKKxFggsUSVfssgR2fw");
            await dsc.StartAsync();

            dsc.Connected += Dsc_Connected;
            dsc.Ready += Dsc_Ready;
            //dsc.MessageReceived += Dsc_MessageReceived;
            dsc.LatencyUpdated += Dsc_LatencyUpdated;


            ni.Click += icn_click;

            await dsc.DownloadUsersAsync(dsc.Guilds);

            Com = new ComHandler(dsc, cs);

            await Task.Delay(Timeout.Infinite);
        }

        private void icn_click(object? sender, EventArgs e)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(delegate
                {
                    if (!form.Visible)
                    {
                        form.Show();
                        form.WindowState = FormWindowState.Maximized;
                        form.BringToFront();
                    }
                    else
                    {
                        form.Hide();
                    }
                });
                return;
            }
            else
            {
                if (!form.Visible)
                    form.Show();
                else
                    form.Hide();
            }
        }

        public async Task Formloop()
        {
            form = new Form
            {
                Size = new Size(600, 400),
                Text = "HouseBot"
            };

            tb1 = new TextBox();
            tb1.Dock = DockStyle.Fill;
            tb1.ReadOnly = true;
            tb1.Multiline = true;
            tb1.BackColor = System.Drawing.Color.Black;
            tb1.ForeColor = System.Drawing.Color.White;
            tb1.Parent = form;
            tb1.WordWrap = true;
            tb1.Font = new Font(tb1.Font.FontFamily, 16);
            tb1.ScrollBars = ScrollBars.Vertical;


            form.Click += Form_OnClick;
            form.Activated += Form_OnActivate;
            form.Load += Form_OnLoad;
            form.FormClosed += Form_Closed;
            Application.Run(form);

            await Task.Delay(Timeout.Infinite);
        }

        private void Form_Closed(object? sender, FormClosedEventArgs e)
        {
            ni.Visible = false;
            ni.Dispose();
            Environment.Exit(Environment.ExitCode);
        }

        private void Form_OnLoad(object? sender, EventArgs e)
        {
            tb1.Show();
        }

        private void Form_OnActivate(object? sender, EventArgs e)
        {

        }

        private void Form_OnClick(object? sender, EventArgs e)
        {

        }

        public static void WriteTextSafe(string text, TextBox? tb)
        {

            if (tb.InvokeRequired)
            {
                Action safeWrite = delegate { WriteTextSafe(text, tb); };
                tb.Invoke(safeWrite);
            }
            else
            {
                tb.Text += text + Environment.NewLine;
            }

        }

        private async Task Dsc_LatencyUpdated(int arg1, int arg2)
        {
            //WriteTextSafe(dsc.Latency);
            //if (DateTime.Now.Hour == times.First().Hour && DateTime.Now.Minute == times.First().Minute)
            //{
            //    await Announcements("Daily Reminder " + kidstag + "https://cdn.discordapp.com/attachments/870416615190167565/971059992646004756/unknown.png");
            //}

            //if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && DateTime.Now.Hour == times[1].Hour)
            //{
            //    if (DateTime.Now.Minute == times[1].Minute)
            //    {
            //        await Announcements("Weekly Reminder " + kidstag + "https://cdn.discordapp.com/attachments/660268338324439062/971155259068145794/unknown.png");
            //    }
            //}
        }


        private async Task Dsc_Ready()
        {
            SocketTextChannel gen = GetSocketTextChannel(General);
            await gen.SendMessageAsync(text: "I am Online");
        }

        private async Task Dsc_Connected()
        {
            WriteTextSafe("Connection Established", tb1);
        }

        public SocketTextChannel GetSocketTextChannel(ulong chanID)
        {
            return (SocketTextChannel)dsc.GetChannelAsync(chanID, RequestOptions.Default).Result;
        }

        public async Task Announcements(string str)
        {
            SocketTextChannel ann = GetSocketTextChannel(Announcement);
            await ann.SendMessageAsync(text: str);
        }
    }
}
