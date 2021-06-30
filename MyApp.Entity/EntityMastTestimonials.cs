using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entity
{
    public class EntityMastTestimonials
    {
        public Int32 MAST_TESTIMONIALS_KEY { get; set; }
        public String CLIENT_IMAGE { get; set; }
        public String CLIENT_NAME { get; set; }
        public String CLIENT_FEEDBACK { get; set; }

        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public EntityMastTestimonials() { }
    }
}
