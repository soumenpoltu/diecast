using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
    public class EntityProductCatg
    {
      
        public Int32 MAST_CATEGORY_KEY { get; set; }
        public String CATEGORY_NAME { get; set; }



        public Int32 SUB_CATEGORY_KEY { get; set; }
        public String SUB_CATEGORY_NAME { get; set; }



        public Int32 SUB_SUB_CATEGORY_KEY { get; set; }
        public String SUB_SUB_CATEGORY_NAME { get; set; }


        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public EntityProductCatg() { }
    }
}
