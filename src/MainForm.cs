using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace gdracul_project
{
    public sealed class MainForm : Form
    {
        private readonly DataLoader data = new DataLoader();

        private ComboBox comboCategories;
        private TextBox filterBox;
        private ListView listView;
        private Label countLabel;

        // Paleta DracuCybersec (preto / vermelho)
        private static readonly Color Ink = Color.FromArgb(20, 20, 20);
        private static readonly Color Red = Color.FromArgb(183, 28, 28);
        private static readonly Color RedHi = Color.FromArgb(211, 47, 47);

        public MainForm()
        {
            BuildUi();
        }

        // ---------------------------------------------------------------- UI
        private void BuildUi()
        {
            SuspendLayout();

            Text = "GDorks Dracul  —  Google Dork Searcher Engine";
            ClientSize = new Size(820, 648);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Arial", 10f);
            try { Icon = Assets.Icon("dragon.ico"); } catch { }

            // ---- Cabecalho (dragao + titulo) ----
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 112,
                BackColor = Ink
            };
            PictureBox logo = new PictureBox
            {
                Location = new Point(14, 8),
                Size = new Size(96, 96),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Image = Assets.Image("dragon_header.png")
            };
            Label title = new Label
            {
                AutoSize = true,
                Location = new Point(120, 20),
                Text = "GDorks Dracul",
                ForeColor = Color.White,
                Font = new Font("Arial", 27f, FontStyle.Bold)
            };
            Label subtitle = new Label
            {
                AutoSize = true,
                Location = new Point(124, 72),
                Text = "Google Dork Searcher Engine",
                ForeColor = RedHi,
                Font = new Font("Arial", 12f, FontStyle.Regular)
            };
            Label version = new Label
            {
                AutoSize = true,
                Location = new Point(740, 86),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "v2.0",
                ForeColor = Color.Gainsboro,
                Font = new Font("Arial", 9f)
            };
            header.Controls.Add(logo);
            header.Controls.Add(title);
            header.Controls.Add(subtitle);
            header.Controls.Add(version);

            // ---- Linha de categoria + filtro ----
            PictureBox catIcon = new PictureBox
            {
                Location = new Point(16, 128),
                Size = new Size(26, 26),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Assets.Image("category.png")
            };
            comboCategories = new ComboBox
            {
                Location = new Point(50, 126),
                Size = new Size(392, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 12f),
                FlatStyle = FlatStyle.Flat
            };
            foreach (DorkCategory c in DataLoader.Categories)
                comboCategories.Items.Add(c.Display);
            comboCategories.SelectedIndexChanged += OnCategoryChanged;

            PictureBox searchIcon = new PictureBox
            {
                Location = new Point(462, 128),
                Size = new Size(26, 26),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Assets.Image("search.png")
            };
            filterBox = new TextBox
            {
                Location = new Point(496, 126),
                Size = new Size(308, 30),
                Font = new Font("Arial", 12f),
                BorderStyle = BorderStyle.FixedSingle
            };
            filterBox.TextChanged += (s, e) => data.Filter(listView, filterBox.Text);

            // ---- Lista de dorks ----
            listView = new ListView
            {
                Location = new Point(16, 172),
                Size = new Size(788, 388),
                View = View.Details,
                FullRowSelect = true,
                HideSelection = false,
                MultiSelect = false,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                GridLines = false,
                Font = new Font("DejaVu Sans Mono", 10.5f),
                BorderStyle = BorderStyle.FixedSingle
            };
            listView.DoubleClick += (s, e) => OpenSelectedInGoogle();
            listView.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { OpenSelectedInGoogle(); e.Handled = true; }
            };

            // ---- Rodape ----
            countLabel = new Label
            {
                AutoSize = true,
                Location = new Point(16, 578),
                Text = "Dorks Found: 0",
                ForeColor = Color.ForestGreen,
                Font = new Font("Arial", 11f, FontStyle.Bold)
            };

            Button btnCopy = MakePrimary("Copy to Clipboard", new Point(320, 572), 148);
            btnCopy.Click += OnCopy;
            Button btnOpen = MakePrimary("Open in Google", new Point(474, 572), 140);
            btnOpen.Click += (s, e) => OpenSelectedInGoogle();
            Button btnAbout = MakeSecondary("About", new Point(620, 572), 86);
            btnAbout.Click += (s, e) => new AboutForm().ShowDialog(this);
            Button btnClose = MakeSecondary("Close", new Point(712, 572), 92);
            btnClose.Click += (s, e) => Close();

            LinkLabel credits = new LinkLabel
            {
                AutoSize = true,
                Location = new Point(16, 620),
                Text = "Developed by Dracul  •  draculcybersec.com  •  Buy me a coffee",
                LinkColor = Red,
                Font = new Font("Arial", 9f)
            };
            AddLink(credits, "draculcybersec.com", "https://www.draculcybersec.com");
            AddLink(credits, "Buy me a coffee", "https://www.buymeacoffee.com/dracul");
            credits.LinkClicked += (s, e) => OpenUrl((string)e.Link.LinkData);

            Controls.Add(header);
            Controls.Add(catIcon);
            Controls.Add(comboCategories);
            Controls.Add(searchIcon);
            Controls.Add(filterBox);
            Controls.Add(listView);
            Controls.Add(countLabel);
            Controls.Add(btnCopy);
            Controls.Add(btnOpen);
            Controls.Add(btnAbout);
            Controls.Add(btnClose);
            Controls.Add(credits);

            ResumeLayout(false);
            PerformLayout();

            Load += (s, e) => { if (comboCategories.Items.Count > 0) comboCategories.SelectedIndex = 0; };
        }

        private static void AddLink(LinkLabel label, string part, string url)
        {
            int i = label.Text.IndexOf(part, StringComparison.Ordinal);
            if (i >= 0)
                label.Links.Add(i, part.Length, url);
        }

        private Button MakePrimary(string text, Point at, int width)
        {
            Button b = new Button
            {
                Text = text,
                Location = at,
                Size = new Size(width, 42),
                FlatStyle = FlatStyle.Flat,
                BackColor = Red,
                ForeColor = Color.White,
                Font = new Font("Arial", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = RedHi;
            return b;
        }

        private Button MakeSecondary(string text, Point at, int width)
        {
            Button b = new Button
            {
                Text = text,
                Location = at,
                Size = new Size(width, 42),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.WhiteSmoke,
                ForeColor = Ink,
                Font = new Font("Arial", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderColor = Color.Silver;
            return b;
        }

        // ------------------------------------------------------------ Events
        private void OnCategoryChanged(object sender, EventArgs e)
        {
            int i = comboCategories.SelectedIndex;
            if (i < 0 || i >= DataLoader.Categories.Length)
                return;
            filterBox.Text = "";
            data.Load(DataLoader.Categories[i], listView);
            countLabel.Text = "Dorks Found: " + data.Count;
        }

        private void OnCopy(object sender, EventArgs e)
        {
            string dork = SelectedDork();
            if (dork == null)
            {
                Alert("Select a row to be copied.");
                return;
            }
            try
            {
                Clipboard.SetText(dork);
                countLabel.Text = "Copied to clipboard!";
            }
            catch (Exception ex)
            {
                Alert("Could not copy: " + ex.Message);
            }
        }

        private void OpenSelectedInGoogle()
        {
            string dork = SelectedDork();
            if (dork == null)
            {
                Alert("Select a row to open.");
                return;
            }
            OpenUrl("https://www.google.com/search?q=" + Uri.EscapeDataString(dork));
        }

        private string SelectedDork()
        {
            if (listView.SelectedItems.Count == 0)
                return null;
            return listView.SelectedItems[0].Text;
        }

        private void Alert(string msg)
        {
            MessageBox.Show(msg, "GDorks Dracul", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>Abre uma URL de forma portavel (Windows e Linux/Mono).</summary>
        internal static void OpenUrl(string url)
        {
            try
            {
                bool unix = Environment.OSVersion.Platform == PlatformID.Unix
                         || Environment.OSVersion.Platform == PlatformID.MacOSX;
                if (unix)
                    Process.Start("xdg-open", url);
                else
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
                MessageBox.Show("The URL could not be opened. Try again!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
