﻿using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class PasswordModel : BaseItemModel
    {
        public string PasswordHash { get; set; }
        public int UserID { get; set; }
        public int IsInitial { get; set; }

        public override string ToString()
        {
            return string.Format("[Password: PasswordHash={0}, UserID={1}, IsInitial={2}]", PasswordHash, UserID, IsInitial);
        }
    }
}
