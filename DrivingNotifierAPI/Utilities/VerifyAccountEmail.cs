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
    public class VerifyAccountEmail
    {
        private static readonly String apiBaseUrl = "https://drivingnotifierapi20180709102032.azurewebsites.net";
        //private static readonly String apiBaseUrl = "http://localhost:2522";

        public static async Task SendVerifyEmail(Models.User user)
        {

            var dataUser = new DataAccessUser();
            var userFetched = dataUser.GetUserByEmail(user.Email);
            String id = userFetched.IdEntity.ToString();
            String urlVerification = apiBaseUrl + "/api/Users/ActivateAccount/" + id;
            String urlDeclination = apiBaseUrl + "/api/Users/DeleteUser/" + id;

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
                 {"Subject", "Welcome to Driving Notifier App!"},
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
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:10px 25px;padding-top:0px;padding-bottom:0px;padding-right:0px;padding-left:0px;' align='center'><table role='presentation' cellpadding='0' cellspacing='0' style='border-collapse:collapse;border-spacing:0px;' align='center' border='0'><tbody><tr><td style='width:577px;'><img alt=' title=' height='auto' src='http://90p1.mjt.lu/tplimg/90p1/b/6u2s/1pwj.jpeg' style='border:none;border-radius:;display:block;font-size:13px;outline:none;text-decoration:none;width:100%;height:auto;' width='577'></td></tr></tbody></table></td></tr></tbody></table></div><!--[if mso | IE]>
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
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 25px 0px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:left;'><h1 style='font-size: 20px; font-weight: bold;'>WELCOME to Driving Notifier App!</h1></div></td></tr><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 25px 0px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:left;'><p style='font-size: 13px; margin: 10px 0;'>Hello " + user.FullName +@" ,</p><p style='font-size: 13px; margin: 10px 0;'>From the Driving Notifier Team, we would like to give you our warmest welcome to our App. To start using Driving Notifier App you still need to verify your e-mail by clicking the next button:</p></div></td></tr></tbody></table></div><!--[if mso | IE]>
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
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:10px 25px;' align='center'><table role='presentation' cellpadding='0' cellspacing='0' style='border-collapse:separate;' align='center' border='0'><tbody><tr><td style='border:none;border-radius:3px;color:#ffffff;cursor:auto;padding:10px 25px;' align='center' valign='middle' bgcolor='#20a79c'><a href='" + urlVerification + @"' style='text-decoration:none;background:#20a79c;color:#ffffff;font-family:Arial, sans-serif;font-size:13px;font-weight:normal;line-height:120%;text-transform:none;margin:0px;' target='_blank'><b>Verify</b></a></td></tr></tbody></table></td></tr></tbody></table></div><!--[if mso | IE]>
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
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:10px 25px;padding-top:0px;padding-bottom:0px;' align='left'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:13px;line-height:22px;text-align:left;'><p style='font-size: 13px; margin: 10px 0;'>If you have not registered and created a new account, please ignore this message and click <a target='_blank' href='"+ urlDeclination +@"' style='color: rgb(32, 167, 156);'><span style='color:#20a79c'>here</span></a>.</p><p style='font-size: 13px; margin: 10px 0;'>THANK YOU for using Driving Notifier.</p><p style='font-size: 13px; margin: 10px 0;'> </p></div></td></tr></tbody></table></div><!--[if mso | IE]>
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
      <![endif]--><div class='mj-column-per-100 outlook-group-fix' style='vertical-align:top;display:inline-block;direction:ltr;font-size:13px;text-align:left;width:100%;'><table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 20px 0px 20px;padding-top:0px;padding-bottom:0px;' align='center'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:11px;line-height:22px;text-align:center;'><p style='font-size: 13px; margin: 10px 0;'>This e-mail has been sent to [[EMAIL_TO]], <a href='[[UNSUB_LINK_EN]]' style='color:inherit;text-decoration:none;' target='_blank'>click here to unsubscribe</a>.</p></div></td></tr><tr><td style='word-wrap:break-word;font-size:0px;padding:0px 20px 0px 20px;padding-top:0px;padding-bottom:0px;' align='center'><div style='cursor:auto;color:#55575d;font-family:Arial, sans-serif;font-size:11px;line-height:22px;text-align:center;'><p style='font-size: 13px; margin: 10px 0;'>DK</p></div></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></td></tr></tbody></table></div><!--[if mso | IE]>
      </td></tr></table>
      <![endif]--></div>
</body>
</html>"}
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
