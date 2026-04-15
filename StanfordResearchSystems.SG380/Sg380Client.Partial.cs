using System.Globalization;

using StanfordResearchSystems.SG380.Enums;
using StanfordResearchSystems.SG380.Models;

namespace StanfordResearchSystems.SG380
{
    public sealed partial class Sg380Client
    {
        #region Common IEEE-488.2 Commands

        /// <summary>
        /// Возвращает строку идентификации прибора (производитель, модель, серийный номер, версия прошивки).
        /// </summary>
        public string GetIdentity() => SendQuery("*IDN?\n");

        /// <inheritdoc cref="GetIdentity"/>
        public async Task<string> GetIdentityAsync(CancellationToken ct = default) => await SendQueryAsync("*IDN?\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает разобранную информацию идентификации.
        /// </summary>
        public IdentityInfo GetIdentityInfo() => IdentityInfo.Parse(GetIdentity());

        /// <inheritdoc cref="GetIdentityInfo"/>
        public async Task<IdentityInfo> GetIdentityInfoAsync(CancellationToken ct = default) => IdentityInfo.Parse(await GetIdentityAsync(ct).ConfigureAwait(false));

        /// <summary>
        /// Сбрасывает прибор в настройки по умолчанию.
        /// </summary>
        public void Reset() => SendCommand("*RST\n");

        /// <inheritdoc cref="Reset"/>
        public async Task ResetAsync(CancellationToken ct = default) => await SendCommandAsync("*RST\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Очищает регистры событий и буфер ошибок.
        /// </summary>
        public void ClearStatus() => SendCommand("*CLS\n");

        /// <inheritdoc cref="ClearStatus"/>
        public async Task ClearStatusAsync(CancellationToken ct = default) => await SendCommandAsync("*CLS\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущее значение маски регистра событий стандартного статуса.
        /// </summary>
        public int GetStandardEventEnable() => int.Parse(SendQuery("*ESE?\n"));

        /// <summary>
        /// Устанавливает маску регистра событий стандартного статуса.
        /// </summary>
        /// <param name="mask">Битовая маска.</param>
        public void SetStandardEventEnable(int mask) => SendCommand($"*ESE {mask}\n");

        /// <inheritdoc cref="GetStandardEventEnable"/>
        public async Task<int> GetStandardEventEnableAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("*ESE?\n", ct).ConfigureAwait(false));

        /// <inheritdoc cref="SetStandardEventEnable"/>
        public async Task SetStandardEventEnableAsync(int mask, CancellationToken ct = default) => await SendCommandAsync($"*ESE {mask}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает и очищает регистр событий стандартного статуса.
        /// </summary>
        public int GetStandardEventRegister() => int.Parse(SendQuery("*ESR?\n"));

        /// <inheritdoc cref="GetStandardEventRegister"/>
        public async Task<int> GetStandardEventRegisterAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("*ESR?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Возвращает флаг очистки регистров при включении питания.
        /// </summary>
        public int GetPowerOnStatusClear() => int.Parse(SendQuery("*PSC?\n"));

        /// <summary>
        /// Устанавливает флаг очистки регистров при включении питания.
        /// </summary>
        /// <param name="value">0 – сохранять *SRE и *ESE, 1 – очищать при включении.</param>
        public void SetPowerOnStatusClear(int value) => SendCommand($"*PSC {value}\n");

        /// <inheritdoc cref="GetPowerOnStatusClear"/>
        public async Task<int> GetPowerOnStatusClearAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("*PSC?\n", ct).ConfigureAwait(false));

        /// <inheritdoc cref="SetPowerOnStatusClear"/>
        public async Task SetPowerOnStatusClearAsync(int value, CancellationToken ct = default) => await SendCommandAsync($"*PSC {value}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Сохраняет текущие настройки в ячейку энергонезависимой памяти (0–9). Ячейка 0 зарезервирована.
        /// </summary>
        /// <param name="location">Номер ячейки (0–9).</param>
        public void SaveSettings(int location) => SendCommand($"*SAV {location}\n");

        /// <inheritdoc cref="SaveSettings"/>
        public async Task SaveSettingsAsync(int location, CancellationToken ct = default) => await SendCommandAsync($"*SAV {location}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Восстанавливает настройки из ячейки памяти (0–9). Ячейка 0 соответствует настройкам по умолчанию.
        /// </summary>
        /// <param name="location">Номер ячейки (0–9).</param>
        public void RecallSettings(int location) => SendCommand($"*RCL {location}\n");

        /// <inheritdoc cref="RecallSettings"/>
        public async Task RecallSettingsAsync(int location, CancellationToken ct = default) => await SendCommandAsync($"*RCL {location}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущее значение маски регистра обслуживания (SRE).
        /// </summary>
        public int GetServiceRequestEnable() => int.Parse(SendQuery("*SRE?\n"));

        /// <summary>
        /// Устанавливает маску регистра обслуживания (SRE).
        /// </summary>
        /// <param name="mask">Битовая маска.</param>
        public void SetServiceRequestEnable(int mask) => SendCommand($"*SRE {mask}\n");

        /// <inheritdoc cref="GetServiceRequestEnable"/>
        public async Task<int> GetServiceRequestEnableAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("*SRE?\n", ct).ConfigureAwait(false));

        /// <inheritdoc cref="SetServiceRequestEnable"/>
        public async Task SetServiceRequestEnableAsync(int mask, CancellationToken ct = default) => await SendCommandAsync($"*SRE {mask}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает байт статуса (STB). Регистр не очищается.
        /// </summary>
        public int GetStatusByte() => int.Parse(SendQuery("*STB?\n"));

        /// <inheritdoc cref="GetStatusByte"/>
        public async Task<int> GetStatusByteAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("*STB?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Ожидает завершения всех предыдущих команд.
        /// </summary>
        public void OperationComplete() => SendQuery("*OPC?\n");

        /// <inheritdoc cref="OperationComplete"/>
        public async Task OperationCompleteAsync(CancellationToken ct = default) => await SendQueryAsync("*OPC?\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Запускает самотест прибора. Возвращает true, если тест пройден успешно.
        /// </summary>
        public bool SelfTest() => SendQuery("*TST?\n") == "0";

        /// <inheritdoc cref="SelfTest"/>
        public async Task<bool> SelfTestAsync(CancellationToken ct = default) => (await SendQueryAsync("*TST?\n", ct).ConfigureAwait(false)) == "0";

        /// <summary>
        /// Генерирует триггер для режима списка.
        /// </summary>
        public void Trigger() => SendCommand("*TRG\n");

        /// <inheritdoc cref="Trigger"/>
        public async Task TriggerAsync(CancellationToken ct = default) => await SendCommandAsync("*TRG\n", ct).ConfigureAwait(false);

        #endregion

        #region Signal Synthesis Commands

        /// <summary>
        /// Устанавливает частоту выходного сигнала (в герцах).
        /// </summary>
        /// <param name="frequencyHz">Частота в Гц.</param>
        public void SetFrequency(double frequencyHz) => SendCommand($"FREQ {frequencyHz.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetFrequency"/>
        public async Task SetFrequencyAsync(double frequencyHz, CancellationToken ct = default) => await SendCommandAsync($"FREQ {frequencyHz.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущее значение частоты (Гц).
        /// </summary>
        public double GetFrequency() => double.Parse(SendQuery("FREQ?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetFrequency"/>
        public async Task<double> GetFrequencyAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("FREQ?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает фазу выходного сигнала (в градусах).
        /// </summary>
        /// <param name="degrees">Значение фазы в градусах (±360°).</param>
        public void SetPhase(double degrees) => SendCommand($"PHAS {degrees.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPhase"/>
        public async Task SetPhaseAsync(double degrees, CancellationToken ct = default) => await SendCommandAsync($"PHAS {degrees.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущее значение фазы (градусы).
        /// </summary>
        public double GetPhase() => double.Parse(SendQuery("PHAS?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPhase"/>
        public async Task<double> GetPhaseAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("PHAS?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Сбрасывает отображаемую фазу в 0° без изменения реального фазового сдвига.
        /// </summary>
        public void RelPhase() => SendCommand("RPHS\n");

        /// <inheritdoc cref="RelPhase"/>
        public async Task RelPhaseAsync(CancellationToken ct = default) => await SendCommandAsync("RPHS\n", ct).ConfigureAwait(false);

        // Амплитуды
        /// <summary>
        /// Устанавливает амплитуду BNC‑выхода.
        /// </summary>
        /// <param name="value">Числовое значение.</param>
        /// <param name="unit">Единица измерения: dBm, rms или Vpp.</param>
        public void SetBncAmplitude(double value, AmplitudeUnit unit = AmplitudeUnit.dBm) => SendCommand($"AMPL {value} {unit.ToString()}\n");

        /// <inheritdoc cref="SetBncAmplitude"/>
        public async Task SetBncAmplitudeAsync(double value, AmplitudeUnit unit = AmplitudeUnit.dBm, CancellationToken ct = default) => await SendCommandAsync($"AMPL {value} {unit.ToString()}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает амплитуду BNC‑выхода.
        /// </summary>
        /// <param name="unit">Желаемая единица измерения.</param>
        public double GetBncAmplitude(AmplitudeUnit unit = AmplitudeUnit.dBm) => double.Parse(SendQuery($"AMPL? {unit.ToString()}\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetBncAmplitude"/>
        public async Task<double> GetBncAmplitudeAsync(AmplitudeUnit unit = AmplitudeUnit.dBm, CancellationToken ct = default) => double.Parse(await SendQueryAsync($"AMPL? {unit.ToString()}\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает амплитуду Type‑N выхода.
        /// </summary>
        /// <param name="value">Числовое значение.</param>
        /// <param name="unit">Единица измерения: dBm, rms или Vpp.</param>
        public void SetTypeNAmplitude(double value, AmplitudeUnit unit = AmplitudeUnit.dBm) => SendCommand($"AMPR {value} {unit.ToString()}\n");

        /// <inheritdoc cref="SetTypeNAmplitude"/>
        public async Task SetTypeNAmplitudeAsync(double value, AmplitudeUnit unit = AmplitudeUnit.dBm, CancellationToken ct = default) => await SendCommandAsync($"AMPR {value} {unit.ToString()}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает амплитуду Type‑N выхода.
        /// </summary>
        /// <param name="unit">Желаемая единица измерения.</param>
        public double GetTypeNAmplitude(AmplitudeUnit unit = AmplitudeUnit.dBm) => double.Parse(SendQuery($"AMPR? {unit.ToString()}\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetTypeNAmplitude"/>
        public async Task<double> GetTypeNAmplitudeAsync(AmplitudeUnit unit = AmplitudeUnit.dBm, CancellationToken ct = default) => double.Parse(await SendQueryAsync($"AMPR? {unit.ToString()}\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает амплитуду дифференциальных часов (опция 1) в Вольтах пик‑пик.
        /// </summary>
        /// <param name="valueVpp">Амплитуда в Vpp.</param>
        public void SetClockAmplitude(double valueVpp) => SendCommand($"AMPC {valueVpp.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetClockAmplitude"/>
        public async Task SetClockAmplitudeAsync(double valueVpp, CancellationToken ct = default) => await SendCommandAsync($"AMPC {valueVpp.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает амплитуду дифференциальных часов (опция 1) в Vpp.
        /// </summary>
        public double GetClockAmplitude() => double.Parse(SendQuery("AMPC?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetClockAmplitude"/>
        public async Task<double> GetClockAmplitudeAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("AMPC?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает амплитуду выхода удвоителя (опция 2).
        /// </summary>
        /// <param name="value">Числовое значение.</param>
        /// <param name="unit">Единица измерения: dBm, rms или Vpp.</param>
        public void SetDoublerAmplitude(double value, AmplitudeUnit unit = AmplitudeUnit.dBm) => SendCommand($"AMPH {value} {unit.ToString()}\n");

        /// <inheritdoc cref="SetDoublerAmplitude"/>
        public async Task SetDoublerAmplitudeAsync(double value, AmplitudeUnit unit = AmplitudeUnit.dBm, CancellationToken ct = default) => await SendCommandAsync($"AMPH {value} {unit.ToString()}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает амплитуду выхода удвоителя (опция 2).
        /// </summary>
        /// <param name="unit">Желаемая единица измерения.</param>
        public double GetDoublerAmplitude(AmplitudeUnit unit = AmplitudeUnit.dBm) => double.Parse(SendQuery($"AMPH? {unit.ToString()}\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetDoublerAmplitude"/>
        public async Task<double> GetDoublerAmplitudeAsync(AmplitudeUnit unit = AmplitudeUnit.dBm, CancellationToken ct = default) => double.Parse(await SendQueryAsync($"AMPH? {unit.ToString()}\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        // Смещения (Offset)
        /// <summary>
        /// Устанавливает постоянное смещение BNC‑выхода (вольты).
        /// </summary>
        /// <param name="volts">Напряжение смещения в вольтах.</param>
        public void SetBncOffset(double volts) => SendCommand($"OFSL {volts.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetBncOffset"/>
        public async Task SetBncOffsetAsync(double volts, CancellationToken ct = default) => await SendCommandAsync($"OFSL {volts.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает постоянное смещение BNC‑выхода (вольты).
        /// </summary>
        public double GetBncOffset() => double.Parse(SendQuery("OFSL?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetBncOffset"/>
        public async Task<double> GetBncOffsetAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("OFSL?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает постоянное смещение дифференциальных часов (опция 1) в вольтах.
        /// </summary>
        /// <param name="volts">Напряжение смещения в вольтах.</param>
        public void SetClockOffset(double volts) => SendCommand($"OFSC {volts.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetClockOffset"/>
        public async Task SetClockOffsetAsync(double volts, CancellationToken ct = default) => await SendCommandAsync($"OFSC {volts.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает постоянное смещение дифференциальных часов (опция 1) в вольтах.
        /// </summary>
        public double GetClockOffset() => double.Parse(SendQuery("OFSC?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetClockOffset"/>
        public async Task<double> GetClockOffsetAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("OFSC?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает выход постоянного напряжения на задней панели (опция 2) в вольтах.
        /// </summary>
        /// <param name="volts">Напряжение в вольтах.</param>
        public void SetRearDcOffset(double volts) => SendCommand($"OFSD {volts.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetRearDcOffset"/>
        public async Task SetRearDcOffsetAsync(double volts, CancellationToken ct = default) => await SendCommandAsync($"OFSD {volts.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает выход постоянного напряжения на задней панели (опция 2) в вольтах.
        /// </summary>
        public double GetRearDcOffset() => double.Parse(SendQuery("OFSD?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetRearDcOffset"/>
        public async Task<double> GetRearDcOffsetAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("OFSD?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        // Включение выходов
        /// <summary>
        /// Включает или отключает BNC‑выход.
        /// </summary>
        /// <param name="enable">true – включить, false – отключить.</param>
        public void EnableBncOutput(bool enable) => SendCommand($"ENBL {(enable ? 1 : 0)}\n");

        /// <inheritdoc cref="EnableBncOutput"/>
        public async Task EnableBncOutputAsync(bool enable, CancellationToken ct = default) => await SendCommandAsync($"ENBL {(enable ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние BNC‑выхода.
        /// </summary>
        public bool IsBncOutputEnabled() => SendQuery("ENBL?\n") == "1";

        /// <inheritdoc cref="IsBncOutputEnabled"/>
        public async Task<bool> IsBncOutputEnabledAsync(CancellationToken ct = default) => (await SendQueryAsync("ENBL?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Включает или отключает Type‑N выход.
        /// </summary>
        /// <param name="enable">true – включить, false – отключить.</param>
        public void EnableTypeNOutput(bool enable) => SendCommand($"ENBR {(enable ? 1 : 0)}\n");

        /// <inheritdoc cref="EnableTypeNOutput"/>
        public async Task EnableTypeNOutputAsync(bool enable, CancellationToken ct = default) => await SendCommandAsync($"ENBR {(enable ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние Type‑N выхода.
        /// </summary>
        public bool IsTypeNOutputEnabled() => SendQuery("ENBR?\n") == "1";

        /// <inheritdoc cref="IsTypeNOutputEnabled"/>
        public async Task<bool> IsTypeNOutputEnabledAsync(CancellationToken ct = default) => (await SendQueryAsync("ENBR?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Включает или отключает выход дифференциальных часов (опция 1).
        /// </summary>
        /// <param name="enable">true – включить, false – отключить.</param>
        public void EnableClockOutput(bool enable) => SendCommand($"ENBC {(enable ? 1 : 0)}\n");

        /// <inheritdoc cref="EnableClockOutput"/>
        public async Task EnableClockOutputAsync(bool enable, CancellationToken ct = default) => await SendCommandAsync($"ENBC {(enable ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние выхода дифференциальных часов.
        /// </summary>
        public bool IsClockOutputEnabled() => SendQuery("ENBC?\n") == "1";

        /// <inheritdoc cref="IsClockOutputEnabled"/>
        public async Task<bool> IsClockOutputEnabledAsync(CancellationToken ct = default) => (await SendQueryAsync("ENBC?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Включает или отключает выход удвоителя (опция 2).
        /// </summary>
        /// <param name="enable">true – включить, false – отключить.</param>
        public void EnableDoublerOutput(bool enable) => SendCommand($"ENBH {(enable ? 1 : 0)}\n");

        /// <inheritdoc cref="EnableDoublerOutput"/>
        public async Task EnableDoublerOutputAsync(bool enable, CancellationToken ct = default) => await SendCommandAsync($"ENBH {(enable ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние выхода удвоителя.
        /// </summary>
        public bool IsDoublerOutputEnabled() => SendQuery("ENBH?\n") == "1";

        /// <inheritdoc cref="IsDoublerOutputEnabled"/>
        public async Task<bool> IsDoublerOutputEnabledAsync(CancellationToken ct = default) => (await SendQueryAsync("ENBH?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Устанавливает режим шума PLL (1 или 2) для оптимизации фазового шума.
        /// </summary>
        /// <param name="mode">1 – оптимизация вблизи несущей, 2 – оптимизация для отстроек > 100 кГц.</param>
        public void SetNoiseMode(int mode) => SendCommand($"NOIS {mode}\n");

        /// <inheritdoc cref="SetNoiseMode"/>
        public async Task SetNoiseModeAsync(int mode, CancellationToken ct = default) => await SendCommandAsync($"NOIS {mode}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущий режим шума PLL.
        /// </summary>
        public int GetNoiseMode() => int.Parse(SendQuery("NOIS?\n"));

        /// <inheritdoc cref="GetNoiseMode"/>
        public async Task<int> GetNoiseModeAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("NOIS?\n", ct).ConfigureAwait(false));

        #endregion

        #region Modulation Commands

        /// <summary>
        /// Включает или отключает модуляцию.
        /// </summary>
        /// <param name="enable">true – включить, false – отключить.</param>
        public void EnableModulation(bool enable) => SendCommand($"MODL {(enable ? 1 : 0)}\n");

        /// <inheritdoc cref="EnableModulation"/>
        public async Task EnableModulationAsync(bool enable, CancellationToken ct = default) => await SendCommandAsync($"MODL {(enable ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние модуляции.
        /// </summary>
        public bool IsModulationEnabled() => SendQuery("MODL?\n") == "1";

        /// <inheritdoc cref="IsModulationEnabled"/>
        public async Task<bool> IsModulationEnabledAsync(CancellationToken ct = default) => (await SendQueryAsync("MODL?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Устанавливает тип модуляции.
        /// </summary>
        /// <param name="type">Тип модуляции (AM, FM, ΦM, Sweep, Pulse, Blank, IQ).</param>
        public void SetModulationType(ModulationType type) => SendCommand($"TYPE {(int)type}\n");

        /// <inheritdoc cref="SetModulationType"/>
        public async Task SetModulationTypeAsync(ModulationType type, CancellationToken ct = default) => await SendCommandAsync($"TYPE {(int)type}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущий тип модуляции.
        /// </summary>
        public ModulationType GetModulationType() => (ModulationType)int.Parse(SendQuery("TYPE?\n"));

        /// <inheritdoc cref="GetModulationType"/>
        public async Task<ModulationType> GetModulationTypeAsync(CancellationToken ct = default) => (ModulationType)int.Parse(await SendQueryAsync("TYPE?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Устанавливает функцию модуляции для AM/FM/ΦM.
        /// </summary>
        /// <param name="function">Функция: синус, пила, треугольник, меандр, шум, внешний источник.</param>
        public void SetModulationFunction(ModulationFunction function) => SendCommand($"MFNC {(int)function}\n");

        /// <inheritdoc cref="SetModulationFunction"/>
        public async Task SetModulationFunctionAsync(ModulationFunction function, CancellationToken ct = default) => await SendCommandAsync($"MFNC {(int)function}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую функцию модуляции для AM/FM/ΦM.
        /// </summary>
        public ModulationFunction GetModulationFunction() => (ModulationFunction)int.Parse(SendQuery("MFNC?\n"));

        /// <inheritdoc cref="GetModulationFunction"/>
        public async Task<ModulationFunction> GetModulationFunctionAsync(CancellationToken ct = default) => (ModulationFunction)int.Parse(await SendQueryAsync("MFNC?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Устанавливает частоту модуляции (Гц) для AM/FM/ΦM.
        /// </summary>
        /// <param name="rateHz">Частота модуляции в Гц.</param>
        public void SetModulationRate(double rateHz) => SendCommand($"RATE {rateHz.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetModulationRate"/>
        public async Task SetModulationRateAsync(double rateHz, CancellationToken ct = default) => await SendCommandAsync($"RATE {rateHz.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает частоту модуляции (Гц) для AM/FM/ΦM.
        /// </summary>
        public double GetModulationRate() => double.Parse(SendQuery("RATE?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetModulationRate"/>
        public async Task<double> GetModulationRateAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("RATE?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает глубину амплитудной модуляции (AM) в процентах.
        /// </summary>
        /// <param name="percent">Глубина модуляции (0–100%).</param>
        public void SetAmDepth(double percent) => SendCommand($"ADEP {percent.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetAmDepth"/>
        public async Task SetAmDepthAsync(double percent, CancellationToken ct = default) => await SendCommandAsync($"ADEP {percent.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает глубину AM в процентах.
        /// </summary>
        public double GetAmDepth() => double.Parse(SendQuery("ADEP?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetAmDepth"/>
        public async Task<double> GetAmDepthAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("ADEP?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает глубину шумовой амплитудной модуляции (ANDP) в процентах.
        /// </summary>
        /// <param name="percent">Глубина модуляции (0–100%).</param>
        public void SetAmNoiseDepth(double percent) => SendCommand($"ANDP {percent.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetAmNoiseDepth"/>
        public async Task SetAmNoiseDepthAsync(double percent, CancellationToken ct = default) => await SendCommandAsync($"ANDP {percent.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает глубину шумовой AM в процентах.
        /// </summary>
        public double GetAmNoiseDepth() => double.Parse(SendQuery("ANDP?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetAmNoiseDepth"/>
        public async Task<double> GetAmNoiseDepthAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("ANDP?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает девиацию частотной модуляции (FM) в герцах.
        /// </summary>
        /// <param name="deviationHz">Девиация в Гц.</param>
        public void SetFmDeviation(double deviationHz) => SendCommand($"FDEV {deviationHz.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetFmDeviation"/>
        public async Task SetFmDeviationAsync(double deviationHz, CancellationToken ct = default) => await SendCommandAsync($"FDEV {deviationHz.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает девиацию FM в герцах.
        /// </summary>
        public double GetFmDeviation() => double.Parse(SendQuery("FDEV?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetFmDeviation"/>
        public async Task<double> GetFmDeviationAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("FDEV?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает среднеквадратичную девиацию шумовой FM (FNDV) в герцах.
        /// </summary>
        /// <param name="deviationHz">Среднеквадратичная девиация в Гц.</param>
        public void SetFmNoiseDeviation(double deviationHz) => SendCommand($"FNDV {deviationHz.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetFmNoiseDeviation"/>
        public async Task SetFmNoiseDeviationAsync(double deviationHz, CancellationToken ct = default) => await SendCommandAsync($"FNDV {deviationHz.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает среднеквадратичную девиацию шумовой FM в герцах.
        /// </summary>
        public double GetFmNoiseDeviation() => double.Parse(SendQuery("FNDV?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetFmNoiseDeviation"/>
        public async Task<double> GetFmNoiseDeviationAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("FNDV?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает девиацию фазовой модуляции (ΦM) в градусах.
        /// </summary>
        /// <param name="degrees">Девиация в градусах.</param>
        public void SetPhaseDeviation(double degrees) => SendCommand($"PDEV {degrees.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPhaseDeviation"/>
        public async Task SetPhaseDeviationAsync(double degrees, CancellationToken ct = default) => await SendCommandAsync($"PDEV {degrees.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает девиацию ΦM в градусах.
        /// </summary>
        public double GetPhaseDeviation() => double.Parse(SendQuery("PDEV?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPhaseDeviation"/>
        public async Task<double> GetPhaseDeviationAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("PDEV?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает среднеквадратичную девиацию шумовой ΦM (PNDV) в градусах.
        /// </summary>
        /// <param name="degrees">Среднеквадратичная девиация в градусах.</param>
        public void SetPhaseNoiseDeviation(double degrees) => SendCommand($"PNDV {degrees.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPhaseNoiseDeviation"/>
        public async Task SetPhaseNoiseDeviationAsync(double degrees, CancellationToken ct = default) => await SendCommandAsync($"PNDV {degrees.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает среднеквадратичную девиацию шумовой ΦM в градусах.
        /// </summary>
        public double GetPhaseNoiseDeviation() => double.Parse(SendQuery("PNDV?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPhaseNoiseDeviation"/>
        public async Task<double> GetPhaseNoiseDeviationAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("PNDV?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        // Sweep
        /// <summary>
        /// Устанавливает функцию Sweep (синус, пила, треугольник, внешний).
        /// </summary>
        /// <param name="function">Функция модуляции.</param>
        public void SetSweepFunction(SweepFunction function) => SendCommand($"SFNC {(int)function}\n");

        /// <inheritdoc cref="SetSweepFunction"/>
        public async Task SetSweepFunctionAsync(SweepFunction function, CancellationToken ct = default) => await SendCommandAsync($"SFNC {(int)function}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую функцию Sweep.
        /// </summary>
        public SweepFunction GetSweepFunction() => (SweepFunction)int.Parse(SendQuery("SFNC?\n"));

        /// <inheritdoc cref="GetSweepFunction"/>
        public async Task<SweepFunction> GetSweepFunctionAsync(CancellationToken ct = default) => (SweepFunction)int.Parse(await SendQueryAsync("SFNC?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Устанавливает скорость Sweep (Гц).
        /// </summary>
        /// <param name="rateHz">Скорость качания частоты в Гц.</param>
        public void SetSweepRate(double rateHz) => SendCommand($"SRAT {rateHz.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetSweepRate"/>
        public async Task SetSweepRateAsync(double rateHz, CancellationToken ct = default) => await SendCommandAsync($"SRAT {rateHz.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает скорость Sweep (Гц).
        /// </summary>
        public double GetSweepRate() => double.Parse(SendQuery("SRAT?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetSweepRate"/>
        public async Task<double> GetSweepRateAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("SRAT?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает девиацию Sweep (Гц).
        /// </summary>
        /// <param name="deviationHz">Девиация частоты в Гц.</param>
        public void SetSweepDeviation(double deviationHz) => SendCommand($"SDEV {deviationHz.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetSweepDeviation"/>
        public async Task SetSweepDeviationAsync(double deviationHz, CancellationToken ct = default) => await SendCommandAsync($"SDEV {deviationHz.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает девиацию Sweep (Гц).
        /// </summary>
        public double GetSweepDeviation() => double.Parse(SendQuery("SDEV?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetSweepDeviation"/>
        public async Task<double> GetSweepDeviationAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("SDEV?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        // Pulse/Blank
        /// <summary>
        /// Устанавливает функцию импульсной модуляции (меандр, PRBS, внешний).
        /// </summary>
        /// <param name="function">Функция импульсной модуляции.</param>
        public void SetPulseFunction(PulseFunction function) => SendCommand($"PFNC {(int)function}\n");

        /// <inheritdoc cref="SetPulseFunction"/>
        public async Task SetPulseFunctionAsync(PulseFunction function, CancellationToken ct = default) => await SendCommandAsync($"PFNC {(int)function}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую функцию импульсной модуляции.
        /// </summary>
        public PulseFunction GetPulseFunction() => (PulseFunction)int.Parse(SendQuery("PFNC?\n"));

        /// <inheritdoc cref="GetPulseFunction"/>
        public async Task<PulseFunction> GetPulseFunctionAsync(CancellationToken ct = default) => (PulseFunction)int.Parse(await SendQueryAsync("PFNC?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Устанавливает период импульсной модуляции (секунды).
        /// </summary>
        /// <param name="seconds">Период в секундах.</param>
        public void SetPulsePeriod(double seconds) => SendCommand($"PPER {seconds.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPulsePeriod"/>
        public async Task SetPulsePeriodAsync(double seconds, CancellationToken ct = default) => await SendCommandAsync($"PPER {seconds.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает период импульсной модуляции (секунды).
        /// </summary>
        public double GetPulsePeriod() => double.Parse(SendQuery("PPER?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPulsePeriod"/>
        public async Task<double> GetPulsePeriodAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("PPER?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает длительность импульса (секунды) для импульсной модуляции.
        /// </summary>
        /// <param name="seconds">Длительность в секундах.</param>
        public void SetPulseWidth(double seconds) => SendCommand($"PWID {seconds.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPulseWidth"/>
        public async Task SetPulseWidthAsync(double seconds, CancellationToken ct = default) => await SendCommandAsync($"PWID {seconds.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает длительность импульса (секунды).
        /// </summary>
        public double GetPulseWidth() => double.Parse(SendQuery("PWID?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPulseWidth"/>
        public async Task<double> GetPulseWidthAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("PWID?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает коэффициент заполнения импульсной модуляции (проценты).
        /// </summary>
        /// <param name="percent">Коэффициент заполнения (0–100%).</param>
        public void SetPulseDutyFactor(double percent) => SendCommand($"PDTY {percent.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPulseDutyFactor"/>
        public async Task SetPulseDutyFactorAsync(double percent, CancellationToken ct = default) => await SendCommandAsync($"PDTY {percent.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает коэффициент заполнения импульсной модуляции (проценты).
        /// </summary>
        public double GetPulseDutyFactor() => double.Parse(SendQuery("PDTY?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPulseDutyFactor"/>
        public async Task<double> GetPulseDutyFactorAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("PDTY?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Устанавливает длину псевдослучайной бинарной последовательности (PRBS) для шумовой импульсной модуляции (5–19 бит).
        /// </summary>
        /// <param name="bits">Длина последовательности в битах.</param>
        public void SetPrbsLength(int bits) => SendCommand($"PRBS {bits}\n");

        /// <inheritdoc cref="SetPrbsLength"/>
        public async Task SetPrbsLengthAsync(int bits, CancellationToken ct = default) => await SendCommandAsync($"PRBS {bits}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает длину PRBS.
        /// </summary>
        public int GetPrbsLength() => int.Parse(SendQuery("PRBS?\n"));

        /// <inheritdoc cref="GetPrbsLength"/>
        public async Task<int> GetPrbsLengthAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("PRBS?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Устанавливает период PRBS (секунды) для импульсной модуляции.
        /// </summary>
        /// <param name="seconds">Период в секундах.</param>
        public void SetPrbsPeriod(double seconds) => SendCommand($"RPER {seconds.ToString(CultureInfo.InvariantCulture)}\n");

        /// <inheritdoc cref="SetPrbsPeriod"/>
        public async Task SetPrbsPeriodAsync(double seconds, CancellationToken ct = default) => await SendCommandAsync($"RPER {seconds.ToString(CultureInfo.InvariantCulture)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает период PRBS (секунды).
        /// </summary>
        public double GetPrbsPeriod() => double.Parse(SendQuery("RPER?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetPrbsPeriod"/>
        public async Task<double> GetPrbsPeriodAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("RPER?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        // I/Q
        /// <summary>
        /// Устанавливает функцию I/Q модуляции (шум или внешний).
        /// </summary>
        /// <param name="function">Функция I/Q модуляции.</param>
        public void SetIqFunction(IQFunction function) => SendCommand($"QFNC {(int)function}\n");

        /// <inheritdoc cref="SetIqFunction"/>
        public async Task SetIqFunctionAsync(IQFunction function, CancellationToken ct = default) => await SendCommandAsync($"QFNC {(int)function}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую функцию I/Q модуляции.
        /// </summary>
        public IQFunction GetIqFunction() => (IQFunction)int.Parse(SendQuery("QFNC?\n"));

        /// <inheritdoc cref="GetIqFunction"/>
        public async Task<IQFunction> GetIqFunctionAsync(CancellationToken ct = default) => (IQFunction)int.Parse(await SendQueryAsync("QFNC?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Устанавливает связь внешнего входа модуляции (DC или AC).
        /// </summary>
        /// <param name="coupling">DC – постоянная связь, AC – через конденсатор (4 Гц).</param>
        public void SetModulationCoupling(InputCoupling coupling) => SendCommand($"COUP {(int)coupling}\n");

        /// <inheritdoc cref="SetModulationCoupling"/>
        public async Task SetModulationCouplingAsync(InputCoupling coupling, CancellationToken ct = default) => await SendCommandAsync($"COUP {(int)coupling}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущий режим связи внешнего входа модуляции.
        /// </summary>
        public InputCoupling GetModulationCoupling() => (InputCoupling)int.Parse(SendQuery("COUP?\n"));

        /// <inheritdoc cref="GetModulationCoupling"/>
        public async Task<InputCoupling> GetModulationCouplingAsync(CancellationToken ct = default) => (InputCoupling)int.Parse(await SendQueryAsync("COUP?\n", ct).ConfigureAwait(false));

        #endregion

        #region List Commands

        /// <summary>
        /// Создаёт список состояний указанного размера. Возвращает 1 при успехе, иначе 0.
        /// </summary>
        /// <param name="size">Размер списка.</param>
        public int CreateList(int size) => int.Parse(SendQuery($"LSTC? {size}\n"));

        /// <inheritdoc cref="CreateList"/>
        public async Task<int> CreateListAsync(int size, CancellationToken ct = default) => int.Parse(await SendQueryAsync($"LSTC? {size}\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Удаляет текущий список и освобождает память.
        /// </summary>
        public void DeleteList() => SendCommand("LSTD\n");

        /// <inheritdoc cref="DeleteList"/>
        public async Task DeleteListAsync(CancellationToken ct = default) => await SendCommandAsync("LSTD\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Включает или отключает режим списка.
        /// </summary>
        /// <param name="enable">true – включить, false – отключить.</param>
        public void EnableList(bool enable) => SendCommand($"LSTE {(enable ? 1 : 0)}\n");

        /// <inheritdoc cref="EnableList"/>
        public async Task EnableListAsync(bool enable, CancellationToken ct = default) => await SendCommandAsync($"LSTE {(enable ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние режима списка.
        /// </summary>
        public bool IsListEnabled() => SendQuery("LSTE?\n") == "1";

        /// <inheritdoc cref="IsListEnabled"/>
        public async Task<bool> IsListEnabledAsync(CancellationToken ct = default) => (await SendQueryAsync("LSTE?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Устанавливает индекс текущей точки списка.
        /// </summary>
        /// <param name="index">Номер точки (0…размер‑1).</param>
        public void SetListIndex(int index) => SendCommand($"LSTI {index}\n");

        /// <inheritdoc cref="SetListIndex"/>
        public async Task SetListIndexAsync(int index, CancellationToken ct = default) => await SendCommandAsync($"LSTI {index}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущий индекс списка.
        /// </summary>
        public int GetListIndex() => int.Parse(SendQuery("LSTI?\n"));

        /// <inheritdoc cref="GetListIndex"/>
        public async Task<int> GetListIndexAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("LSTI?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Записывает состояние в заданную точку списка.
        /// </summary>
        /// <param name="index">Индекс точки.</param>
        /// <param name="state">Состояние (для неизменяемых параметров указывать null).</param>
        public void SetListPoint(int index, ListState state) => SendCommand($"LSTP {index}, {state.ToCommandString()}\n");

        /// <inheritdoc cref="SetListPoint"/>
        public async Task SetListPointAsync(int index, ListState state, CancellationToken ct = default) => await SendCommandAsync($"LSTP {index}, {state.ToCommandString()}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние из заданной точки списка.
        /// </summary>
        /// <param name="index">Индекс точки.</param>
        public ListState GetListPoint(int index) => ListState.Parse(SendQuery($"LSTP? {index}\n"));

        /// <inheritdoc cref="GetListPoint"/>
        public async Task<ListState> GetListPointAsync(int index, CancellationToken ct = default) => ListState.Parse(await SendQueryAsync($"LSTP? {index}\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Сбрасывает индекс списка в 0.
        /// </summary>
        public void ResetList() => SendCommand("LSTR\n");

        /// <inheritdoc cref="ResetList"/>
        public async Task ResetListAsync(CancellationToken ct = default) => await SendCommandAsync("LSTR\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает размер текущего списка.
        /// </summary>
        public int GetListSize() => int.Parse(SendQuery("LSTS?\n"));

        /// <inheritdoc cref="GetListSize"/>
        public async Task<int> GetListSizeAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("LSTS?\n", ct).ConfigureAwait(false));

        #endregion

        #region Status and Display Commands

        /// <summary>
        /// Включает или отключает фронтальный дисплей.
        /// </summary>
        /// <param name="on">true – включить, false – отключить.</param>
        public void SetDisplay(bool on) => SendCommand($"DISP {(on ? 1 : 0)}\n");

        /// <inheritdoc cref="SetDisplay"/>
        public async Task SetDisplayAsync(bool on, CancellationToken ct = default) => await SendCommandAsync($"DISP {(on ? 1 : 0)}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает состояние фронтального дисплея.
        /// </summary>
        public bool IsDisplayOn() => SendQuery("DISP?\n") == "1";

        /// <inheritdoc cref="IsDisplayOn"/>
        public async Task<bool> IsDisplayOnAsync(CancellationToken ct = default) => (await SendQueryAsync("DISP?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Устанавливает маску регистра состояния прибора (INSE).
        /// </summary>
        /// <param name="mask">Битовая маска.</param>
        public void SetInstrumentStatusEnable(int mask) => SendCommand($"INSE {mask}\n");

        /// <inheritdoc cref="SetInstrumentStatusEnable"/>
        public async Task SetInstrumentStatusEnableAsync(int mask, CancellationToken ct = default) => await SendCommandAsync($"INSE {mask}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую маску INSE.
        /// </summary>
        public int GetInstrumentStatusEnable() => int.Parse(SendQuery("INSE?\n"));

        /// <inheritdoc cref="GetInstrumentStatusEnable"/>
        public async Task<int> GetInstrumentStatusEnableAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("INSE?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Возвращает и очищает регистр состояния прибора (INSR).
        /// </summary>
        public int GetInstrumentStatusRegister() => int.Parse(SendQuery("INSR?\n"));

        /// <inheritdoc cref="GetInstrumentStatusRegister"/>
        public async Task<int> GetInstrumentStatusRegisterAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("INSR?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Возвращает список последних ошибок.
        /// </summary>
        public List<ErrorInfo> GetLastErrors() => ErrorInfo.ParseErrors(SendQuery("LERR?\n"));

        /// <inheritdoc cref="GetLastErrors"/>
        public async Task<List<ErrorInfo>> GetLastErrorsAsync(CancellationToken ct = default) => ErrorInfo.ParseErrors(await SendQueryAsync("LERR?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Возвращает наличие установленной опции (1 – установлена, 0 – нет).
        /// </summary>
        /// <param name="optionId">Номер опции (1–4).</param>
        public int GetInstalledOptions(int optionId) => int.Parse(SendQuery($"OPTN? {optionId}\n"));

        /// <inheritdoc cref="GetInstalledOptions"/>
        public async Task<int> GetInstalledOptionsAsync(int optionId, CancellationToken ct = default) => int.Parse(await SendQueryAsync($"OPTN? {optionId}\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Проверяет перегрузку выхода.
        /// </summary>
        /// <param name="outputId">Идентификатор выхода (0 – BNC, 1 – Type‑N, 2 – часы, 3 – удвоитель).</param>
        public bool IsOutputOverRange(int outputId) => SendQuery($"ORNG? {outputId}\n") == "1";

        /// <inheritdoc cref="IsOutputOverRange"/>
        public async Task<bool> IsOutputOverRangeAsync(int outputId, CancellationToken ct = default) => (await SendQueryAsync($"ORNG? {outputId}\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Возвращает температуру RF-блока в градусах Цельсия.
        /// </summary>
        public double GetRfBlockTemperature() => double.Parse(SendQuery("TEMP?\n"), CultureInfo.InvariantCulture);

        /// <inheritdoc cref="GetRfBlockTemperature"/>
        public async Task<double> GetRfBlockTemperatureAsync(CancellationToken ct = default) => double.Parse(await SendQueryAsync("TEMP?\n", ct).ConfigureAwait(false), CultureInfo.InvariantCulture);

        /// <summary>
        /// Возвращает информацию о установленном опорном генераторе (OCXO или рубидий).
        /// </summary>
        public string GetTimebaseInfo() => SendQuery("TIMB?\n");

        /// <inheritdoc cref="GetTimebaseInfo"/>
        public async Task<string> GetTimebaseInfoAsync(CancellationToken ct = default) => await SendQueryAsync("TIMB?\n", ct).ConfigureAwait(false);

        #endregion

        #region Interface Commands

        /// <summary>
        /// Возвращает MAC-адрес Ethernet-интерфейса.
        /// </summary>
        public string GetEthernetMacAddress() => SendQuery("EMAC?\n");

        /// <inheritdoc cref="GetEthernetMacAddress"/>
        public async Task<string> GetEthernetMacAddressAsync(CancellationToken ct = default) => await SendQueryAsync("EMAC?\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Устанавливает скорость физического уровня Ethernet (10 или 100 Мбит/с).
        /// </summary>
        /// <param name="speed">Скорость: 10 или 100.</param>
        public void SetEthernetPhy(int speed) => SendCommand($"EPHY {speed}\n");

        /// <inheritdoc cref="SetEthernetPhy"/>
        public async Task SetEthernetPhyAsync(int speed, CancellationToken ct = default) => await SendCommandAsync($"EPHY {speed}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую настройку скорости Ethernet.
        /// </summary>
        public int GetEthernetPhy() => int.Parse(SendQuery("EPHY?\n"));

        /// <inheritdoc cref="GetEthernetPhy"/>
        public async Task<int> GetEthernetPhyAsync(CancellationToken ct = default) => int.Parse(await SendQueryAsync("EPHY?\n", ct).ConfigureAwait(false));

        /// <summary>
        /// Настраивает интерфейс (GPIB, RS‑232, LAN).
        /// </summary>
        /// <param name="iface">0 – RS‑232, 1 – GPIB, 2 – LAN.</param>
        /// <param name="enable">1 – включить, 0 – отключить.</param>
        /// <param name="param">Дополнительный параметр (адрес GPIB, скорость RS‑232).</param>
        public void SetInterfaceConfiguration(int iface, int enable, int param = 0) => SendCommand($"IFCF {iface}, {enable}, {param}\n");

        /// <inheritdoc cref="SetInterfaceConfiguration"/>
        public async Task SetInterfaceConfigurationAsync(int iface, int enable, int param = 0, CancellationToken ct = default) => await SendCommandAsync($"IFCF {iface}, {enable}, {param}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает текущую конфигурацию интерфейса.
        /// </summary>
        /// <param name="iface">0 – RS‑232, 1 – GPIB, 2 – LAN.</param>
        public string GetInterfaceConfiguration(int iface) => SendQuery($"IFCF? {iface}\n");

        /// <inheritdoc cref="GetInterfaceConfiguration"/>
        public async Task<string> GetInterfaceConfigurationAsync(int iface, CancellationToken ct = default) => await SendQueryAsync($"IFCF? {iface}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Сбрасывает указанный интерфейс (RS‑232, GPIB, LAN).
        /// </summary>
        /// <param name="iface">0 – RS‑232, 1 – GPIB, 2 – LAN.</param>
        public void ResetInterface(int iface) => SendCommand($"IFRS {iface}\n");

        /// <inheritdoc cref="ResetInterface"/>
        public async Task ResetInterfaceAsync(int iface, CancellationToken ct = default) => await SendCommandAsync($"IFRS {iface}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Возвращает активную TCP/IP конфигурацию (ссылка, IP, маска, шлюз).
        /// </summary>
        /// <param name="param">0 – состояние соединения, 1 – IP-адрес, 2 – маска, 3 – шлюз.</param>
        public string GetActiveIpConfiguration(int param) => SendQuery($"IPCF? {param}\n");

        /// <inheritdoc cref="GetActiveIpConfiguration"/>
        public async Task<string> GetActiveIpConfigurationAsync(int param, CancellationToken ct = default) => await SendQueryAsync($"IPCF? {param}\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Переводит прибор в локальный режим (включает переднюю панель).
        /// </summary>
        public void GoLocal() => SendCommand("LCAL\n");

        /// <inheritdoc cref="GoLocal"/>
        public async Task GoLocalAsync(CancellationToken ct = default) => await SendCommandAsync("LCAL\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Запрашивает блокировку прибора для монопольного управления через данный интерфейс.
        /// </summary>
        public bool RequestLock() => SendQuery("LOCK?\n") == "1";

        /// <inheritdoc cref="RequestLock"/>
        public async Task<bool> RequestLockAsync(CancellationToken ct = default) => (await SendQueryAsync("LOCK?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Переводит прибор в удалённый режим (блокирует переднюю панель).
        /// </summary>
        public void GoRemote() => SendCommand("REMT\n");

        /// <inheritdoc cref="GoRemote"/>
        public async Task GoRemoteAsync(CancellationToken ct = default) => await SendCommandAsync("REMT\n", ct).ConfigureAwait(false);

        /// <summary>
        /// Освобождает блокировку прибора, полученную ранее через RequestLock.
        /// </summary>
        public bool ReleaseLock() => SendQuery("UNLK?\n") == "1";

        /// <inheritdoc cref="ReleaseLock"/>
        public async Task<bool> ReleaseLockAsync(CancellationToken ct = default) => (await SendQueryAsync("UNLK?\n", ct).ConfigureAwait(false)) == "1";

        /// <summary>
        /// Устанавливает символы завершения ответов прибора (последовательность байтов).
        /// </summary>
        /// <param name="term1">Первый байт (обычно 13 – CR).</param>
        /// <param name="term2">Второй байт (обычно 10 – LF).</param>
        /// <param name="term3">Третий байт (0 – не используется).</param>
        public void SetInterfaceTerminator(int term1, int term2 = 0, int term3 = 0) => SendCommand($"XTRM {term1}, {term2}, {term3}\n");

        /// <inheritdoc cref="SetInterfaceTerminator"/>
        public async Task SetInterfaceTerminatorAsync(int term1, int term2 = 0, int term3 = 0, CancellationToken ct = default) => await SendCommandAsync($"XTRM {term1}, {term2}, {term3}\n", ct).ConfigureAwait(false);

        #endregion
    }
}