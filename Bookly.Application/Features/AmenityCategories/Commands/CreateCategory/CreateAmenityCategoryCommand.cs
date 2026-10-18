using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.AmenityCategories.Commands.CreateCategory;

public record CreateAmenityCategoryCommand(string NameEn,
    string NameAr,
    int SortOrder) : IRequest<Result<CreateAmenityCategoryResponse>>;
