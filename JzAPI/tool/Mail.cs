using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dm.Model.V20151123;
using DAL.Tools;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JzAPI.tool
{
    public class Mail
    {
        public static void SendEmailAsync(string sendTo, int id)
        {
            string path = AppConfig.Configuration["url"];
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("发件人", "systemadmin@coursewhale.com"));
            message.Subject = "CourseWhale--密码找回通知";
            var Encrypt = DES.Encode(id.ToString() + "&" + DateTime.Now.ToString());
            message.Body = new TextPart(TextFormat.Plain) { Text = "亲爱的用户:\n\n您好!CourseWhale系统检测到您正在进行'密码找回'操作，系统已经验证您的身份。\n\n为了账号完全考虑，请点击以下链接进行重置密码:\n\n" + path + "?k=" + Encrypt + "\n\nCourseWhale账号中心\n" + DateTime.Now.ToString() };//发送内容
            message.To.Add(new MailboxAddress(sendTo));//收件人
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.MessageSent += (sender, args) => { };

                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);

                smtp.Authenticate("systemadmin@coursewhale.com", "AnswerWang123!");//发件人邮箱 

                smtp.Send(message);

                smtp.Disconnect(true);

            }

        }
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="sendTo"></param>
        /// <param name="id"></param>
        public static void SendEmail(string sendTo, int id)
        {
            string path = AppConfig.Configuration["url"];
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "y477o7DhqDXNWdLm", "KQXXPABKIW8hVN5IdVAp7FtnqFTkiy");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendMailRequest request = new SingleSendMailRequest();
            try
            {

                request.AccountName = "info@coursewhale.com";
                request.FromAlias = "CourseWhale";
                request.AddressType = 1;
                //request.TagName = "控制台创建的标签";
                request.ReplyToAddress = true;
                request.ToAddress = sendTo;
                request.Subject = "CourseWhale--密码找回通知";
                var Encrypt = DES.Encode(id.ToString() + "&" + DateTime.Now.ToString());
                request.HtmlBody = "亲爱的用户:<br/><br/>您好!CourseWhale系统检测到您正在进行'密码找回'操作，系统已经验证您的身份。<br/><br/>为了账号完全考虑，请及时<a href=" + path + "?k=" + Encrypt + ">重置密码</a>。<br/><br/>CourseWhale账号中心<br/>" + DateTime.Now.ToString();
                SingleSendMailResponse httpResponse = client.GetAcsResponse(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 发邮件验证邮箱
        /// </summary>
        /// <param name="sendTo"></param>
        /// <param name="id"></param>
        public static void SendMail(string sendTo, int id)
        {
            string path = AppConfig.Configuration["emailurl"];
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "y477o7DhqDXNWdLm", "KQXXPABKIW8hVN5IdVAp7FtnqFTkiy");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendMailRequest request = new SingleSendMailRequest();
            try
            {

                request.AccountName = "info@coursewhale.com";
                request.FromAlias = "CourseWhale";
                request.AddressType = 1;
                //request.TagName = "控制台创建的标签";
                request.ReplyToAddress = true;
                request.ToAddress = sendTo;
                request.Subject = "CourseWhale--邮箱验证通知";
                var Encrypt = DES.Encode(id.ToString() + "&" + DateTime.Now.ToString());
                request.HtmlBody = "亲爱的用户:<br/><br/>您好!感谢您注册CourseWhale网站，邮箱验证成功后，您可以获得7天VIP会员，点击链接<a href=" + path + "?key=" + Encrypt + ">完成邮箱验证</a>。<br/><br/>CourseWhale账号中心<br/>" + DateTime.Now.ToString();
                SingleSendMailResponse httpResponse = client.GetAcsResponse(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

