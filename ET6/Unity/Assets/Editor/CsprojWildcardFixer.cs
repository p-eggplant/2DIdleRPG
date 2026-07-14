using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [InitializeOnLoad]
    public class CsprojWildcardFixer
    {
        private class ProjectInfo
        {
            public string CsprojName;
            public string SourcePrefix;
        }

        private static readonly ProjectInfo[] Projects =
        {
            new ProjectInfo { CsprojName = "Unity.ModelView.csproj",  SourcePrefix = @"Codes\ModelView" },
            new ProjectInfo { CsprojName = "Unity.HotfixView.csproj", SourcePrefix = @"Codes\HotfixView" },
            new ProjectInfo { CsprojName = "Unity.Hotfix.csproj",     SourcePrefix = @"Codes\Hotfix" },
            new ProjectInfo { CsprojName = "Unity.Model.csproj",      SourcePrefix = @"Codes\Model" },
            new ProjectInfo { CsprojName = "Unity.Editor.csproj",     SourcePrefix = @"Assets\Editor" },
        };

        static CsprojWildcardFixer()
        {
            EditorApplication.delayCall += FixAllProjects;
        }

        [MenuItem("ET/Fix Csproj Wildcards")]
        public static void FixAllProjects()
        {
            string projectDir = Directory.GetParent(Application.dataPath).FullName;
            bool anyFixed = false;

            foreach (var proj in Projects)
            {
                string csprojFile = Path.Combine(projectDir, proj.CsprojName);
                if (!File.Exists(csprojFile))
                    continue;

                string content = File.ReadAllText(csprojFile);
                if (!HasExplicitCompileEntries(content, proj.SourcePrefix))
                    continue;

                content = ReplaceExplicitWithWildcard(content, proj.SourcePrefix);
                File.WriteAllText(csprojFile, content);
                anyFixed = true;
            }

            if (anyFixed)
            {
                AssetDatabase.Refresh();
            }
        }

        private static bool HasExplicitCompileEntries(string content, string sourcePrefix)
        {
            string escaped = Regex.Escape(sourcePrefix);
            var matches = Regex.Matches(content, $@"<Compile Include=""{escaped}\\[^""]*\.cs"" />");
            if (matches.Count <= 1)
                return false;
            string firstInclude = matches[1].Value;
            return !firstInclude.Contains("**");
        }

        private static string ReplaceExplicitWithWildcard(string content, string sourcePrefix)
        {
            string escaped = Regex.Escape(sourcePrefix);
            string pattern = $@"(\s*<Compile Include=""{escaped}\\.*\.cs"" />\s*\r?\n)+";
            string replacement = $@"    <Compile Include=""{sourcePrefix}\**\*.cs"" />" + "\r\n";
            return Regex.Replace(content, pattern, replacement);
        }
    }
}
