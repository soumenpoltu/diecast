using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
   public class EntityCart
    {
        public Int32 DTLS_CART_KEY { get; set; }
        public Int32 DTLS_PRODUCT_KEY { get; set; }
        public Int32 HEAD_PRODUCT_KEY { get; set; }

        public String SHEET_CODE { get; set; }

        public Int32 SHIPPING_ADDRESS_KEY { get; set; }
        public Int32 BILLING_ADDRESS_KEY { get; set; }
        public Decimal PRICE { get; set; }
        public Int32 QUANTITY { get; set; }
        public Decimal TAX { get; set; }

        public Decimal GROSS_AMOUNT { get; set; }
        public Decimal TOTAL_AMOUNT { get; set; }

        public Decimal SHIPPING_AMOUNT { get; set; }

        public String CUSTOMER_EMAIL { get; set; }

        public String TRANSACTION_ID { get; set; }
        public String INVOICE { get; set; }

        public Byte PAYMENT_STATUS { get; set; }




        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public Byte SAME_BILLING_ADDRESS { get; set; }

        public EntityCart() { }
    }
}
