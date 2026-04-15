using System.Collections.Generic;
using System.Linq;

namespace StanfordResearchSystems.SG380.Models
{
    public class ErrorInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public static List<ErrorInfo> ParseErrors(string response)
        {
            var list = new List<ErrorInfo>();
            if (string.IsNullOrWhiteSpace(response)) return list;
            var lines = response.Split('\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ', 2);
                if (parts.Length >= 2 && int.TryParse(parts[0], out int code))
                    list.Add(new ErrorInfo { Code = code, Message = parts[1] });
                else
                    list.Add(new ErrorInfo { Code = -1, Message = line });
            }
            return list;
        }
    }
}