using VAlgo.JudgeWorker.Execution.Specs;

namespace VAlgo.JudgeWorker.Languages
{
    public static class LanguageRegistry
    {
        public static IReadOnlyDictionary<string, LanguageSpec> All { get; } =
            new Dictionary<string, LanguageSpec>(StringComparer.OrdinalIgnoreCase)
            {
                ["cpp"] = new LanguageSpec
                {
                    Image = "gcc:12.2.0",
                    SourceFile = "main.cpp",
                    CompileCommand = "g++ main.cpp -O2 -std=gnu++17 -o main 1>compile.out 2>compile.err",
                    RunCommand = "timeout -s SIGKILL {timeSec}s ./main < input.txt 1>stdout.txt 2>stderr.txt"
                },
                ["c"] = new LanguageSpec
                {
                    Image = "gcc:12.2.0",
                    SourceFile = "main.c",
                    CompileCommand = "gcc main.c -O2 -std=gnu17 -o main 1>compile.out 2>compile.err",
                    RunCommand = "timeout -s SIGKILL {timeSec}s ./main < input.txt 1>stdout.txt 2>stderr.txt"
                },
                ["python"] = new LanguageSpec
                {
                    Image = "python:3.11-slim",
                    SourceFile = "main.py",
                    RunCommand = "timeout -s SIGKILL {timeSec}s python3 main.py < input.txt 1>stdout.txt 2>stderr.txt",
                    TimeMultiplier = 2
                },
                ["java"] = new LanguageSpec
                {
                    Image = "eclipse-temurin:17-jdk",
                    SourceFile = "Main.java",
                    CompileCommand = "javac Main.java 1>compile.out 2>compile.err",
                    RunCommand = "timeout -s SIGKILL {timeSec2}s java -Xms64m - Xmx{memMb}m Main < input.txt 1>stdout.txt 2>stderr.txt",
                    TimeMultiplier = 2
                },
                ["csharp"] = new LanguageSpec
                {
                    Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                    SourceFile = "Program.cs",
                    CompileCommand = """
                        dotnet new console -n App -o app -force &&
                        mv Program.cs app/Program.cs &&
                        dotnet publish app -c Release -o out --nologo
                    """,
                    RunCommand = "timeout -s SIGKILL {timeSec}s dotnet out/App.dll < input.txt 1>stdout.txt 2>stderr.txt"
                },
                ["javascript"] = new LanguageSpec
                {
                    Image = "node:20-slim",
                    SourceFile = "main.js",
                    RunCommand = "timeout -s SIGKILL {timeSec}s node main.js < input.txt 1>stdout.txt 2>stderr.txt"
                }
            };

        public static LanguageSpec Get(string language)
            => All.TryGetValue(language, out var spec)
                ? spec
                : throw new NotSupportedException($"Language '{language}' is not supported");
    }
}