using System;

namespace MyApp.Entity
{
    public class EntityBlogCat
    {
        public Int32 MAST_BLOG_CATEGORY_KEY { get; set; }
        public String BLOG_CAT_DESC { get; set; }



        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public EntityBlogCat() { }
    }
}
