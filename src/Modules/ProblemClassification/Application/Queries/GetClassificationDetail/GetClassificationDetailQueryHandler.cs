using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail
{
    public sealed class GetClassificationDetailQueryHandler : IRequestHandler<GetClassificationDetailQuery, ClassificationDetailDto>
    {
        private readonly IClassificationQueries _classificationQueries;

        public GetClassificationDetailQueryHandler(IClassificationQueries classificationQueries)
            => _classificationQueries = classificationQueries;

        public async Task<ClassificationDetailDto> Handle(GetClassificationDetailQuery request, CancellationToken cancellationToken)
        {
            var classification = await _classificationQueries.GetDetailAsync(request.ClassificationId, cancellationToken);

            var classificationId = ClassificationId.From(request.ClassificationId);

            if (classification == null)
                throw new ClassificationNotFoundException(classificationId);

            return classification;
        }
    }
}