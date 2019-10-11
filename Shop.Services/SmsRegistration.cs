using Shop.Services.Abstract;
using System;
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
    public class SmsRegistration : IRegistration
    {
        private static Random random = new Random();
        public int verificationCode { get; private set; }

        public void Register(string mobileNumber)
        {
            verificationCode = random.Next(1000, 10000);
            const string accountSid = "AC928729392640ee1bdba44804389f71cf";
            const string authToken = "f1c5007c43c5c0f1a8e07b38a9115de9";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Your verification code: {verificationCode}",
                from: new Twilio.Types.PhoneNumber("+13184794933"),
                to: new Twilio.Types.PhoneNumber("+77786226134")
            );

            if(CheckCode(verificationCode)){

            }
        }

        public bool CheckCode(int code)
        {
            return code == int.Parse(Console.ReadLine());
        }
    }
}
