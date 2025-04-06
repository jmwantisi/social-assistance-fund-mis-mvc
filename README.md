# Social Assistance Fund MIS

This project is a .NET Core application for managing the Social Assistance Fund. It includes various features like managing applications, applicants, and sending notifications via email.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Configuration Steps](#configuration-steps)
    - [Database Configuration](#database-configuration)
    - [Email Service Configuration](#email-service-configuration)
3. [Build and Run](#build-and-run)
    - [Building the Application](#building-the-application)
    - [Running the Application](#running-the-application)
4. [Pending Database Migrations](#pending-database-migrations)

---

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

- [.NET 6.0 SDK or higher](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any other compatible database)
- [Visual Studio 2022 or Visual Studio Code](https://code.visualstudio.com/)
- [MailKit](https://github.com/jstedfast/MailKit) for email functionality

---

## Cloning

```
  git clone https://github.com/your-repository.git
  cd your-repository
```

## Configuration Steps

### Database Configuration

1. **Connection String:**
   The connection string for the database needs to be set in your `appsettings.json` file.

   Example for SQL Server:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=SocialAssistanceFundDB;User Id=yourUsername;Password=yourPassword;"
     }
   }
2. **Email Service Configuration**
   Configure Email Settings: The project uses the MailKit library for sending emails.
   In order to configure the email service, you need to set up the SMTP server and credentials in the EmailService class.
   - Replace the smtpServer, smtpPort, smtpUsername, and smtpPassword with your own SMTP details.
   - Ensure the fromEmail and fromName are set to the email from which notifications will be sent.
   ```
     public class EmailService : IEmailService
      {
          private readonly string smtpServer = "smtp-relay.brevo.com";
          private readonly int smtpPort = 587;
          private readonly string smtpUsername = "89b417002@smtp-brevo.com";
          private readonly string smtpPassword = "QtIKX107kpHBS62N";
      
          private readonly string fromEmail = "89b417001@smtp-brevo.com";
          private readonly string fromName = "SOCIAL ASSISTANCE FUND";
      
          public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
          {
              var email = new MimeMessage();
              email.From.Add(new MailboxAddress(fromName, fromEmail));
              email.To.Add(MailboxAddress.Parse(toEmail));
              email.Subject = subject;
              email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
              {
                  Text = htmlMessage
              };
      
              using var smtp = new SmtpClient();
              await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
              await smtp.AuthenticateAsync(smtpUsername, smtpPassword);
              await smtp.SendAsync(email);
              await smtp.DisconnectAsync(true);
          }
      }
   ```
   ## Build and Run
   ### Building the Application
   - Restore NuGet Packages: Ensure all required packages are installed by running the following command:
     ```
       dotnet restore
     ```
   -  Build the Application: Build the project using:
       ```
         dotnet build
       ```
   ### Running the Application
   ```
     dotnet run
   ```
   ### Access Application
   ```
      http://localhost:<port>
   ```
   

  
