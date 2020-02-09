using Shop.DataAccess;
using Shop.Services.Abstract;
using System;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
/*
 * SID AC928729392640ee1bdba44804389f71cf
 * SID SKc4579e5e06264341965f856e0a6bbcc6
 * AuthToken f1c5007c43c5c0f1a8e07b38a9115de9
 * Trial Number +13184794933
 * SECRET MLt2PuPpWBXjee63rq4it3rIYxTylnAf
 */

namespace Shop.Services
{
    public class SmsVerification : IRegistration
    {
        private const string accountSid = "AC928729392640ee1bdba44804389f71cf";
        private const string authToken = "f1c5007c43c5c0f1a8e07b38a9115de9";
        private static Random random = new Random();

        public int SendCode(string phoneNumber)
        {
            var verificationCode = random.Next(1000, 10000);
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Your verification code: {verificationCode}",
                from: new Twilio.Types.PhoneNumber("+13184794933"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
            return verificationCode;
        }
    }
}
