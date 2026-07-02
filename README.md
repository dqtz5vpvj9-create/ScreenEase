# ScreenEase

ScreenEase is an open-source eye-care display controller for Windows.

The backend is a front-end-agnostic core service. The desktop client is a native WPF/.NET 8 Windows UI that talks to the service over Windows named pipes by default and does not use WebView.

## What Is Implemented

- Windows display tint and brightness through the public GDI `SetDeviceGammaRamp` API.
- Monitor enumeration through `EnumDisplayMonitors` and `GetMonitorInfoW`.
- Built-in profiles: 明亮, 柔和, 清晰, 影音, 高亮, 舒缓, 我的模式.
- Day/night schedule selection with separate night profile values.
- Layered-window dimming API kept as an internal compatibility path.
- Global hotkey registration through `RegisterHotKey`.
- Rest timer state machine with work, short break, long break, pause, resume, reset.
- JSON settings persistence.
- Import from legacy INI-style settings files.
- Windows named pipe IPC for native desktop control without a TCP listener.
- REST API suitable for a separate frontend.
- Native WPF desktop client for profiles, manual display control, rest timer, and monitor status.
- `memory` driver for safe API testing without changing the screen.

## Project Layout

```text
src/ScreenEase.Core         Domain models, gamma ramp, settings, timer, drivers
src/ScreenEase.CoreService  ASP.NET Core REST service and background loop
src/ScreenEase.Desktop      Native WPF/.NET 8 Windows UI
src/ScreenEase.NativeHost   Chromium native messaging stdio host
tests/ScreenEase.Tests      No-package console test runner
docs/API.md                   HTTP API reference
docs/DESKTOP_UI.md            Native desktop UI notes
docs/NAMED_PIPE_IPC.md        Native Windows IPC protocol
docs/STATUS.md                Current milestone status and remaining parity work
docs/NATIVE_MESSAGING.md      Native messaging protocol and install notes
```

## Build

```powershell
dotnet build .\ScreenEase.sln -c Release
```

## Test

```powershell
dotnet run --project .\tests\ScreenEase.Tests\ScreenEase.Tests.csproj -c Release
```

## Run Safely With Memory Driver

This mode exercises the core service through a Windows named pipe without touching display gamma or opening a TCP port.

```powershell
$env:ScreenEase__Driver = 'memory'
$env:ScreenEase__SettingsPath = "$PWD\.local\settings.json"
dotnet run --project .\src\ScreenEase.CoreService\ScreenEase.CoreService.csproj -c Release -- --pipe-only
```

Run the native desktop UI:

```powershell
dotnet run --project .\src\ScreenEase.Desktop\ScreenEase.Desktop.csproj -c Release
```

The desktop UI defaults to:

```text
pipe:screenease.core
```

## Run HTTP API For Debugging

```powershell
$env:ScreenEase__Driver = 'memory'
$env:ScreenEase__SettingsPath = "$PWD\.local\settings.json"
dotnet run --project .\src\ScreenEase.CoreService\ScreenEase.CoreService.csproj -c Release -- --urls http://127.0.0.1:5128
```

Open:

```text
http://127.0.0.1:5128/api/state
```

The desktop UI can also connect to this endpoint when its address box is set to:

```text
http://127.0.0.1:5128
```

## Run With Windows Display Driver

```powershell
$env:ScreenEase__Driver = 'windows'
dotnet run --project .\src\ScreenEase.CoreService\ScreenEase.CoreService.csproj -c Release -- --pipe-only
```

Stopping the service resets gamma to 6500K and 100 percent brightness.

## Example API Calls

Apply the reading profile:

```powershell
Invoke-RestMethod `
  -Uri http://127.0.0.1:5128/api/apply `
  -Method Post `
  -ContentType application/json `
  -Body '{"profileId":"reading","enabled":true}'
```

Import legacy INI settings:

```powershell
Invoke-RestMethod `
  -Uri http://127.0.0.1:5128/api/import/legacy-settings `
  -Method Post `
  -ContentType application/json `
  -Body '{"path":"C:\\Path\\To\\settings.dat"}'
```

Read hotkey configuration and active registrations:

```powershell
Invoke-RestMethod -Uri http://127.0.0.1:5128/api/hotkeys
```

Run native messaging host in safe memory mode:

```powershell
$env:ScreenEase__Driver = 'memory'
dotnet run --project .\src\ScreenEase.NativeHost\ScreenEase.NativeHost.csproj -- --memory
```


