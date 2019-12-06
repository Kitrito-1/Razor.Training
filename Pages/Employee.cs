using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Razor.Training
{
    public enum Sex { male = 1, Female = 2}

    public class Employee
    {
        [StringLength(10, ErrorMessage = "{0} 長度必須介於2與10字元之間", MinimumLength = 2)]
        [Required(ErrorMessage = "{0} 必須輸入")]
        [Display(Name = "姓名", Prompt = "請輸入姓名")]
        public String Name { get; set; }

        [Display(Name = "性別")]
        public Sex Sex { get; set; }

        [StringLength(10, ErrorMessage = "{0} 長度必須介於6與10字元之間", MinimumLength = 6)]
        [Required(ErrorMessage = "{0} 必須輸入")]
        [Display(Name = "員工編號", Prompt = "請輸入員工編號")]
        public String Empno { get; set; }

        [Required(ErrorMessage = "{0} 必須輸入")]
        [Display(Name = "到職日")]
        public DateTime EnterDate { get; set; }

        [EmailAddress(ErrorMessage = "{0} 格式不正確")]
        [Display(Name = "電子郵件", Prompt = "admin@doublegreen.com")]
        public String Email { get; set; }


    }
}
