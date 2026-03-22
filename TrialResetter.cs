using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace JetBrainsResetTrialConsole;

public class LogMessage
{
    public string Text { get; set; } = default!;
    public ConsoleColor TextColor { get; set; }
}

public class TrialResetter(Action<LogMessage> onLog)
{
    public Action<LogMessage> OnLog = onLog;

    public async Task ResetProductsAsync(List<string> products)
    {
        try
        {
            string os = Environment.OSVersion.Platform.ToString();

            // .NET returns Win32NT
            if (os.Contains("Win"))
            {
                await ResetWindowsAsync();
            }
            else if (os.Contains("Unix") || os.Contains("Linux"))
            {
                if (Directory.Exists("/System/Library/CoreServices"))
                {
                    await ResetMacAsync(products);
                }
                else
                {
                    await ResetLinuxAsync(products);
                }
            }
            else if (os.Contains("Mac"))
            {
                await ResetMacAsync(products);
            }
            else
            {
                Log("Isletim sistemi taninmadi.", ConsoleColor.Red);
            }
        }
        catch (Exception ex)
        {
            Log($"Hata: {ex.Message}", ConsoleColor.Red);
        }
    }

    private async Task ResetWindowsAsync()
    {
        string command = @"@echo off
echo [ INFO ] JavaSoft kullanici anahtari kaldiriliyor...
reg delete ""HKEY_CURRENT_USER\Software\JavaSoft"" /f >nul 2>&1
if %ERRORLEVEL% equ 0 (
echo [ OK ] Anahtar Basariyla Kaldirildi.
) else (
echo [ ! ] Anahtar Zaten Kaldirilmis.
)
echo [ INFO ] Kullanici Kimligi Kaldiriliyor...
del /F /Q ""%APPDATA%\JetBrains\PermanentUserId"" >nul 2>&1
if %ERRORLEVEL% equ 0 (
echo [ OK ] Kullanici Kimligi Kaldirildi.
) else (
echo [ ! ] Kullanici Kimligi Zaten Kaldirilmis.
)";
        await RunScriptAsync("windows.bat", command, ConsoleColor.Blue);
    }

    private async Task ResetMacAsync(List<string> products)
    {
        StringBuilder sb = new();
        foreach (var product in products)
        {
            sb.AppendLine($@"
#!/bin/bash
product_name={product}
               
prefs_file_one=~/Library/Preferences/com.apple.java.util.prefs.plist
prefs_file_two=~/Library/Preferences/com.jetbrains.*.plist
prefs_file_three=~/Library/Preferences/jetbrains.*.*.plist
               
prefs_files=($prefs_file_one $prefs_file_two $prefs_file_three)
               
echo ""[ INFO ] Deneme suresi sifirlaniyor: $product_name""
               
   echo ""Degerlendirme anahtari kaldiriliyor...""
   if rm -rf ~/Library/Preferences/$product_name*/eval; then
      echo ""[ OK ] Anahtar basariyla kaldirildi."" 
   else
      echo ""[ ERROR ] Anahtar kaldirilamadi.""
   fi
   
   if rm -rf ~/Library/Application\ Support/JetBrains/$product_name/eval; then
      echo ""[ OK ] Son surum anahtari basariyla kaldirildi.""
   else
      echo ""[ ERROR ] Son surum anahtari kaldirilamadi.""
   fi
   
   echo ""options.xml dosyasindaki evlsprt siliniyor...""
   sed -i '' '/evlsprt/d' ~/Library/Preferences/$product_name*/options/other.xml
   
   if [ $? -eq 0 ]; then
       echo ""[ OK ] evlsprt basariyla silindi.""
   else
       echo ""[ ERROR ] evlsprt silinemedi.""
   fi
   
   sed -i '' '/evlsprt/d' ~/Library/Application\ Support/JetBrains/$product_name*/options/other.xml
   if [ $? -eq 0 ]; then
       echo ""[ OK ] Son surum evlsprt basariyla silindi.""
   else
       echo ""[ ERROR ] Son surum evlsprt silinemedi.""
   fi
             
 echo ""[ INFO ] Ek plist dosyalari kaldiriliyor...""
               
   for file in ${{prefs_files[@]}}; do
       echo ""[ INFO ] ${{file##*/}} dosyasi kaldiriliyor...""
         if rm ""$file""; then
            echo ""[ OK ] ${{file##*/}} basariyla kaldirildi.""
         else
            echo ""[ ERROR ] ${{file##*/}} kaldirilamadi.""
         fi
   done
");
        }
        await RunScriptAsync("mac.sh", sb.ToString(), ConsoleColor.Magenta, true);
    }

    private async Task ResetLinuxAsync(List<string> products)
    {
        string configPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.config/JetBrains/";
        StringBuilder sb = new();

        foreach (var product in products)
        {
            string resultPath = configPath + product;
            sb.AppendLine($@"
#!/bin/bash
config_path={configPath}
result_path={resultPath}
product_name=$(basename $result_path*)
name_path=$config_path$product_name
if [ -d $name_path ]; then
    echo ""[ INFO ] deneme suresi sifirlaniyor: [$product_name]""
    echo ""[ INFO ] Degerlendirme anahtari kaldiriliyor...""
    rm -rf $name_path/eval &>/dev/null
    if [ $? -eq 0 ]; then
        echo ""[ OK ] Degerlendirme anahtari basariyla kaldirildi.""
    else
        echo ""[ ERROR ] Degerlendirme anahtari kaldirilamadi.""
    fi
    echo ""[ INFO ] options.xml dosyasindaki tum evlsprt ozellikleri kaldiriliyor...""
    sed -i 's/evlsprt//' $name_path/options/other.xml &>/dev/null
    if [ $? -eq 0 ]; then
        echo ""[ OK ] evlsprt ozellikleri basariyla kaldirildi.""
    else
        echo ""[ ERROR ] evlsprt ozellikleri kaldirilamadi.""
    fi
    echo ""[ INFO ] userPrefs dosyalari kaldiriliyor...""
    rm -rf ~/.java/.userPrefs &>/dev/null
    if [ $? -eq 0 ]; then
        echo ""[ OK ] userPrefs dosyalari basariyla kaldirildi.""
    else
        echo ""[ ERROR ] userPrefs dosyalari kaldirilamadi.""
    fi
else
    echo ""[ WARN ] $product_name icin klasor bulunamadi.""
fi
");
        }
        await RunScriptAsync("linux.sh", sb.ToString(), ConsoleColor.DarkMagenta, true);
    }

    private async Task RunScriptAsync(string fileName, string scriptContent, ConsoleColor color, bool isBash = false)
    {
        try
        {
            await File.WriteAllTextAsync(fileName, scriptContent);

            ProcessStartInfo psi = new();
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            if (isBash)
            {
                try
                {
                    Process? chmod = Process.Start(new ProcessStartInfo("chmod", $"+x ./{fileName}") { CreateNoWindow = true, UseShellExecute = false });
                    chmod?.WaitForExit();
                }
                catch { }
                
                psi.FileName = "bash";
                psi.Arguments = $"./{fileName}";
            }
            else
            {
                psi.FileName = "cmd.exe";
                psi.Arguments = $"/c {fileName}";
            }

            using (Process? process = Process.Start(psi))
            {
                if (process == null) return;

                process.OutputDataReceived += (s, e) => {
                    if (e.Data != null)
                    {
                        Log(e.Data, color);
                    }
                };
                process.ErrorDataReceived += (s, e) => {
                    if (e.Data != null)
                    {
                        Log(e.Data, color);
                    }
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                Log("Betik basariyla silindi.", ConsoleColor.DarkGray);
            }
            else
            {
                Log("Betik silinemedi.", ConsoleColor.DarkGray);
            }
        }
        catch (Exception ex)
        {
            Log($"Hata: {ex.Message}", ConsoleColor.Red);
        }
    }

    private void Log(string message, ConsoleColor color)
    {
        OnLog?.Invoke(new LogMessage { Text = message, TextColor = color });
    }
}
