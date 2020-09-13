using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using WebCore.Entities;
using WebCore.Entities.Entities;
using WebModelCore;
using WebModelCore.ContractModel;

namespace WebAppCoreBlazorServer.Pages
{
    public partial class Contract
    {
        public AddContract ContractModel { get; set; }
        private EditContext editContext;
        protected override void OnInitialized()
        {

            ContractModel = new AddContract();
            ContractModel.Products.Add(new CLPRODUCTModel {CATEGORY="" });
            editContext = new EditContext(ContractModel);
        }
        public async Task Save()
        {
            //HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var excute = new RestOutput<string>();
            string store = "";
            string modId = "";
            string keyEdit = "";            
            //Dongpv:
            excute = (await moduleService.UpdateData(modId, store, keyEdit, Conver2FieldInfos(ContractModel)));
        }

        private List<ModuleFieldInfo> Conver2FieldInfos(AddContract contract)
        {
            var fieldInfos = new List<ModuleFieldInfo>();
            foreach (var prop in contract.LoanAmount.GetType().GetProperties())
            {
                var name = prop.CustomAttributes.Where(x => x.AttributeType.Name.ToLower() == "ColumnAttribute".ToLower()).FirstOrDefault();
                fieldInfos.Add(new ModuleFieldInfo
                {
                    FieldID = name == null ? prop.Name : (name.NamedArguments.FirstOrDefault().TypedValue.Value ?? "").ToString(),
                    FieldName = name == null ? prop.Name : (name.NamedArguments.FirstOrDefault().TypedValue.Value ?? "").ToString(),
                    Value = (prop.GetType().GetField(prop.Name)==null?"": prop.GetType().GetField(prop.Name).ToString())
                }); ;
                //Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(prop, null));
            }
            var propProducts = contract.Products.GetType().GetProperties().FirstOrDefault();
            var nameProducts= propProducts.CustomAttributes.Where(x => x.AttributeType.Name.ToLower() == "ColumnAttribute".ToLower()).FirstOrDefault();
            var fldProduct= new ModuleFieldInfo
            {
                FieldID = nameProducts == null ? propProducts.Name : (nameProducts.NamedArguments.FirstOrDefault().TypedValue.Value ?? "").ToString(),
                FieldName = nameProducts == null ? propProducts.Name : (nameProducts.NamedArguments.FirstOrDefault().TypedValue.Value ?? "").ToString(),
                //Value = (prop.GetType().GetField(prop.Name) == null ? "" : prop.GetType().GetField(prop.Name).ToString())
            };
            
            foreach (var item in contract.Products)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    var name=prop.CustomAttributes.Where(x=>x.AttributeType.Name.ToLower()== "ColumnAttribute".ToLower()).FirstOrDefault();

                    fldProduct.FieldChilds.Add(new ModuleFieldInfo
                    {
                        FieldID = name==null? prop.Name: (name.NamedArguments.FirstOrDefault().TypedValue.Value??"").ToString(),
                        FieldName = name == null ? prop.Name : (name.NamedArguments.FirstOrDefault().TypedValue.Value ?? "").ToString(),
                        Value = (prop.GetType().GetField(prop.Name) == null ? "" : prop.GetType().GetField(prop.Name).ToString())
                    }); ;
                    //Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(prop, null));
                }
            }
            fieldInfos.Add(fldProduct);
            return fieldInfos;
        }

    }
}
