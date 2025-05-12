using System.Globalization;
using System.Text;

using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Order.CreateOrder;

using Exceptions = Domain.Exceptions.Exceptions;
using Order = Domain.Aggregates.Order.Entities.Order;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork,
                                     UserManager<AppUser> userManager,
                                     IProductRepository productRepository,
                                     IOrderRepository orderRepository,
                                     IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.CustomerId.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var customerName = user.GetFullName();

        var orderItemDetailsForEmail = new List<(string ProductName, int Quantity, decimal ProductPrice, decimal TotalPrice)>();

        var order = Order.Create(request.CustomerId,
                                 request.Note,
                                 request.CustomerPhoneNumber,
                                 request.PaymentMethod,
                                 request.ShippingAddress);

        foreach (var item in request.OrderItems)
        {
            Guid productId = item.ProductId;
            int quantity = item.Quantity;

            var product = await _productRepository.FindByIdAsync(item.ProductId);

            if (product is null)
                throw new Exceptions.Product.NotFound();

            var unitPrice = product.ProductPrice.Amount;

            var totalPrice = item.Quantity * product.ProductPrice.Amount;

            orderItemDetailsForEmail.Add((product.Name, quantity, unitPrice, totalPrice));

            order.CreateOrderDetail(productId, quantity, unitPrice);
        }

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // send order notification
        string subject = $"Order Confirmation - #{order.Id}";
        string content = GenerateOrderConfirmationHtml(order, orderItemDetailsForEmail, order.CreatedDateTime, request.Email, customerName, request.CustomerPhoneNumber);
        await _emailService.SendEmailAsync(new EmailContent
        {

            ToEmail = request.Email,
            Subject = subject,
            Body = content,
        });

        return Result.Success("Order successfully.");
    }

    private static string GenerateOrderConfirmationHtml(Order order,
                                                        List<(string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice)> itemDetails,
                                                        DateTimeOffset OrderDate,
                                                        string customerEmail,
                                                        string customerName,
                                                        string shippingPhoneNumber)
    {
        var culture = new CultureInfo("vi-VN");
        var html = new StringBuilder();

        html.Append("<!DOCTYPE html>");
        html.Append("<html lang=\"vi\">");

        html.Append("<head><meta charset=\"UTF-8\"><title>Xác nhận đơn hàng</title>");

        html.Append("<style>");
        html.Append("body { font-family: sans-serif; line-height: 1.6; color: #333; }");
        html.Append(".container { max-width: 600px; margin: 20px auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px; }");
        html.Append("h1, h2 { color: #0056b3; }");
        html.Append("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
        html.Append("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
        html.Append("th { background-color: #f2f2f2; }");
        html.Append(".total { font-weight: bold; text-align: right; }");
        html.Append(".footer { margin-top: 20px; font-size: 0.9em; color: #777; text-align: center; }");
        html.Append("</style>");

        html.Append("</head>");

        html.Append("<body>");
        html.Append("<div class=\"container\">");
        html.Append($"<h1>Cảm ơn bạn đã đặt hàng, {customerEmail}!</h1>"); // Vietnamese greeting
        html.Append($"<p>Đơn hàng <strong>#{order.Id}</strong> của bạn đặt vào ngày {OrderDate.ToString("dd/MM/yyyy HH:mm", culture)} đã được xác nhận.</p>"); // Vietnamese confirmation

        html.Append("<h2>Địa chỉ giao hàng</h2>");

        html.Append("<p>");
        if (!string.IsNullOrEmpty(customerName)) html.Append($"<strong>{customerName}</strong><br>");
        if (!string.IsNullOrEmpty(shippingPhoneNumber)) html.Append($"<strong>{shippingPhoneNumber}</strong><br>");
        if (!string.IsNullOrEmpty(order.ShippingAddress.StreetAddress)) html.Append($"{order.ShippingAddress.StreetAddress}<br>");
        if (!string.IsNullOrEmpty(order.ShippingAddress.Ward)) html.Append($"{order.ShippingAddress.Ward}<br>");
        if (!string.IsNullOrEmpty(order.ShippingAddress.District)) html.Append($"{order.ShippingAddress.District}<br>");
        if (!string.IsNullOrEmpty(order.ShippingAddress.CityProvince)) html.Append($"{order.ShippingAddress.CityProvince}<br>");
        html.Append("</p>");

        html.Append("<h2>Chi tiết đơn hàng</h2>");
        html.Append("<table>");
        html.Append("<thead><tr><th>Sản phẩm</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr></thead>");
        html.Append("<tbody>");

        foreach (var item in itemDetails)
        {
            html.Append("<tr>");
            html.Append($"<td>{item.ProductName}");
            html.Append("</td>");
            html.Append($"<td>{item.Quantity}</td>");
            html.Append($"<td>{item.UnitPrice.ToString("C", culture)}</td>");
            html.Append($"<td>{item.TotalPrice.ToString("C", culture)}</td>");
            html.Append("</tr>");
        }

        html.Append("</tbody>");
        html.Append("<tfoot>");
        html.Append($"<tr><td colspan=\"3\" class=\"total\">Tổng cộng:</td><td class=\"total\">{order.TotalPrice.ToString("C", culture)}</td></tr>");
        html.Append("</tfoot>");
        html.Append("</table>");

        html.Append($"<p>Phương thức thanh toán: {order.PaymentMethod}</p>");

        html.Append("<div class=\"footer\">");
        html.Append("<p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với đội ngũ hỗ trợ của chúng tôi.</p>");
        html.Append("<p>&copy; " + DateTime.Now.Year + " HeadphoneStore</p>");
        html.Append("</div>");

        html.Append("</div>");
        html.Append("</body>");
        html.Append("</html>");

        return html.ToString();
    }
}
