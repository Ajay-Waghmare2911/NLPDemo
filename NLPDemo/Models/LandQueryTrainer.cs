// LandQueryTrainer.cs  ← REPLACE ENTIRE FILE WITH THIS
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Text.RegularExpressions;

namespace NLPDemo.Models
{
    public class QueryInput
    {
        public string Text { get; set; } = string.Empty;
    }

    public class QueryPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedIntent { get; set; } = "property";
    }

    public class ParsedQuery
    {
        public string Intent { get; set; } = "property";
        public string SurveyNumber { get; set; } = "";
        public string Village { get; set; } = "";
        public decimal? MinArea { get; set; }   // bigger than
        public decimal? MaxArea { get; set; }   // less than / under
        public string Owner { get; set; } = "";
    }

    public static class LandQueryTrainer
    {
        private static PredictionEngine<QueryInput, QueryPrediction>? _engine;

        public static void Initialize()
        {
            if (File.Exists("wwwroot/models/LandQueryModel.zip"))
            {
                var mlContext = new MLContext();
                using var stream = File.OpenRead("wwwroot/models/LandQueryModel.zip");
                var model = mlContext.Model.Load(stream, out var _);
                _engine = mlContext.Model.CreatePredictionEngine<QueryInput, QueryPrediction>(model);
            }
        }

        public static ParsedQuery Parse(string query)
        {
            query = query.ToLower().Trim();
            var result = new ParsedQuery();

            // Population?
            if (Regex.IsMatch(query, @"population|जनसंख्या|लोकसंख्या|people"))
            {
                result.Intent = "population";
                return result;
            }

            // Survey number
            var surveyMatch = Regex.Match(query, @"\b(\d{1,5}(?:\/\w+)?)\b");
            if (surveyMatch.Success)
                result.SurveyNumber = surveyMatch.Value;

            // Village
            var villageMatch = Regex.Match(query, @"(honnapur|yallatti|mudhol|kerur|bilagi|jamkhandi|bagalkot|rabakavi|badami|ilkal|guledgudd|mahalingpur)", RegexOptions.IgnoreCase);
            if (villageMatch.Success)
                result.Village = villageMatch.Value;

            // BIGGER THAN → MinArea
            // "more than 9 acres", "greater than 7 acre", "9 acre se jyada"
            var bigMatch = Regex.Match(query, @"(?:more|greater|bigger|jyada|से ज्यादा)\s*than\s+(\d+(?:\.\d+)?)\s*acre", RegexOptions.IgnoreCase);
            if (!bigMatch.Success)
                bigMatch = Regex.Match(query, @"(\d+(?:\.\d+)?)\s*acre\s*(?:se jyada|se zyada|or more)", RegexOptions.IgnoreCase);

            if (bigMatch.Success && decimal.TryParse(bigMatch.Groups[1].Value, out var min))
                result.MinArea = min;

            // "less than 10 acres", "under 8 acre", "10 acre se kam"
            var lessMatch = Regex.Match(query, @"(?:less|under|below|kam|कम|से कम)\s*than\s+(\d+(?:\.\d+)?)\s*acre", RegexOptions.IgnoreCase);
            if (!lessMatch.Success)
                lessMatch = Regex.Match(query, @"(\d+(?:\.\d+)?)\s*acre\s*(?:se kam|se chhota|or less|below)", RegexOptions.IgnoreCase);

            if (lessMatch.Success && decimal.TryParse(lessMatch.Groups[1].Value, out var max))
                result.MaxArea = max;

            // Owner
            var ownerMatch = Regex.Match(query, @"owner\s+([a-z\s]+)", RegexOptions.IgnoreCase);
            if (ownerMatch.Success)
                result.Owner = ownerMatch.Groups[1].Value.Trim();

            return result;
        }
    }
}