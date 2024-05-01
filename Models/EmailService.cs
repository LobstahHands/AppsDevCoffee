using System;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        // Set up SMTP client for Gmail
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587, // Gmail SMTP port
            Credentials = new NetworkCredential("kopaczjm@gmail.com", "Your-Gmail-Password"),
            EnableSsl = true // Enable SSL encryption
        };

        // Create and configure the email message
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("kopaczjm@gmail.com"); // Sender's email address
        mailMessage.To.Add(to); // Recipient's email address
        mailMessage.Subject = subject; // Email subject
        mailMessage.Body = body; // Email body

        try
        {
            // Send the email
            smtpClient.Send(mailMessage);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email: " + ex.Message);
        }
        finally
        {
            // Dispose of the SmtpClient to release resources
            smtpClient.Dispose();
        }
    }
}
