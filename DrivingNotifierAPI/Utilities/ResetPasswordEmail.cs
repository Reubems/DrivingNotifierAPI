using DrivingNotifierAPI.Data;
using DrivingNotifierAPI.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DrivingNotifierAPI.Utilities
{
    public class ResetPasswordEmail
    {
        private static readonly String apiBaseUrl = "https://drivingnotifierapi20180709102032.azurewebsites.net";
        //private static readonly String apiBaseUrl = "http://localhost:2522";

        public static async Task SendResetPasswordEmail(Models.User user)
        {

            var dataUser = new DataAccessUser();
            var userFetched = dataUser.GetUserByEmail(user.Email);
            String code = userFetched.ResetCode.ToString();

            MailjetClient client = new MailjetClient("aaa4cc8830c2429e4ded6fe052dd58b4", "a7b15b276450f99715b15ee20f3e4ece")
            //MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"), Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"))
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "rms@trifork.com"},
                  {"Name", "Driving Notifier App"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", user.Email},
                   {"Name", user.Email}
                   }
                  }},
                 {"Subject", "Reset Password: Driving Notifier App"},
                 {"TemplateLanguage", true},
                 {"HTMLPart",@"<!doctype html>
<html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>
<head>
  <title>Someone wants to add you!</title>
  <!--[if !mso]><!-- -->
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <!--<![endif]-->
<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<style type='text/css'>
  #outlook a { padding: 0; }
  .ReadMsgBody { width: 100%; }
  .ExternalClass { width: 100%; }
  .ExternalClass * { line-height:100%; }
  body { margin: 0; padding: 0; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }
  table, td { border-collapse:collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; }
  img { border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; }
  p { display: block; margin: 13px 0; }
</style>
<!--[if !mso]><!-->
<style type='text/css'>
  @media only screen and (max-width:480px) {
    @-ms-viewport { width:320px; }
    @viewport { width:320px; }
  }
</style>
<!--<![endif]-->
<!--[if mso]>
<xml>
  <o:OfficeDocumentSettings>
    <o:AllowPNG/>
    <o:PixelsPerInch>96</o:PixelsPerInch>
  </o:OfficeDocumentSettings>
</xml>
<![endif]-->
<!--[if lte mso 11]>
<style type='text/css'>
  .outlook-group-fix {
    width:100% !important;
  }
</style>
<![endif]-->
<style type='text/css'>
  @media only screen and (min-width:480px) {
    .mj-column-per-100 { width:100%!important; }
  }
</style>
</head>
<body style='background: #F4F4F4;'>
  
  <div class='mj-container' style='background-color:#F4F4F4;'><!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='600' align='center' style='width:600px;'>
        <tr>
          <td style='line-height:0px;font-size:0px;mso-line-height-rule:exactly;'>
      <![endif]--><div style='margin:0px auto;max-width:600px;'><table role='presentation' cellpadding='0' cellspacing='0' style='font-size:0px;width:100%;' align='center' border='0'><tbody><tr><td style='text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 0px;padding-bottom:0px;padding-top:0px;'><!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
        <tr>
          <td style='vertical-align:top;width:600px;'>
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:10px 25px;padding-top:0px;padding-bottom:0px;padding-right:0px;padding-left:0px;' align='center'><table role='presentation' cellpadding='0' cellspacing='0' style='border-collapse:collapse;border-spacing:0px;' align='center' border='0'><tbody><tr><td style='width:577px;'><img alt=' title=' height='auto' src='http://90p1.mjt.lu/tplimg/90p1/b/6vm8/1hks.jpeg' style='border:none;border-radius:;display:block;font-size:13px;outline:none;text-decoration:none;width:100%;height:auto;' width='577'></td></tr></tbody></table></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]-->
      <!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='600' align='center' style='width:600px;'>
        <tr>
          <td style='line-height:0px;font-size:0px;mso-line-height-rule:exactly;'>
      <![endif]--><div style='margin:0px auto;max-width:600px;background:#ffffff;'><table role='presentation' cellpadding='0' cellspacing='0' style='font-size:0px;width:100%;background:#ffffff;' align='center' border='0'><tbody><tr><td style='text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 0px 20px 0px;'><!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
        <tr>
          <td style='vertical-align:top;width:600px;'>
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 25px 0px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:left;'><h1 style='font-size: 20px; font-weight: bold;'> Driving Notifier App: Password Reset</h1></div></td></tr><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 25px 0px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:left;'><p style='font-size: 13px; margin: 10px 0;'>Hello,</p><p style='font-size: 13px; margin: 10px 0;'>We have noticed that you want to reset your password for accessing the App. For doing so, you can use the following code and then update your password:</p></div></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]-->
      <!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='600' align='center' style='width:600px;'>
        <tr>
          <td style='line-height:0px;font-size:0px;mso-line-height-rule:exactly;'>
      <![endif]--><div style='margin:0px auto;max-width:600px;background:#ffffff;'><table role='presentation' cellpadding='0' cellspacing='0' style='font-size:0px;width:100%;background:#ffffff;' align='center' border='0'><tbody><tr><td style='text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 0px;padding-bottom:0px;padding-top:0px;'><!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
        <tr>
          <td style='vertical-align:top;width:600px;'>
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:10px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:24px;text-align:left;'><p style='font-size: 13px; text-align: center; margin: 10px 0;'><span style='font-family:Lucida Console,Helvetica,Arial,sans-serif'>PASSWORD CODE: " + code + @"</span></p></div></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]-->
      <!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='600' align='center' style='width:600px;'>
        <tr>
          <td style='line-height:0px;font-size:0px;mso-line-height-rule:exactly;'>
      <![endif]--><div style='margin:0px auto;max-width:600px;background:#ffffff;'><table role='presentation' cellpadding='0' cellspacing='0' style='font-size:0px;width:100%;background:#ffffff;' align='center' border='0'><tbody><tr><td style='text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 0px;'><!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
        <tr>
          <td style='vertical-align:top;width:600px;'>
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:10px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:left;'><p style='font-size: 13px; margin: 10px 0;'> </p><p style='font-size: 13px; margin: 10px 0;'>You can generate a new code as many times as you need it, and later you can reset you password again through the Reset Password section in the main view of our App.</p><p style='font-size: 13px; margin: 10px 0;'>Best regards,</p><p style='font-size: 13px; margin: 10px 0;'>Driving Notifier App</p><p style='font-size: 13px; margin: 10px 0;'> </p></div></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]-->
      <!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='600' align='center' style='width:600px;'>
        <tr>
          <td style='line-height:0px;font-size:0px;mso-line-height-rule:exactly;'>
      <![endif]--><div style='margin:0px auto;max-width:600px;'><table role='presentation' cellpadding='0' cellspacing='0' style='font-size:0px;width:100%;' align='center' border='0'><tbody><tr><td style='text-align:center;vertical-align:top;direction:ltr;font-size:0px;padding:20px 0px 20px 0px;'><!--[if mso | IE]>
      <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
        <tr>
          <td style='vertical-align:top;width:600px;'>
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 20px 0px 20px;padding-top:0px;padding-bottom:0px;' align='center'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:11px;line-height:22px;text-align:center;'><p style='font-size: 13px; margin: 10px 0;'>This e-mail has been sent to [[EMAIL_TO]], <a href='https://www.youtube.com/watch?v=dQw4w9WgXcQ' style='color:inherit;text-decoration:none;' target='_blank'>click here to unsubscribe</a>.</p></div></td></tr><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 20px 0px 20px;padding-top:0px;padding-bottom:0px;' align='center'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:11px;line-height:22px;text-align:center;'><p style='font-size: 13px; margin: 10px 0;'>DK</p></div></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></div>
</body>
</html>"
               }
         }
   });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }
    }
}
