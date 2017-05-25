using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core;
using StarterPack.Models;
using StarterPack.Core.Controllers;
using StarterPack.Mail;
using StarterPack.Core.Validation;
using FluentValidation;
using FluentValidation.Results;
using StarterPack.Core.Controllers.Attributes;

namespace StarterPack.Controllers
{
    [Route("api/v1/")]
    [Authorize("*:admin")]
    public class MailsController : BaseController
    {
        [HttpPost]
        [Route("mails")]
        public void mails([FromBody] MailView mail) {
            ModelValidator<MailView> validator = new ModelValidator<MailView>();
            validator.RuleFor(m => m.Message).NotEmpty();
            validator.RuleFor(m => m.Subject).NotEmpty();
            validator.RuleFor(m => m.Users).NotEmpty();

            ValidationResult results = validator.Validate(mail);

            if(!results.IsValid) {
                throw new Core.Exception.ValidationException(results.Errors);
            }

            new GenericMail(mail).Send();
        }
    }
}

