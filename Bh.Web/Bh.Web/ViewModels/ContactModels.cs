using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Bh.Web.ViewModels.App_LocalResources;

namespace Bh.Web.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ModelResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ModelResource))]
        [StringLength(50, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ModelResource))]
        public string Name { get; set; }
        
        [Display(Name=@"E-mail")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ModelResource))]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessageResourceName = "EmailAddress", ErrorMessageResourceType = typeof(ModelResource))]
        public string Email { get; set; }

         [Display(Name = "Phone", ResourceType = typeof(ModelResource))]
        [StringLength(50, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ModelResource))]
        public string Phone { get; set; }

        [Display(Name = "Message", ResourceType = typeof(ModelResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ModelResource))]
        public string Message { get; set; }

        [Display(Name = "CaptchaWeekDay", ResourceType = typeof(ModelResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ModelResource))]
        public string CaptchaWeekDay { get; set; }

        public bool CaptchaIsValid()
        {
            return CaptchaWeekDay.ToLower().Trim() == ViewModels.App_LocalResources.ModelResource.CaptchaResult;
        }
        
    }

 
}
