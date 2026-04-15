using System;
using System.Globalization;

namespace StanfordResearchSystems.SG380.Models
{
    public class ListState
    {
        public double? Frequency { get; set; }
        public double? Phase { get; set; }
        public double? BncAmplitude { get; set; }
        public double? BncOffset { get; set; }
        public double? TypeNAmplitude { get; set; }
        public int? Display { get; set; }
        public int? EnablesDisables { get; set; }
        public int? ModulationType { get; set; }
        public int? ModulationFunction { get; set; }
        public double? ModulationRate { get; set; }
        public double? ModulationDeviation { get; set; }
        public double? ClockAmplitude { get; set; }
        public double? ClockOffset { get; set; }
        public double? DoublerAmplitude { get; set; }
        public double? RearDcOffset { get; set; }

        public string ToCommandString()
        {
            var values = new string[15];
            values[0] = Frequency?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[1] = Phase?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[2] = BncAmplitude?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[3] = BncOffset?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[4] = TypeNAmplitude?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[5] = Display?.ToString() ?? "N";
            values[6] = EnablesDisables?.ToString() ?? "N";
            values[7] = ModulationType?.ToString() ?? "N";
            values[8] = ModulationFunction?.ToString() ?? "N";
            values[9] = ModulationRate?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[10] = ModulationDeviation?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[11] = ClockAmplitude?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[12] = ClockOffset?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[13] = DoublerAmplitude?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            values[14] = RearDcOffset?.ToString("G", CultureInfo.InvariantCulture) ?? "N";
            return string.Join(",", values);
        }

        public static ListState Parse(string response)
        {
            var parts = response.Split(',');
            if (parts.Length != 15) throw new FormatException("Invalid list state string");
            var state = new ListState();
            if (parts[0] != "N") state.Frequency = double.Parse(parts[0], CultureInfo.InvariantCulture);
            if (parts[1] != "N") state.Phase = double.Parse(parts[1], CultureInfo.InvariantCulture);
            if (parts[2] != "N") state.BncAmplitude = double.Parse(parts[2], CultureInfo.InvariantCulture);
            if (parts[3] != "N") state.BncOffset = double.Parse(parts[3], CultureInfo.InvariantCulture);
            if (parts[4] != "N") state.TypeNAmplitude = double.Parse(parts[4], CultureInfo.InvariantCulture);
            if (parts[5] != "N") state.Display = int.Parse(parts[5]);
            if (parts[6] != "N") state.EnablesDisables = int.Parse(parts[6]);
            if (parts[7] != "N") state.ModulationType = int.Parse(parts[7]);
            if (parts[8] != "N") state.ModulationFunction = int.Parse(parts[8]);
            if (parts[9] != "N") state.ModulationRate = double.Parse(parts[9], CultureInfo.InvariantCulture);
            if (parts[10] != "N") state.ModulationDeviation = double.Parse(parts[10], CultureInfo.InvariantCulture);
            if (parts[11] != "N") state.ClockAmplitude = double.Parse(parts[11], CultureInfo.InvariantCulture);
            if (parts[12] != "N") state.ClockOffset = double.Parse(parts[12], CultureInfo.InvariantCulture);
            if (parts[13] != "N") state.DoublerAmplitude = double.Parse(parts[13], CultureInfo.InvariantCulture);
            if (parts[14] != "N") state.RearDcOffset = double.Parse(parts[14], CultureInfo.InvariantCulture);
            return state;
        }
    }
}