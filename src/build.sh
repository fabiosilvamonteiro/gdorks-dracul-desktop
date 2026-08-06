#!/bin/sh
# Compila o GDorks Dracul com o Mono (mcs). Gera ../gdracul.exe self-contained.
set -e
cd "$(dirname "$0")"
OUT="${1:-../gdracul.exe}"

mcs -target:winexe -out:"$OUT" -langversion:latest -sdk:4.7.2 \
    -r:System.dll -r:System.Drawing.dll -r:System.Windows.Forms.dll \
    -win32icon:assets/dragon.ico \
    -resource:assets/dragon.ico,dragon.ico \
    -resource:assets/dragon.png,dragon.png \
    -resource:assets/dragon_header.png,dragon_header.png \
    -resource:assets/search.png,search.png \
    -resource:assets/category.png,category.png \
    AssemblyInfo.cs Program.cs Assets.cs DataLoader.cs MainForm.cs AboutForm.cs

echo "Built: $OUT"
