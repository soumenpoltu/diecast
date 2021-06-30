using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
    public class EntityAboutUs
    {
        public String ABOUT_US_DESC { get; set; }
        public String ABOUT_US_IMAGE { get; set; }

        public String REMARKS { get; set; }
        public String ICON_IMG_1 { get; set; }

        public String ICON_IMG_2 { get; set; }
        public String ICON_IMG_3 { get; set; }

        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public EntityAboutUs() { }
    }
}
