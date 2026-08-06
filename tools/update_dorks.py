#!/usr/bin/env python3
"""
update_dorks.py - Atualiza as Google Dorks do GDorks Dracul a partir da
Google Hacking Database (GHDB) oficial do Exploit-DB.

As 14 categorias do app correspondem 1:1 (e na mesma ordem) as categorias da
GHDB. O script baixa todas as dorks de cada categoria via o endpoint JSON
(DataTables) do Exploit-DB e grava um arquivo .dorks por categoria, uma dork
por linha. Tambem gera um google_dorks.json consolidado.

Uso:  python3 tools/update_dorks.py
"""
import html
import json
import os
import re
import sys
import time
import urllib.request

try:
    # Corrige mojibake (texto UTF-8 multi-codificado) presente em alguns
    # registros antigos da propria GHDB.  pip install ftfy
    from ftfy import fix_text
except ImportError:  # fallback: mantem o texto como veio
    def fix_text(s):
        return s

BASE = "https://www.exploit-db.com/google-hacking-database"
UA = ("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 "
      "(KHTML, like Gecko) Chrome/120 Safari/537.36")

# GHDB category_id -> (nome de exibicao, arquivo .dorks)  -- ordem = dropdown do app
CATEGORIES = [
    (1,  "Footholds",                      "footholds.dorks"),
    (2,  "Files Containing Usernames",     "files_containing_usernames.dorks"),
    (3,  "Sensitive Directories",          "sensitive_directories.dorks"),
    (4,  "Web Server Detection",           "web_server_detection.dorks"),
    (5,  "Vulnerable Files",               "vulnerable_files.dorks"),
    (6,  "Vulnerable Servers",             "vulnerable_servers.dorks"),
    (7,  "Error Messages",                 "error_messages.dorks"),
    (8,  "Files Containing Juicy Info",    "files_containing_juicy_info.dorks"),
    (9,  "Files Containing Passwords",     "files_containing_passwords.dorks"),
    (10, "Sensitive Online Shopping Info", "sensitive_online_shopping_info.dorks"),
    (11, "Network or Vulnerability Data",  "network_or_vulnerability_data.dorks"),
    (12, "Pages Containing Login Portals", "pages_containing_login_portals.dorks"),
    (13, "Various Online Devices",         "various_online_devices.dorks"),
    (14, "Advisories and Vulnerabilities", "advisories_and_vulnerabilities.dorks"),
]

TAG_RE = re.compile(r"<[^>]+>")


def fetch_category(cat_id):
    """Retorna a lista de strings de dork de uma categoria da GHDB."""
    url = (f"{BASE}?draw=1&start=0&length=100000&category={cat_id}")
    req = urllib.request.Request(url, headers={
        "User-Agent": UA,
        "X-Requested-With": "XMLHttpRequest",
        "Accept": "application/json, text/javascript, */*; q=0.01",
    })
    with urllib.request.urlopen(req, timeout=60) as r:
        data = json.load(r)
    dorks = []
    for row in data.get("data", []):
        # url_title = '<a href="/ghdb/382">DORK AQUI</a>'
        inner = TAG_RE.sub("", row.get("url_title", "")).strip()
        inner = html.unescape(inner)
        inner = fix_text(inner).strip()
        if inner:
            dorks.append(inner)
    return dorks


def main():
    root = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
    dorks_dir = os.path.join(root, "gdracul", "dorks")
    os.makedirs(dorks_dir, exist_ok=True)

    consolidated = {}
    grand_total = 0
    for cat_id, name, fname in CATEGORIES:
        for attempt in range(1, 4):
            try:
                dorks = fetch_category(cat_id)
                break
            except Exception as e:  # noqa
                print(f"  ! tentativa {attempt} falhou p/ {name}: {e}", file=sys.stderr)
                time.sleep(3)
        else:
            print(f"ERRO: nao consegui baixar '{name}'", file=sys.stderr)
            sys.exit(1)
        # dedup preservando ordem
        seen, uniq = set(), []
        for d in dorks:
            if d not in seen:
                seen.add(d)
                uniq.append(d)
        out = os.path.join(dorks_dir, fname)
        with open(out, "w", encoding="utf-8") as f:
            f.write("\n".join(uniq) + "\n")
        consolidated[name] = uniq
        grand_total += len(uniq)
        print(f"  {name:38s} {len(uniq):5d} -> {fname}")
        time.sleep(1)

    with open(os.path.join(dorks_dir, "google_dorks.json"), "w", encoding="utf-8") as f:
        json.dump(consolidated, f, ensure_ascii=False, indent=2)

    print(f"\nTotal: {grand_total} dorks em {len(CATEGORIES)} categorias.")


if __name__ == "__main__":
    main()
