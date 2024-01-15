using Norexia.Core.Domain.Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetAttachedDigitalInvoices;
public class AttachedDigitalInvoiceDto
{
    public Guid? Id { get; set; }
    public string? Label { get; set; }
    public DateTime? Created { get; set; }
    public FileBase64? File { get; set; }
}
