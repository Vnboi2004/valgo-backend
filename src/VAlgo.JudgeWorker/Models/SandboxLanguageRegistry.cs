namespace VAlgo.JudgeWorker.Models
{
    public sealed class SandboxLanguageRegistry
    {
        private static readonly Dictionary<string, SandboxLanguage> _languages = new()
        {
            ["python"] = new SandboxLanguage(
                Name: "python",
                DockerImage: "valgo/python:latest",
                SourceFileName: "main.py",
                RunCommand: "python main.py",
                CompileCommand: ""
            ),

            ["cpp"] = new SandboxLanguage(
                Name: "cpp",
                DockerImage: "gcc:12.2.0",
                SourceFileName: "main.cpp",
                RunCommand: "./main",
                CompileCommand: "g++ main.cpp -O2 -std=c++17 -o main"
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
