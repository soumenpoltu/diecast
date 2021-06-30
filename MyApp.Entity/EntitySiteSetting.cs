using System;

namespace MyApp.Entity
{
    public class EntitySiteSetting
    {

        public String CONTACT_NO { get; set; }
        public String MAIL { get; set; }
        public String ADDRESS { get; set; }
        public String FOOTER_NOTE { get; set; }
        public String HEADER_LOGO { get; set; }
        public String FOOTER_LOGO { get; set; }
        public String BANNER_IMAGE { get; set; }

        public String FACEBOOK_LINK { get; set; }
        public String TWITTER_LINK { get; set; }
        public String LINKEDIN_LINK { get; set; }
        public String INSTAGRAM_LINK { get; set; }
        public String TELEGRAM_LINK { get; set; }
        public String PRINTEREST_LINK { get; set; }
        public String MEDIUM_LINK { get; set; }

        public String BANNER_DESC { get; set; }
        public String BANNER_VIDEO_LINK { get; set; }
        public String LARGE_VIDEO_LINK { get; set; }
      
        public Int32 DTLS_USEFULL_LINKS_KEY { get; set; }
        public String TITLE { get; set; }
        public String LINK { get; set; }

        public Int32 DTLS_PAGE_SETTING_KEY { get; set; }
        public Int32 MAST_PAGE_KEY { get; set; }
        public String PAGE_TITLE { get; set; }
        public String META_DESCRIPTION { get; set; }
        public String META_KEYWORD { get; set; }

        public String PAGE_NAME { get; set; }
        public Int32 MAST_TESTIMONIALS_KEY { get; set; }
        public String CLIENT_IMAGE { get; set; }
        public String CLIENT_NAME { get; set; }
        public DateTime POSTING_DATE { get; set; }
        public String CLIENT_FEEDBACK { get; set; }


        public Int32 ENT_USER_KEY { get; set; }
        public DateTime ENT_DATE { get; set; }
        public DateTime ENT_TIME { get; set; }
        public Int32 EDIT_USER_KEY { get; set; }
        public DateTime EDIT_DATE { get; set; }
        public DateTime EDIT_TIME { get; set; }
        public Byte TAG_ACTIVE { get; set; }
        public Byte TAG_DELETE { get; set; }

        public EntitySiteSetting() { }
    }
}
