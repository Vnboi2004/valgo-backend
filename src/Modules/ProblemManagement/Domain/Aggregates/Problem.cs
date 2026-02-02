using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.Events;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

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

        public void UpdateMetadata(string title, string statement, string? shortDescription)
        {
            if (Status == ProblemStatus.Archived)
                throw new InvalidProblemStateException(Id.Value, Status);

            if (string.IsNullOrWhiteSpace(Title))
                throw new InvalidProblemTitleException();

            // Publised: không được sử statement
            if (Status == ProblemStatus.Published)
            {
                Title = title;
                ShortDescription = shortDescription;
                return;
            }

            if (string.IsNullOrWhiteSpace(statement))
                throw new InvalidProblemStatementException();

            Title = title;
            Statement = statement;
            ShortDescription = shortDescription;
        }

        public void UpdateDifficulty(Difficulty difficulty)
        {
            if (Status == ProblemStatus.Archived)
                throw new InvalidProblemStateException(Id.Value, Status);

            Difficulty = difficulty;
        }

        public void UpdateConstraints(int timeLimitMs, int memoryLimitKb)
        {
            if (Status == ProblemStatus.Archived)
                throw new InvalidProblemStateException(Id.Value, Status);

            ValidateConstraints(timeLimitMs, memoryLimitKb);

            TimeLimitMs = timeLimitMs;
            MemoryLimitKb = memoryLimitKb;
        }

        public void RemoveTestCase(TestCaseId testCaseId)
        {
            EnsureDraft();

            var testCase = _testCases.FirstOrDefault(x => x.Id == testCaseId);

            if (testCase == null)
                throw new TestCaseNotFoundException(testCaseId);

            if (testCase.IsSample && _testCases.Count(x => x.IsSample) == 1)
                throw new CannotRemoveLastSampleTestCaseException(Id.Value);

            _testCases.Remove(testCase);

            int order = 1;
            foreach (var tc in _testCases.OrderBy(x => x.Order))
            {
                tc.SetOrder(order++);
            }
        }

        public void ReorderTestCases(IReadOnlyList<TestCaseId> orderedIds)
        {
            EnsureDraft();

            if (orderedIds.Count != _testCases.Count)
                throw new InvalidTestCaseException("Mismatch test case count.");

            var distinctIds = orderedIds.Distinct().ToList();
            if (distinctIds.Count != orderedIds.Count)
                throw new InvalidTestCaseException("Duplicate test case ids.");

            var lookup = _testCases.ToDictionary(x => x.Id);

            foreach (var id in orderedIds)
            {
                if (!lookup.ContainsKey(id))
                    throw new TestCaseNotFoundException(id);
            }

            int order = 1;
            foreach (var id in orderedIds)
            {
                lookup[id].SetOrder(order++);
            }
        }

        public void RemoveAllowedLanguage(string language)
        {
            EnsureDraft();

            var target = new AllowedLanguage(language);

            if (!_allowedLanguages.Contains(target))
                throw new AllowedLanguageNotFoundException(language);

            if (_allowedLanguages.Count == 1)
                throw new CannotRemoveLastAllowedLanguageException(Id.Value);

            _allowedLanguages.Remove(target);
        }

        public void UnassignClassification(Guid classificationId)
        {
            if (Status == ProblemStatus.Archived)
                throw new InvalidProblemStateException(Id.Value, Status);

            var target = _classifications.FirstOrDefault(x => x.ClassificationId == classificationId);

            if (target == null)
                throw new ClassificationNotFoundException(classificationId);

            _classifications.Remove(target);
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

            if (!_allowedLanguages.Any())
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