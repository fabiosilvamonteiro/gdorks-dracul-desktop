using System;
using System.Drawing;
using System.Windows.Forms;

namespace gdracul_project
{
    public sealed class AboutForm : Form
    {
        private static readonly Color Ink = Color.FromArgb(20, 20, 20);
        private static readonly Color Red = Color.FromArgb(183, 28, 28);

        public AboutForm()
        {
            SuspendLayout();

            Text = "About  —  GDorks Dracul";
            ClientSize = new Size(420, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            try { Icon = Assets.Icon("dragon.ico"); } catch { }

            PictureBox logo = new PictureBox
            {
                Location = new Point(140, 18),
                Size = new Size(140, 140),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Assets.Image("dragon.png")
            };
            Label title = new Label
            {
                AutoSize = false,
                Location = new Point(20, 164),
                Size = new Size(380, 34),
                Text = "GDorks Dracul",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Ink,
                Font = new Font("Arial", 20f, FontStyle.Bold)
            };
            Label subtitle = new Label
            {
                AutoSize = false,
                Location = new Point(20, 198),
                Size = new Size(380, 22),
                Text = "Google Dorks Searcher Engine",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Red,
                Font = new Font("Arial", 11f, FontStyle.Bold)
            };
            Label desc = new Label
            {
                AutoSize = false,
                Location = new Point(30, 232),
                Size = new Size(360, 60),
                Text = "Dorks atualizadas automaticamente a partir da\n" +
                       "Google Hacking Database (GHDB) do Exploit-DB.",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Ink,
                Font = new Font("Arial", 10f)
            };
            Label dev = new Label
            {
                AutoSize = false,
                Location = new Point(20, 296),
                Size = new Size(380, 20),
                Text = "Developed by Dracul  •  draculwhitehat@gmail.com",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DimGray,
                Font = new Font("Arial", 9f)
            };
            LinkLabel bmc = new LinkLabel
            {
                AutoSize = false,
                Location = new Point(20, 320),
                Size = new Size(380, 22),
                Text = "Buy me a coffee",
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = Red,
                Font = new Font("Arial", 10f, FontStyle.Bold)
            };
            bmc.Links.Add(0, bmc.Text.Length, "https://www.buymeacoffee.com/dracul");
            bmc.LinkClicked += (s, e) => MainForm.OpenUrl((string)e.Link.LinkData);

            Button ok = new Button
            {
                Text = "OK",
                Size = new Size(100, 34),
                Location = new Point(160, 352),
                FlatStyle = FlatStyle.Flat,
                BackColor = Red,
                ForeColor = Color.White,
                Font = new Font("Arial", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            ok.FlatAppearance.BorderSize = 0;
            ok.Click += (s, e) => Close();
            AcceptButton = ok;

            Controls.Add(logo);
            Controls.Add(title);
            Controls.Add(subtitle);
            Controls.Add(desc);
            Controls.Add(dev);
            Controls.Add(bmc);
            Controls.Add(ok);

            ResumeLayout(false);
        }
    }
}
