# Stanford Research Systems SG380 .NET Library

Библиотека для удалённого управления RF-генераторами серии Stanford Research Systems SG380 (SG382, SG384, SG386) через Ethernet (TCP/IP).

## 📋 Описание

Библиотека предоставляет полный контроль над генераторами сигналов SG380 через Ethernet-интерфейс. Поддерживаются все основные функции прибора:

- Управление частотой, фазой и амплитудой
- Все типы модуляции (AM, FM, ΦM, Sweep, Pulse, Blank, IQ)
- Управление списками состояний (List Mode)
- Чтение статусов и ошибок
- Полная поддержка IEEE-488.2 команд

## 🚀 Установка

### Склонируйте репозиторий и добавление ссылки на проект

```bash
git clone https://github.com/Kamenskiy/StanfordResearchSystems.SG380.git
```

Добавьте в ваш .csproj файл:
```xml
<ItemGroup>
  <ProjectReference Include="..\StanfordResearchSystems.SG380\StanfordResearchSystems.SG380.csproj" />
</ItemGroup>
```

## 📦 Требования
.NET 10.0 или выше
Сетевое подключение к генератору SG380
Порт 5025 (TCP/IP raw socket)

## 🔌 Быстрый старт
```csharp
using StanfordResearchSystems.SG380;
using StanfordResearchSystems.SG380.Enums;

class Program
{
    static async Task Main()
    {
        using var client = new Sg380Client("192.168.88.101");
        await client.ConnectAsync();
        
        var identity = await client.GetIdentityInfoAsync();
        Console.WriteLine(identity);
    }
}
```
## 📖 Примеры использования
### 1. Базовая настройка сигнала
```csharp
using StanfordResearchSystems.SG380;
using StanfordResearchSystems.SG380.Enums;

using var client = new Sg380Client("192.168.88.101");
await client.ConnectAsync();

// Сброс в настройки по умолчанию
await client.ResetAsync();

// Установка параметров
await client.SetFrequencyAsync(100e6);                       // 100 МГц
await client.SetPhaseAsync(45);                             // 45 градусов
await client.SetTypeNAmplitudeAsync(0, AmplitudeUnit.dBm);  // 0 dBm
await client.EnableTypeNOutputAsync(true);                  // Включить выход

// Чтение параметров
double freq = await client.GetFrequencyAsync();
double phase = await client.GetPhaseAsync();
double amplitude = await client.GetTypeNAmplitudeAsync();

Console.WriteLine($"Частота: {freq} Гц");
Console.WriteLine($"Фаза: {phase}°");
Console.WriteLine($"Амплитуда: {amplitude} dBm");
```

### 2. Sweep-модуляция (качание частоты)
```csharp
// Настройка несущей
await client.SetFrequencyAsync(104e6);                      // 104 МГц
await client.SetTypeNAmplitudeAsync(0, AmplitudeUnit.dBm);
await client.EnableTypeNOutputAsync(true);

// Конфигурация Sweep
await client.SetModulationTypeAsync(ModulationType.Sweep);
await client.SetSweepFunctionAsync(SweepFunction.Triangle); // треугольник
await client.SetSweepRateAsync(10);                        // скорость 10 Гц
await client.SetSweepDeviationAsync(4e6);                  // девиация ±4 МГц

// Включение модуляции
await client.EnableModulationAsync(true);

Console.WriteLine("Sweep модуляция активна");
```

### 3. FM-модуляция
```csharp
await client.SetModulationTypeAsync(ModulationType.FM);
await client.SetModulationFunctionAsync(ModulationFunction.Sine);
await client.SetModulationRateAsync(1e3);        // 1 кГц
await client.SetFmDeviationAsync(10e3);          // девиация ±10 кГц
await client.EnableModulationAsync(true);

double rate = await client.GetModulationRateAsync();
double deviation = await client.GetFmDeviationAsync();
Console.WriteLine($"FM: {rate} Гц, девиация {deviation} Гц");
```

### 4. AM-модуляция
```csharp
await client.SetModulationTypeAsync(ModulationType.AM);
await client.SetModulationFunctionAsync(ModulationFunction.Sine);
await client.SetModulationRateAsync(1e3);        // 1 кГц
await client.SetAmDepthAsync(50);                // глубина 50%
await client.EnableModulationAsync(true);

double depth = await client.GetAmDepthAsync();
Console.WriteLine($"AM глубина: {depth}%");
```

### 5. Phase-модуляция
```csharp
await client.SetModulationTypeAsync(ModulationType.Phase);
await client.SetModulationFunctionAsync(ModulationFunction.Sine);
await client.SetModulationRateAsync(1e3);        // 1 кГц
await client.SetPhaseDeviationAsync(45);         // девиация ±45°
await client.EnableModulationAsync(true);

double phaseDev = await client.GetPhaseDeviationAsync();
Console.WriteLine($"ΦM девиация: {phaseDev}°");
```

### 6. Pulse-модуляция
```csharp
await client.SetModulationTypeAsync(ModulationType.Pulse);
await client.SetPulseFunctionAsync(PulseFunction.Square);
await client.SetPulsePeriodAsync(1e-3);          // период 1 мс
await client.SetPulseWidthAsync(100e-6);         // ширина 100 мкс
await client.EnableModulationAsync(true);

// Альтернативно через коэффициент заполнения
await client.SetPulseDutyFactorAsync(30);        // 30% duty cycle
```

### 7. Сканирование частот через список (List Mode)
```csharp
// Создание списка
int size = await client.CreateListAsync(5);

// Настройка частот от 100 до 500 МГц
await client.SetListPointAsync(0, new ListState { Frequency = 100e6 });
await client.SetListPointAsync(1, new ListState { Frequency = 200e6 });
await client.SetListPointAsync(2, new ListState { Frequency = 300e6 });
await client.SetListPointAsync(3, new ListState { Frequency = 400e6 });
await client.SetListPointAsync(4, new ListState { Frequency = 500e6 });

// Включение режима списка
await client.EnableListAsync(true);

// Запуск сканирования
for (int i = 0; i < 5; i++)
{
    await client.TriggerAsync();
    Console.WriteLine($"Частота: {await client.GetFrequencyAsync() / 1e6:F1} МГц");
    await Task.Delay(500);
}

// Очистка
await client.DeleteListAsync();
```

### 8. Работа с BNC выходом (НЧ диапазон)
```csharp
// BNC выход работает до 62.5 МГц
await client.SetFrequencyAsync(10e6);                           // 10 МГц
await client.SetBncAmplitudeAsync(0.5, AmplitudeUnit.rms);     // 0.5 В RMS
await client.SetBncOffsetAsync(0.2);                           // смещение 0.2 В
await client.EnableBncOutputAsync(true);

double vpp = await client.GetBncAmplitudeAsync(AmplitudeUnit.Vpp);
double offset = await client.GetBncOffsetAsync();
Console.WriteLine($"BNC: {vpp:F2} Vpp, смещение {offset:F2} В");
```

### 9. Чтение статусов и ошибок
```csharp
// Получение статусов
int statusByte = await client.GetStatusByteAsync();
int standardEvent = await client.GetStandardEventRegisterAsync();
int instrumentStatus = await client.GetInstrumentStatusRegisterAsync();

// Получение ошибок
var errors = await client.GetLastErrorsAsync();
foreach (var error in errors)
{
    Console.WriteLine($"Ошибка {error.Code}: {error.Message}");
}

// Температура RF блока
double temp = await client.GetRfBlockTemperatureAsync();
Console.WriteLine($"Температура: {temp:F1} °C");
```

### 10. Сохранение и восстановление настроек
```csharp
// Сохранение в ячейку 5
await client.SaveSettingsAsync(5);

// Изменение настроек
await client.SetFrequencyAsync(200e6);
await client.SetTypeNAmplitudeAsync(-10, AmplitudeUnit.dBm);

// Восстановление
await client.RecallSettingsAsync(5);
Console.WriteLine($"Восстановлена частота: {await client.GetFrequencyAsync()} Гц");
```
