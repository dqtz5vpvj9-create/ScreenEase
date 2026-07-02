namespace ScreenEase.Core;

public static class Defaults
{
    public static EyeCareSettings CreateSettings() =>
        new(
            Enabled: false,
            ActiveProfileId: "health",
            UseNightValues: true,
            UseSchedule: false,
            Sunrise: new TimeOnly(7, 0),
            Sunset: new TimeOnly(19, 0),
            SmoothTransitions: true,
            TransitionDuration: TimeSpan.FromSeconds(2),
            Overlay: new OverlaySettings(
                Enabled: false,
                OpacityPercent: 18,
                ColorHex: "#FFC98A"),
            Hotkeys:
            [
                new HotkeyBinding("toggle-enabled", HotkeyAction.ToggleEnabled, "Ctrl+Alt+F9", false),
                new HotkeyBinding("brightness-up", HotkeyAction.IncreaseBrightness, "Ctrl+Alt+Up", false),
                new HotkeyBinding("brightness-down", HotkeyAction.DecreaseBrightness, "Ctrl+Alt+Down", false),
                new HotkeyBinding("temperature-up", HotkeyAction.IncreaseColorTemperature, "Ctrl+Alt+Right", false),
                new HotkeyBinding("temperature-down", HotkeyAction.DecreaseColorTemperature, "Ctrl+Alt+Left", false),
                new HotkeyBinding("reading-profile", HotkeyAction.ApplyReadingProfile, "Ctrl+Alt+R", false),
                new HotkeyBinding("health-profile", HotkeyAction.ApplyHealthProfile, "Ctrl+Alt+H", false),
                new HotkeyBinding("toggle-overlay", HotkeyAction.ToggleOverlay, "Ctrl+Alt+D", false)
            ],
            RestTimer: new RestTimerSettings(
                Enabled: false,
                WorkMinutes: 25,
                ShortBreakMinutes: 5,
                LongBreakMinutes: 15,
                LongBreakEveryWorkSessions: 4,
                AutoStart: false),
            Profiles:
            [
                new EyeProfile("office", "明亮", 5500, 100, 5000, 100),
                new EyeProfile("reading", "柔和", 5500, 85, 5500, 85),
                new EyeProfile("editing", "清晰", 6500, 85, 6500, 85),
                new EyeProfile("movie", "影音", 6000, 90, 5500, 90),
                new EyeProfile("game", "高亮", 6500, 90, 6000, 90),
                new EyeProfile("health", "舒缓", 5000, 90, 3700, 80),
                new EyeProfile("custom", "我的模式", 5500, 90, 5500, 90)
            ]);

    public static RestTimerState CreateRestTimerState() =>
        new(
            Phase: RestTimerPhase.Stopped,
            StartedAt: null,
            EndsAt: null,
            PausedRemaining: null,
            PausedFrom: null,
            CompletedWorkSessions: 0);

    public static DisplayEffect CreateEffect(DateTimeOffset now) =>
        new(
            Enabled: false,
            ProfileId: "health",
            ColorTemperatureKelvin: 5000,
            BrightnessPercent: 90,
            IsNightValue: false,
            AppliedAt: now);
}


