using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Models;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetAttachedDigitalInvoices;
public class GetAttachedDigitalInvoicesQueryHandler : IRequestHandler<GetAttachedDigitalInvoicesQuery, IEnumerable<AttachedDigitalInvoiceDto>?>
{
    private readonly IApplicationDbContext _context;
    public GetAttachedDigitalInvoicesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AttachedDigitalInvoiceDto>?> Handle(GetAttachedDigitalInvoicesQuery request, CancellationToken cancellationToken)
    {
        var attachedDigitalInvoices = await _context.AttachedDigitalInvoices.Where(i => i.InvoiceId == request.Id).ToListAsync(cancellationToken);
        List<AttachedDigitalInvoiceDto> attachedDigitalInvoiceDtos = new();

        var provider = new FileExtensionContentTypeProvider();

        foreach (var attachedDigitalInvoice in attachedDigitalInvoices)
        {
            AttachedDigitalInvoiceDto attachedDigitalInvoiceDto = new()
            {
                Id = attachedDigitalInvoice.Id,
                Label = attachedDigitalInvoice.Label,
                Created = attachedDigitalInvoice.Created
            };

            FileBase64 fileData = new();

            provider.TryGetContentType(attachedDigitalInvoice.Path!, out string? filenameContentType);

            fileData.Name = Path.GetFileName(attachedDigitalInvoice.Path!);
            fileData.ContentType = filenameContentType!;

            using (var fileInput = new FileStream(attachedDigitalInvoice.Path!, FileMode.Open, FileAccess.Read))
            {
                MemoryStream memoryStream = new();
                await fileInput.CopyToAsync(memoryStream, cancellationToken);

                var buffer = memoryStream.ToArray();
                fileData.Base64Data = Convert.ToBase64String(buffer);
            }

            attachedDigitalInvoiceDto.File = fileData;
            attachedDigitalInvoiceDtos.Add(attachedDigitalInvoiceDto);

        }

        return attachedDigitalInvoiceDtos;
    }
}

public record GetAttachedDigitalInvoicesQuery(Guid Id) : IRequest<IEnumerable<AttachedDigitalInvoiceDto>?>;
