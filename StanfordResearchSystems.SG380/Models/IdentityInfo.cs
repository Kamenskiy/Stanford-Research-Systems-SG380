using System;

namespace StanfordResearchSystems.SG380.Models
{
    public class IdentityInfo
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }

        public static IdentityInfo Parse(string response)
        {
            var parts = response.Split(',');
            if (parts.Length < 4) throw new FormatException("Invalid identity string");
            return new IdentityInfo
            {
                Manufacturer = parts[0],
                Model = parts[1],
                SerialNumber = parts[2]?.Replace("s/n", ""),
                FirmwareVersion = parts[3]?.Replace("ver", "")
            };
        }

        public override string ToString() => $"{Manufacturer},{Model},s/n{SerialNumber},ver{FirmwareVersion}";
    }
}