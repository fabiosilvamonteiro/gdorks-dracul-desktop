# GDorks Dracul

**Google Dork Searcher Engine** — um aplicativo desktop que organiza e pesquisa
Google Dorks por categoria, usando a base oficial **GHDB (Google Hacking
Database)** do Exploit-DB.

![GDorks Dracul](src/assets/dragon.png)

> Dorks são consultas avançadas do Google que refinam os resultados de busca,
> permitindo reconhecimento (OSINT) mais eficiente e específico. Uso destinado a
> testes de segurança **autorizados**, pesquisa e educação.

## Novidades da v2.0

- 🐉 **Novo visual** com o dragão do DracuCybersec como ícone do app e da janela.
- 🔎 **Dorks atualizadas** (~7.800) direto das 14 categorias da GHDB do Exploit-DB.
- 🐧 **Roda no Linux** (via Mono) e no Windows — o bug de caminho com `\` do
  Windows foi corrigido (agora usa `Path.Combine`, sem precisar de `MONO_IOMAP`).
- 🧱 **Código-fonte incluído** (`src/`) — antes o repositório só tinha o binário.
- 🔁 **Script de atualização** das dorks (`tools/update_dorks.py`).
- 🖥️ **Layout refeito** — todos os ícones e controles se enquadram corretamente.

## Recursos

- **14 categorias** da GHDB: Footholds, Files Containing Usernames, Sensitive
  Directories, Web Server Detection, Vulnerable Files, Vulnerable Servers, Error
  Messages, Files Containing Juicy Info, Files Containing Passwords, Sensitive
  Online Shopping Info, Network or Vulnerability Data, Pages Containing Login
  Portals, Various Online Devices, Advisories and Vulnerabilities.
- **Filtro em tempo real** por texto.
- **Copiar** a dork para a área de transferência.
- **Abrir no Google** (duplo-clique, Enter ou botão) — funciona em Windows e
  Linux (`xdg-open`).

## Como usar

1. Escolha a **categoria** no seletor.
2. (Opcional) digite no **filtro** para refinar.
3. Selecione uma dork e clique em **Copy to Clipboard** ou **Open in Google**
   (ou dê duplo-clique na linha).

## Executando

### Windows
Baixe/clone o repositório e execute `gdracul.exe`. A pasta `gdracul/dorks/`
precisa ficar **ao lado** do executável.

### Linux (Kali/Debian) via Mono
```bash
sudo apt install -y mono-complete libgdiplus
mono gdracul.exe
```

## Compilando a partir do código

O executável é self-contained (ícones/imagens embutidos).

### Linux (Mono)
```bash
cd src
./build.sh          # gera ../gdracul.exe
```

### Windows / .NET
```bash
cd src
dotnet build -c Release
```

## Atualizando as dorks

As dorks vêm da GHDB do Exploit-DB. Para baixar as versões mais recentes:

```bash
pip install ftfy          # opcional, corrige acentuação de registros antigos
python3 tools/update_dorks.py
```

Isso regrava os 14 arquivos `gdracul/dorks/*.dorks` e o `google_dorks.json`.

## Estrutura

```
gdracul.exe               # aplicativo (rebuild via src/build.sh)
gdracul/dorks/*.dorks     # dorks por categoria (uma por linha)
gdracul/dorks/google_dorks.json
src/                      # código-fonte C# (WinForms)
tools/update_dorks.py     # atualizador das dorks (GHDB)
```

## Créditos

Desenvolvido por **Dracul** — [draculcybersec.com](https://www.draculcybersec.com)
· Dorks: [Exploit-DB GHDB](https://www.exploit-db.com/google-hacking-database)
· [Buy me a coffee](https://www.buymeacoffee.com/dracul)
