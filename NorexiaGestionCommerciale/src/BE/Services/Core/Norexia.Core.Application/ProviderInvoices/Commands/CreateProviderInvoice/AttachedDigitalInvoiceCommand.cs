using Norexia.Core.Domain.Common.Models;

namespace Norexia.Core.Application.ProviderInvoices.Commands.CreateProviderInvoice;
public class AttachedDigitalInvoiceCommand
{
    public Guid? Id { get; set; }
    public string? Label { get; set; }
    public FileBase64? File { get; set; }
}
