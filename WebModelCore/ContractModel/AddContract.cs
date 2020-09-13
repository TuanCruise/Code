using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Entities.Entities;

namespace WebModelCore.ContractModel
{
    public class AddContract
    {
        public List<CLPRODUCTModel> Products { get; set; }
        public LoanAmount LoanAmount { get; set; }
        public AddContract()
        {
            Products = new List<CLPRODUCTModel>();
            LoanAmount = new LoanAmount();
            LoanAmount.ContractNo = "";
        }
    }
    public class LoanAmount
    {
        public string ContractNo { get; set; }
        public DateTime LoanDate { get; set; }
        /// <summary>
        /// Số ngày vay
        /// </summary>
        public int LoanDay { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal PriceOffer { get; set; }

        public decimal Amount { get; set; }
        public decimal InterestRateAndFee { get; set; }
        public int Promotion { get; set; }
        
    }
}

