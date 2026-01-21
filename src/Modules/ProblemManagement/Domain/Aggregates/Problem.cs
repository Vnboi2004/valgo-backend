using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.Events;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Domain.Aggregates
{
    public sealed class Problem : AggregateRoot<ProblemId>
    {
        public string Code { get; private set; } = null!;
        public string Title { get; private set; } = null!;
        public string Statement { get; private set; } = null!;
        public string? ShortDescription { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public ProblemStatus Status { get; private set; }
        public int TimeLimitMs { get; private set; }
        public int MemoryLimitKb { get; private set; }

        // AllowLanguage
        private readonly List<AllowedLanguage> _allowedLanguages = new();
        public IReadOnlyCollection<AllowedLanguage> AllowedLanguages => _allowedLanguages;

        // TestCases
        private readonly List<TestCase> _testCases = new();
        public IReadOnlyCollection<TestCase> TestCases => _testCases;

        // Classification problem
        private readonly List<ProblemClassificationRef> _classifications = new();
        public IReadOnlyList<ProblemClassificationRef> Classifications => _classifications;

        private Problem() { }

        private Problem(
            ProblemId id,
            string code,
            string title,
            string statement,
            string? shortDescription,
            Difficulty difficulty,
            int timeLimitMs,
            int memoryLimitKb
        ) : base(id)
        {
            Code = NormalizeCode(code);
            Title = title;
            Statement = statement;
            ShortDescription = shortDescription;
            Difficulty = difficulty;
            ValidateConstraints(timeLimitMs, memoryLimitKb);
            TimeLimitMs = timeLimitMs;
            MemoryLimitKb = memoryLimitKb;
            Status = ProblemStatus.Draft;
        }

        public static Problem Create(
            string code,
            string title,
            string statement,
            string? shortDescription,
            Difficulty difficulty,
            int timeLimitMs,
            int memoryLimitKb
        )
        {
            return new Problem(ProblemId.New(), code, title, statement, shortDescription, difficulty, timeLimitMs, memoryLimitKb);
        }


        public void AddTestCase(string input, string expectedOutput, OutputComparisonStrategy outputComparisonStrategy, bool isSample)
        {
            EnsureDraft();

            int nextOrder = _testCases.Any() ? _testCases.Max(x => x.Order) + 1 : 1;
            _testCases.Add(TestCase.Create(nextOrder, input, expectedOutput, outputComparisonStrategy, isSample));
        }

        public void AddAllowedLanguage(string language)
        {
            EnsureDraft();

            var allowed = new AllowedLanguage(language);

            if (_allowedLanguages.Contains(allowed))
                return;

            _allowedLanguages.Add(allowed);
        }

        public void AddClassificationRef(Classification classification)
        {
            if (!classification.IsActive)
                throw new InvalidClassificationException();

            if (_classifications.Any(x => x.ClassificationId == classification.Id.Value))
                return;

            _classifications.Add(ProblemClassificationRef.Create(classification.Id.Value));
        }

        public void Publish(DateTime now)
        {
            EnsureDraft();

            if (!_testCases.Any())
                throw new ProblemWithoutTestCasesException(Id.Value);

            if (_allowedLanguages.Any())
                throw new ProblemWithoutAllowedLanguagesException(Id.Value);

            if (string.IsNullOrWhiteSpace(ShortDescription))
                throw new MissingShortDescriptionException(Id.Value);

            Status = ProblemStatus.Published;

            AddDomainEvent(new ProblemPublishedDomainEvent(Id.Value, now));
        }

        public void Archive(DateTime now)
        {
            if (Status != ProblemStatus.Published)
                throw new InvalidProblemStateException(Id.Value, Status);

            Status = ProblemStatus.Archived;
            AddDomainEvent(new ProblemArchivedDomainEvent(Id.Value, now));
        }

        public static void ValidateConstraints(int timeLimitMs, int memoryLimitKb)
        {
            if (timeLimitMs <= 0)
                throw new InvalidTimeLimitException(timeLimitMs);

            if (memoryLimitKb <= 0)
                throw new InvalidMemoryLimitException(memoryLimitKb);
        }

        private static string NormalizeCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new InvalidProblemCodeException();

            return code.Trim().ToUpperInvariant();
        }

        private void EnsureDraft()
        {
            if (Status != ProblemStatus.Draft)
                throw new ProblemAlreadyPublishedException(Id.Value);
        }
    }
}