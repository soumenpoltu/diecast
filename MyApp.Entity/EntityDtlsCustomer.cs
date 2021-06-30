using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
    public class EntityDtlsCustomer
    {
        public Int32 CUSTOMER_KEY { get; set; }

        public Int32 CUSTOMER_ADRESS_KEY { get; set; }
        public Int32 BILLING_CUSTOMER_ADRESS_KEY { get; set; }

        public String CUSTOMER_FNAME { get; set; }
        public String CUSTOMER_LNAME { get; set; }
        public String CUSTOMER_ADDRESS { get; set; }
        public String CUSTOMER_CITY { get; set; }
        public String CUSTOMER_COUNTRY { get; set; }
        public String CUSTOMER_PHONE { get; set; }
        public String CUSTOMER_PINCODE { get; set; }
        

        public String CUSTOMER_EMAIL { get; set; }
        public String ADDRESS_CUSTOMER_EMAIL { get; set; }

        
        public String CUSTOMER_PASSWORD { get; set; }
        public String CUSTOMER_CPASSWORD { get; set; }

        public String CUSTOMER_CURRENTPASSWORD { get; set; }


        public Int32 USER_TYPE_KEY { get; set; }
        public String ACTIVATION_CODE { get; set; }
        public Int32 TAG_ACTIVATION { get; set; }


        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public String CUSTOMER_NAME { get; set; }

        public String BILLING_CUSTOMER_ADDRESS { get; set; }
        public String BILLING_CUSTOMER_CITY { get; set; }
        public String BILLING_CUSTOMER_COUNTRY { get; set; }
        public String BILLING_CUSTOMER_PHONE { get; set; }
        public String BILLING_CUSTOMER_PINCODE { get; set; }



        public EntityDtlsCustomer() { }
    }
}
