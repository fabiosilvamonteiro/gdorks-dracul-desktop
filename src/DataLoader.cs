using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace gdracul_project
{
    /// <summary>Uma categoria da GHDB exibida no app.</summary>
    internal sealed class DorkCategory
    {
        public readonly string Display;   // rotulo no ComboBox
        public readonly string FileName;  // arquivo .dorks
        public readonly string Column;    // titulo da coluna do ListView

        public DorkCategory(string display, string fileName, string column)
        {
            Display = display;
            FileName = fileName;
            Column = column;
        }
    }

    /// <summary>
    /// Carrega e filtra as dorks. Os caminhos sao montados com Path.Combine,
    /// portanto funcionam nativamente em Windows e Linux (via Mono) — sem
    /// depender de MONO_IOMAP nem de separador de diretorio especifico.
    /// </summary>
    internal sealed class DataLoader
    {
        // Ordem identica a das categorias da GHDB (category_id 1..14).
        public static readonly DorkCategory[] Categories =
        {
            new DorkCategory("Footholds",                      "footholds.dorks",                      "FOOTHOLDS"),
            new DorkCategory("Files Containing Usernames",     "files_containing_usernames.dorks",     "FILES CONTAINING USERNAMES"),
            new DorkCategory("Sensitive Directories",          "sensitive_directories.dorks",          "SENSITIVE DIRECTORIES"),
            new DorkCategory("Web Server Detection",           "web_server_detection.dorks",           "WEB SERVER DETECTION"),
            new DorkCategory("Vulnerable Files",               "vulnerable_files.dorks",               "VULNERABLE FILES"),
            new DorkCategory("Vulnerable Servers",             "vulnerable_servers.dorks",             "VULNERABLE SERVERS"),
            new DorkCategory("Error Messages",                 "error_messages.dorks",                 "ERROR MESSAGES"),
            new DorkCategory("Files Containing Juicy Info",    "files_containing_juicy_info.dorks",    "FILES CONTAINING JUICY INFO"),
            new DorkCategory("Files Containing Passwords",     "files_containing_passwords.dorks",     "FILES CONTAINING PASSWORDS"),
            new DorkCategory("Sensitive Online Shopping Info", "sensitive_online_shopping_info.dorks", "SENSITIVE ONLINE SHOPPING INFO"),
            new DorkCategory("Network or Vulnerability Data",  "network_or_vulnerability_data.dorks",  "NETWORK OR VULNERABILITY DATA"),
            new DorkCategory("Pages Containing Login Portals", "pages_containing_login_portals.dorks", "PAGES CONTAINING LOGIN PORTALS"),
            new DorkCategory("Various Online Devices",         "various_online_devices.dorks",         "VARIOUS ONLINE DEVICES"),
            new DorkCategory("Advisories and Vulnerabilities", "advisories_and_vulnerabilities.dorks", "ADVISORIES AND VULNERABILITIES"),
        };

        private List<ListViewItem> allItems = new List<ListViewItem>();

        public int Count { get { return allItems.Count; } }

        /// <summary>Pasta gdracul/dorks ao lado do executavel.</summary>
        public static string DorksDir()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(baseDir, "gdracul", "dorks");
        }

        /// <summary>Le uma categoria para dentro do ListView e guarda os itens.</summary>
        public void Load(DorkCategory cat, ListView list)
        {
            string path = Path.Combine(DorksDir(), cat.FileName);

            list.BeginUpdate();
            list.Clear();
            list.View = View.Details;
            list.Columns.Add(cat.Column, list.ClientSize.Width - 4, HorizontalAlignment.Left);

            allItems = new List<ListViewItem>();
            if (File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    string dork = line.Trim();
                    if (dork.Length == 0)
                        continue;
                    ListViewItem item = new ListViewItem(dork);
                    allItems.Add(item);
                }
                list.Items.AddRange(allItems.ToArray());
            }
            else
            {
                MessageBox.Show(
                    "Arquivo de dorks nao encontrado:\n" + path,
                    "GDorks Dracul", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            list.EndUpdate();
        }

        /// <summary>Filtra os itens carregados pelo texto informado.</summary>
        public void Filter(ListView list, string text)
        {
            text = (text ?? string.Empty).Trim().ToLowerInvariant();
            list.BeginUpdate();
            list.Items.Clear();
            if (text.Length == 0)
                list.Items.AddRange(allItems.ToArray());
            else
                list.Items.AddRange(
                    allItems.Where(i => i.Text.ToLowerInvariant().Contains(text)).ToArray());
            list.EndUpdate();
        }
    }
}
