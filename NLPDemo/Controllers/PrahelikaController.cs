using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Newtonsoft.Json.Linq;
using NLPDemo.Database;
using NLPDemo.Model;
using NLPDemo.Models;
using System;
using System.Text.RegularExpressions;

namespace NLPDemo.Controllers
{
    public class PrahelikaController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PrahelikaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Search(string q)
        //{
        //    if (string.IsNullOrWhiteSpace(q))
        //    {
        //        return Json(new { error = "Enter a search term" });
        //    }

        //    string query = q.Trim().ToLower();

        //    // Population search
        //    if (query.Contains("population") || query.Contains("people") || query.Contains("जनसंख्या"))
        //    {
        //        var villages = await _context.tblvillageinfo.ToListAsync();
        //        string villageFilter = "";

        //        // Extract village name if mentioned
        //        var match = Regex.Match(query, @"(honnapur|yallatti|kerur|sameerwadi)", RegexOptions.IgnoreCase);
        //        if (match.Success)
        //        {
        //            villageFilter = match.Value;
        //            villages = villages.Where(v => v.VillageName.ToLower().Contains(villageFilter)).ToList();
        //        }

        //        return Json(new { type = "population", data = villages });
        //    }

        //    // Village name search (fallback to population)
        //    var village = await _context.tblvillageinfo.FirstOrDefaultAsync(v => v.VillageName.ToLower().Contains(query));
        //    if (village != null)
        //    {
        //        return Json(new { type = "population", data = new List<tblVillageInfo> { village } });
        //    }

        //    // Property search (fuzzy match)
        //    string cleanQuery = Regex.Replace(query, @"[^0-9\/a-zA-Z]", ""); // Clean input
        //    var properties = await _context.tblproperty
        //        .Where(p => p.SurveyNumber.Contains(cleanQuery) ||
        //                    (p.SubDivision != null && p.SubDivision.Contains(cleanQuery)) ||
        //                    (p.SurveyNumber + "/" + p.SubDivision).Contains(cleanQuery))
        //        .ToListAsync();

        //    if (!properties.Any())
        //    {
        //        return Json(new { error = "No results found" });
        //    }

        //    return Json(new { type = "property", data = properties });
        //}

        //===========================================================================================================================================================
        //[HttpGet]
        //public async Task<IActionResult> Search(string q)
        //{
        //    if (string.IsNullOrWhiteSpace(q))
        //        return Json(new { error = "Type something..." });

        //    var parsed = LandQueryTrainer.Parse(q);

        //    if (parsed.Intent == "population")
        //    {
        //        var villages = string.IsNullOrEmpty(parsed.Village)
        //            ? await _context.tblvillageinfo.ToListAsync()
        //            : await _context.tblvillageinfo
        //                .Where(v => EF.Functions.Like(v.VillageName, $"%{parsed.Village}%"))
        //                .ToListAsync();

        //        return Json(new { type = "population", data = villages });
        //    }

        //    var queryable = _context.tblproperty.AsQueryable();

        //    // Survey number
        //    if (!string.IsNullOrEmpty(parsed.SurveyNumber))
        //    {
        //        string s = parsed.SurveyNumber.Replace("/", "").Replace("\\", "");
        //        queryable = queryable.Where(p =>
        //            p.SurveyNumber.Contains(s) ||
        //            (p.SubDivision != null && p.SubDivision.Contains(s)) ||
        //            (p.SubDivision != null && (p.SurveyNumber + p.SubDivision).Contains(s)));
        //    }

        //    // Village
        //    if (!string.IsNullOrEmpty(parsed.Village))
        //        queryable = queryable.Where(p => EF.Functions.Like(p.Village, $"%{parsed.Village}%"));

        //    // MIN AREA → "more than X acres"
        //    if (parsed.MinArea.HasValue)
        //    {
        //        decimal minAcres = parsed.MinArea.Value;
        //        queryable = queryable.Where(p =>
        //            p.AreaAcre + (p.AreaGunta / 40.0m) > minAcres);   // > 9 means 9.01 and above
        //    }

        //    // MAX AREA → "less than X acres" or "under X acres"
        //    if (parsed.MaxArea.HasValue)
        //    {
        //        decimal maxAcres = parsed.MaxArea.Value;
        //        queryable = queryable.Where(p =>
        //            p.AreaAcre + (p.AreaGunta / 40.0m) < maxAcres);   // < 10 means up to 9.99
        //    }
        //    // Owner
        //    if (!string.IsNullOrEmpty(parsed.Owner))
        //        queryable = queryable.Where(p => EF.Functions.Like(p.OwnerName, $"%{parsed.Owner}%"));

        //    var results = await queryable.Take(100).ToListAsync();

        //    return results.Any()
        //        ? Json(new { type = "property", data = results })
        //        : Json(new { error = "No results found" });
        //}
        //===================================================================================================================================================

        //[HttpGet]
        //public async Task<IActionResult> Search(string q)
        //{
        //    if (string.IsNullOrWhiteSpace(q))
        //        return Json(new { error = "Type anything..." });

        //    string input = " " + q.Trim().ToLowerInvariant() + " ";

        //    // 1. POPULATION
        //    if (ContainsAny(input, "population", "जनसंख्या", "लोकसंख्या", "people", "जनगणना"))
        //    {
        //        var village = ExtractVillage(input);
        //        var data = string.IsNullOrEmpty(village)
        //            ? await _context.tblvillageinfo.ToListAsync()
        //            : await _context.tblvillageinfo.Where(v => EF.Functions.Like(v.VillageName.ToLower(), $"%{village}%")).ToListAsync();
        //        return Json(new { type = "population", data });
        //    }

        //    var properties = _context.tblproperty.AsQueryable();

        //    // 2. SURVEY NUMBER
        //    var surveyNum = ExtractNumber(input);
        //    if (!string.IsNullOrEmpty(surveyNum))
        //    {
        //        string s = surveyNum.Replace("/", "").Replace("-", "");
        //        properties = properties.Where(p =>
        //            p.SurveyNumber.Contains(s) ||
        //            (p.SubDivision != null && p.SubDivision.Contains(s)));
        //    }

        //    // 3. VILLAGE
        //    var villageName = ExtractVillage(input);
        //    if (!string.IsNullOrEmpty(villageName))
        //        properties = properties.Where(p => EF.Functions.Like(p.Village.ToLower(), $"%{villageName}%"));

        //    // 4. OWNER NAME
        //    var ownerName = ExtractWordAfter(input, "owner", "malik", "मालिक");
        //    if (!string.IsNullOrEmpty(ownerName))
        //        properties = properties.Where(p => EF.Functions.Like(p.OwnerName.ToLower(), $"%{ownerName}%"));

        //    // 5. AREA FILTERS
        //    var minArea = ExtractAreaAfter(input, "more than", "greater than", "bigger than", "jyada", "से ज्यादा");
        //    var maxArea = ExtractAreaAfter(input, "less than", "under", "below", "kam", "कम", "से कम");

        //    if (minArea.HasValue)
        //        properties = properties.Where(p => p.AreaAcre + p.AreaGunta / 40m > minArea.Value);
        //    if (maxArea.HasValue)
        //        properties = properties.Where(p => p.AreaAcre + p.AreaGunta / 40m < maxArea.Value);

        //    // 6. GOVERNMENT LAND
        //    if (ContainsAny(input, "government", "govt", "sarkari", "सरकारी", "panchayat"))
        //    {
        //        properties = properties.Where(p =>
        //            p.OwnerName.ToLower().Contains("government") ||
        //            p.OwnerName.ToLower().Contains("gram panchayat") ||
        //            p.OwnerName.ToLower().Contains("panchayat"));
        //    }

        //    // 7. TAX PENDING
        //    if (ContainsAny(input, "tax pending", "tax not paid", "बकाया"))
        //        properties = properties.Where(p => p.LastPaidYear != "2024-25" && p.LastPaidYear != "2025");

        //    // 8. LAND TYPE FILTER — THIS FIXES "residential"
        //    if (ContainsAny(input, "residential", "house", "home", "मकान", "घर"))
        //        properties = properties.Where(p => p.LandType != null && p.LandType.ToLower().Contains("residential"));

        //    if (ContainsAny(input, "commercial", "shop", "दुकान"))
        //        properties = properties.Where(p => p.LandType != null && p.LandType.ToLower().Contains("commercial"));

        //    if (ContainsAny(input, "agricultural", "agri", "खेती", "कृषि", "wet", "dry"))
        //        properties = properties.Where(p => p.LandType != null &&
        //            (p.LandType.ToLower().Contains("agri") ||
        //             p.LandType.ToLower().Contains("wet") ||
        //             p.LandType.ToLower().Contains("dry")));

        //    // 9. TOP 10
        //    if (ContainsAny(input, "top 10", "biggest", "largest"))
        //    {
        //        var top10 = await properties
        //            .OrderByDescending(p => p.AreaAcre + p.AreaGunta / 40m)
        //            .Take(10)
        //            .Select(p => new
        //            {
        //                p.OwnerName,
        //                p.Village,
        //                Area = $"{p.AreaAcre} Acre {p.AreaGunta} Gunta",
        //                Survey = p.SurveyNumber + (p.SubDivision != null ? "/" + p.SubDivision : "")
        //            })
        //            .ToListAsync();
        //        return Json(new { type = "top10", data = top10 });
        //    }

        //    var results = await properties.Take(100).ToListAsync();

        //    return results.Any()
        //        ? Json(new { type = "property", data = results })
        //        : Json(new { error = "No results found" });
        //}

        //// ──────────────── KEEP YOUR EXISTING HELPERS BELOW (unchanged) ────────────────
        //private bool ContainsAny(string text, params string[] words)
        //    => words.Any(w => text.Contains(w.ToLowerInvariant()));

        //private string ExtractVillage(string text)
        //    => Regex.Match(text, @"(honnapur|yallatti|mudhol|kerur|bilagi|jamkhandi|bagalkot|rabakavi|badami|ilkal|mahalingpur|guledgudd)", RegexOptions.IgnoreCase)?.Value ?? "";

        //private string ExtractNumber(string text)
        //    => Regex.Match(text, @"\b\d{1,6}(?:[\/-]\w+)?\b")?.Value ?? "";

        //private string ExtractWordAfter(string text, params string[] triggers)
        //{
        //    foreach (var t in triggers)
        //    {
        //        var m = Regex.Match(text, $@"{Regex.Escape(t)}\s+([a-zA-Z\s]+?)(?:\s|$)", RegexOptions.IgnoreCase);
        //        if (m.Success) return m.Groups[1].Value.Trim();
        //    }
        //    return "";
        //}

        //private decimal? ExtractAreaAfter(string text, params string[] triggers)
        //{
        //    foreach (var t in triggers)
        //    {
        //        var m = Regex.Match(text, $@"{Regex.Escape(t)}\s*(\d+(?:\.\d+)?)\s*acre", RegexOptions.IgnoreCase);
        //        if (m.Success && decimal.TryParse(m.Groups[1].Value, out var val))
        //            return val;
        //    }
        //    return null;
        //}
        //===================================================================================================================================================================

        //[HttpGet]
        //public async Task<IActionResult> Search(string q)
        //{
        //    if (string.IsNullOrWhiteSpace(q))
        //        return Json(new { error = "Type anything..." });

        //    string input = " " + q.Trim().ToLowerInvariant() + " ";

        //    // 1. POPULATION
        //    if (input.Contains("population") || input.Contains("जनसंख्या"))
        //    {
        //        var village1 = ExtractWord(input, GetVillageList());
        //        var data = string.IsNullOrEmpty(village1)
        //            ? await _context.tblvillageinfo.ToListAsync()
        //            : await _context.tblvillageinfo.Where(v => EF.Functions.Like(v.VillageName.ToLower(), $"%{village1}%")).ToListAsync();
        //        return Json(new { type = "population", data });
        //    }

        //    var query = _context.tblproperty.AsQueryable();

        //    // === LAND TYPE FILTERS (supports AND logic) ===
        //    var landTypes = new List<string>();
        //    if (input.Contains("residential")) landTypes.Add("residential");
        //    if (input.Contains("garden")) landTypes.Add("garden");
        //    if (input.Contains("wet")) landTypes.Add("wet");
        //    if (input.Contains("dry")) landTypes.Add("dry");
        //    if (input.Contains("commercial")) landTypes.Add("commercial");
        //    if (input.Contains("agri")) landTypes.Add("agri");

        //    if (landTypes.Any())
        //    {
        //        query = query.Where(p => p.LandType != null &&
        //            landTypes.All(type => p.LandType.ToLower().Contains(type)));
        //    }

        //    // === VILLAGE ===
        //    var village = ExtractWord(input, GetVillageList());
        //    if (!string.IsNullOrEmpty(village))
        //        query = query.Where(p => EF.Functions.Like(p.Village.ToLower(), $"%{village}%"));

        //    // === SURVEY NUMBER ===
        //    var survey = Regex.Match(input, @"\b\d{1,6}(?:[\/-]\w+)?\b")?.Value;
        //    if (!string.IsNullOrEmpty(survey))
        //    {
        //        string s = survey.Replace("/", "").Replace("-", "");
        //        query = query.Where(p => p.SurveyNumber.Contains(s) ||
        //                                (p.SubDivision != null && p.SubDivision.Contains(s)));
        //    }

        //    // === AREA FILTERS (supports "under 5 acre", "bigger than 10", etc.) ===
        //    var minArea = ExtractNumberAfterKeywords(input, "more than", "greater than", "bigger than", "above", "over", "jyada");
        //    var maxArea = ExtractNumberAfterKeywords(input, "less than", "under", "below", "kam", "se kam");

        //    if (minArea.HasValue)
        //        query = query.Where(p => p.AreaAcre + p.AreaGunta / 40m > minArea.Value);

        //    if (maxArea.HasValue)
        //        query = query.Where(p => p.AreaAcre + p.AreaGunta / 40m < maxArea.Value);

        //    // === OWNER NAME ===
        //    var owner = ExtractWordAfterKeywords(input, "owner", "malik", "मालिक", "name");
        //    if (!string.IsNullOrEmpty(owner))
        //        query = query.Where(p => EF.Functions.Like(p.OwnerName.ToLower(), $"%{owner}%"));

        //    // === GOVERNMENT / TAX / TOP 10 (only if no other strong filters) ===
        //    if (!landTypes.Any() && string.IsNullOrEmpty(village) && string.IsNullOrEmpty(survey))
        //    {
        //        if (input.Contains("government") || input.Contains("sarkari") || input.Contains("govt"))
        //            query = query.Where(p => p.OwnerName.ToLower().Contains("government") || p.OwnerName.ToLower().Contains("panchayat"));

        //        else if (input.Contains("tax"))
        //            query = query.Where(p => p.LastPaidYear != "2024-25" && p.LastPaidYear != "2025");

        //        else if (input.Contains("top 10") || input.Contains("biggest"))
        //        {
        //            var top = await query.OrderByDescending(p => p.AreaAcre + p.AreaGunta / 40m).Take(10)
        //                .Select(p => new { p.OwnerName, p.Village, Area = $"{p.AreaAcre}A {p.AreaGunta}G", p.SurveyNumber })
        //                .ToListAsync();
        //            return Json(new { type = "top10", data = top });
        //        }
        //    }

        //    var results = await query.Take(100).ToListAsync();

        //    return results.Any()
        //        ? Json(new { type = "property", data = results })
        //        : Json(new { error = $"Nothing found for: {q}" });
        //}

        //// ——————————————— FINAL HELPERS ———————————————
        //private string[] GetVillageList() => new[] { "honnapur", "yallatti", "mudhol", "kerur", "bilagi", "jamkhandi", "bagalkot", "rabakavi", "badami", "ilkal", "mahalingpur", "guledgudd" };

        //private string ExtractWord(string text, string[] words)
        //    => words.FirstOrDefault(w => text.Contains(w)) ?? "";

        //private string ExtractWordAfterKeywords(string text, params string[] keywords)
        //{
        //    foreach (var kw in keywords)
        //    {
        //        var match = Regex.Match(text, $@"{kw}\s+([a-zA-Z\s]+?)(?:\s|$|\.|,)", RegexOptions.IgnoreCase);
        //        if (match.Success) return match.Groups[1].Value.Trim();
        //    }
        //    return "";
        //}

        //private decimal? ExtractNumberAfterKeywords(string text, params string[] keywords)
        //{
        //    foreach (var kw in keywords)
        //    {
        //        var match = Regex.Match(text, $@"{kw}\s+(\d+(?:\.\d+)?)", RegexOptions.IgnoreCase);
        //        if (match.Success && decimal.TryParse(match.Groups[1].Value, out var n))
        //            return n;
        //    }
        //    return null;
        //}

        //======================================================================================================================

        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new { error = "Please type something" });

            var lower = q.ToLowerInvariant();

            // Retrieve relevant records from YOUR DB only
            var records = await _context.tblproperty
                .Where(p =>
                    EF.Functions.Like(p.LandType ?? "", $"%{q}%") ||
                    EF.Functions.Like(p.Village ?? "", $"%{q}%") ||
                    EF.Functions.Like(p.OwnerName ?? "", $"%{q}%") ||
                    EF.Functions.Like(p.SurveyNumber ?? "", $"%{q}%") ||
                    lower.Contains("government") && p.OwnerName.Contains("Government") ||
                    lower.Contains("population")
                )
                .Take(25)
                .Select(p => new
                {
                    survey = p.SurveyNumber + (p.SubDivision != null ? "/" + p.SubDivision : ""),
                    owner = p.OwnerName,
                    village = p.Village,
                    area = $"{p.AreaAcre} Acre {p.AreaGunta} Gunta",
                    type = p.LandType,
                    tax = p.TaxAmount
                })
                .ToListAsync();

            var context = records.Any()
                ? string.Join("\n", records.Select(r => $"Survey: {r.survey}, Owner: {r.owner}, Village: {r.village}, Area: {r.area}, Type: {r.type}"))
                : "No records found in database.";

            // Use Ollama (offline) or Grok API
            string answer = "Sorry, no LLM configured yet.";

            try
            {
                var client = new HttpClient();
                var response = await client.PostAsJsonAsync("http://localhost:11434/api/generate", new
                {
                    model = "llama3.1:8b",
                    prompt = $"""
                You are a helpful Karnataka village land assistant.
                Use ONLY the data below to answer in simple English/Hinglish.

                DATA:
                {context}

                USER QUESTION: {q}
                ANSWER (natural, friendly tone):
                """,
                    stream = false
                });

                var json = await response.Content.ReadAsStringAsync();
                var lines = json.Split('\n').Where(l => l.Contains("\"response\""));
                answer = string.Join("", lines.Select(l => JObject.Parse(l)["response"]?.ToString()));
            }
            catch { answer = "LLM not running. Install Ollama + pull llama3.1:8b"; }

            return Json(new
            {
                answer,
                raw_records = records,
                matched_records_count = records.Count
            });
        }
    }
}
