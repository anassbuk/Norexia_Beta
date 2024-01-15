using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Models;
namespace Norexia.Core.Application.Products.Queries.GetProductImages;

public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQuery, IEnumerable<FileBase64>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductImagesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FileBase64>?> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _context.ProductImages.Where(i => i.ProductId == request.Id).ToListAsync(cancellationToken: cancellationToken);
        List<FileBase64> imagesData = new();

        var provider = new FileExtensionContentTypeProvider();

        foreach (var img in images)
        {
            FileBase64 imgData = new();
            provider.TryGetContentType(img.Path!, out string? filenameContentType);

            imgData.Name = Path.GetFileName(img.Path!);
            imgData.ContentType = filenameContentType!;

            using (var fileInput = new FileStream(img.Path!, FileMode.Open, FileAccess.Read))
            {
                MemoryStream memoryStream = new();
                await fileInput.CopyToAsync(memoryStream, cancellationToken);

                var buffer = memoryStream.ToArray();
                imgData.Base64Data = Convert.ToBase64String(buffer);
            }

            imagesData.Add(imgData);

        }

        return imagesData;
    }


}

public record GetProductImagesQuery(Guid Id) : IRequest<IEnumerable<FileBase64>?>;
