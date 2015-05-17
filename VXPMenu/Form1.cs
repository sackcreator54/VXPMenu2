using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VXPMenu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        struct PathMono
        {
            public string path;
            public bool isMono;
        }

        List<Button> tabs = new List<Button>();
        List<Panel> tabPanels = new List<Panel>();
        //       game name,           path,  isMono
        Dictionary<string, PathMono> gameList = new Dictionary<string, PathMono>();
        Dictionary<string, PathMono> emulatorList = new Dictionary<string, PathMono>();
        Dictionary<string, Bitmap> iconList = new Dictionary<string, Bitmap>();
        Color mainColor = Color.White;
        Color secondaryColor = Color.LightGray;

        private void Form1_Load(object sender, EventArgs e)
        {
            int w = this.Width;
            int h = this.Height;

            tabPanels.Add(gameView);
            tabPanels.Add(emulatorView);
            tabPanels.Add(settingView);
            tabPanels.Add(consoleView);
            tabPanels.Add(powerView);

            tabs.Add(games);
            tabs.Add(emulators);
            tabs.Add(settings);
            tabs.Add(console);
            tabs.Add(power);

            iconList.Add("Quake 3", global::VXPMenu.Properties.Resources.quake3);
            iconList.Add("Minecraft Pi", global::VXPMenu.Properties.Resources.minecraft);

            StreamReader settingReader = new StreamReader("Resources/settings.txt");
            string content;
            while((content = settingReader.ReadLine()) != null) {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"((""((?<token>.*?)(?<!\\)"")|(?<token>[\w]+))(\s)*)", options);
                var result = (from Match m in regex.Matches(content)
                              where m.Groups["token"].Success
                              select m.Groups["token"].Value).ToList();

                switch(result[0])
                {
                    case "game":
                        PathMono pm = new PathMono();
                        pm.path = result[2];
                        switch (result[3])
                        {
                            case "false":
                                pm.isMono = false;
                                break;

                            case "true":
                                pm.isMono = true;
                                break;
                        }
                        gameList.Add(result[1], pm);
                        break;

                    case "emulator":
                        PathMono _pm = new PathMono();
                        pm.path = result[2];
                        switch (result[3])
                        {
                            case "false":
                                pm.isMono = false;
                                break;

                            case "true":
                                pm.isMono = true;
                                break;
                        }
                        emulatorList.Add(result[1], _pm);
                        break;

                    case "setting":
                        switch(result[1])
                        {
                            case "primarycolor":
                                mainColor = Color.FromArgb(Convert.ToInt16(result[2]), Convert.ToInt16(result[3]), Convert.ToInt16(result[4]));
                                break;

                            case "secondarycolor":
                                secondaryColor = Color.FromArgb(Convert.ToInt16(result[2]), Convert.ToInt16(result[3]), Convert.ToInt16(result[4]));
                                break;
                        }
                        break;
                }
            }

            int gameID = 0;
            int gameIDRow = 0;
            int gamesPerRow = 0;

            foreach (KeyValuePair<string, PathMono> gameKV in gameList)
            {
                string name = gameKV.Key;
                string path = gameKV.Value.path;
                bool isMono = gameKV.Value.isMono;

                Button _l = new Button();
                _l.FlatStyle = FlatStyle.Flat;
                _l.BackColor = mainColor;
                if (iconList.ContainsKey(name))
                {
                    _l.BackgroundImage = iconList[name];
                }
                else
                {
                    _l.BackgroundImage = global::VXPMenu.Properties.Resources.unknown;
                }
                _l.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                _l.FlatAppearance.BorderColor = secondaryColor;
                _l.FlatAppearance.BorderSize = 0;
                _l.Font = new Font("Segoe UI Light", 12);
                _l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                _l.Location = new System.Drawing.Point(0, 0);
                _l.Text = name;
                _l.Click += new EventHandler(delegate (object _sender, EventArgs _e) { OpenApplication(path, isMono); });
                _l.TextAlign = ContentAlignment.BottomLeft;
                _l.Size = new System.Drawing.Size((int)(w / 4.2f), (int)(w / 4.2f));
                _l.Location = new Point((int)(10 + gameID * w / 4.2f + (gameID * 10)), 10 + ((gameIDRow) * _l.Height) + (gameIDRow * 10));
                _l.TabIndex = 9;
                _l.UseVisualStyleBackColor = false;

                gameView.Controls.Add(_l);

                gameID++;
                if (gameView.Width - (_l.Location.X) < _l.Width / 2)
                {
                    gameIDRow++;
                    gamesPerRow = gameID;
                    gameID = 0;
                }
            }

            int emulatorID = 0;
            int emulatorIDRow = 0;
            int emulatorsPerRow = 0;

            foreach (KeyValuePair<string, PathMono> emulatorKV in emulatorList)
            {
                string name = emulatorKV.Key;
                string path = emulatorKV.Value.path;
                bool isMono = emulatorKV.Value.isMono;

                Button _l = new Button();
                _l.FlatStyle = FlatStyle.Flat;
                _l.BackColor = mainColor;
                if (iconList.ContainsKey(name))
                {
                    _l.BackgroundImage = iconList[name];
                }
                else
                {
                    _l.BackgroundImage = global::VXPMenu.Properties.Resources.unknown;
                }
                _l.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                _l.FlatAppearance.BorderColor = secondaryColor;
                _l.FlatAppearance.BorderSize = 0;
                _l.Font = new Font("Segoe UI Light", 12);
                _l.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                _l.Location = new System.Drawing.Point(0, 0);
                _l.Text = name;
                _l.Click += new EventHandler(delegate (object _sender, EventArgs _e) { OpenApplication(path, isMono); });
                _l.TextAlign = ContentAlignment.BottomLeft;
                _l.Size = new System.Drawing.Size((int)(w / 4.2f), (int)(w / 4.2f));
                _l.Location = new Point((int)(10 + emulatorID * w / 4.2f + (emulatorID * 10)), 10 + ((emulatorIDRow) * _l.Height) + (emulatorIDRow * 10));
                _l.TabIndex = 9;
                _l.UseVisualStyleBackColor = false;

                emulatorView.Controls.Add(_l);

                emulatorID++;
                if (emulatorView.Width - (_l.Location.X) < _l.Width / 2)
                {
                    emulatorIDRow++;
                    emulatorsPerRow = emulatorID;
                    emulatorID = 0;
                }
            }

            this.BackColor = secondaryColor;       
               
            games.Location = new Point(0, 0);
            games.Size = new Size(w/6, h / tabs.Count);
            games.BackColor = mainColor;

            emulators.Location = new Point(0, h / tabs.Count);
            emulators.Size = new Size(w / 6, h / tabs.Count);
            emulators.BackColor = mainColor;

            settings.Location = new Point(0, (h / tabs.Count) * 2);
            settings.Size = new Size(w / 6, h / tabs.Count);
            settings.BackColor = mainColor;

            console.Location = new Point(0, (h / tabs.Count) * 3);
            console.Size = new Size(w / 6, h / tabs.Count);
            console.BackColor = mainColor;

            power.Location = new Point(0, (h / tabs.Count) * 4);
            power.Size = new Size(w / 6, h / tabs.Count);
            power.BackColor = mainColor;

            gameView.Location = new Point(w / 6, 0);
            gameView.Size = new Size(w - w / 6, h);
            gameView.Visible = false;
            gameView.BackColor = secondaryColor;
            gameView.AutoScroll = true;

            emulatorView.Location = new Point(w / 6, 0);
            emulatorView.Size = new Size(w - w / 6, h);
            emulatorView.Visible = false;
            emulatorView.BackColor = secondaryColor;
            emulatorView.AutoScroll = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void games_Click(object sender, EventArgs e)
        {
            foreach(Button _b in tabs)
            {
                _b.BackColor = mainColor;
            }
            games.BackColor = secondaryColor;

            foreach (Panel _p in tabPanels)
            {
                _p.Visible = false;
            }
            gameView.Visible = true;
        }

        private void emulators_Click(object sender, EventArgs e)
        {
            foreach (Button _b in tabs)
            {
                _b.BackColor = mainColor;
            }
            emulators.BackColor = secondaryColor;

            foreach (Panel _p in tabPanels)
            {
                _p.Visible = false;
            }
            emulatorView.Visible = true;
        }

        private void settings_Click(object sender, EventArgs e)
        {
            foreach (Button _b in tabs)
            {
                _b.BackColor = mainColor;
            }
            settings.BackColor = secondaryColor;

            foreach (Panel _p in tabPanels)
            {
                _p.Visible = false;
            }
            settingView.Visible = true;
        }

        private void console_Click(object sender, EventArgs e)
        {
            foreach (Button _b in tabs)
            {
                _b.BackColor = mainColor;
            }
            console.BackColor = secondaryColor;

            foreach (Panel _p in tabPanels)
            {
                _p.Visible = false;
            }
            consoleView.Visible = true;
        }

        private void power_Click(object sender, EventArgs e)
        {
            foreach (Button _b in tabs)
            {
                _b.BackColor = mainColor;
            }
            power.BackColor = secondaryColor;

            foreach (Panel _p in tabPanels)
            {
                _p.Visible = false;
            }
            powerView.Visible = true;
        }

        public void OpenApplication(string path, bool isMono)
        {
            Debug.Print("Opening program @ " + path + "!");
        }
    }
}
