using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Elasticsearch.Net.Specification.MachineLearningApi;
using FluentValidation;
using WebCore.Entities;

namespace WebAppCoreBlazorServer.Common
{
    public class FluentValidation : AbstractValidator<ModuleFieldInfo>
    {
        public FluentValidation()
        {

            RuleFor(p => p.Value).NotNull().NotEmpty().When(m => m.Nullable != "Y").WithMessage("Đây là trường bắt buộc");
        }

        private async Task<bool> IsUniqueAsync(string name)
        {
            await Task.Delay(300);
            return name.ToLower() != "test";
        }
        private async Task<bool> IsNumner(string name)
        {
            return false;
        }


    }
    public class FieldFluentValidation : AbstractValidator<List<ModuleFieldInfo>>
    {
        public FieldFluentValidation()
        {
            //RuleFor(p => p.ForEach(x=>x.Value)..NotNull().NotEmpty().When(m => m.Nullable != "Y").WithMessage("Đây là trường bắt buộc"));
        }

        private async Task<bool> IsUniqueAsync(string name)
        {
            await Task.Delay(300);
            return name.ToLower() != "test";
        }
        private async Task<bool> IsNumner(string name)
        {
            return false;
        }


    }
}
