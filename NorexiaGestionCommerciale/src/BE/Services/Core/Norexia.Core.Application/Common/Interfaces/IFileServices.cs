namespace Norexia.Core.Application.Common.Interfaces;

public interface IFileServices
{
    Task<string> StoreProductPictureAsync(Guid productId, string fileName, string data, CancellationToken cancellationToken = default);
    Task<string> StoreAttachedDigitalInvoiceAsync(string invoiceRef, string fileName, string data, CancellationToken cancellationToken = default);
}
