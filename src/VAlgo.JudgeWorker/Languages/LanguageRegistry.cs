using VAlgo.JudgeWorker.Execution.Specs;

namespace VAlgo.JudgeWorker.Languages
{
    public static class LanguageRegistry
    {
        private static readonly IReadOnlyDictionary<string, LanguageSpec> _languages =
            new Dictionary<string, LanguageSpec>(StringComparer.OrdinalIgnoreCase)
            {
                ["cpp"] = new LanguageSpec
                {
                    Key = "cpp",
                    Image = "gcc:12.2.0",
                    SourceFile = "main.cpp",
                    CompileCommand = "g++ main.cpp -O2 -std=gnu++17 -o main",
                    RunCommand = "./main"
                },

                ["c"] = new LanguageSpec
                {
                    Key = "c",
                    Image = "gcc:12.2.0",
                    SourceFile = "main.c",
                    CompileCommand = "gcc main.c -O2 -std=gnu17 -o main",
                    RunCommand = "./main"
                },

                ["python"] = new LanguageSpec
                {
                    Key = "python",
                    Image = "python:3.11-slim",
                    SourceFile = "main.py",
                    CompileCommand = "python3 -m py_compile main.py",
                    RunCommand = "python3 main.py"
                },

                ["java"] = new LanguageSpec
                {
                    Key = "java",
                    Image = "eclipse-temurin:17-jdk",
                    SourceFile = "Main.java",
                    CompileCommand = "javac Main.java",
                    RunCommand = "java Main"
                },

                ["csharp"] = new LanguageSpec
                {
                    Key = "csharp",
                    Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                    SourceFile = "Program.cs",
                    CompileCommand =
                        "dotnet new console -n App -o app --force && " +
                        "mv Program.cs app/Program.cs && " +
                        "dotnet publish app -c Release -o out --nologo",
                    RunCommand = "dotnet out/App.dll"
                },

                ["javascript"] = new LanguageSpec
                {
                    Key = "javascript",
                    Image = "node:20-slim",
                    SourceFile = "main.js",
                    CompileCommand = "node --check main.js",
                    RunCommand = "node main.js"
                }
            };

        public static LanguageSpec Get(string language)
        {
            if (!_languages.TryGetValue(language, out var spec))
                throw new NotSupportedException(
                    $"Language '{language}' is not supported");

            return spec;
        }

        public static IEnumerable<string> SupportedLanguages
            => _languages.Keys;
    }
}
