using MediatR;

namespace VAlgo.SharedKernel.Abstractions;

public interface IQuery<out TResult> : IRequest<TResult> { }