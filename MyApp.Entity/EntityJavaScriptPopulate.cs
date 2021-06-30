using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
   public class EntityJavaScriptPopulate
    {

        public String CATEGORY_LIST { get; set; }
        public String PRODUCT_LIST { get; set; }

        public decimal PRODUCT_PRICE { get; set; }

        public Int32 CUSTOMER_ADRESS_KEY { get; set; }
        public String CUSTOMER_FNAME { get; set; }
        public String CUSTOMER_LNAME { get; set; }
        public String CUSTOMER_ADDRESS { get; set; }
        public String CUSTOMER_CITY { get; set; }
        public String CUSTOMER_COUNTRY { get; set; }
        public String CUSTOMER_PHONE { get; set; }
        public String CUSTOMER_PINCODE { get; set; }

        public String ADDRESS_CUSTOMER_EMAIL { get; set; }



        public Int32 SUB_CATEGORY_KEY { get; set; }

        public String SUB_CATEGORY_NAME { get; set; }


        public Int32 SUB_SUB_CATEGORY_KEY { get; set; }

        public String SUB_SUB_CATEGORY_NAME { get; set; }

    }
}
