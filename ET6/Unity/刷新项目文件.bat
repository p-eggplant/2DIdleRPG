@echo off
chcp 65001 >nul
REM ============================================
REM 刷新项目文件.bat
REM 创建新的 .cs 文件后，双击运行此脚本：
REM   1. 自动将逐文件列举的 .csproj 改为通配符
REM   2. 触发 Visual Studio 重新加载项目
REM ============================================

set "SCRIPT_DIR=%~dp0"
set "SCRIPT_DIR=%SCRIPT_DIR:~0,-1%"

PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& {
    $projectDir = '%SCRIPT_DIR%';
    $directories = @('ModelView', 'HotfixView', 'Hotfix', 'Model');
    $fixed = $false;

    foreach ($dir in $directories) {
        $csproj = Join-Path $projectDir \"Unity.$dir.csproj\";
        if (-not (Test-Path -LiteralPath $csproj)) { continue; }

        $content = Get-Content -LiteralPath $csproj -Raw;

        # 检测是否为逐文件列举模式
        $explicitPattern = '\r?\n\s*<Compile Include=\"Codes\\' + [regex]::Escape($dir) + '\\[^*][^\""]*\.cs\" />';
        $matchCount = ([regex]::Matches($content, $explicitPattern)).Count;

        if ($matchCount -gt 1) {
            # 将所有逐文件列举替换为一个通配符
            $newContent = $content -replace '(\r?\n\s*<Compile Include=\"Codes\\' + [regex]::Escape($dir) + '\\[^*][^\""]*\.cs\" />)+', (\"`r`n    <Compile Include=\""Codes\\$dir\\**\\*.cs\"" />\");
            Set-Content -LiteralPath $csproj -Value $newContent -NoNewline;
            Write-Host \"[FIX] Unity.$dir.csproj: 逐文件列举 -> 通配符\" -ForegroundColor Green;
            $fixed = $true;
        }
        else {
            # 通配符模式，更新时间戳触发 VS 重新加载
            (Get-Item -LiteralPath $csproj).LastWriteTime = Get-Date;
            Write-Host \"[OK] Unity.$dir.csproj\" -ForegroundColor Green;
        }
    }

    Write-Host '';
    if ($fixed) {
        Write-Host '已修复 .csproj 文件，切换回 Visual Studio 选择 \"重新加载\"' -ForegroundColor Cyan;
    } else {
        Write-Host '已刷新所有 .csproj，切换回 Visual Studio 选择 \"重新加载\"' -ForegroundColor Cyan;
    }
    Write-Host '';
    pause
}"
