using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
    public class EntityProduct
    {
      
        public Int32 MAST_CATEGORY_KEY { get; set; }

        public Int32 SUB_CATEGORY_KEY { get; set; }

        public Int32 SUB_SUB_CATEGORY_KEY { get; set; }


        public Int32 HEAD_PRODUCT_KEY { get; set; }
        public String PRICE { get; set; }
        public String QTY { get; set; }
        public String PRODUCT_NAME { get; set; }
        public Int32 DTLS_PRODUCT_KEY { get; set; }
        public String SHEET_CODE { get; set; }
        public String PRODUCT_IMAGE { get; set; }
        public String PRODUCT_IMAGE_2 { get; set; }
        public String MAIN_IMAGE { get; set; }
        public String PRODUCT_DESC { get; set; }

        public String SHEET_CODE_1 { get; set; }

        public String SHEET_CODE_2 { get; set; }
        public String SHEET_CODE_3 { get; set; }
        public String SHEET_CODE_4 { get; set; }

        public String SHEET_CODE_1_IMG { get; set; }

        public String SHEET_CODE_1_PDF { get; set; }
        public String SHEET_CODE_2_IMG { get; set; }
        public String SHEET_CODE_2_PDF { get; set; }

        public String SHEET_CODE_3_IMG { get; set; }

        public String SHEET_CODE_3_PDF { get; set; }
        public String SHEET_CODE_4_IMG { get; set; }
        public String SHEET_CODE_4_PDF { get; set; }


        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public EntityProduct() { }
    }
}
