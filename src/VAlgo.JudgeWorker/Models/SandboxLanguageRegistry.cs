namespace VAlgo.JudgeWorker.Models
{
    public sealed class SandboxLanguageRegistry
    {
        private static readonly Dictionary<string, SandboxLanguage> _languages = new()
        {
            ["cpp"] = new SandboxLanguage(
                Name: "cpp",
                DockerImage: "valgo/cpp:latest",
                SourceFileName: "main.cpp",
                RunCommand: "./main",
                CompileCommand: "g++ main.cpp -O2 -std=c++17 -o main"
            ),

            ["c"] = new SandboxLanguage(
                Name: "c",
                DockerImage: "valgo/c:latest",
                SourceFileName: "main.c",
                RunCommand: "./main",
                CompileCommand: "gcc main.c -O2 -o main"
            ),

            ["python"] = new SandboxLanguage(
                Name: "python",
                DockerImage: "valgo/python:latest",
                SourceFileName: "main.py",
                RunCommand: "python main.py",
                CompileCommand: ""
            ),

            ["csharp"] = new SandboxLanguage(
                Name: "csharp",
                DockerImage: "valgo/csharp:latest",
                SourceFileName: "main.cs",
                RunCommand: "dotnet script main.cs",
                CompileCommand: ""
            ),

            ["java"] = new SandboxLanguage(
                Name: "java",
                DockerImage: "valgo/java:latest",
                SourceFileName: "Main.java",
                RunCommand: "java Main",
                CompileCommand: "javac Main.java"
            ),

            ["javascript"] = new SandboxLanguage(
                Name: "javascript",
                DockerImage: "valgo/javascript:latest",
                SourceFileName: "main.js",
                RunCommand: "node main.js",
                CompileCommand: ""
            ),

            ["typescript"] = new SandboxLanguage(
                Name: "typescript",
                DockerImage: "valgo/typescript:latest",
                SourceFileName: "main.ts",
                RunCommand: "ts-node main.ts",
                CompileCommand: ""
            )

        };

        public static SandboxLanguage Resolve(string language)
        {
            if (!_languages.TryGetValue(language.ToLowerInvariant(), out var sandboxLang))
                throw new InvalidOperationException($"Unsupported language: {language}");

            return sandboxLang;
        }
    }
}
