using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Application.Contracts.Services.Email;
using Bookly.Domain.Enums;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Bookly.Infrastructure.Services.BackgroundJobs.Jobs;

public class BookingConfirmationEmailJob(IEmailService emailService,
    AppDbContext appDbContext,
    ILogger<BookingConfirmationEmailJob> logger)
    : IBookingConfirmationEmailJob
{
    public async Task SendAsync(int bookingId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting booking confirmation email job for booking {BookingId}.",
            bookingId);

        var booking = await appDbContext.Bookings
            .Include(b => b.Hotel)
            .Include(b => b.BookingRooms)
            .FirstOrDefaultAsync(
                b => b.Id == bookingId,
                cancellationToken);

        if (booking is null)
        {
            logger.LogWarning(
                "Booking {BookingId} was not found. Confirmation email will not be sent.",
                bookingId);

            return;
        }
        if (booking.Status != BookingStatus.Confirmed)
        {
            logger.LogWarning(
                "Booking {BookingId} is not confirmed. Current status: {Status}.",
                bookingId,
                booking.Status);
            return;
        }
        if (string.IsNullOrWhiteSpace(booking.Email))
        {
            logger.LogWarning(
                "Booking {BookingId} has no email address.",
                bookingId);
            return;
        }
        var subject = $"Booking Confirmation - {booking.Reference}";
        var guestName = WebUtility.HtmlEncode(booking.GuestName);
        var reference = WebUtility.HtmlEncode(booking.Reference);


        var body = $"""
<!DOCTYPE html>
<html>
<body style="margin:0;padding:25px;background:#f4f6f9;font-family:Arial,sans-serif;color:#333;">
    <table width="100%" cellpadding="0" cellspacing="0" style="max-width:520px;margin:auto;background:#fff;border-radius:10px;overflow:hidden;">
        <tr>
            <td style="background:#183b56;padding:22px;text-align:center;">
                <h1 style="margin:0;color:#fff;font-size:25px;">Bookly</h1>
            </td>
        </tr>

        <tr>
            <td style="padding:25px 25px 15px;text-align:center;">
                <h2 style="margin:0 0 10px;color:#218653;font-size:21px;">✓ Booking Confirmed!</h2>
                <p style="font-size:14px;line-height:1.6;color:#667085;">
                    Hello {guestName},<br>
                    Your booking has been successfully confirmed.
                    We're excited to welcome you!
                </p>
            </td>
        </tr>

        <tr>
            <td style="padding:15px 25px;">
                <div style="background:#f4f7fa;padding:15px;text-align:center;border-radius:8px;">
                    <p style="margin:0 0 7px;color:#667085;font-size:11px;">BOOKING REFERENCE</p>
                    <strong style="font-size:21px;color:#183b56;">{reference}</strong>
                </div>
            </td>
        </tr>

        <tr>
            <td style="padding:10px 25px 25px;">
                <table width="100%" cellpadding="8" cellspacing="0">
                    <tr>
                        <td style="color:#667085;font-size:13px;">Check-in</td>
                        <td align="right" style="font-size:13px;font-weight:bold;">
                            {booking.CheckIn:dd MMM yyyy}
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#667085;font-size:13px;">Check-out</td>
                        <td align="right" style="font-size:13px;font-weight:bold;">
                            {booking.CheckOut:dd MMM yyyy}
                        </td>
                    </tr>
                    <tr>
                        <td style="color:#183b56;font-size:14px;font-weight:bold;">
                            Total Price
                        </td>
                        <td align="right" style="font-size:17px;font-weight:bold;color:#218653;">
                            {booking.TotalPrice:N2} EGP
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td style="background:#f8fafc;padding:18px;text-align:center;">
                <p style="margin:0;color:#183b56;font-size:13px;font-weight:bold;">
                    Thank you for choosing Bookly!
                </p>
                <p style="margin:7px 0 0;color:#98a2b3;font-size:11px;">
                    We look forward to making your stay memorable.
                </p>
            </td>
        </tr>
    </table>
</body>
</html>
""";
        await emailService.SendEmailAsync(
            booking.Email,
            subject,
            body);
        logger.LogInformation(
            "Booking confirmation email sent successfully for booking {BookingId} to {Email}.",
            bookingId,
            booking.Email);
    }
}