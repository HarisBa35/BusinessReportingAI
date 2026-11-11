# BusinessReportingAI
# AI Business Reporting Assistant (NL2SQL / RAG)

Aplikacija omogućava generisanje poslovnih izvještaja iz SQL Server baze na osnovu pitanja na prirodnom jeziku pomoću lokalnog LLM-a (**Ollama**).

## 🚀 Tehnologije
- **C# / .NET 8 (ili 9)** ASP.NET Core Razor Pages
- **Ollama** (Llama 3 / Qwen2.5-Coder)
- **Microsoft SQL Server**
- **Dapper & ADO.NET**

## 🛡️ Arhitektura & Sigurnost
1. **Context Grounding (RAG)**: LLM dobija definisanu šemu baze prije procesiranja upita.
2. **Čišćenje upita**: Uklanjaju se markdown blokovi i praznine.
3. **Validacija (SqlValidator)**:
   - Dozvoljeni su samo `SELECT` i `WITH` iskazi.
   - Detekcija i zabrana destruktivnih ključnih riječi (`DROP`, `DELETE`, `UPDATE`, `ALTER`, `TRUNCATE`, `EXEC`).
   - Onemogućeno ulančavanje više upita (prevencija SQL Injectiona).
4. **Read-Only izvršavanje**: Upiti se izvršavaju sa definisanim timeoutom nad bazom.

## ⚙️ Pokretanje projekta

### 1. Pokretanje Ollame
Preuzmite [Ollama](https://ollama.com/) i pokrenite željeni model u terminalu:
ollama run llama3
