using StanfordResearchSystems.SG380;
using StanfordResearchSystems.SG380.Enums;

class Program
{
    static async Task Main()
    {
        using (var client = new Sg380Client("192.168.88.101"))
        {
            await client.ConnectAsync();
            var id = await client.GetIdentityInfoAsync();
            Console.WriteLine(id);
            await client.ResetAsync();


            await client.SetFrequencyAsync(104e6);          // 104 МГц
            await client.SetTypeNAmplitudeAsync(0, AmplitudeUnit.dBm); // уровень 0 dBm
            await client.EnableTypeNOutputAsync(true);      // включить Type-N

            // Конфигурация Sweep
            await client.SetModulationTypeAsync(ModulationType.Sweep);
            await client.SetSweepFunctionAsync(SweepFunction.Triangle); // треугольник (туда-обратно)
            await client.SetSweepRateAsync(10);             // скорость 10 Гц
            await client.SetSweepDeviationAsync(4e6);       // девиация ±4 МГц

            // Включить модуляцию
            await client.EnableModulationAsync(true);

            //await client.ResetAsync();
            //await client.GetLastErrorsAsync();
            //await client.SetFrequencyAsync(0);
            //await client.GetLastErrorsAsync();
            //await client.SetTypeNAmplitudeAsync(0.0, AmplitudeUnit.dBm);
            //await client.GetLastErrorsAsync();
            ////await client.SetModulationTypeAsync(ModulationType.FM);
            ////await client.SetModulationFunctionAsync(ModulationFunction.Sine);
            ////await client.SetFmDeviationAsync(10e3);
            ////await client.SetModulationRateAsync(1e3);
            ////await client.EnableModulationAsync(false);
            //for (double i = 0; i < 100e7; i++)
            //{
            //    await client.SetFrequencyAsync(i);
            //    var e = await client.GetLastErrorsAsync();
            //    Console.WriteLine("Frequency: " + await client.GetFrequencyAsync());
            //}

            //Console.WriteLine("Frequency: " + await client.GetFrequencyAsync());
            //Console.WriteLine("Type-N amplitude: " + await client.GetTypeNAmplitudeAsync());

            //await client.EnableModulationAsync(false);
        }
    }
}