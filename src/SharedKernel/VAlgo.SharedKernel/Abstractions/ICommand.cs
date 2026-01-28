using MediatR;

namespace VAlgo.SharedKernel.Abstractions;

public interface ICommand<out TResult> : IRequest<TResult> { }