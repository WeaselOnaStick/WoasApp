﻿namespace WoasApp.Areas.Identity.Data
{
    public class UserLoginTime
    {
        public int Id { get; set; }
        public DateTime LoginTime { get; set; }

        //foreign
        public string UserId { get; set; }
        public WoasAppUser User { get; set; }
    }
}
