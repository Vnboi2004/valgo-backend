using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.Events;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Domain.Aggregates
{
    public sealed class Problem : AggregateRoot<ProblemId>
    {
        public string Code { get; private set; } = null!;
        public string Title { get; private set; } = null!;
        public string Statement { get; private set; } = null!;
        public string? ShortDescription { get; private set; }

        public string? Constraints { get; private set; }
        public string? InputFormat { get; private set; }
        public string? OutputFormat { get; private set; }
        public string? FollowUp { get; private set; }
        public string? Editorial { get; private set; }

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

        private readonly List<ProblemExample> _examples = new();
        public IReadOnlyCollection<ProblemExample> Examples => _examples;

        private readonly List<ProblemHint> _hints = new();
        public IReadOnlyCollection<ProblemHint> Hints => _hints;

        private readonly List<ProblemCompanyRef> _companies = new();
        public IReadOnlyCollection<ProblemCompanyRef> Companies => _companies;

        private readonly List<SimilarProblemRef> _similarProblems = new();
        public IReadOnlyCollection<SimilarProblemRef> SimilarProblems => _similarProblems;

        // Classification problem
        private readonly List<ProblemClassificationRef> _classifications = new();
        public IReadOnlyList<ProblemClassificationRef> Classifications => _classifications;

        private readonly List<ProblemCodeTemplate> _codeTemplates = new();
        public IReadOnlyList<ProblemCodeTemplate> CodeTemplates => _codeTemplates;

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

        public void AddCodeTemplate(
            string language,
            string userTemplate,
            string judgeTemplateHeader,
            string judgeTemplateFooter)
        {
            EnsureDraft();

            if (string.IsNullOrWhiteSpace(language))
                throw new InvalidOperationException("Language is required.");

            if (string.IsNullOrWhiteSpace(userTemplate))
                throw new InvalidOperationException("User template is required.");

            if (string.IsNullOrWhiteSpace(judgeTemplateHeader))
                throw new InvalidOperationException("Judge template header is required.");

            if (string.IsNullOrWhiteSpace(judgeTemplateFooter))
                throw new InvalidOperationException("Judge template footer is required.");

            var normalizedLanguage = language.Trim().ToLowerInvariant();

            if (_codeTemplates.Any(t => t.Language == normalizedLanguage))
                throw new DuplicateCodeTemplateException(normalizedLanguage);

            _codeTemplates.Add(ProblemCodeTemplate.Create(
                language,
                userTemplate,
                judgeTemplateHeader,
                judgeTemplateFooter));
        }

        public void UpdateCodeTemplate(
            string language,
            string userTemplate,
            string judgeTemplateHeader,
            string judgeTemplateFooter)
        {
            EnsureDraft();

            var normalizedLanguage = language.Trim().ToLowerInvariant();

            var template = _codeTemplates.FirstOrDefault(t => t.Language == normalizedLanguage)
                ?? throw new CodeTemplateNotFoundException(normalizedLanguage);

            if (string.IsNullOrWhiteSpace(userTemplate))
                throw new InvalidOperationException("User template is required.");

            if (string.IsNullOrWhiteSpace(judgeTemplateHeader))
                throw new InvalidOperationException("Judge template header is required.");

            if (string.IsNullOrWhiteSpace(judgeTemplateFooter))
                throw new InvalidOperationException("Judge template footer is required.");

            template.Update(userTemplate, judgeTemplateHeader, judgeTemplateFooter);
        }

        public void DeleteCodeTemplate(string language)
        {
            EnsureDraft();

            var normalizedLanguage = language.Trim().ToLowerInvariant();

            var template = _codeTemplates.FirstOrDefault(t => t.Language == normalizedLanguage)
                ?? throw new CodeTemplateNotFoundException(normalizedLanguage);

            _codeTemplates.Remove(template);
        }

        public void UpdateContent(
            string statement,
            string? constraints,
            string? inputFormat,
            string? outputFormat,
            string? followUp
        )
        {
            EnsureDraft();

            if (string.IsNullOrWhiteSpace(statement))
                throw new InvalidProblemStatementException();

            Statement = statement;
            Constraints = constraints;
            InputFormat = inputFormat;
            OutputFormat = outputFormat;
            FollowUp = followUp;
        }

        public void SetEditorial(string editorial)
        {
            if (Status != ProblemStatus.Published)
                throw new InvalidProblemStateException(Id.Value, Status);

            Editorial = editorial;
        }

        public void AddExample(string input, string output, string? explanation)
        {
            EnsureDraft();

            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(output))
                throw new InvalidProblemExampleException();


            int order = _examples.Any() ? _examples.Max(x => x.Order) + 1 : 1;

            _examples.Add(
                ProblemExample.Create(order, input, output, explanation)
            );
        }

        public void UpdateExample(ProblemExampleId exampleId, string input, string output, string? explanation)
        {
            EnsureDraft();
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(output))
                throw new InvalidOperationException("invalid fields.");

            var example = _examples.FirstOrDefault(x => x.Id == exampleId);

            if (example == null)
                throw new InvalidOperationException("Example not found.");

            example.Update(input, output, explanation);
        }

        public void DeleteExample(ProblemExampleId exampleId)
        {
            EnsureDraft();

            var example = _examples.FirstOrDefault(x => x.Id == exampleId);

            if (example == null)
                throw new InvalidOperationException("Example not found.");

            _examples.Remove(example);

            int order = 1;
            foreach (var ex in _examples.OrderBy(x => x.Order))
            {
                ex.SetOrder(order++);
            }
        }

        public void AddHint(string content)
        {
            EnsureDraft();

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidProblemHintException();

            int order = _hints.Any() ? _hints.Max(x => x.Order) + 1 : 1;

            _hints.Add(
                ProblemHint.Create(order, content)
            );
        }

        public void UpdateHint(ProblemHintId hintId, string content)
        {
            EnsureDraft();

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidProblemHintException();

            var hint = _hints.FirstOrDefault(x => x.Id == hintId);

            if (hint == null)
                throw new InvalidOperationException("Hint not found.");

            hint.Update(content);
        }

        public void DeleteHint(ProblemHintId hintId)
        {
            EnsureDraft();

            var example = _hints.FirstOrDefault(x => x.Id == hintId);

            if (example == null)
                throw new InvalidOperationException("Hint not found.");

            _hints.Remove(example);

            int order = 1;
            foreach (var h in _hints.OrderBy(x => x.Order))
            {
                h.SetOrder(order++);
            }
        }

        public void AddCompany(Guid companyId)
        {
            if (_companies.Any(x => x.CompanyId == companyId))
                return;

            _companies.Add(
                ProblemCompanyRef.Create(companyId)
            );
        }

        public void AddSimilarProblem(ProblemId problemId)
        {
            if (problemId == Id)
                throw new InvalidSimilarProblemException();

            if (_similarProblems.Any(x => x.ProblemId == problemId))
                return;

            _similarProblems.Add(
                SimilarProblemRef.Create(problemId)
            );
        }

        public void UpdateMetadata(string title, string? shortDescription)
        {
            if (Status == ProblemStatus.Archived)
                throw new InvalidProblemStateException(Id.Value, Status);

            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidProblemTitleException();

            Title = title;
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

        public void UpdateTestCase(TestCaseId testCaseId, string input, string expectedOutput, OutputComparisonStrategy comparisonStrategy, bool isSample)
        {
            EnsureDraft();

            var testCase = _testCases.FirstOrDefault(x => x.Id == testCaseId)
                ?? throw new TestCaseNotFoundException(testCaseId);

            // Nếu đang unmark sample, đảm bảo vẫn còn ít nhất 1 sample khác
            if (testCase.IsSample && !isSample && _testCases.Count(x => x.IsSample) == 1)
                throw new CannotRemoveLastSampleTestCaseException(Id.Value);

            testCase.Update(input, expectedOutput, comparisonStrategy, isSample);
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

        public void DeleteSimilar(SimilarProblemRefId similarProblemId)
        {
            EnsureDraft();

            var similar = _similarProblems.FirstOrDefault(x => x.Id == similarProblemId);

            if (similar == null)
                throw new InvalidOperationException("Similar problem not found.");

            _similarProblems.Remove(similar);
        }

        public void DeleteCompany(Guid companyId)
        {
            EnsureDraft();

            var company = _companies.FirstOrDefault(x => x.CompanyId == companyId);

            if (company == null)
                throw new InvalidOperationException("Company not found");

            _companies.Remove(company);
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

        public void ReorderExamples(IReadOnlyList<ProblemExampleId> exampleIds)
        {
            EnsureDraft();

            if (exampleIds.Count != _examples.Count)
                throw new InvalidOperationException("Mismatch example count.");

            var distinctIds = exampleIds.Distinct().ToList();
            if (distinctIds.Count != exampleIds.Count)
                throw new InvalidTestCaseException("Duplicate example ids.");

            var lockup = _examples.ToDictionary(x => x.Id);

            foreach (var id in exampleIds)
            {
                if (!lockup.ContainsKey(id))
                    throw new InvalidOperationException("Example not found.");
            }

            int order = 1;
            foreach (var id in exampleIds)
            {
                lockup[id].SetOrder(order++);
            }
        }

        public void ReorderHints(IReadOnlyList<ProblemHintId> hintIds)
        {
            EnsureDraft();

            if (hintIds.Count != _hints.Count)
                throw new InvalidOperationException("Mismatch example count.");

            var distinctIds = hintIds.Distinct().ToList();
            if (distinctIds.Count != hintIds.Count)
                throw new InvalidTestCaseException("Duplicate hint ids.");

            var lookup = _hints.ToDictionary(x => x.Id);

            foreach (var id in hintIds)
            {
                if (!lookup.ContainsKey(id))
                    throw new InvalidOperationException("Example not found.");
            }

            int order = 1;
            foreach (var id in hintIds)
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

            var missTemplates = _allowedLanguages
                .Where(lang => !_codeTemplates.Any(t => t.Language == lang.Value.ToLowerInvariant()))
                .Select(lang => lang.Value)
                .ToList();

            if (missTemplates.Any())
                throw new InvalidOperationException("Missing code template.");

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