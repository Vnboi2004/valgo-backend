using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReactivateCompany
{
    public sealed class ReactivateCompanyCommandHandler : IRequestHandler<ReactivateCompanyCommand, Unit>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReactivateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReactivateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(CompanyId.From(request.CompanyId), cancellationToken);

            if (company == null)
                throw new InvalidOperationException("Company not found.");

            company.Reactivate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}