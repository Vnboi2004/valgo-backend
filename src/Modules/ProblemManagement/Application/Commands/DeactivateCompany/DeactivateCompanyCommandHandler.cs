using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeactivateCompany
{
    public sealed class DeactivateCompanyCommandHandler : IRequestHandler<DeactivateCompanyCommand, Unit>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeactivateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(CompanyId.From(request.CompanyId), cancellationToken);

            if (company == null)
                throw new InvalidOperationException("Company not found.");

            company.Deactivate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}