# Watch-NewCSFiles.ps1
# 监控 Codes 目录下新增的 .cs 文件，自动触发 VS 项目重新加载
# 用法：右键此文件 → 使用 PowerShell 运行，或在终端中 .\Watch-NewCSFiles.ps1

param(
    [string]$WatchPath = $PSScriptRoot,
    [string[]]$CsprojFiles = @(
        "Unity.ModelView.csproj",
        "Unity.HotfixView.csproj",
        "Unity.Hotfix.csproj",
        "Unity.Model.csproj",
        "Unity.Mono.csproj",
        "Unity.ThirdParty.csproj"
    ),
    [int]$CooldownSeconds = 3
)

$ErrorActionPreference = "Stop"

# 统一路径分隔符
$WatchPath = $WatchPath -replace '/', '\'
$WatchPath = $WatchPath.TrimEnd('\')

Write-Host "================================================" -ForegroundColor Cyan
Write-Host " ET6 Unity C# 文件监控已启动" -ForegroundColor Cyan
Write-Host " 监控路径: $WatchPath" -ForegroundColor Cyan
Write-Host " 按 Ctrl+C 停止监控" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan

# 记录已处理的文件，防止重复触发
$processed = [System.Collections.Generic.HashSet[string]]::new()
$lastTrigger = [DateTime]::MinValue

$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = $WatchPath
$watcher.Filter = "*.cs"
$watcher.IncludeSubdirectories = $true
$watcher.NotifyFilter = [System.IO.NotifyFilters]::FileName -bor [System.IO.NotifyFilters]::LastWrite
$watcher.EnableRaisingEvents = $true

$action = {
    $path = $Event.SourceEventArgs.FullPath
    $changeType = $Event.SourceEventArgs.ChangeType
    $name = $Event.SourceEventArgs.Name

    # 忽略 obj 和 bin 目录
    if ($path -match '\\obj\\' -or $path -match '\\bin\\' -or $path -match '\\Temp\\') {
        return
    }

    # 去重 + 冷却时间
    $now = [DateTime]::Now
    if (($now - $lastTrigger).TotalSeconds -lt $CooldownSeconds) {
        return
    }
    if (-not $processed.Add($path)) {
        return
    }
    # 清理旧记录（超过60秒的文件）
    $processed.RemoveWhere({ $true }) | Out-Null

    $eventTime = $now.ToString("HH:mm:ss")
    Write-Host "[$eventTime]" -NoNewline -ForegroundColor Gray
    Write-Host " 检测到: $name" -NoNewline -ForegroundColor Yellow
    Write-Host " ($changeType)" -ForegroundColor Gray

    # 判断属于哪个 csproj
    $relativePath = $path.Substring($WatchPath.Length).TrimStart('\')
    $targetCsproj = $null

    if ($relativePath -match '^Codes\\HotfixView\\') {
        $targetCsproj = "Unity.HotfixView.csproj"
    }
    elseif ($relativePath -match '^Codes\\ModelView\\') {
        $targetCsproj = "Unity.ModelView.csproj"
    }
    elseif ($relativePath -match '^Codes\\Hotfix\\') {
        $targetCsproj = "Unity.Hotfix.csproj"
    }
    elseif ($relativePath -match '^Codes\\Model\\') {
        $targetCsproj = "Unity.Model.csproj"
    }
    elseif ($relativePath -match '^Codes\\Mono\\') {
        $targetCsproj = "Unity.Mono.csproj"
    }

    if ($targetCsproj) {
        $csprojPath = Join-Path $WatchPath $targetCsproj
        if (Test-Path -LiteralPath $csprojPath) {
            # 更新 csproj 的修改时间，触发 VS 感知
            (Get-Item -LiteralPath $csprojPath).LastWriteTime = $now
            $script:lastTrigger = $now
            Write-Host "    → 已触发 $targetCsproj 重新加载" -ForegroundColor Green
        }
    }
}

$handlers = @()
$handlers += Register-ObjectEvent -InputObject $watcher -EventName "Created" -Action $action
$handlers += Register-ObjectEvent -InputObject $watcher -EventName "Renamed" -Action $action

try {
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
finally {
    $handlers | ForEach-Object { Unregister-Event -SourceIdentifier $_.Name -ErrorAction SilentlyContinue }
    $watcher.Dispose()
    Write-Host "监控已停止。" -ForegroundColor Cyan
}
