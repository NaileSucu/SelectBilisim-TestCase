using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class UserRules : AbstractValidator<User>
    {
        public UserRules()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad Alanı Boş Geçilemez!");
            RuleFor(x => x.FirstName).MaximumLength(30).WithMessage("30 Karakterden Fazla Girilmemesi Gerekmektedir!");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad Alanı Boş Geçilemez!"); 
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Mail Boş Geçilemez!"); 
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir E-Mail Adresi Giriniz!"); 
        }
    }
}
